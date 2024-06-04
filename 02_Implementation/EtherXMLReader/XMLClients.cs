using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EtherXMLReader
{
    public class XMLClients
    {
        private readonly string _clientName = "Name";
        private readonly string _clientIP = "IP";
        private readonly string _clientPort = "Port";

        private string _name;
        private string _ip;
        private string _port;

        public string Name => _name;
        public string IP => _ip;
        public string Port => _port;
        public XMLClients(XElement client) 
        {
            XNode? node = client.FirstNode;
            
            if (node != null)
            {
                _name = GetValue(_clientName, node.ToString());

                node = node.NextNode;                
                _ip = GetValue(_clientIP, node.ToString());

                node = node.NextNode;
                _port = GetValue(_clientPort, node.ToString());
            }
            else
            {
                throw new Exception("Node is null, unable to assign name/ip/port");
            }
            
        }


        private string GetValue(string param, string str)
        {
            string frontTag = $"<{param}>";
            string endTag = $"<\\{param}>";
            int from = str.IndexOf(frontTag)+frontTag.Length;
            int to = str.IndexOf(endTag);

            string result = str.Substring(from, to - from);
            return result;

        }
    }
}
 