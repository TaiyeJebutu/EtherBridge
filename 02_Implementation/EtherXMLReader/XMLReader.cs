using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EtherXMLReader
{
    public class XMLReader
    {
        private readonly string _clientsParam = "Clients";
        private readonly string _clientParam = "Client";
        private readonly string _MessagesParam = "Message";
        private readonly string _FieldParam = "Field";
        private readonly string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());


        private XDocument? _document;

        public XDocument Document
        {
            get { return _document; }
        }
        public XMLReader(string xml)         
        {
            
            _document = XDocument.Load(xml);
            
            

        }


        public List<XMLICDMessage> GetICDMessages()
        {
            List < XMLICDMessage > icdMessages = new List<XMLICDMessage> ();

            if (_document != null)
            {
                var messages = _document.Descendants(_MessagesParam).ToList();

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

        public List<XMLClients> GetClients()
        {
            List<XMLClients> clients = new List<XMLClients>();

            if (_document != null)
            {
                var xClients = _document.Descendants(_clientParam).ToList();
               

                foreach ( var xclient in xClients)
                {
                    XMLClients client = new XMLClients(xclient);
                    clients.Add(client);
                }

            }

            return clients;
        }

    }
}