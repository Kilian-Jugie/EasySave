using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasySaveGUI
{
    class TreeBuilder
    {
        TreeView treeView;
        TextBlock textBox;
        public TreeBuilder(TextBlock box)
        {
            textBox = box;
        }

        public void LoadDirectories(TreeView tree)
        {
            treeView = tree;
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                treeView.Items.Add(_GetItem(drive));
            }
        }

        private TreeViewItem _GetItem(DriveInfo drive)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = drive.Name,
                DataContext = drive,
                Tag = drive
            };
            _AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            item.Selected += new RoutedEventHandler(item_Selected);
            return item;
        }

        private TreeViewItem _GetItem(DirectoryInfo directory)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = directory.Name,
                DataContext = directory,
                Tag = directory
            };
            _AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            item.Selected += new RoutedEventHandler(item_Selected);
            return item;
        }

        private TreeViewItem _GetItem(FileInfo file)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = file.Name,
                DataContext = file,
                Tag = file
            };
            item.Selected += new RoutedEventHandler(item_Selected);
            return item;
        }

        private void _AddDummy(TreeViewItem item)
        {
            item.Items.Add(new DummyTreeViewItem());
        }

        private bool _HasDummy(TreeViewItem item)
        {
            return item.HasItems && (item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem).Count > 0);
        }

        private void _RemoveDummy(TreeViewItem item)
        {
            List<TreeViewItem> dummies = item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem);
            foreach (TreeViewItem dummy in dummies)
            {
                item.Items.Remove(dummy);
            }
        }

        private void _ExploreDirectories(TreeViewItem item)
        {
            DirectoryInfo directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                bool isHidden = (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                bool isSystem = (directory.Attributes & FileAttributes.System) == FileAttributes.System;
                if (!isHidden && !isSystem)
                {
                    item.Items.Add(_GetItem(directory));
                }
            }
        }

        private void _ExploreFiles(TreeViewItem item)
        {
            DirectoryInfo directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                bool isHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                bool isSystem = (file.Attributes & FileAttributes.System) == FileAttributes.System;
                if (!isHidden && !isSystem)
                {
                    item.Items.Add(_GetItem(file));
                }
            }
        }
        void item_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (_HasDummy(item))
            {
                treeView.Cursor = Cursors.Wait;
                _RemoveDummy(item);
                _ExploreDirectories(item);
                _ExploreFiles(item);
                treeView.Cursor = Cursors.Arrow;
            }
        }

        void item_Selected(object sender, RoutedEventArgs e)
        {
            textBox.Text = _PATH_TO_SAVE + ((TreeViewItem)treeView.SelectedItem).Tag.ToString();
        }

        private string _PATH_TO_SAVE = "Full Path to Save : ";
    }
}
