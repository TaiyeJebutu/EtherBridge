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
    }
}
