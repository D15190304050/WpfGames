using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace GobangServer
{
    // Given that the HTTP protocol is a stateless protocol (It can only send response message one and the link will
    // be closed as long as the response is sent), socket will be used for the server.
    public class Program
    {
        public static int Main(string[] args)
        {
            GameServer.Report += Console.WriteLine;
            GameServer.Start();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }
    }
}
