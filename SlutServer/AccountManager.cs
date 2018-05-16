using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutServer
{
    class AccountManager
    {
        string accountFilePath = @"accounts.xml";
        public static List<Account> GetAllAccounts(int personId)
        {
            return new List<Account>();
        }
        public static Account GetSpecificAccounts(int personId, int accountId, bool allPersonAccounts)
        {
            return new SavingsAccount(123, 1555);
        }
        public static Account GetSpecificAccounts(int personId)
        {
            return new SavingsAccount(123, 1556);
        }

        public static string OpenAccount(int personId, int accountType)
        {
            return "failed";
        }
        public static string RemoveAccount(int personId, int accountId)
        {
            return "Failed";
        }
    }
}
;