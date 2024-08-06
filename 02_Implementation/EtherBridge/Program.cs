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
            
            ConsoleMenu consoleMenu = new ConsoleMenu();


            // Create Server

            


            /*
            // Read message icd

            EtherXMLReader.XMLReader xmlReader = new XMLReader("icd_config.xml");
            
            // Generate internal message ICD
            List<XMLICDMessage> icdMessages =  xmlReader.GetICDMessages();
            Translator translator = new Translator(icdMessages);

            TranslatedMessage result = translator.TranslateMessage("10101101");



            // Create database

            DBManager dBManager = new DBManager();
            

            List<string> tables = new List<string>();
            foreach(XMLICDMessage icdMessage in icdMessages)
            {
                tables.Add(dBManager.CreateDatabaseTableString(icdMessage));
            }  
            
            dBManager.CreateTable(tables);

            dBManager.AddTranslatedMessage(result);

            // Test values in database

            JSONTester tester = new JSONTester(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dBManager);
            tester.DeserialiseTests();
            tester.RunTests();



            Console.WriteLine("\nProgram Ended"); */
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
        private EtherXMLReader.XMLReader _xmlReader;
        private JSONTester _tester;


        public ConsoleMenu()
        {
             Console.WriteLine("***Ether Bride Started***");        

            

            // Generate ICDMessages
            _xmlReader = new XMLReader("icd_config.xml", "network_config.xml");
            GenerateICDMessages();          


            // Database
            _dBManager = new DBManager();
            SetupDatabase();

            // JSON Tester
            _tester = new JSONTester(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _dBManager);
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
        
        private void GenerateICDMessages()
        {
            Console.WriteLine("Loading ICD");
            _icdMessages = _xmlReader.GetICDMessages();
            _translator.CreateMessageMap(_icdMessages);
        }        
        
        private void Forceshutdown(string reason)
        {
            Environment.FailFast(reason);

        }
    }
}