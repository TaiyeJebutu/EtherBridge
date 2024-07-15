using System;
using EtherBridge;
using EtherXMLReader;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            EtherXMLReader.XMLReader xmlReader = new XMLReader("icd_config.xml");
            
            List<XMLICDMessage> icdMessages =  xmlReader.GetICDMessages();
            Translator translator = new Translator(icdMessages);

            TranslatedMessage result = translator.TranslateMessage("10101101");

            Console.WriteLine("Hello World!");
        }
    }
}