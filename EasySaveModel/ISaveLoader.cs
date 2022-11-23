using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave {
    public interface ISaveLoader {
        string Extension { get; }
        
        ISave Load(string filename);
        void Write(string filename, ISave save);
    }
}
