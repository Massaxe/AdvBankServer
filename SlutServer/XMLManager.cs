using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.IO;

namespace SlutServer
{
    class XMLManager
    {

        string accountFilePath = @"accounts.xml";
        public XMLManager()
        {
            CreateXMLDocument();
        }
        public void CreateXMLDocument()
        {
            if (!File.Exists(accountFilePath))
            {
                using (XmlWriter xw = XmlWriter.Create(accountFilePath))
                {
                }
                Console.WriteLine("File does not exist");
            }
            else
            {
                Console.WriteLine("File does exist");
            }
        }
    }
}
