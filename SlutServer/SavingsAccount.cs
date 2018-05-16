using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutServer
{
    class SavingsAccount:Account
    {
        static float interest = 0.1f;
        public SavingsAccount(int ownerId, double balance) : base(ownerId, balance)
        { }
    }
}
