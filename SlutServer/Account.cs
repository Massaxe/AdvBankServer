using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutServer
{
    class Account
    {
        int ownerId;
        double balance;

        public Account(int ownerId, double balance)
        {
            this.ownerId = ownerId;
            this.balance = balance;
        }
    }
}
