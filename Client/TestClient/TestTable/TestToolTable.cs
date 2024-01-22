using System;
using System.Collections.Generic;
using System.Linq;
using Service.Core;

namespace GameBase.Template.GameBase.Table
{
	public class TestToolTable : ITableData
	{
		public int test1 = new int();
		public bool test2 = new bool();
		public short test3 = new short();
		public float test4 = new float();
		public DateTime test5 = new DateTime();
		public Byte test6 = new Byte();
		public string test7 = string.Empty;
		public List<int> test8 = new List<int>();
		public List<bool> test9 = new List<bool>();
		public List<short> test10 = new List<short>();
		public List<float> test11 = new List<float>();
		public List<DateTime> test12 = new List<DateTime>();
		public List<Byte> test13 = new List<Byte>();
		public List<string> test14 = new List<string>();

		public void Serialize(Dictionary<string, string> data)
		{
			if (data.ContainsKey("test1") == true) { test1 = int.Parse(data["test1"]); }
			if (data.ContainsKey("test2") == true) { test2 = bool.Parse(data["test2"]); }
			if (data.ContainsKey("test3") == true) { test3 = short.Parse(data["test3"]); }
			if (data.ContainsKey("test4") == true) { test4 = float.Parse(data["test4"]); }
			if (data.ContainsKey("test5") == true) { test5 = (data["test5"] == "-1") ? default(DateTime) : DateTime.Parse(data["test5"]); }
			if (data.ContainsKey("test6") == true) { test6 = Byte.Parse(data["test6"]); }
			if (data.ContainsKey("test7") == true) { test7 = data["test7"].Replace("{$}", ","); }
			if (data.ContainsKey("test8") == true) { if (data["test8"] != "-1") test8 = data["test8"].Split('|').Select(int.Parse).ToList(); }
			if (data.ContainsKey("test9") == true) { if (data["test9"] != "-1") test9 = data["test9"].Split('|').Select(bool.Parse).ToList(); }
			if (data.ContainsKey("test10") == true) { if (data["test10"] != "-1") test10 = data["test10"].Split('|').Select(short.Parse).ToList(); }
			if (data.ContainsKey("test11") == true) { if (data["test11"] != "-1") test11 = data["test11"].Split('|').Select(float.Parse).ToList(); }
			if (data.ContainsKey("test12") == true) { if (data["test12"] != "-1") test12 = data["test12"].Split('|').Select(DateTime.Parse).ToList(); }
			if (data.ContainsKey("test13") == true) { if (data["test13"] != "-1") test13 = data["test13"].Split('|').Select(Byte.Parse).ToList(); }
			if (data.ContainsKey("test14") == true) { if (data["test14"] != "-1") test14 = data["test14"].Split('|').ToList(); }
		}
	}
}
