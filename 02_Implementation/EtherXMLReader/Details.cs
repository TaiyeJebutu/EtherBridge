using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherXMLReader
{
    public class Details
    {
        public double OriginalValue;
        public string NewValue;
        public Details(double originalValue, string newValue) 
        {
            OriginalValue = originalValue;
            NewValue = newValue;
        }
    }
}
