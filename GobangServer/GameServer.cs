﻿using System;
using System.Collections.Generic;
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
        public const string LocalhostIPAddress = "223.2.16.234";
        private const int ServerPort = 8086;
        
        public static Socket ServerSocket;
        private static LinkedList<ClientInfo> clientInfos;

        // Call the Report() method to signal the caller when something happen and some message should be notified to the caller.
        public static event Action<string> Report;

        private static BackgroundWorker serverWorker;

        public static void Start()
        {
            clientInfos = new LinkedList<ClientInfo>();
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

                ClientInfo client = new ClientInfo
                {
                    ClientSocket = clientSocket,
                    Worker = clientWorker,
                    State = ClientState.Idle,
                    Account = null
                };
                clientInfos.AddLast(client);

                clientWorker.DoWork += DoClientWork;
                clientWorker.RunWorkerAsync(client);
            }
        }

        public static void Close()
        {
            // Stop listening new socket connection request.
            // Place a "?" here to prevent an exception being thrown if this method is called before the Start() method.
            serverWorker?.CancelAsync();


        }

        private static void DoClientWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is ClientInfo clientInfo)
            {
                Socket clientSocket = clientInfo.ClientSocket;
                byte[] receiveBuffer = new byte[1024];

                for (;;)
                {
                    int receivedLength = clientSocket.Receive(receiveBuffer);
                    string jsonText = Encoding.UTF8.GetString(receiveBuffer, 0, receivedLength);
                    JObject jsonPackage = JObject.Parse(jsonText);

                    Report?.Invoke("123");
                    //Report?.Invoke(jsonPackage[JsonPackageKeys.Type].ToString());

                    switch (jsonPackage[JsonPackageKeys.Type].ToString())
                    {
                        case JsonPackageKeys.Register:
                            Register(jsonPackage[JsonPackageKeys.Body], clientSocket);
                            break;
                    }
                }
            }
        }

        private static void Register(JToken accountInfo, Socket clientSocket)
        {
            JObject registerResponseMessage;
            byte[] registerResponseBytes;

            string account = accountInfo[JsonPackageKeys.Account].ToString();

            // Send error message if the account name already exits.
            // Otherwise, create the account.
            if (SqlExecutor.Exists(account))
            {
                registerResponseMessage = JObject.FromObject(new
                {
                    Type = "Error",
                    Body = new
                    {
                        DetailedError = "Account already exists"
                    }
                });
            }
            else
            {
                string password = accountInfo[JsonPackageKeys.Password].ToString();
                string mailAddress = accountInfo[JsonPackageKeys.MailAddress].ToString();

                SqlExecutor.CreateAccount(account, password, mailAddress);
                registerResponseMessage = JObject.FromObject(new
                {
                    Type = "Success",
                    Body = ""
                });
            }

            registerResponseBytes = Encoding.UTF8.GetBytes(registerResponseMessage.ToString());
            clientSocket.Send(registerResponseBytes);
        }
    }
}
