using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave
{
    public delegate void ProcessSaveData(ISave saveInformations, IEasySaveController controller);

    public interface ISaveType
    {
        string Name { get; set; }
        ProcessSaveData Process { get; }
        public static ProcessSaveData processData { get; }

        public static readonly string FULL_SAVE_LABEL = "FULL_SAVE";

        public static readonly string DIFFERENTIAL_SAVE_LABEL = "DIFFERENTIAL_SAVE";
    }

}
