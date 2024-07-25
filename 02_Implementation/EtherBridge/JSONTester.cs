using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EtherBridge
{
    public class JSONTester
    {
        private List<string> _testFileLocations = new List<string>();
        private string _dir;
        private Regex _regex = new Regex("test.*json$");
        public List<JSONTest> Tests  = new List<JSONTest>();

        public JSONTester(string dir) 
        { 
            _dir = dir;
            GetTestFiles();
        }

        private void GetTestFiles()
        {
            var matches = Directory.EnumerateFiles(_dir).Where(f => _regex.IsMatch(f));
            if(matches != null)
            {
                foreach(string match in matches)
                {
                    _testFileLocations.Add(match);
                }
            }
        }

        public void DeserialiseTests()
        {
            foreach (string file in _testFileLocations)
            {
                List<JSONTest> tests = JsonConvert.DeserializeObject<List<JSONTest>>(File.ReadAllText(file));
                foreach (JSONTest test in tests)
                {
                    Tests.Add(test);
                }
            }
        }

        public void RunTests()
        {

        }
    }
}
