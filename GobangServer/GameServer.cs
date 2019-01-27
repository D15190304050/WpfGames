using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GobangServer
{
    public static class GameServer
    {
        public const string LocalhostIPAddress = "223.2.16.234";
        private const int ServerPort = 8086;

        public static Socket ServerSocket;
        public static Dictionary<Socket, ClientState> ClientSockets;

        // Call the Report() method to signal the caller when something happen and some message should be notified to the caller.
        public static event Action<string> Report;

        public static void Start()
        {
            ClientSockets = new Dictionary<Socket, ClientState>();

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
            Thread socketListeningThread = new Thread(() =>
            {
                for (; ; )
                {
                    Socket clientSocket = ServerSocket.Accept();
                    ClientSockets.Add(clientSocket, ClientState.Idle);
                    Report?.Invoke("Connection established...");
                }
            });

            socketListeningThread.Start();

            //ServerSocket.BeginAccept(AcceptSocket, null);
        }

        private static void AcceptSocket(IAsyncResult asyncResult)
        {
            Socket clientSocket = ServerSocket.EndAccept(asyncResult);
            ClientSockets.Add(clientSocket, ClientState.Idle);

            Report?.Invoke("Connection established...");
        }
    }
}
