using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasySave {
    public static class Utils {
        public static IList<string> BusinessSoftware = new List<string>();
        public static IEnumerable<T> GenericIntersect<T, N>(this IEnumerable<T> first, IEnumerable<N> second, Func<T, N, bool> comp) {
            foreach(var felem in first) {
                foreach(var selem in second) {
                    if (comp(felem, selem)) yield return felem;
                }
            }
        }

        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> first, Func<T, T> apply) {
            foreach (var elem in first) yield return apply(elem);
        }

        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> first, Action<T> apply) {
            foreach (var elem in first) apply(elem);
            return first;
        }

        public static bool IsBusinessSoftwareRunning()
        {
            foreach(var software in BusinessSoftware)
            {
                if ((System.Diagnostics.Process.GetProcesses().FirstOrDefault(u => u.ProcessName == software) != default))
                    return true;
            }

            return false;
        }
    }
}
