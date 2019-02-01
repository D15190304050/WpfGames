using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangServer
{
    public static class Communication
    {
        private const int BufferSize = 1024;
        private static byte[] receiveBuffer;

        // To prevent build the connection twice when call the Start() method twice.
        private static bool started;

        static Communication()
        {
            if (!started)
            {
                started = true;
                receiveBuffer = new byte[BufferSize];
            }
        }

        public static void Send(Socket clientSocket, string messageType, object messageBody)
        {
            JObject jsonToSend = JObject.FromObject(new
            {
                Type = messageType,
                Body = messageBody
            });

            int x = jsonToSend.ToString() == "{}" ? 1 : 0;

            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonToSend.ToString());
            clientSocket.Send(jsonBytes);
        }

        public static JObject Receive(Socket clientSocket)
        {
            int receivedLength = clientSocket.Receive(receiveBuffer);
            string responseText = Encoding.UTF8.GetString(receiveBuffer, 0, receivedLength);
            JObject responseMessage = JObject.Parse(responseText);

            return responseMessage;
        }
    }
}
