using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EasySave
{
    public class SaveLoaderXml : SaveLoader
    {
        public SaveLoaderXml() : base("xml")
        {

        }

        public override ISave Load(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save));
            using (StreamReader streamReader = new StreamReader(filename))
            {
                return xmlSerializer.Deserialize(streamReader) as Save;
            }
        }

        public override void Write(string savefoldername, ISave save)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Save));
            using (StreamWriter streamWiter = new StreamWriter(string.Concat(new string[] { savefoldername, "\\", save.Name, ".", Extension })))
            {
                xmlSerializer.Serialize(streamWiter, save);
            }
        }
    }
}
