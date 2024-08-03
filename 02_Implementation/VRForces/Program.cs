using System.Net.Sockets;
using System.Net;
using System.Text;

namespace VRForces
{
    internal class Program
    {


        static List<string> messsages = new List<string>()
        {
            "10101101",
            "10100101",
            "10100011",
            "10101111",
            "10100000",
        };
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        static void ExecuteClient()
        {

            try
            {

                // Establish the remote endpoint 
                // for the socket. This example 
                // uses port 11111 on the local 
                // computer.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

                // Creation TCP/IP Socket using 
                // Socket Class Constructor
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // Connect Socket to the remote 
                    // endpoint using method Connect()
                    sender.Connect(localEndPoint);

                    // We print EndPoint information 
                    // that we are connected
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Creation of message that
                    // we will send to Server

                    foreach(string message in Program.messsages)
                    {
                        byte[] messageSent = Encoding.ASCII.GetBytes(message + "<EOF>");
                        int byteSent = sender.Send(messageSent);
                        Thread.Sleep(3);
                    }
                    


                    // Close Socket using 
                    // the method Close()
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
    }
}