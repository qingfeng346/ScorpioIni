using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


class Program {
    static void Main(string[] args) {
        try {
            var file = Path.Combine(Environment.CurrentDirectory, "../sample.ini");
            ScorpioIni ini = new ScorpioIni(file, Encoding.UTF8);
            Console.WriteLine(ini.Get("key1"));
            Console.WriteLine(ini.Get("sec1", "key1"));
            Console.WriteLine(ini.GetString());
        } catch (Exception e) {
            Console.WriteLine("error : " + e.ToString());
        }
    }
}

