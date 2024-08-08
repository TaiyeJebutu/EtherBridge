using System;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Xml;
using EtherBridge;
using EtherXMLReader;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            ConsoleMenu consoleMenu = new ConsoleMenu(args);

            
        }  

    }

    public class ConsoleMenu
    {

        public IPHostEntry ipHost;
        public IPAddress ipAddr;
        public Server server;
        public bool shutdown;
        
        private bool _serverstarted;
        private Thread _menuThread;
        private Thread _serverThread;

        private DBManager _dBManager;
        private Translator _translator = new Translator();
        private List<XMLICDMessage> _icdMessages;
        private List<Formats> _icdCustomFormats;
        private EtherXMLReader.XMLReader _xmlReader;
        private JSONTester _tester;


        private string _icd;
        private string _testFileLocation;
        private string _networkConfig;
        

        public ConsoleMenu(string[] args)
        {
            Console.WriteLine("***Ether Bride Started***");        
            
            _icd = args[0];
            _testFileLocation = args[1];
            _networkConfig = args[2];

            // Generate ICDMessages
           
            _xmlReader = new XMLReader(_icd, _networkConfig);
            LoadICD();          


            // Database
            _dBManager = new DBManager();
            SetupDatabase();

            // JSON Tester
            _tester = new JSONTester(_testFileLocation, _dBManager, _translator);
            _tester.DeserialiseTests();

            // Server infomation
           
            ServerConfig serverConfig = _xmlReader.GetServerConfig();

            if (serverConfig.Port == null | serverConfig.ServerAddress == null) { Forceshutdown("Error occured while trying to read IP address or Port"); }
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[0];
            server = new Server(serverConfig, _translator, _dBManager);

            server.Create();
            _tester.RunTests();
            
        }


        public void LoadServerConfig()
        {
            Console.WriteLine("Loading NetworkConfig");
        }  
        
        private void SetupDatabase()
        {
            Console.WriteLine("Creating database tables");
            List<string> tables = new List<string>();
            foreach (XMLICDMessage icdMessage in _icdMessages)
            {
                tables.Add(_dBManager.CreateDatabaseTableString(icdMessage));
            }

            _dBManager.CreateTable(tables);
        }
        
        private void LoadICD()
        {
            Console.WriteLine("Loading ICD");
            _icdMessages = _xmlReader.GetICDMessages();
            _icdCustomFormats = _xmlReader.GetCustomFormats();
            _translator.CreateMessageMap(_icdMessages);
            _translator.CreateCustomFormatMap(_icdCustomFormats);
        }        
        
        private void Forceshutdown(string reason)
        {
            Environment.FailFast(reason);

        }
    }
}