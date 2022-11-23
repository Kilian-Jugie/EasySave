using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace EasySaveGUI
{
    class DummyTreeViewItem : TreeViewItem
    {
        public DummyTreeViewItem() : base()
        {
            base.Header = "Dummy";
            base.Tag = "Dummy";
        }
    }
}
