using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
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
        private DBManager _dbManager;
        private Dictionary<string, bool> _results = new Dictionary<string, bool>();

        public JSONTester(string dir, DBManager dBManager) 
        { 
            _dir = dir;
            _dbManager = dBManager;
            GetTestFiles();
        }

        private void GetTestFiles()
        {
            Console.WriteLine("Loading test files");
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
            Console.WriteLine($"Number of Tests Found : {Tests.Count}");
        }

        public void RunTests()
        {
            Console.WriteLine("RUNNING TESTS\n");

            using (var progress = new ProgressBar())
            {
                int testNumber = 1;
                foreach (JSONTest test in Tests)
                {
                    progress.Report((double) testNumber / Tests.Count);
                    bool result = false;

                    // Call the correct operatino based on the test type
                    switch (test.Test.Assertion.type)
                    {
                        case "Equality":
                            result = EqualityTest(test.Test.Assertion);
                            break;
                        case "Range":
                            result = RangeTest(test.Test.Assertion);
                            break;
                    }
                    _results.Add("(" + testNumber.ToString() + ")" + " " + test.Test.TestName, result);
                   
                    Thread.Sleep(30);
                    testNumber++;
                }
            }

            foreach (KeyValuePair<string, bool> entry in _results)
            {
                string result;

                if(entry.Value) { Console.ForegroundColor = ConsoleColor.DarkGreen; result = "PASS"; } else { Console.ForegroundColor = ConsoleColor.DarkRed; result = "FAIL"; }
                Console.WriteLine($"\t{entry.Key}... {result}");
            }
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public bool EqualityTest(Assertion test)
        {
            string tablename = test.messagename;
            string fieldname = test.fieldname;
            double fieldvalue = double.Parse(test.fieldvalue, System.Globalization.CultureInfo.InvariantCulture);

            string query = $"SELECT COUNT(*) FROM {tablename} where {fieldname}={fieldvalue}";



            bool result = _dbManager.Query(query);

            return result;
        }

        public bool RangeTest(Assertion test) 
        {
            string tablename = test.messagename;
            string fieldname = test.fieldname;  
            string greaterthan = test.greaterthan;
            string lessthan = test.lessthan;
            


            string query = $"SELECT COUNT(*) FROM {tablename} where {fieldname}<{lessthan} AND {fieldname}>{greaterthan} ";



            bool result = _dbManager.Query(query);

            return result;
        }
    }
}
