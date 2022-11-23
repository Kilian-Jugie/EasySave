using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasySaveGUI
{
    /// <summary>
    /// Logique d'interaction pour FolderExplorerWindow.xaml
    /// </summary>
    public partial class FolderExplorerWindow : Window
    {
        private TreeBuilder _TreeBuilder;
        private TempPath _Path;
        public FolderExplorerWindow(TempPath path)
        {
            InitializeComponent();
            _Init(path);
        }

        private void _Init(TempPath path)
        {
            _Path = path;
            _TreeBuilder = new TreeBuilder(___CurentSelectedPath_);
            _TreeBuilder.LoadDirectories(___FolderExplorer_);
        }

        private void ___SelectPath__Click(object sender, RoutedEventArgs e)
        {
            _Path.Name = ((TreeViewItem)___FolderExplorer_.SelectedItem).Tag.ToString();
            this.Close();
        }
    }
}
