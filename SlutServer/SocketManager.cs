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

        static Socket handler;
        public static string data = null;

        public static void StartListening()
        {
            ´//Buffert
            byte[] bytes = new Byte[1024];

            // Simpelt sätt att få samma adress på både client och server
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 25565);

            // Skapa en TCP socket för att lyssna  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Börja lyssna
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Eftersom att handler väntar på en connection kommer programmet att blockeras fram till att någon ansluter. 
                    handler = listener.Accept();
                    data = null;

                    // Om den passerar föregående block kommer den att handera datan som skickades. 
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOM>") > -1)
                        {
                            break;
                        }
                    }



                    //Hantera datan som togs emot från klienten
                    DataHandler(handler, data);

                    // Stäng socket
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
            /*
                Hanteringen av datan sker genom en hel del if-satser som använder sig av IndexOf för att se vilken
                typ av data som skickades via TCP.
                Datan är separerad med komman [,] och börjar alltid med request type. Tex "reg" vid registrering
            */
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
                UpdateUser(content);
            }
            else if(content.IndexOf("new_account") > -1)
            {
                AddAccountParser(RemoveEOM(content));
            }
            else if(content.IndexOf("send_money") > -1)
            {
                SendMoney(RemoveEOM(content));
            }
            else if(content.IndexOf("remove_account") > -1)
            {
                //Används för debugging. Kan tas bort
                if (!RemoveAccount(RemoveEOM(content)))
                {
                    Console.WriteLine("Sometin wong");
                }
                else
                {
                    UpdateUser(content);
                }
            }
        }


        private static bool RemoveAccount(string content)
        {
            //Separerar meddelandet in i dess informations delar.
            string[] cA = content.Split(',');
            if(AccountManager.RemoveAccount(cA[1], cA[2]))
            {
                return true;
            }
            return false;

        }

        public static void UpdateUser(string content)
        {
            Send(handler, "user_data," + PersonManager.GetSpecifcPersonData(GetPersonIdFromMessage(RemoveEOM(content))));
        }
        public static void SendMoney(string content)
        {
            string[] cA = content.Split(',');
            if (AccountManager.ReduceMoney(cA[1], cA[2], cA[3]))
            {
                AccountManager.IncreaseMoney(cA[4], cA[3]);
            }
        }
        public static void AddAccountParser(string content)
        {
            Console.WriteLine("content: " + content);
            string[] contentArray = content.Split(',');
            foreach (string cA in contentArray)
            {
                Console.WriteLine(cA);
            }

            string personId = contentArray[1].ToString();
            string accountType = contentArray[3].ToString();
            string accountId = contentArray[4].ToString();
            string initBalance = contentArray[2].ToString();

            if(AccountManager.OpenAccount(personId, accountType, accountId, initBalance))
            {
                UpdateUser(content);
            }
            else
            {
                Console.WriteLine("Something went wrong account creation.");
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
