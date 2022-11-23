using EasySave;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace EasySaveGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static RoutedEventHandler _EditSaveEvent;
        private static RoutedEventHandler _CreateSaveEvent;
        public static TempPath PathFrom { get; }
        public static TempPath PathTo { get; }
        private IEasySaveController controller;
        static MainWindow()
        {
            PathFrom = new TempPath();
            PathTo = new TempPath();
        }
        public MainWindow(IEasySaveController control)
        {
            controller = control;
            InitializeComponent();
            _Init();
            _SetLabels();
        }

        private void _SetLabels() {
            ___GridView_.Columns[0].Header = Localizer.Instance.Localize("gui.project.name");
            ___GridView_.Columns[1].Header = Localizer.Instance.Localize("gui.project.savetype");
            ___GridView_.Columns[2].Header = Localizer.Instance.Localize("gui.project.source");
            ___GridView_.Columns[3].Header = Localizer.Instance.Localize("gui.project.destination");
            ___LoadSavesProjectButton_.Content = Localizer.Instance.Localize("gui.button.load");
            ___SelectAllSaveProjectButton_.Content = Localizer.Instance.Localize("gui.button.selectall");
            ___ExecuteSavesProjectButton_.Content = Localizer.Instance.Localize("gui.button.execute");
            ___EditOrCreateSaveProjectButton_.Content = Localizer.Instance.Localize("gui.button.edit");
            ___CurrentSaveTextBlock_.Text = Localizer.Instance.Localize("gui.label.currentproject");
            ___SourcePath1_.Text = Localizer.Instance.Localize("gui.label.source");
            ___DestinationPath1_.Text = Localizer.Instance.Localize("gui.label.destination");
            ___SaveType1_.Text = Localizer.Instance.Localize("gui.label.savetype");
            ___ProcessProgress_.Text = Localizer.Instance.Localize("gui.label.processprogress");
            ___ProjectSaveName_.Text = Localizer.Instance.Localize("gui.label.projectsavename");
            ___SourcePath_.Text = Localizer.Instance.Localize("gui.label.source");
            ___DestinationPath_.Text = Localizer.Instance.Localize("gui.label.destination");
            ___SaveType2_.Text = Localizer.Instance.Localize("gui.label.savetype");
            ___ExecuteEditOrCreateSaveProject_.Content = Localizer.Instance.Localize("gui.label.create");
        }

        private void _Init()
        {
            _EditSaveEvent = new RoutedEventHandler(_EditSave);
            _CreateSaveEvent = new RoutedEventHandler(_CreateSave);
            Localizer.Instance.Langs.Foreach(l => {
                MenuItem litem = new MenuItem {
                    Header = l.Name
                };
                litem.Click += _SetLang;
                ___LangMenuItem_.Items.Add(litem);
                });

            ___CurentProjectSaveGrid_.DataContext = SaveType.processData;
            ___SaveProjectPathFrom_.DataContext = PathFrom;
            ___SaveProjectPathTo_.DataContext = PathTo;
            ___EditOrCreateSaveProjectButton_.Content = _EDIT_SAVE;
            ___ExecuteEditOrCreateSaveProject_.Content = _CREATE_THE_SAVE;
            ___ProjectSaveListView_.ItemsSource = null;
            foreach (ISaveType type in SaveType.SaveTypes)
            {
                ___SaveProjectSaveType_.Items.Add(type.Name);
            }
            ___ExecuteEditOrCreateSaveProject_.Click += new RoutedEventHandler(_CreateSave);
        }

        private void ___LoadSavesProjectButton__Click(object sender, RoutedEventArgs e)
        {
            ___ProjectSaveListView_.ItemsSource = controller.ProjectSaver.Saves;
        }

        private void ___ExecuteSavesProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (___ProjectSaveListView_.Items.Count > 0)
            {
                if (___ProjectSaveListView_.SelectedItems.Count > 0)
                {
                    List<string> savesSelected = new List<string>();
                    foreach (ISave save in ___ProjectSaveListView_.SelectedItems)
                    {
                        savesSelected.Add(save.Name);
                    }
                    System.Threading.Thread thread = new System.Threading.Thread(controller.ExecuteSaveProjects);
                    thread.Start(savesSelected);
                }
                else if (___ProjectSaveListView_.SelectedItems.Count == 0)
                {
                    MessageBox.Show("You must select some Project!!", "WARNING");
                }
            }
            else
            {
                MessageBox.Show("You must load or create some save project!!", "WARNING");
            }
        }
        private void ___SelectAllSavesButton_Click(object sender, RoutedEventArgs e)
        {
            if (___ProjectSaveListView_.Items.Count > 0)
            {
                ___ProjectSaveListView_.SelectAll();
            }
            else
            {
                MessageBox.Show("You must load or create some save project!!", "WARNING");
            }
        }

        private void ___EditOrCreateSaveProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)___EditOrCreateSaveProjectButton_.Content == _EDIT_SAVE)
            {
                if (___ProjectSaveListView_.SelectedItems.Count == 0)
                {
                    MessageBox.Show("You must Load or Create some Save Project!!", "WARNING");
                }
                else if (___ProjectSaveListView_.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Please select only the project you would like to edit!!", "WARNING");
                }
                else
                {
                    ISave save = ((ISave)___ProjectSaveListView_.SelectedItem);
                    ___SaveProjectLabel_.Text = save.Name;
                    PathFrom.Name = save.PathFrom;
                    PathTo.Name = save.PathTo;
                    ___SaveProjectSaveType_.SelectedItem = save.Type;
                    ___ExecuteEditOrCreateSaveProject_.Content = _EDIT_THE_SAVE;
                    ___ExecuteEditOrCreateSaveProject_.Click -= _CreateSaveEvent;
                    ___ExecuteEditOrCreateSaveProject_.Click += _EditSaveEvent;
                    ___EditOrCreateSaveProjectButton_.Content = _CREATE_SAVE;
                }
            }
            else if ((string)___EditOrCreateSaveProjectButton_.Content == _CREATE_SAVE)
            {
                _ClearFormEditOrCreate();
                ___ExecuteEditOrCreateSaveProject_.Content = _CREATE_THE_SAVE;
                ___ExecuteEditOrCreateSaveProject_.Click -= _EditSaveEvent;
                ___ExecuteEditOrCreateSaveProject_.Click += _CreateSaveEvent;
                ___EditOrCreateSaveProjectButton_.Content = _EDIT_SAVE;
            }
        }

        private void _EditSave(object sender, RoutedEventArgs e)
        {
            if (___ProjectSaveListView_.SelectedItem != null)
            {
                ISave save = ((ISave)___ProjectSaveListView_.SelectedItem);
                ISave saveUpdated = new Save()
                {
                    Name = ___SaveProjectLabel_.Text,
                    PathFrom = PathFrom.Name,
                    PathTo = PathTo.Name,
                    Type = ___SaveProjectSaveType_.Text
                };
                controller.EditSaveProject(save.Name, saveUpdated);
                _ClearFormEditOrCreate();
                ___LoadSavesProjectButton__Click(sender, e);
            }
            else
                MessageBox.Show("Please reselect the project you would like to edit!!", "WARNING");
        }
        private void _CreateSave(object sender, RoutedEventArgs e)
        {
            controller.CreateSaveProject(new Save()
            {
                Name = ___SaveProjectLabel_.Text,
                PathFrom = PathFrom.Name,
                PathTo = PathTo.Name,
                Type = ___SaveProjectSaveType_.SelectedItem.ToString()
            });
            ___LoadSavesProjectButton__Click(sender, e);
            _ClearFormEditOrCreate();
        }

        private void _ClearFormEditOrCreate()
        {
            ___SaveProjectLabel_.Text = "";
            PathFrom.Name = "";
            PathTo.Name = "";
            ___SaveProjectSaveType_.SelectedItem = null;
        }

        private void ___SearchForSourceFolder__Click(object sender, RoutedEventArgs e)
        {
            OpenFolderExplorerWindows(PathFrom);
        }

        private void ___SearchForDestinationFolder__Click(object sender, RoutedEventArgs e)
        {
            OpenFolderExplorerWindows(PathTo);
        }

        private void OpenFolderExplorerWindows(TempPath path)
        {
            Thread t = new Thread(StartFolderExplorerSTA);
            t.SetApartmentState(ApartmentState.STA);
            t.Start(path);
        }
        private void StartFolderExplorerSTA(object obj)
        {
            FolderExplorerWindow folderExplorer = new FolderExplorerWindow((TempPath)obj);
            folderExplorer.ShowDialog();
        }
        /*
         * these methode will be call when we click occur on the correspondng item in the menu
         */
        private void _SetLang(object sender, RoutedEventArgs e) {
            Localizer.Instance.CurrentLang = Localizer.Instance.Langs.First(l => l.Name == (string)((MenuItem)sender).Header);
            controller.Parameters["lang"] = Localizer.Instance.CurrentLang.Tag;
            _SetLabels();
        }

        private void ___LangMenuItem__Click(object sender, RoutedEventArgs e) {
            
        }

        private void ___ParamMenuItem__Click(object sender, RoutedEventArgs e)
        {

        }

        private void ___ConfigMenuItem__Click(object sender, RoutedEventArgs e)
        {

        }

        private void ___WriteSavesMenuItem__Click(object sender, RoutedEventArgs e)
        {
            controller.ProjectSaver.WriteAllSaves(IEasySaveController.PATH_PROJECTS, Saver.DEFAULT_EXTENSION);
        }

        private void ___HelpMenuItem__Click(object sender, RoutedEventArgs e)
        {

        }

        private void ___BusinessSoftwareMenuItem__Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(StartBusinessSoftwareSTA);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void StartBusinessSoftwareSTA()
        {
            BusinessSoftwareExplorer businessSoftwareExplorer = new BusinessSoftwareExplorer();
            businessSoftwareExplorer.ShowDialog();
        }
        private readonly string _CREATE_SAVE = "Create save";
        private readonly string _EDIT_SAVE = "Edit save";
        private readonly string _CREATE_THE_SAVE = "Create the save";
        private readonly string _EDIT_THE_SAVE = "Edit the save";
    }
}

