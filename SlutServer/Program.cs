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
            /*Starta programmet med att initiera en instans av XMLManager vilket har skapandet av ett XML document i sin konstruktor.
             *Sedan att använda sig av StartListening() för att vänta på inkommande anslutningar från klienten.
             */
            XMLManager xM = new XMLManager();
            SocketManager.StartListening();
            




        }
    }
}
