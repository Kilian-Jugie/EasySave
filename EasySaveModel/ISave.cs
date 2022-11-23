using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave
{
    public interface ISave
    {
        string Name { get; set; }
        string PathFrom { get; set; }
        string PathTo { get; set; }
        string Type { get; set; }
    }
}
