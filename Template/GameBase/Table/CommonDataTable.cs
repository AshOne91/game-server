using System;
using System.Collections.Generic;
using System.Linq;
using Service.Core;

namespace GameBase.Template.GameBase.Table
{
	public class CommonDataTable : ITableData
	{
		public string id = string.Empty;
		public int value1 = new int();
		public List<int> value2 = new List<int>();

		public void Serialize(Dictionary<string, string> data)
		{
			if (data.ContainsKey("id") == true) { id = data["id"].Replace("{$}", ","); }
			if (data.ContainsKey("value1") == true) { value1 = int.Parse(data["value1"]); }
			if (data.ContainsKey("value2") == true) { if (data["value2"] != "-1") value2 = data["value2"].Split('|').Select(int.Parse).ToList(); }
		}
	}
}
