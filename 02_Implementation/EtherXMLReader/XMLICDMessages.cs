using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EtherXMLReader
{
    public class XMLICDMessage
    {
        private string _name;
        private string _header;
        private List<XMLFields> _fields;

        public string Name { get { return _name; } }
        public int Header { get { return Int32.Parse(_header); } }
        public List<XMLFields> Fields { get { return _fields; } }
        

        public XMLICDMessage(string fieldName, string header, List<XMLFields> fields)
        {
            _name = fieldName;
            _header = header;
            _fields = fields;
        }
    }
}
 