using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace SlutServer
{
    class SocketManager
    {

        // Incoming data from the client.  
        public static string data = null;

        public static void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 25565);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOM>") > -1)
                        {
                            break;
                        }
                    }

                    // Show the data on the console. 

                    DataHandler(handler, data);

                    // Echo the data back to the client
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
        private static void DataHandler(Socket handler, string content)
        {
            using (StreamWriter file = new StreamWriter(@"datalog.txt", true))
            {
                file.WriteLine(content);
            }

            if (content.IndexOf("reg") > -1)
            {
                RegisterPerson(content);
            }
            else if (content.IndexOf("login") > -1)
            {
                LoginPerson(content, handler);
            }
            else if (content.IndexOf("init_user_data") > -1)
            {
                Console.WriteLine("|DATA| init_user_data");
                Send(handler, "user_data," + PersonManager.GetSpecifcPersonData(GetPersonIdFromMessage(RemoveEOM(content))));
            }
        }
        public static string GetPersonIdFromMessage(string content)
        {
            string[] contentArray = content.Split(',');
            return contentArray[1];
        }
        public static string RemoveEOM(string content)
        {
            return content.Replace("<EOM>", "");
        }
        private static void LoginPerson(string content, Socket handler)
        {
            string[] contentArray = content.Split(',');
            if (contentArray[0] == "99" && contentArray[1] == "admin")
            {
                Send(handler, "login_admin");
            }
            else if (PersonManager.IsAccount(contentArray[0], contentArray[1]))
            {
                Console.WriteLine("|DATA|login_success ");
                Send(handler, $"login_success,{contentArray[0]}");
            }
            else if(!(PersonManager.IsAccount(contentArray[0], contentArray[1])))
            {
                Send(handler, "login_failed");
            }
        }
        private static void RegisterPerson(string content)
        {
            string[] contentArray = content.Split(',');
            PersonManager.AddPerson(int.Parse(contentArray[0]), contentArray[1]);
        }
        private static void Send(Socket handler, String data)
        {
            byte[] msg = Encoding.UTF8.GetBytes(data);

            handler.Send(msg);
        }
    }
}
