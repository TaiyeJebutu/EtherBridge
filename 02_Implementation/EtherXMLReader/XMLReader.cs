using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EtherXMLReader
{
    public class XMLReader
    {
        
        private readonly string _MessagesParam = "Message";
        private readonly string _FieldParam = "Field";
        private readonly string _ServerParam = "Server";
       


        private XDocument? _icd;
        private XDocument? _network;

        public XDocument Document
        {
            get { return _icd; }
        }
        public XMLReader(string xml, string network)         
        {
            
            _icd = XDocument.Load(xml);
            _network = XDocument.Load(network);
            

        }


        public List<XMLICDMessage> GetICDMessages()
        {
            List < XMLICDMessage > icdMessages = new List<XMLICDMessage> ();

            if (_icd != null)
            {
                var messages = _icd.Descendants(_MessagesParam).ToList();

                // Loop through every type of message
                foreach (var message in messages)
                {
                    // Get the name and header of the message
                    string msgName = message.Attribute("name").Value;
                    string msgHeader = message.Attribute("header").Value;


                    var fields = message.Descendants(_FieldParam).ToList();
                    List<XMLFields> fieldsList = new List<XMLFields>(); 


                    // loop through every field in a message
                    foreach (var field in fields)
                    {
                        

                        string fieldName = field.Attribute("name").Value; 
                        string startingbit = field.Attribute("startingbit").Value; 
                        string endingbit = field.Attribute("endingbit").Value;

                        
                        string customformat = field.Attribute("customformat").Value;                     
                       
                        string format = field.Attribute("format").Value;
                        string resolution = field.Attribute("resolution").Value;
                        
                        XMLFields msgField = new XMLFields(fieldName, startingbit, endingbit, format, customformat, resolution);
                        fieldsList.Add(msgField);
                    }
                    XMLICDMessage msg = new XMLICDMessage(msgName, msgHeader, fieldsList);

                    icdMessages.Add(msg);
                }
            }
            return icdMessages;
        }

        public ServerConfig GetServerConfig()
        {
            ServerConfig serverConfig = new ServerConfig();
            Console.WriteLine("Loading Network Config");
            if(_network != null)
            {
                var serverDetails = _network.Descendants(_ServerParam).ToList()[0];

                string ip = serverDetails.Attribute("IP").Value;
                string port = serverDetails.Attribute("Port").Value;

                serverConfig.Port = Int32.Parse(port);
                serverConfig.ServerAddress = IPAddress.Parse(ip);
            }          

            return serverConfig;
        }

    }
}