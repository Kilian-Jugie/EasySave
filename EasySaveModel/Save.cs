using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave
{
    public class Save : ISave
    {
        public string Name { get; set; }
        public string PathFrom { get; set; }
        public string PathTo { get; set; }
        public string Type { get; set; }
        public Save() { }
    }

}
