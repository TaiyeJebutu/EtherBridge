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

        public Server(IPAddress address, int port) 
        
        {            
            _address = address;
            _ep = new IPEndPoint(address, port);
            _serverStarted = false;
        }

        public void Create()
        {
            Socket listener =  new Socket(_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _serverStarted = true;

            try
            {
                listener.Bind(_ep);
                listener.Listen(10);

                while (_serverStarted)
                {
                    Console.WriteLine("Waiting for client connection ....\n");

                    Socket clientSocket = listener.Accept();

                    // data buffer
                    byte[] bytes = new byte[1024];
                    string data = null;

                    while(true || _serverStarted)
                    {
                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (data.IndexOf("<EOF>") > 1)
                            break;
                    }

                    Console.WriteLine($"Message Received -> {data}");
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void CloseServer()
        {
            _serverStarted = true;
        }

    }
}
