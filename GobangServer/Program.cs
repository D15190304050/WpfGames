using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace GobangServer
{
    // Given that the HTTP protocol is a stateless protocol (It can only send response message one and the link will
    // be closed as long as the response is sent), socket will be used for the server.
    public class Program
    {
        public const string LocalhostIPAddress = "http://223.2.16.234/";
        public static void Main(string[] args)
        {
            #region Parsing JSON
            string jsonText = @"{
    ""companyID"": ""15"",

    ""employees"": [
        {
            ""firstName"": ""Bill"",
            ""lastName"": ""Gates""
        },
        {
            ""firstName"": ""George"",
            ""lastName"": ""Bush""
        }
    ],

    ""manager"": [
        {
            ""salary"": ""6000"",
            ""age"": ""23""
        },
        {
            ""salary"": ""8000"",
            ""age"": ""26""
        }
    ]

}";

            //Console.WriteLine("Original JSON object: ");
            //Console.WriteLine(jsonText);

            //RootObject rb = JsonConvert.DeserializeObject<RootObject>(jsonText);

            //Console.WriteLine(rb.companyID);

            //Console.WriteLine(rb.employees[0].firstName);

            //foreach (Manager ep in rb.manager)
            //    Console.WriteLine(ep.age);
            #endregion

            #region Data interaction, server part.

            // Set up the HttpListener for listening Http requests.
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(LocalhostIPAddress);
            listener.Start();
            Console.WriteLine("Listening");

            // Get the request and response object.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            // Get the request stream to read request content from it.
            byte[] inputBuffer = new byte[1024];
            Stream requestStream = request.InputStream;
            int readLength = requestStream.Read(inputBuffer, 0, 1024);
            string requestContent = Encoding.UTF8.GetString(inputBuffer, 0, readLength);

            Console.WriteLine("Original JSON object: ");
            Console.WriteLine(requestContent);

            RootObject rb = JsonConvert.DeserializeObject<RootObject>(jsonText);

            Console.WriteLine(rb.companyID);
            Console.WriteLine(rb.employees[0].firstName);
            foreach (Manager ep in rb.manager)
                Console.WriteLine(ep.age);

            // Prepare the response message.
            string responseString = "JSON object received.\n";
            byte[] outputBuffer = Encoding.UTF8.GetBytes(responseString);

            // Get a response stream and write the response to it.
            response.ContentLength64 = outputBuffer.Length;
            Stream responseStream = response.OutputStream;
            responseStream.Write(outputBuffer, 0, outputBuffer.Length);
            responseStream.Flush();

            // Get another request message.
            Thread.Sleep(500);
            readLength = requestStream.Read(inputBuffer, 0, 1024);
            requestContent = Encoding.UTF8.GetString(inputBuffer, 0, readLength);
            Console.WriteLine(requestContent);

            // Response it.
            responseString = "Finish.\n";
            outputBuffer = Encoding.UTF8.GetBytes(responseString);
            responseStream.Write(outputBuffer, 0, outputBuffer.Length);
            responseStream.Flush();

            // Clean up all streams.
            responseStream.Close();
            requestStream.Close();

            #endregion

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void HttpRequestTest()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(LocalhostIPAddress);
            request.Method = "POST";

        }
    }

    #region Json serialization classes.
    public class Employees
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class Manager
    {
        public string salary { get; set; }
        public string age { get; set; }
    }

    public class RootObject
    {
        public string companyID { get; set; }
        public List<Employees> employees { get; set; }
        public List<Manager> manager { get; set; }
    }
    #endregion
}
