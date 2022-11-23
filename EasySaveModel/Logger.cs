using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Collections.Concurrent;

namespace EasySave {
    /// <summary>
    /// This class create a xml file and write strings in it
    /// </summary>
    public class Logger : ILogger {
        private readonly string FileName;

        ConcurrentQueue<string> WriteBuffer = new ConcurrentQueue<string>();
        ConcurrentQueue<string> FlushBuffer = new ConcurrentQueue<string>();
        private int BufferMaxSize = 10000;

        private static readonly Mutex LogMutex = new Mutex();

        public Logger(string fileName){
            
            FileName = fileName;
            if (!File.Exists(fileName)) {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.NewLineOnAttributes = true;
                xmlWriterSettings.Indent = true;

                XmlWriter xml = XmlWriter.Create(FileName);
                xml.WriteStartDocument();
                xml.WriteEndDocument();
                xml.Close();
            }
            
        }

        ~Logger() {
            Flush();
        }

        /// <summary>
        /// Write param in log file
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="nameFunction"></param>
        public void AddLog(object[] objects, string nameFunction) {
            BufferedAddLog(objects, nameFunction);
        }

        public void Flush() {
            LogMutex.WaitOne();
            if (FlushBuffer.Count > 0) {
                 File.AppendAllLines(FileName, FlushBuffer.ToArray());
            }
            LogMutex.ReleaseMutex();
        }

        public void SetBufferMaxSize(int count) {
            BufferMaxSize = count;
        }

        public void BufferedAddLog(object[] objects, string logType) {
            if (WriteBuffer.Count >= BufferMaxSize) {
                FlushBuffer = WriteBuffer;
                WriteBuffer.Clear();
                Flush();
            }
            foreach (var obj in objects) {
                WriteBuffer.Enqueue($"<m l=\"{logType}\" d=\"{DateTime.Now}\">{obj}</m>");
            }
        }

        /// <summary>
        /// Call AddLog method as debug state
        /// </summary>
        /// <param name="objects"></param>
        public void Debug(params object[] objects) {
            AddLog(objects, "debug");         
        }

        /// <summary>
        /// Call AddLog method as error state
        /// </summary>
        /// <param name="objects"></param>
        public void Error(params object[] objects) {
            AddLog(objects, "error");
        }

        /// <summary>
        /// Call AddLog method as info state
        /// </summary>
        /// <param name="objects"></param>
        public void Info(params object[] objects) {
            AddLog(objects, "info");
        }

        public void BufferedInfo(params object[] objects) {
            BufferedAddLog(objects, "info");
        }

        /// <summary>
        /// Call AddLog method as warning state
        /// </summary>
        /// <param name="objects"></param>
        public void Warning(params object[] objects) {
            AddLog(objects, "warning");
        }
    }
}