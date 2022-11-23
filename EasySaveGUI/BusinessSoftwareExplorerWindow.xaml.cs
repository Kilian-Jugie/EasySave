using EasySave;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour BusinessSoftwareExplorer.xaml
    /// </summary>
    public partial class BusinessSoftwareExplorer : Window
    {
        public BusinessSoftwareExplorer()
        {
            InitializeComponent();
            _Init();

        }

        private void _Init()
        {
            ___PrcocessListView_.ItemsSource = Utils.BusinessSoftware;
        }

        private void ___AddProcessButton__Click(object sender, RoutedEventArgs e)
        {
            if (___ProcessNameTextBox_.Text.Length!=0)
            {
                Utils.BusinessSoftware.Add(___ProcessNameTextBox_.Text);
                ___ProcessNameTextBox_.Text = null;
                ___PrcocessListView_.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please enter a process name before adding it!!","WARNING");
            }
        }

        private void ___DeleteProcessButton__Click(object sender, RoutedEventArgs e)
        {
            if (___PrcocessListView_.SelectedItem!=null)
            {
                Utils.BusinessSoftware.Remove((string)___PrcocessListView_.SelectedItem);
                ___PrcocessListView_.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a process before deleting it!!", "WARNING");
            }
        }
    }
}
