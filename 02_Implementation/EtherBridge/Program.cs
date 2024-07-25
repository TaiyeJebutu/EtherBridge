using System;
using System.Data.Common;
using System.Reflection;
using EtherBridge;
using EtherXMLReader;
using Microsoft.Data.Sqlite;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Read message icd
            Console.WriteLine("Hello World!");
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

            JSONTester tester = new JSONTester(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            tester.DeserialiseTests();
            tester.RunTests();



            Console.WriteLine("Hello World!");
        }


        
    }
}