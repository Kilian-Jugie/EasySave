using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoSoft {
    public static class Utils {
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> first, Func<T, T> apply) {
            foreach (var elem in first) yield return apply(elem);
        }

        public static int IndexOf<T>(this IEnumerable<T> first, T what) {
            int i = 0;
            foreach (var elem in first) {
                if (elem.Equals(what)) return i;
                i++;
            }
            return -1;
        }
    }
}
