using EtherXMLReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EtherBridge
{
    public class Translator
    {
        private string _customFormats;
        private string _defaultFormat = "Default";
        private string _TwosComplimentFormat = "Twos Compliment";
        private Dictionary<int, XMLICDMessage> _messageMap = new Dictionary<int, XMLICDMessage>();

        public Translator()        
        {
            
            
        }

        public void CreateMessageMap(List<XMLICDMessage> messages)
        {
            
            foreach(XMLICDMessage message in messages)
            {
                _messageMap.Add(message.Header, message);
            }           
        }

        public TranslatedMessage TranslateMessage(string text)
        {
            

            // Assumption is that the header has size of 1 byte
            // Get the header value

            string header = text.Substring(0, 4);
            int headerValue = Convert.ToInt32(header, 2);

            // Match header value to message Map

            XMLICDMessage message = _messageMap[headerValue];
            TranslatedMessage translatedMessage = new TranslatedMessage(message.Name);



            // Translate each field in the message
            foreach (XMLFields field in message.Fields)
            {
                TranslateMessageField(translatedMessage, text, field);
            }


            


            return translatedMessage;
        }

        private void TranslateMessageField(TranslatedMessage translatedMessage ,string text, XMLFields field)
        {
            string fieldName = field._name;
            int startingbit = Int32.Parse(field._startingbit);
            int endingbit = Int32.Parse(field._endingbit);
            int resolution = Int32.Parse(field._resolution);
            string format = field._format;
            string binary = text.Substring(startingbit - 1, (endingbit - startingbit) + 1);
            //string binary = text.Substring(4, 2); 


            int fieldValue = int.MinValue;
            
            if (format == _defaultFormat)
            {
                fieldValue = Convert.ToInt32(binary, 2);
                fieldValue = fieldValue * resolution;
            }
            if(format == _TwosComplimentFormat)
            {
                fieldValue = TwosCompliment(binary);
                fieldValue = fieldValue * resolution;
            }
            



            translatedMessage.AddResult(fieldName, fieldValue.ToString());

        }

        private int TwosCompliment(string bits)
        {
            List<string> bitList = new List<string>();


            foreach (char c in bits)
            {
                bitList.Add(c.ToString());
            }

            List<int> multipliers = new List<int>();
            

            for(int i = 1; i <= bits.Length; i = i*2)
            {
                multipliers.Add(i);
            }
            

            int negative = multipliers[multipliers.Count - 1] * -1;
            
            for(int i = 1; i < bitList.Count; i++)
            {
                negative = negative + (Convert.ToInt32(bitList[i],2) * multipliers[i-1]);
            }

            return negative;
        }
    }
}
