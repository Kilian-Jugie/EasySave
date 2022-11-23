using EasySave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

namespace EasySave
{
    /// <summary>
    ///     This class contain the differents save methode
    /// </summary>
    public class SaveType : ISaveType
    {
        public const int SOFT_THREADS_PER_THREADS = 75;
        public string Name { get; set; }
        public static SaveProcessData processData { get; set; }
        public ProcessSaveData Process { get; set; }
        /// <summary>
        /// Constructor of a SaveType
        /// </summary>
        /// <param name="name"></param>
        /// <param name="process"></param>
        public SaveType(string name, ProcessSaveData process)
        {
            Name = name;
            Process = process;
        }

        /// <summary>
        /// Static contructor, he register all save type
        /// </summary>
        static SaveType()
        {
            processData = new SaveProcessData()
            {
                SaveName = "",
                PathFrom = "",
                PathTo = "",
                SaveType = "",
                NbTask = 1,
                NbClear = 0
            };
            _Register();
        }

        /// <summary>
        /// Return the SaveType Coresponding to his name
        /// </summary>
        /// <param name="name"></param>
        /// <returns type=string>
        /// Return a SaveType Depending on his name
        /// </returns>
        public static SaveType GetTypeFromName(string name)
        {
            return SaveTypes.Find(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// This method register all different type of save in the List SaveTypes.
        /// If we need to add a new type of save we need to add it to this List
        /// </summary>
        private static void _Register()
        {
            SaveTypes.Add(new SaveType(ISaveType.FULL_SAVE_LABEL, FullSaveProcess));
            SaveTypes.Add(new SaveType(ISaveType.DIFFERENTIAL_SAVE_LABEL, DifferentialSaveProcess));
        }
        /// <summary>
        /// Create the Destination directory and return his path
        /// </summary>
        /// <param name="pathTo"></param>
        /// <param name="saveTypeLabel"></param>
        /// <returns type=string>
        /// Return the path of the FULL save of the folder
        /// </returns>
        private static string _BuildDirDestination(string pathTo, string saveTypeLabel)
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            string pathToTemp = Path.Combine(pathTo, string.Format("{0}-{1}", saveTypeLabel.Substring(0, 3), date));
            DirectoryInfo dirto = new DirectoryInfo(pathToTemp);
            if (!dirto.Exists)
                dirto.Create();
            return pathToTemp;
        }

        private static void _SetProcessData(ISave save, int nbTask, int nbClear)
        {
            processData.SaveName = save.Name;
            processData.PathFrom = save.PathFrom;
            processData.PathTo = save.PathTo;
            processData.SaveType = save.Type;
            processData.NbTask = nbTask;
            processData.NbClear = nbClear;
        }

        private static void _ExecRecursively(string folder, Action<string> action) {
            Directory.GetFiles(folder).Foreach(action);
            Directory.GetDirectories(folder).Foreach((folder) => _ExecRecursively(folder, action));
        }

        class ThreadedRecurseCopyParams {
            public int ThreadCount { get; set; }
            public ConcurrentQueue<FileInfo> Queue { get; set; }
            public DirectoryInfo PathFrom { get; set; }
            public DirectoryInfo PathTo { get; set; }
            public bool UseCryptoSoft { get; set; }
            public string[] ExtensionsToCrypt { get; set; }
            public IEasySaveController Controller { get; set; }
            public ISave SaveProject { get; set; }
        }

        private static void _ThreadedCopy(ThreadedRecurseCopyParams copyParams) {
            long totalFileCount = copyParams.Queue.Count;
            long totalFileSize = 0;
            _SetProcessData(copyParams.SaveProject, (int)totalFileCount, (int)totalFileCount - copyParams.Queue.Count);
            Mutex copyMutex = new Mutex();
            copyParams.Queue.Foreach(f => totalFileSize += f.Length);
            Parallel.For(0, copyParams.Queue.Count, new ParallelOptions { MaxDegreeOfParallelism = copyParams.ThreadCount }, i => {
                if (copyParams.Queue.TryDequeue(out FileInfo srcFile)) {
                    long execTime;
                    string destinationFilePath = srcFile.FullName.Replace(copyParams.PathFrom.FullName, copyParams.PathTo.FullName);
                    Directory.CreateDirectory(destinationFilePath[0..destinationFilePath.LastIndexOf(Path.DirectorySeparatorChar)]);
                    if (copyParams.UseCryptoSoft && copyParams.ExtensionsToCrypt.FirstOrDefault(e => e == srcFile.Extension) != default)
                        execTime = CryptoSoft.CryptoSoft.Call(srcFile.FullName, destinationFilePath, CryptoSoft.CryptoSoft.DEFAULT_KEY, null);
                    else {
                        Stopwatch timer = new Stopwatch();
                        timer.Start();
                        File.Copy(srcFile.FullName, destinationFilePath);
                        timer.Stop();
                        execTime = timer.ElapsedMilliseconds;
                    }

                    processData.IncrementNbClear();

                    //copyMutex.WaitOne();

                    _WriteLogsLogger(copyParams.Controller, new string[] { srcFile.FullName, copyParams.PathFrom.FullName,
                        copyParams.PathTo.FullName, srcFile.Length.ToString(), execTime.ToString() });

                    float progress = 100 - (copyParams.Queue.Count * 100 / (int)totalFileCount);

                    _WriteLogsStates(copyParams.Controller, new string[] { DateTime.Now.ToString(), copyParams.SaveProject.Name,
                        STATE_ACTIVE, totalFileCount.ToString(), totalFileSize.ToString(), progress.ToString(), copyParams.Queue.Count.ToString(),
                        totalFileSize.ToString(), copyParams.PathFrom.FullName, copyParams.PathTo.FullName
                    });

                    //copyMutex.ReleaseMutex();
                }
            });
        }

        /// <summary>
        /// Execute a Full save process depending on the saveinformation
        /// </summary>
        /// <param name="saveInformations"></param>
        /// <param name="controller"></param>
        public static void FullSaveProcess(ISave saveInformations, IEasySaveController controller)
        {
            string pathTo = _BuildDirDestination(saveInformations.PathTo, ISaveType.FULL_SAVE_LABEL);
            ConcurrentQueue<FileInfo> _files = new ConcurrentQueue<FileInfo>();
            _ExecRecursively(saveInformations.PathFrom, f => _files.Enqueue(new FileInfo(f)));
            _ThreadedCopy(new ThreadedRecurseCopyParams() {
                Queue = _files,
                PathFrom = new DirectoryInfo(saveInformations.PathFrom),
                PathTo = new DirectoryInfo(pathTo),
                Controller = controller,
                SaveProject = saveInformations,
                ThreadCount = SOFT_THREADS_PER_THREADS * Environment.ProcessorCount,
                UseCryptoSoft = true,
                ExtensionsToCrypt = controller.Parameters["encrypt_exts"].Split(',')
            });
        }

        private static DirectoryInfo _GetLastFullSaveDir(string containingFolder) {
            DirectoryInfo ret = null;
            DateTime newest = DateTime.MinValue;
            Directory.GetDirectories(containingFolder).Foreach(folder => {
                string folderName = folder[(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1)..];
                if (Regex.IsMatch(folderName, $@"^{ISaveType.FULL_SAVE_LABEL[0..3]}.+")) {

                    DateTime dateTime = new DateTime();
                    DateTime.TryParseExact(folderName[4..], "dd-MM-yyyy-HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                    if (dateTime > newest) {
                        ret = new DirectoryInfo(folder);
                        newest = dateTime;
                    }
                }
            });
            return ret;
        }

        /// <summary>
        /// Execute a Differential save process depending on the saveinformation
        /// </summary>
        /// <param name="saveInformations"></param>
        /// <param name="controller"></param>
        public static void DifferentialSaveProcess(ISave saveInformations, IEasySaveController controller) {
            string pathTo = _BuildDirDestination(saveInformations.PathTo, ISaveType.DIFFERENTIAL_SAVE_LABEL);
            ConcurrentQueue<FileInfo> _files = new ConcurrentQueue<FileInfo>();
            DirectoryInfo lastFullSave = _GetLastFullSaveDir(saveInformations.PathTo);
            if(lastFullSave is null) {
                //No full save available to compare
                _ExecRecursively(saveInformations.PathFrom, f => _files.Enqueue(new FileInfo(f)));
            }
            else {
                bool DifComparer(FileInfo f) {
                    string oldPath = f.FullName.Replace(saveInformations.PathFrom, lastFullSave.FullName);
                    if (!File.Exists(oldPath)) return true;
                    FileInfo fold = new FileInfo(oldPath);
                    if (fold.LastWriteTime > f.LastWriteTime) return false;
                    return true;
                }

                _ExecRecursively(saveInformations.PathFrom, f => {
                    FileInfo finfo = new FileInfo(f);
                    if (DifComparer(finfo)) _files.Enqueue(finfo);
                });
            }

            if (_files.Count > 0) {
                _ThreadedCopy(new ThreadedRecurseCopyParams() {
                    Queue = _files,
                    PathFrom = new DirectoryInfo(saveInformations.PathFrom),
                    PathTo = new DirectoryInfo(pathTo),
                    Controller = controller,
                    SaveProject = saveInformations,
                    ThreadCount = SOFT_THREADS_PER_THREADS * Environment.ProcessorCount,
                    UseCryptoSoft = true,
                    ExtensionsToCrypt = controller.Parameters["encrypt_exts"].Split(',')
                });
            }
        }

        /// <summary>
        /// Send all data needed for the Logger log
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        private static void _WriteLogsLogger(IEasySaveController controller, string[] data)
        {
            string[] toLog = { string.Format("File name: {0}", data[0]),
                string.Format("\t-Source: {0}", data[1]),
                string.Format("\t-Destination: {0}", data[2]),
                string.Format("\t-Size: {0} Bytes", data[3]),
                string.Format("\t-Execution time: {0}", data[4]) };
            controller.SavesLogger.BufferedInfo(toLog);
        }
        /// <summary>
        /// Send all data needed for the States log
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data"></param>
        private static void _WriteLogsStates(IEasySaveController controller, string[] data)
        {
            string[] toLog = { string.Format("Curent time: {0}", data[0]),
                string.Format("Save name: {0}", data[1]),
                string.Format("Save state: {0}", data[2]),
                string.Format("\t-Number of file: {0}", data[3]),
                string.Format("\t-Total size: {0} Bytes", data[4]),
                string.Format("\t-Progress: {0}%", data[5]),
                string.Format("\t-Number of file Remaining: {0}", data[6]),
                string.Format("\t-Size of file Remaining: {0} Bytes", data[7]),
                string.Format("\t-Fully source folder path: {0}", data[8]),
                string.Format("\t-Fully destination folder path: {0}", data[9])
            };
            controller.StateLogger.BufferedInfo(toLog);
        }

        private static void _FlushLogs(IEasySaveController controller) {
            controller.StateLogger.Flush();
            controller.SavesLogger.Flush();
        }

        public static List<SaveType> SaveTypes = new List<SaveType>();
        private static string STATE_ACTIVE = "ACTIF";
    }
}

