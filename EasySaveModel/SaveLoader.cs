using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave {
    abstract public class SaveLoader : ISaveLoader {
        public string Extension { get; }

        public static IDictionary<string, ISaveLoader> SaveLoadersRegistry { get; set; }

        protected SaveLoader(string extension) {
            Extension = extension;
        }

        static SaveLoader()
        {
            SaveLoadersRegistry = new Dictionary<string, ISaveLoader>();
        }

        abstract public ISave Load(string filename);
        abstract public void Write(string filename, ISave save);
    }
}
