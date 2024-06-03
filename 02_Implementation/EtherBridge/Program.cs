using System;
using EtherXMLReader;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            EtherXMLReader.XMLReader xmlReader = new XMLReader("C:\\Users\\taiye\\source\\repos\\TaiyeJebutu\\EtherBridge\\03_Test\\Configs\\example_network_config.xml");
            
            var result = xmlReader.GetClients();
        }
    }
}