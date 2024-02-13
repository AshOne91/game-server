using System;
using System.Collections.Generic;
using System.Linq;
using Service.Core;

namespace GameBase.Template.GameBase.Table
{
	public class ShopInfoTable : ITableData
	{
		public int id = new int();
		public string name = string.Empty;
		public int maxCount = new int();
		public bool isShow = new bool();
		public DateTime startDate = new DateTime();
		public DateTime endDate = new DateTime();

		public void Serialize(Dictionary<string, string> data)
		{
			if (data.ContainsKey("id") == true) { id = int.Parse(data["id"]); }
			if (data.ContainsKey("name") == true) { name = data["name"].Replace("{$}", ","); }
			if (data.ContainsKey("maxCount") == true) { maxCount = int.Parse(data["maxCount"]); }
			if (data.ContainsKey("isShow") == true) { isShow = bool.Parse(data["isShow"]); }
			if (data.ContainsKey("startDate") == true) { startDate = (data["startDate"] == "-1") ? default(DateTime) : DateTime.Parse(data["startDate"]); }
			if (data.ContainsKey("endDate") == true) { endDate = (data["endDate"] == "-1") ? default(DateTime) : DateTime.Parse(data["endDate"]); }
		}
	}
}
