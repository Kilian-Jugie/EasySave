using System;
using System.Collections.Generic;
using System.Text;
using EasySave;
using EasySaveViews;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests {
    [TestClass]
    public class ViewTest {
        void SetupLocalizer() {
            ILocalizer localizer = Localizer.Instance;
            localizer.LoadFolder(EasySaveController.EasySaveController.PATH_LANGS);
            localizer.CurrentLang = localizer.GetLang("en-EN");
        }

        [TestMethod]
        public void TestMainHelp() {
            SetupLocalizer();
            EasySaveConsole console = EasySaveConsole.Instance;
            Assert.IsTrue(console.Start(new string[0] { }) == EasySaveConsole.RETURN_CODE_HELP);
            Assert.IsTrue(console.Start(new string[1] {"-h"}) == EasySaveConsole.RETURN_CODE_HELP);
        }

        [TestMethod]
        public void TestCommandSaves() {
            //EasySaveConsole console = new EasySaveConsole(GetLocalizer());

        }
    }
}
