using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EtherXMLReader
{
    public class ServerConfig
    {
        public IPAddress ServerAddress { get; set; }
        public int Port { get; set; }
        public ServerConfig() 
        {            
        }
    }
}
