using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Ini {
    public class localStroage {
        private static string _storageFile;
        private static ScorpioIni ini;
        public static string StorageFile {
            get { return _storageFile; }
            set {
                _storageFile = value;
                ini = new ScorpioIni(_storageFile, Encoding.UTF8);
            }
        }
        public static void set(string key, string value) {
            if (ini == null) { return; }
            ini.Set(key, value);
            ini.SaveToFile();
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
            ini.SaveToFile();
        }
        public static void clear() {
            if (ini == null) { return; }
            ini.Clear();
            ini.SaveToFile();
        }
    }
}
