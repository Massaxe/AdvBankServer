using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SlutServer
{

    

    class Program
    {
        static void Main(string[] args)
        {
            XMLManager xM = new XMLManager();
            SocketManager.StartListening();
            Console.WriteLine("Hello World");
            




        }
    }
}
