using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace EasySave
{
    public class SaveProcessData : INotifyPropertyChanged
    {
        public SaveProcessData()
        {

        }

        private int _NbClear;
        private string _SaveType;
        private string _PathFrom;
        private string _PathTo;
        private string _SaveName;
        private int _NbTask;

        public string SaveName
        {
            get { return _SaveName; }
            set
            {
                _SaveName = value;
                OnPropertyRaised("SaveName");
            }
        }

        public string PathTo
        {
            get { return _PathTo; }
            set
            {
                _PathTo = value;
                OnPropertyRaised("PathTo");
            }
        }

        public string PathFrom
        {
            get { return _PathFrom; }
            set
            {
                _PathFrom = value;
                OnPropertyRaised("PathFrom");
            }
        }

        public string SaveType
        {
            get { return _SaveType; }
            set
            {
                _SaveType = _CamelToPascal(value, ' ');
                OnPropertyRaised("SaveType");
            }
        }

        public int NbClear
        {
            get { return _NbClear; }
            set
            {
                _NbClear = value;
                OnPropertyRaised("NbClear");
            }
        }

        public void IncrementNbClear() {
            Interlocked.Increment(ref _NbClear);
            OnPropertyRaised("NbClear");
        }

        public int NbTask
        {
            get { return _NbTask; }
            set
            {
                _NbTask = value;
                OnPropertyRaised("NbTask");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(object propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((string)propertyname));
        }

        private static string _CamelToPascal(string str, char underscoreReplcacement)
        {
            string camelToPascal = "";
            bool doUpper = false;
            for (int x = 0; x < str.Length; ++x)
            {
                if (x == 0)
                    doUpper = true;
                else if (x > 0)
                {
                    if (str[x - 1] == '_')
                    {
                        doUpper = true;
                    }
                }
                if (str[x] == '_')
                {
                    camelToPascal += underscoreReplcacement;
                }
                else
                {
                    if (doUpper)
                    {
                        doUpper = false;
                        camelToPascal += str[x].ToString().ToUpper();
                    }
                    else
                        camelToPascal += str[x].ToString().ToLower();
                }

            }
            return camelToPascal;
        }
    }
}
