using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EtherXMLReader
{
    public class XMLFields
    {
        public string ?_name;
        public string ?_startingbit;
        public string ?_endingbit;
        public string ?_format;
        public string ?_customformat;
        public string ?_resolution;

        public XMLFields(string fieldName,string startingbit, string endingbit,
                         string? format, string? customformat, string? resolution)
        {
            _name = fieldName;            
            _startingbit = startingbit;
            _endingbit = endingbit;
            _format = format;
            _customformat = customformat;
            _resolution = resolution;
        }
    }
}
 