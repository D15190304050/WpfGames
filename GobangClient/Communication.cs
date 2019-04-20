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
    /// <summary>
    /// <see cref="Communication" />类提供了用于支持在客户端与服务器进行通信的方法。
    /// </summary>
    public static class Communication
    {
        /// <summary>
        /// 接受缓冲区的字节大小。
        /// </summary>
        private const int ReceiveBufferSize = 1024;

        /// <summary>
        /// 服务器IP地址的字符串表示。
        /// </summary>
        /// <remarks>
        /// 在实际部署时记得将此IP地址修改成部署后的服务器IP地址。
        /// </remarks>
        private const string ServerIPAddress = "127.0.0.1";

        /// <summary>
        /// 客户端与服务器通信时用到的服务器端口号。
        /// </summary>
        private const int ServerPort = 8086;

        /// <summary>
        /// 获取或设置用于与服务器通信的套接字。
        /// </summary>
        private static Socket clientSocket;

        /// <summary>
        /// 用于接受来自服务器数据的字节数组。
        /// </summary>
        private static byte[] receiveBuffer;

        // To prevent build the connection twice when call the Start() method twice.
        /// <summary>
        /// 用于防止连接被多次启动的而设置的标记。
        /// </summary>
        private static bool started;

        public static void Start()
        {
            if (!started)
            {
                started = true;
                IPAddress serverIPAddress = IPAddress.Parse(ServerIPAddress);
                IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerPort);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(serverEndPoint);
                receiveBuffer = new byte[ReceiveBufferSize];
            }
        }

        /// <summary>
        /// 向服务器发送数据包。
        /// </summary>
        /// <param name="messageType">数据包里信息的类型。</param>
        /// <param name="messageBody">实际要发送的数据。</param>
        public static void Send(string messageType, object messageBody)
        {
            JObject jsonToSend = JObject.FromObject(new
            {
                Type = messageType,
                Body = messageBody
            });

            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonToSend.ToString());
            clientSocket.Send(jsonBytes);
        }

        // Receive the response message encapsulated in JSON objects.
        // Note that since this is a multi-thread based game, and quite a few of them will send messages to the server, it's obviously that the Communication class may receive several response message at a time.

        /// <summary>
        /// 接收服务器发来的多个JSON对象。
        /// </summary>
        /// <returns>服务器发来的多个JSON对象。</returns>
        public static JObject[] ReceiveMessages()
        {
            int receivedLength = clientSocket.Receive(receiveBuffer);
            string responseText = Encoding.UTF8.GetString(receiveBuffer, 0, receivedLength);

            // 如果服务器发送了多个JSON对象过来，那么第n个JSON对象会以"}"结尾，紧接着就是"{"，即第(n + 1)个JSON对象的开始，因此找到"}{"，就找到了切分点。
            string[] responseJsons = Regex.Split(responseText, "}{");
            for (int i = 0; i < responseJsons.Length; i++)
            {
                // 将大括号补回去，还原成正常的JSON格式。
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

        /// <summary>
        /// 接收服务器发来的单个JSON对象。
        /// </summary>
        /// <returns>服务器发来的单个JSON对象。</returns>
        public static JObject ReceiveMessage()
        {
            int receivedLength = clientSocket.Receive(receiveBuffer);
            string responseText = Encoding.UTF8.GetString(receiveBuffer, 0, receivedLength);
            JObject responseMessage = JObject.Parse(responseText);

            return responseMessage;
        }

        public static void Close()
        {
            clientSocket.Close();
        }
    }
}
