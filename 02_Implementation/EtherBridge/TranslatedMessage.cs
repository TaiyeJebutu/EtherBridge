using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherBridge
{
    public class TranslatedMessage
    {
        public string name;
        public Dictionary<string, string> fields = new Dictionary<string, string>();

        public TranslatedMessage(string messageName) 
        {
            name = messageName;
        }

        public void AddResult(string fieldName, string value)
        {
            fields[fieldName] = value;
        }

    }
}
