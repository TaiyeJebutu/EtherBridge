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
        private string ?_name;        
        private string ?_startingbit;
        private string ?_endingbit;
        private string ?_format;
        private string ?_customformat;
        private string ?_resolution;

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
 