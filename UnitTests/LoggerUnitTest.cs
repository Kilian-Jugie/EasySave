using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Schema;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasySave;

namespace UnitTests {
    [TestClass]
    public class LoggerUnitTest {
        [TestMethod]
        public void AddLogTest() {
            ILogger log = new Logger("logUnitTest.xml");
            string errorFunction = "error";
            object[] objects = new object[2];
            objects[0] = "test1";
            objects[1] = "test2";
            log.AddLog(objects, errorFunction);

            XmlDocument doc = new XmlDocument();
            doc.Load("logUnitTest.xml");
            XmlNodeList xmlNodeList = doc.SelectNodes("/log");

            foreach (XmlNode xmlNode in xmlNodeList) {
                string message = xmlNode["msg"].InnerText;
                for (int i = 0; i < objects.Length; i++) {
                    if (message == objects[i]) {
                        Assert.AreEqual(objects[i], message);
                    }
                }
            }
            File.Delete("logUnitTest.xml");
        }
    }
}