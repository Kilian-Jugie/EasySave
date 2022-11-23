using System;
using System.Collections.Generic;
using System.Text;
using EasySave;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace UnitTests {
    [TestClass]
    public class LangTest {
        public void CreateFrFile() {
            string filename = "fr-FR.lang";
            string content = "local.name=Français\n" +
                "test.hello=Bonjour\n" +
                "test.world=tout le monde\n" +
                "test.helloworld.format={0} {1}\n";
            File.WriteAllText(filename, content);
        }

        public void CreateEnFile() {
            string filename = "en-EN.lang";
            string content = "local.name=English\n" +
                "test.hello=Hello\n" +
                "test.world=world\n" +
                "test.onlyeng=english world\n" +
                "test.helloworld.format={0} {1}\n";
            File.WriteAllText(filename, content);
        }

        [TestMethod]
        public void TestLoadFromFile() {
            CreateFrFile();
            ILang lang_frFR = Lang.LoadFromFile("fr-FR.lang");

            Assert.IsTrue(lang_frFR["test.hello"] == "Bonjour", "Invalid translation");
            Assert.IsTrue(string.Format(lang_frFR["test.helloworld.format"], lang_frFR["test.hello"], lang_frFR["test.world"]) ==
                "Bonjour tout le monde", "Could not apply format on translated string");
        }

        [TestMethod]
        public void TestLoadFolder() {
            CreateFrFile();
            CreateEnFile();
            ILocalizer localizer = Localizer.Instance;
            localizer.Langs.Clear();
            localizer.LoadFolder("./");
            Assert.IsTrue(localizer.Langs.Count == 2, "One or more lang could not be loaded");
        }

        [TestMethod]
        public void TestLocalize() {
            CreateFrFile();
            ILocalizer localizer = Localizer.Instance;
            localizer.LoadLang("fr-FR.lang");
            try {
                localizer.CurrentLang = localizer.GetLang("fr-FR");
            }catch(Exception) {
                Assert.IsTrue(false, "Lang not correctly loaded");
            }
            Assert.IsTrue(localizer.Localize("test.hello") == "Bonjour", "Localize not working");
        }

        [TestMethod]
        public void TestDefaultLocalize() {
            CreateEnFile();
            CreateFrFile();
            ILocalizer localizer = Localizer.Instance;
            localizer.LoadFolder("./");
            try {
                localizer.CurrentLang = localizer.GetLang("fr-FR");
                localizer.DefaultLang = localizer.GetLang("en-EN");
            }
            catch (Exception) {
                Assert.IsTrue(false, "Lang not correctly loaded");
            }
            Assert.IsTrue(localizer.Localize("test.onlyeng") == "english world", "Default localize not working");
        }
    }
}
