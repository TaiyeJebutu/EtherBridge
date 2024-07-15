using System;
using EtherXMLReader;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            EtherXMLReader.XMLReader xmlReader = new XMLReader("icd_config.xml");
            
            xmlReader.GetICDMessages();
        }
    }
}