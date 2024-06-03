using System.Xml;


namespace EtherXMLReader
{
    public class XMLReader
    {

        private XmlTextReader? _reader;

        public XmlTextReader Reader
        {
            get { return _reader; }
        }
        public XMLReader(string xml)         
        {
            _reader = new XmlTextReader(xml);
        }

        public List<string> GetClients()
        {
            List<string> clients = new List<string>();

            if (_reader != null)
            {
                _reader.Read();
                while (_reader.Read())
                {
                    var result = _reader.MoveToElement();
                    clients.Add(result.ToString());
                }
            }

            return clients;
        }

    }
}