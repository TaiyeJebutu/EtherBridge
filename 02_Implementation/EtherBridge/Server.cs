using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EtherBridge
{
    public class Server
    {
        private IPHostEntry _entry =  Dns.GetHostEntry(Dns.GetHostName());
        private IPAddress _address;
        private IPEndPoint _ep;
        private bool _serverStarted;
        Socket _clientSocket;

        Socket _listener;       
        private Translator _translator;
        private DBManager _dbManager;

        public Server(IPAddress address, int port, Translator translator, DBManager dBManager) 
        
        {            
            _address = address;
            _ep = new IPEndPoint(address, port);
            _serverStarted = false;
            Console.WriteLine("Creating Server-Client Connection");
            _listener = new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _translator = translator;
            _dbManager = dBManager;
        }

        public void Create()
        {
            

            _serverStarted = true;

            try
            {
                _listener.Bind(_ep);
                _listener.Listen(10);

                Console.WriteLine("Waiting for client connection ....\n");
                _clientSocket = _listener.Accept();
                Console.WriteLine("Receiving Messaages");
                while (isConnected(_clientSocket))
                {                    
                    Listen(_clientSocket);
                }
                CloseServer();
                
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Server thread closing");
        }

        private void Listen(Socket clientSocket)
        {
            // data buffer
            byte[] bytes = new byte[1024];
            string data = null;
            

            while (isConnected(clientSocket))
            {
                int numByte = clientSocket.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes, 0, numByte);

                if (data.IndexOf("<EOF>") > 1)
                    break;
            }

            if (isConnected(clientSocket)) { Console.WriteLine($"Message Received -> {data}"); TranslateMessage(data); }
            
        }

        public void CloseServer()
        {
            Console.WriteLine("Attempting to close the server ...");            

            if(_clientSocket != null)
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();
            }

            if(_listener != null)
            {
                _listener.Close();
            }
            
        }

        private void TranslateMessage(string message)
        {
            string data = message.Substring(0, message.Length-5);
            TranslatedMessage translatedMessage = _translator.TranslateMessage(data); 
            _dbManager.AddTranslatedMessage(translatedMessage);
        }


        private bool isConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }
    }
}
