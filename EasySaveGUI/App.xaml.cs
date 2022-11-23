using EasySave;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EasySaveGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IView
    {
        private static readonly Lazy<App> _instance = new Lazy<App>(() => new App(), true);
        public static App Instance { get => _instance.Value; }

        public IList<ISave> DisplayedSaveProjects { get; set; }
        public ExecuteSavesCallback ExecuteSaves { get; set; }
        private MainWindow _mainWindow { get; set; }
        public static IEasySaveController ParentController { get; set; }

        private App() : base()
        {

        }

        [STAThread]
        private void STAStart()
        {
            _mainWindow = new MainWindow(ParentController);
            _mainWindow.ShowDialog();
            ParentController.Terminate();
        }

        public void Error(string description)
        {

        }

        public void FatalError(string description)
        {

        }


        public int Start(string[] args)
        {
            Thread t = new Thread(STAStart);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            return 0;
        }

        int IView.Exit()
        {
            _mainWindow.Close();
            return 0;
        }
    }
}
