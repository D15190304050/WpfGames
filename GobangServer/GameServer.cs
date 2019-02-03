using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangServer
{
    public static class GameServer
    {
        public const string LocalhostIPAddress = "127.0.0.1";
        private const int ServerPort = 8086;

        public static Socket ServerSocket;
        private static ConcurrentQueue<ClientInfo> clientInfos;

        // Theoretically, this member is only used in the UserGoOffline() method, so it should be a variable in the method.
        // But for the performance measure, I put it here as a class member.
        // So that this object will only be created once instead of being created again whenever the UserGoOffline() method is called.
        private static LinkedList<ClientInfo> clientInfosBackup;

        // Call the Report() method to signal the caller when something happen and some message should be notified to the caller.
        public static event Action<string> Report;

        private static BackgroundWorker serverWorker;

        public static void Start()
        {
            clientInfos = new ConcurrentQueue<ClientInfo>();
            clientInfosBackup = new LinkedList<ClientInfo>();
            IPAddress serverIPAddress = IPAddress.Parse(LocalhostIPAddress);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerPort);
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(serverEndPoint);
            ServerSocket.Listen(10);

            // Using Socket.BeginAccept() method can accept an incoming socket connection, but it will directly create a new thread.
            // If we use a for loop here, the BeginAccept() method will create new thread one by one without stopping.
            // But what I want is that it can stop there waiting for incoming connection, and only create a new thread when a connection is coming.
            // This makes blocking method Accept() a better choice for this task.
            // Another problem here is for the window program.
            // If Accept() method is used here in the main thread, the entire joint test window will be blocked.
            // As a result, the task of waiting for incoming connection should only be put in a separate thread.
            serverWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            serverWorker.DoWork += BeginListenSocket;
            serverWorker.RunWorkerAsync();

        }

        private static void BeginListenSocket(object sender, DoWorkEventArgs e)
        {
            for (; ; )
            {
                // Accept a new client socket connection request and report it.
                Socket clientSocket = ServerSocket.Accept();
                Report?.Invoke("Connection established...");

                BackgroundWorker clientWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

                clientWorker.DoWork += DoClientWork;
                clientWorker.RunWorkerAsync(clientSocket);
            }
        }

        public static void Close()
        {
            // Stop listening new socket connection request.
            // Place a "?" here to prevent an exception being thrown if this method is called before the Start() method.
            serverWorker?.CancelAsync();

            // Terminate all client threads.
            foreach (ClientInfo client in clientInfos)
                client.Worker.CancelAsync();
        }

        private static void DoClientWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is Socket clientSocket)
            {
                for (; ; )
                {
                    // If the client close the socket, the Receive() method here returns null, indicating that this BackgroundWorker should be stopped.
                    JObject jsonPackage = Communication.Receive(clientSocket);

                    if (jsonPackage == null)
                    {
                        UserGoOffline(clientSocket);
                        e.Cancel = true;
                    }
                    else
                    {
                        // If Report event is raised here, the whole server program will be blocked.
                        // I don't know why this happened and I will fix this.
                        //Report?.Invoke(jsonPackage[JsonPackageKeys.Type].ToString());

                        switch (jsonPackage[JsonPackageKeys.Type].ToString())
                        {
                            case JsonPackageKeys.Register:
                                Register(jsonPackage[JsonPackageKeys.Body], clientSocket);
                                break;
                            case JsonPackageKeys.Login:
                                Login(jsonPackage[JsonPackageKeys.Body], clientSocket, sender as BackgroundWorker);
                                break;
                            case JsonPackageKeys.ValidateAccount:
                                ValidateMailAddress(jsonPackage[JsonPackageKeys.Body], clientSocket);
                                break;
                            case JsonPackageKeys.ModifyPassword:
                                ModifyPassword(jsonPackage[JsonPackageKeys.Body], clientSocket);
                                break;
                            case JsonPackageKeys.RequestForUserList:
                                GetUserList(clientSocket);
                                break;
                            case JsonPackageKeys.RequestForMatch:
                                ForwardMatchRequest(jsonPackage, clientSocket);
                                break;
                            case JsonPackageKeys.AcceptMatch:
                                ForwardMatchAcceptance(jsonPackage[JsonPackageKeys.Body]);
                                break;
                            case JsonPackageKeys.RejectMatch:
                                RejectMatch(jsonPackage[JsonPackageKeys.Body]);
                                break;
                        }
                    }
                }
            }
        }

        private static void Register(JToken accountInfo, Socket clientSocket)
        {
            string account = accountInfo[JsonPackageKeys.Account].ToString();

            // Send error message if the account name already exits.
            // Otherwise, create the account.
            if (SqlExecutor.Exists(account))
            {
                object errorMessage = new
                {
                    DetailedError = JsonPackageKeys.AccountAlreadyExists
                };
                Communication.Send(clientSocket, JsonPackageKeys.Error, errorMessage);
            }
            else
            {
                string password = accountInfo[JsonPackageKeys.Password].ToString();
                string mailAddress = accountInfo[JsonPackageKeys.MailAddress].ToString();

                SqlExecutor.CreateAccount(account, password, mailAddress);
                Communication.Send(clientSocket, JsonPackageKeys.Success, "");
            }
        }

        private static void Login(JToken accountInfo, Socket clientSocket, BackgroundWorker worker)
        {
            string account = accountInfo[JsonPackageKeys.Account].ToString();

            if (!SqlExecutor.Exists(account))
            {
                object errorMessage = new
                {
                    DetailedError = JsonPackageKeys.NoSuchAccount
                };
                Communication.Send(clientSocket, JsonPackageKeys.Error, errorMessage);
            }
            else
            {
                string password = accountInfo[JsonPackageKeys.Password].ToString();

                if (!SqlExecutor.ValidatePassword(account, password))
                {
                    object errorMessage = new
                    {
                        DetailedError = JsonPackageKeys.WrongPassword
                    };
                    Communication.Send(clientSocket, JsonPackageKeys.Error, errorMessage);
                }
                else
                {
                    // Add the new online user to online user list.
                    ClientInfo client = new ClientInfo
                    {
                        Account = account,
                        ClientSocket = clientSocket,
                        State = ClientState.Idle,
                        Worker = worker
                    };
                    clientInfos.Enqueue(client);

                    Communication.Send(clientSocket, JsonPackageKeys.Success, "");
                }
            }
        }

        private static void ValidateMailAddress(JToken accountInfo, Socket clientSocket)
        {
            string account = accountInfo[JsonPackageKeys.Account].ToString();

            if (!SqlExecutor.Exists(account))
            {
                object errorMessage = new
                {
                    DetailedError = JsonPackageKeys.NoSuchAccount
                };
                Communication.Send(clientSocket, JsonPackageKeys.Error, errorMessage);
            }
            else
            {
                string mailAddress = accountInfo[JsonPackageKeys.MailAddress].ToString();

                if (!SqlExecutor.ValidateMailAddress(account, mailAddress))
                {
                    object errorMessage = new
                    {
                        DetailedError = JsonPackageKeys.WrongMailAddress
                    };
                    Communication.Send(clientSocket, JsonPackageKeys.Error, errorMessage);
                }
                else
                    Communication.Send(clientSocket, JsonPackageKeys.Success, "");
            }
        }

        private static void ModifyPassword(JToken accountInfo, Socket clientSocket)
        {
            string account = accountInfo[JsonPackageKeys.Account].ToString();
            string newPassword = accountInfo[JsonPackageKeys.Password].ToString();
            SqlExecutor.ModifyPassword(account, newPassword);
            Communication.Send(clientSocket, JsonPackageKeys.Success, "");
        }

        private static void GetUserList(Socket clientSocket)
        {
            JArray idleUsers = new JArray();
            JArray playingUsers = new JArray();

            ClientInfo[] clientInformation = clientInfos.ToArray();
            foreach (ClientInfo client in clientInfos)
            {
                if (client.State == ClientState.Idle)
                {
                    idleUsers.Add(JObject.FromObject(new
                    {
                        Account = client.Account
                    }));
                }
                else
                {
                    playingUsers.Add(JObject.FromObject(new
                    {
                        Account = client.Account
                    }));
                }
            }

            // Send the count of 2 kind of users so that the client can avoid index out of boundary exception.
            object userList = new
            {
                IdleUserCount = idleUsers.Count,
                PlayingUserCount = playingUsers.Count,
                IdleUsers = idleUsers,
                PlayingUsers = playingUsers
            };

            Communication.Send(clientSocket, JsonPackageKeys.UserList, userList);
        }

        private static void ForwardMatchRequest(JToken requestInfo, Socket initiatorSocket)
        {
            ClientInfo opponent = FindClientByAccount(requestInfo[JsonPackageKeys.Body][JsonPackageKeys.OpponentAccount].ToString());

            // Actually, opponent can never be null.
            if ((opponent != null) && (opponent.State == ClientState.Idle))
                Communication.Send(opponent.ClientSocket, JsonPackageKeys.RequestForMatch, requestInfo[JsonPackageKeys.Body]);
            else
                Communication.Send(initiatorSocket, JsonPackageKeys.OpponentNotAvailable, "");
        }

        private static ClientInfo FindClientByAccount(string account)
        {
            foreach (ClientInfo client in clientInfos)
            {
                if (client.Account == account)
                    return client;
            }

            return null;
        }

        private static void ForwardMatchAcceptance(JToken responseMessage)
        {
            string initiatorAccount = responseMessage[JsonPackageKeys.InitiatorAccount].ToString();
            ClientInfo initiator = FindClientByAccount(initiatorAccount);

            // Actually, opponent can never be null.
            if (initiator != null)
                Communication.Send(initiator.ClientSocket, JsonPackageKeys.AcceptMatch, responseMessage);
        }

        private static void RejectMatch(JToken matchInfo)
        {
            string initiatorAccount = matchInfo[JsonPackageKeys.InitiatorAccount].ToString();
            ClientInfo initiator = FindClientByAccount(initiatorAccount);

            // Actually, opponent can never be null.
            if (initiator != null)
                Communication.Send(initiator.ClientSocket, JsonPackageKeys.RejectMatch, "");
        }

        private static void UserGoOffline(Socket clientSocket)
        {
            ClientInfo clientToGoOffline = null;
            foreach (ClientInfo client in clientInfos)
            {
                if (client.ClientSocket == clientSocket)
                {
                    clientToGoOffline = client;
                    break;
                }
            }

            if (clientToGoOffline != null)
            {
                // Use "lock" keyword to make sure the thread safety.
                lock (clientInfos)
                {
                    // Copy to clientInfoBackup.
                    // Note that the user who goes offline is not copied.
                    clientInfosBackup.Clear();
                    foreach (ClientInfo client in clientInfos)
                    {
                        if (client != clientToGoOffline)
                            clientInfosBackup.AddLast(client);
                    }

                    // Clear the original queue.
                    while (clientInfos.Count != 0)
                        clientInfos.TryDequeue(out ClientInfo r);

                    // Copy back.
                    foreach (ClientInfo client in clientInfosBackup)
                        clientInfos.Enqueue(client);
                }
            }
        }
    }
}
