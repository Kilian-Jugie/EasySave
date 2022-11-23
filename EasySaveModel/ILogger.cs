using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave {
    public interface ILogger {

        /// <summary>
        /// Write param in log file
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="nameFunction"></param>
        void AddLog(object[] objects, string nameFunction);

        /// <summary>
        /// Call AddLog method as info state
        /// </summary>
        /// <param name="objects"></param>
        void Info(params object[] objects);

        /// <summary>
        /// Call AddLog method as error state
        /// </summary>
        /// <param name="objects"></param>
        void Error(params object[] objects);

        /// <summary>
        /// Call AddLog method as warning state
        /// </summary>
        /// <param name="objects"></param>
        void Warning(params object[] objects);

        /// <summary>
        /// Call AddLog method as debug state
        /// </summary>
        /// <param name="objects"></param>
        void Debug(params object[] objects);
        void BufferedInfo(params object[] objects);
        void Flush();
    }
}
