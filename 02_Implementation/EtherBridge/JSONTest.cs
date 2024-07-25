using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherBridge
{
    public class JSONTest
    {
        [JsonProperty("Test")]
        public Test Test { get; set; }
    }

    public class Test
    {
        [JsonProperty("TestName")]
        public string TestName { get; set; }
        [JsonProperty("Assertion")]
        public Assertion Assertion { get; set; }
    }

    public class Assertion
    {
        [JsonProperty] 
        public string type { get; set;}

        [JsonProperty]
        public string messagename { get; set; }
        [JsonProperty]
        public string fieldname { get; set; }
        [JsonProperty]
        public string fieldvalue { get; set; }
        [JsonProperty]
        public string lessthan { get; set; }
        [JsonProperty]
        public string greaterthan { get; set; }
    }
}
