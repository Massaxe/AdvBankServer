using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutServer
{
    abstract class Account
    {
        int ownerId;
        double balance;
        float interest;

        public Account(int ownerId, double balance)
        {
            this.ownerId = ownerId;
            this.balance = balance;
        }
    }
}
