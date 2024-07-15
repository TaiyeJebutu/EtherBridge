using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherBridge
{
    public class TranslatedMessage
    {
        private string _name;
        private Dictionary<string, string> _fields = new Dictionary<string, string>();

        public TranslatedMessage(string name) 
        { 
            _name = name;
        }

        public void AddResult(string fieldName, string value)
        {
            _fields[fieldName] = value;
        }

    }
}
