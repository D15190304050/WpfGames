using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GobangServer
{
    public static class GameServer
    {
        public const string LocalhostIPAddress = "223.2.16.234";
        private const int ServerPort = 8086;

        public static Socket serverSocket;
        public static Dictionary<Socket, ClientState> clientSockets;

        public static event Action<string> Report;

        public static void Start()
        {
            clientSockets = new Dictionary<Socket, ClientState>();

            IPAddress serverIPAddress = IPAddress.Parse(LocalhostIPAddress);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerPort);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(10);

            serverSocket.BeginAccept(AcceptSocket, null);
        }

        private static void AcceptSocket(IAsyncResult asyncResult)
        {
            Socket clientSocket = serverSocket.EndAccept(asyncResult);
            clientSockets.Add(clientSocket, ClientState.Idle);

            Report?.Invoke("Connection established...");
        }
    }
}
