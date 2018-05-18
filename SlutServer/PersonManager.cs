using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlutServer
{
    class PersonManager
    {
        private const string accountPath = "accounts.xml";
        //EJ KLAR
        public static List<Person> GetAllPeople()
        {
            return new List<Person>();
        }
        //EJ KLAR
        public static Person GetSpecificPerson(int personId)
        {
            return new Person("gabriel", 223);
        }
        //Lägger till personen i XML documentet
        public static string AddPerson(int personId, string name)
        {
            name = name.ToLower();

            XElement docTree = new XElement(
                    new XElement("person",
                        new XAttribute("id", personId),
                        new XElement("name", name),
                        new XElement("personId", personId),
                        new XElement("accounts",
                            new XElement("account",
                                new XAttribute("id", 0),
                                new XElement("account_id", 0),
                                new XElement("account_type", 0),
                                new XElement("balance", 0))
                        )
                    )
            );
            XDocument fileDoc = XDocument.Load(accountPath);
            fileDoc.Element("people").Add(docTree);
            fileDoc.Save(accountPath);
            return "Person Added";
        }
        //Hämta specifik person från XML och omvandla till formaterad inline string. [,] dividerar olika typer av person information och [-] dividerar konto information.
        public static string GetSpecifcPersonData(string personId)
        {
            string inlineData = "";
            XDocument fileDoc = XDocument.Load(accountPath);
            XElement person = fileDoc.Descendants("person").Single(e => ((string)e.Attribute("id") == personId));

            inlineData += person.Element("name").Value;
            inlineData += "," + person.Element("personId").Value;

            IEnumerable<XElement> accounts = person.Element("accounts").Elements("account");
            foreach (XElement xE in accounts)
            {
                inlineData += "," + xE.Element("account_id").Value + "-" + xE.Element("account_type").Value + "-" + xE.Element("balance").Value;
            }

            return inlineData;

        }
        //Ser om konto existerar.
        public static bool IsAccount(string personId, string name)
        {
            XDocument fileDoc = XDocument.Load(accountPath);

            XElement person;
            try
            {
                person = fileDoc.Descendants("person").First(e => ((string)e.Attribute("id")) == personId);
            }
            catch
            {
                person = null;
            }

            if (person != null)
                return true;
            else
                return false;
        }
        //EJ KLAR
        public static string RemovePerson(int personId)
        {
            return "failed";
        }
    }
}
