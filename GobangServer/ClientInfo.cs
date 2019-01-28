using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace GobangServer
{
    public class ClientInfo
    {
        public BackgroundWorker Worker { get; set; }
        public ClientState State { get; set; }
        public Socket ClientSocket { get; set; }

        public string Account { get; set; }
    }
}
