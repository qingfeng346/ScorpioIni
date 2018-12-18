using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Ini {
    public static class localSession {
        private static ScorpioIni ini = new ScorpioIni();
        public static void set(string key, string value) {
            if (ini == null) { return; }
            ini.Set(key, value);
        }
        public static string get(string key, string def = "") {
            if (ini == null) { return def; }
            return ini.GetDef(key, def);
        }
        public static bool has(string key) {
            return ini == null ? false : ini.Has(key);
        }
        public static void remove(string key) {
            if (ini == null) { return; }
            ini.Remove(key);
        }
        public static void clear() {
            if (ini == null) { return; }
            ini.Clear();
        }
    }
}
