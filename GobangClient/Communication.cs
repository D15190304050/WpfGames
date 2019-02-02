using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    public static class Communication
    {
        private const int BufferSize = 1024;
        private const string ServerIPAddress = "127.0.0.1";
        private const int ServerPort = 8086;

        private static Socket ClientSocket { get; set; }
        private static byte[] receiveBuffer;

        // To prevent build the connection twice when call the Start() method twice.
        private static bool started;

        public static void Start()
        {
            if (!started)
            {
                started = true;
                IPAddress serverIPAddress = IPAddress.Parse(ServerIPAddress);
                IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerPort);
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ClientSocket.Connect(serverEndPoint);
                receiveBuffer = new byte[BufferSize];
            }
        }

        public static void Send(string messageType, object messageBody)
        {
            JObject jsonToSend = JObject.FromObject(new
            {
                Type = messageType,
                Body = messageBody
            });

            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonToSend.ToString());
            ClientSocket.Send(jsonBytes);
        }

        // Receive the response message encapsulated in JSON objects.
        // Note that since this is a multi-thread based game, and quite a few of them will send messages to the server, it's obviously that the Communication class may receive several response message at a time.
        public static JObject[] Receive()
        {
            int receivedLength = ClientSocket.Receive(receiveBuffer);
            string responseText = Encoding.UTF8.GetString(receiveBuffer, 0, receivedLength);
            string[] responseJsons = Regex.Split(responseText, "}{");
            for (int i = 0; i < responseJsons.Length; i++)
            {
                if (!responseJsons[i].EndsWith("}"))
                    responseJsons[i] = responseJsons[i] + "}";
                if (!responseJsons[i].StartsWith("{"))
                    responseJsons[i] = "{" + responseJsons[i];
            }

            JObject[] responseMessages = new JObject[responseJsons.Length];
            for (int i = 0; i < responseMessages.Length; i++)
                responseMessages[i] = JObject.Parse(responseJsons[i]);

            return responseMessages;
        }

        public static void Close()
        {
            ClientSocket.Close();
        }
    }
}
