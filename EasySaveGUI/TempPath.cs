using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySaveGUI
{
    public class TempPath : INotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyRaised("Name");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(object propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((string)propertyname));
        }
    }
}
