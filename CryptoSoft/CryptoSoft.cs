using System.Text;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CryptoSoft {
    public class CryptoSoft {
        public const ulong DEFAULT_KEY = 0xAA5464026080CCB2;

        private const string DEFAULT_PATH_PARAMS = "./settings.conf";
        private const string PARAM_KEY = "key";
        public const int RC_INVALID_FORMAT = -1;
        public const int RC_INPUT_NOT_FOUND = -3;

        public static IEnumerable<byte> XORGate(byte[] bytes, ulong key) {
            foreach (var b in bytes) yield return (byte)(b ^ key);
        }

        public static int Call(string file, string destination, ulong key, string settings) {
            if (settings != null && File.Exists(settings)) {
                Dictionary<string, string> parsed = new Dictionary<string, string>(new KeyValueParser('=').ParseFile(settings));
                if (parsed.ContainsKey(PARAM_KEY))
                    key = ulong.Parse(parsed[PARAM_KEY]);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            File.WriteAllBytes(destination, XORGate(File.ReadAllBytes(file), key).ToArray());
            stopwatch.Stop();
            //Console.WriteLine($"Done in {stopwatch.ElapsedMilliseconds} ms !");
            //Overflow at ~1491 days
            return (int)stopwatch.ElapsedMilliseconds;
        }

        static void Main(string[] args) {
            static void Exit(string msg, int code) {
                Console.WriteLine(msg);
                Environment.Exit(code);
            }

            static void Exit_If_Else(bool cond, string msg, int code, Action actionElse) {
                if (cond) Exit(msg, code);
                else actionElse?.Invoke();
            }

            static void Exit_If(bool cond, string msg, int code) {
                Exit_If_Else(cond, msg, code, null);
            }

            Exit_If(args.Length < 4 || args[0] != "-src" || args[2] != "-dst",
                "Format: -src <source_file> -dst <destination_file> [-key <encryption_key>] [-set <settings_file]", RC_INVALID_FORMAT);

            ulong key = DEFAULT_KEY;
            string paramPath = DEFAULT_PATH_PARAMS;

            int keyI = args.IndexOf("-key");
            if (keyI != -1)
                Exit_If_Else(args.Length <= keyI + 1, "Invalid key: no key given", RC_INVALID_FORMAT, () => key = ulong.Parse(args[keyI + 1]));

            int paramI = args.IndexOf("-set");
            if(paramI != -1) {
                Exit_If_Else(args.Length <= paramI + 1, "Invalid settings path: no settings path given", RC_INVALID_FORMAT, () => paramPath = args[paramI + 1]);
                Exit_If(!File.Exists(paramPath), "Invalid settings path: file not found", RC_INVALID_FORMAT);
            }

            Exit_If(!File.Exists(args[1]), "Input file not found !", RC_INPUT_NOT_FOUND);
            
            

            Environment.ExitCode = Call(args[1], args[3], key, paramPath);
        }
    }
}
