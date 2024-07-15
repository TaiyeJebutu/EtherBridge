using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EtherXMLReader
{
    public class XMLICDMessages
    {
        private string _name;
        private string _header;
        private List<XMLFields> _fields;
        

        public XMLICDMessages(string fieldName, string header, List<XMLFields> fields)
        {
            _name = fieldName;
            _header = header;
            _fields = fields;
        }
    }
}
 