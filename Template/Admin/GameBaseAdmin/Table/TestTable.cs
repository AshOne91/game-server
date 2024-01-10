using System;
using System.Collections.Generic;
using System.Linq;
using Service.Core;

namespace GameBase.Template.GameBase.Tabe
{
    public class TestTable : ITableData
    {
        public int test1 = new int();
        public bool test2 = new bool();
        public short test3 = new short();
        public float test5 = new float();
        public DateTime test6 = new DateTime();
        public Byte test7 = new Byte();
        public string test8 = string.Empty;
        public List<int> test9 = new List<int>();
        public List<bool> test10 = new List<bool>();
        public List<short> test11 = new List<short>();
        public List<float> test12 = new List<float>();
        public List<DateTime> test13 = new List<DateTime>();
        public List<Byte> test14 = new List<Byte>();
        public List<string> test15 = new List<string>();
        public void Serialize(Dictionary<string, string> data)
        {
            if (data.ContainsKey("test1") == true) { test1 = int.Parse(data["test1"]); }
            if (data.ContainsKey("test2") == true) { test2 = bool.Parse(data["test2"]); }
            if (data.ContainsKey("test3") == true) { test3 = short.Parse(data["test3"]); }
            if (data.ContainsKey("test5") == true) { test5 = float.Parse(data["test5"]); }
            if (data.ContainsKey("test6") == true) { test6 = (data["test6"] == "-1") ? default(DateTime) : DateTime.Parse(data["test6"]); }
            if (data.ContainsKey("test7") == true) { test7 = Byte.Parse(data["test7"]); }
            if (data.ContainsKey("test8") == true) { test8 = data["test8"].Replace("{$}", ","); }
            if (data.ContainsKey("test9") == true) { if (data["test9"] != "-1") test9 = data["test9"].Split('|').Select(int.Parse).ToList(); }
            if (data.ContainsKey("test10") == true) { if (data["test10"] != "-1") test10 = data["test10"].Split('|').Select(bool.Parse).ToList(); }
            if (data.ContainsKey("test11") == true) { if (data["test11"] != "-1") test11 = data["test11"].Split('|').Select(short.Parse).ToList(); }
            if (data.ContainsKey("test12") == true) { if (data["test12"] != "-1") test12 = data["test12"].Split('|').Select(float.Parse).ToList(); }
            if (data.ContainsKey("test13") == true) { if (data["test13"] != "-1") test13 = data["test13"].Split('|').Select(DateTime.Parse).ToList(); }
            if (data.ContainsKey("test14") == true) { if (data["test14"] != "-1") test14 = data["test14"].Split('|').Select(Byte.Parse).ToList(); }
            if (data.ContainsKey("test15") == true) { if (data["test15"] != "-1") test15 = data["test15"].Split('|').ToList(); }
        }
    }
}

