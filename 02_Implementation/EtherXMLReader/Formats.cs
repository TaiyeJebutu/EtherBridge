using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherXMLReader
{
    public class Formats
    {
        public List<Details> details = new List<Details>();
        public string Name;
        public Formats(string name) 
        {
            Name = name;
        }
    }
}
