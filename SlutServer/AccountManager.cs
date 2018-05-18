using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlutServer
{
    class AccountManager
    {
        private const string accountPath = "accounts.xml";
        public static List<Account> GetAllAccounts(int personId)
        {
            return new List<Account>();
        }
        public static Account GetSpecificAccount(int personId, int accountId, bool allPersonAccounts)
        {
            return new SavingsAccount(123, 1555);
        }
        public static Account GetSpecificAccounts(int personId)
        {
            return new SavingsAccount(123, 1556);
        }

        public static bool OpenAccount(string personId, string accountType, string accountId, string initBalance)
        {
            try
            {
                XElement docTree = new XElement(
                    new XElement("account",
                                new XAttribute("id", accountId),
                                new XElement("account_id", accountId),
                                new XElement("account_type", accountType),
                                new XElement("balance", initBalance))
            );
                XDocument fileDoc = XDocument.Load(accountPath);

                XElement person = fileDoc.Descendants("person").Single(e => ((string)e.Attribute("id") == personId));
                person.Element("accounts").Add(docTree);
                fileDoc.Save(accountPath);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool IncreaseMoney(string personId, string amount)
        {
            Console.WriteLine(personId);
            Console.WriteLine(amount);
            try
            {
                XDocument fileDoc = XDocument.Load(accountPath);
                XElement person = fileDoc.Descendants("person").Single(e => ((string)e.Attribute("id") == personId));
                XElement accounts = person.Element("accounts");
                XElement account = accounts.Elements("account").Single(e => (string)e.Attribute("id") == "0");

                double startBalance = double.Parse(account.Element("balance").Value);

                double endBalance = startBalance + double.Parse(amount);

                account.Element("balance").Value = endBalance.ToString();

                fileDoc.Save(accountPath);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static XElement GetAccount(string accountId, string personId, XDocument fileDoc)
        {
            XElement person = fileDoc.Descendants("person").Single(e => ((string)e.Attribute("id") == personId));
            XElement accounts = person.Element("accounts");
            XElement account = accounts.Elements("account").Single(e => (string)e.Attribute("id") == accountId);
            return account;
        }

        public static bool ReduceMoney(string personId, string accountId, string amount)
        {
            try
            {
                /*XDocument fileDoc = XDocument.Load(accountPath);
                XElement person = fileDoc.Descendants("person").Single(e => ((string)e.Attribute("id") == personId));
                XElement accounts = person.Element("accounts");
                XElement account = accounts.Elements("account").Single(e => (string)e.Attribute("id") == accountId);*/


                XDocument fileDoc = XDocument.Load(accountPath);
                XElement account = GetAccount(accountId, personId, fileDoc);

                double startBalance = double.Parse(account.Element("balance").Value);

                

                double endBalance = startBalance - double.Parse(amount);

                if (endBalance < 1)
                {
                    return false;
                }

                account.Element("balance").Value = endBalance.ToString();

                fileDoc.Save(accountPath);

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveAccount(string personId, string accountId)
        {
            try
            {



                XDocument fileDoc = XDocument.Load(accountPath);
                XElement account = GetAccount(accountId, personId, fileDoc);
                account.Remove();
                fileDoc.Save(accountPath);
                return true;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
;