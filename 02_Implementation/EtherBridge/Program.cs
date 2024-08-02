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
            _xmlReader = new XMLReader("icd_config.xml");
            GenerateICDMessages();

            // Database
            _dBManager = new DBManager();
            SetupDatabase();

            // JSON Tester
            _tester = new JSONTester(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _dBManager);
            _tester.DeserialiseTests();

            // Server infomation

            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[0];
            server = new Server(ipAddr, 11111, _translator, _dBManager);
            _serverstarted = false;

            // Threads
            _menuThread = new Thread(new ThreadStart(DisplayOptions));           

             _serverThread = new Thread(new ThreadStart(server.Create));

            _menuThread.Start();
        }


        public void LoadServerConfig()
        {
            Console.WriteLine("Loading NetworkConfig");
        }  
        
        private void SetupDatabase()
        {
            List<string> tables = new List<string>();
            foreach (XMLICDMessage icdMessage in _icdMessages)
            {
                tables.Add(_dBManager.CreateDatabaseTableString(icdMessage));
            }

            _dBManager.CreateTable(tables);
        }
        
        private void GenerateICDMessages()
        {
            _icdMessages = _xmlReader.GetICDMessages();
            _translator.CreateMessageMap(_icdMessages);
        }

        public void CloseServer()
        {
            server.CloseServer();
        }

        public void StartServer()
        {
            _serverThread.Start();  
        }

        public void DisplayOptions()
        {
            while (!shutdown)
            {
                if (_serverstarted)
                {
                    Console.WriteLine($"\n [1] - Close Sever");
                }
                else { Console.WriteLine($"\n [1] - Start Sever"); }

                Console.WriteLine($" [2] - Start Tests");
                Console.WriteLine($" [3] - Shutdown");

                Console.Write("\n:");
                string input = Console.ReadLine();
                Options(input);
            }
            
            Console.WriteLine("Shutting down ...");
        }

        public void Options(string input)
        {
            switch (input)
            {
                case "1": 
                    if (_serverstarted) 
                    { 
                        CloseServer();
                        _serverstarted = false;
                        DisplayOptions();
                    } else 
                    {
                        StartServer();
                        _serverstarted = true;
                        DisplayOptions();
                    }
                    break;
                case "2":
                    _tester.RunTests();
                    DisplayOptions();                   
                    
                    break;
                case "3":
                    shutdown = true;
                    if (_serverstarted) { CloseServer();}
                    DisplayOptions();
                    break;
            }
        }
    }
}