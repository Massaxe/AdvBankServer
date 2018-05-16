using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutServer
{
    class PersonManager
    {
        private static string accountFilePath = @"accounts.xml";
        public static List<Person> GetAllPeople()
        {
            return new List<Person>();
        }
        public static Person GetSpecificPerson(int personId)
        {
            return new Person("gabriel", 223);
        }
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
            XDocument fileDoc = XDocument.Load(accountFilePath);
            fileDoc.Element("people").Add(docTree);
            fileDoc.Save(accountFilePath);
            return "Person Added";
        }
        public static bool IsAccount(int personId, string name)
        {
            XDocument fileDoc = XDocument.Load(accountFilePath);
            /*IEnumerable<XElement> users = (from el in fileDoc.Root.Elements("person")
                                           where (string)el.Attribute("id") == personId.ToString()
                                           select el);*/
            XElement person = fileDoc.Descendants("person").
                            SingleOrDefault(e => ((string)e.Attribute("id")) == personId.ToString());
            if (person != null)
                return true;
            else
                return false;
        }
        public static string RemovePerson(int personId)
        {
            return "failed";
        }
    }
}
