using System;
using System.Collections.Generic;
using System.Linq;
using Service.Core;

namespace GameBase.Template.GameBase.Table
{
	public class ShopProductListTable : ITableData
	{
		public int id = new int();
		public string name = string.Empty;
		public int shopId = new int();
		public int buyType = new int();
		public int buyPrice = new int();
		public int itemId = new int();

		public void Serialize(Dictionary<string, string> data)
		{
			if (data.ContainsKey("id") == true) { id = int.Parse(data["id"]); }
			if (data.ContainsKey("name") == true) { name = data["name"].Replace("{$}", ","); }
			if (data.ContainsKey("shopId") == true) { shopId = int.Parse(data["shopId"]); }
			if (data.ContainsKey("buyType") == true) { buyType = int.Parse(data["buyType"]); }
			if (data.ContainsKey("buyPrice") == true) { buyPrice = int.Parse(data["buyPrice"]); }
			if (data.ContainsKey("itemId") == true) { itemId = int.Parse(data["itemId"]); }
		}
	}
}
