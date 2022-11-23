using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave
{
    public interface ISaver
    {
        IList<ISave> Saves { get; }
        void LoadAllSaves(string folder);
        void WriteAllSaves(string folder, string extension);
        void ExecuteSaves(IList<ISave> ISaves, IEasySaveController controller);
    }
}
