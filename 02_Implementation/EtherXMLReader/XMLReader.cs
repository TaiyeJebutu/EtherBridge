using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EtherXMLReader
{
    public class XMLReader
    {
        private readonly string _clientsParam = "Clients";
        private readonly string _clientParam = "Client";
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