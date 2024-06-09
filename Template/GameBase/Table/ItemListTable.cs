using System;
using System.Collections.Generic;
using System.Linq;
using Service.Core;

namespace GameBase.Template.GameBase.Table
{
	public class ItemListTable : ITableData
	{
		public int id = new int();
		public int itemType = new int();
		public int itemSubType = new int();
		public string name = string.Empty;
		public bool isUse = new bool();
		public long maxValue = new long();

		public void Serialize(Dictionary<string, string> data)
		{
			if (data.ContainsKey("id") == true) { id = int.Parse(data["id"]); }
			if (data.ContainsKey("itemType") == true) { itemType = int.Parse(data["itemType"]); }
			if (data.ContainsKey("itemSubType") == true) { itemSubType = int.Parse(data["itemSubType"]); }
			if (data.ContainsKey("name") == true) { name = data["name"].Replace("{$}", ","); }
			if (data.ContainsKey("isUse") == true) { isUse = bool.Parse(data["isUse"]); }
			if (data.ContainsKey("maxValue") == true) { maxValue = long.Parse(data["maxValue"]); }
		}
	}
}
