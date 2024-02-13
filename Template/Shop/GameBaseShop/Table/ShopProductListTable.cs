using System;
using System.Collections.Generic;
using System.Linq;
using Service.Core;

namespace GameBase.Template.GameBase.Table
{
	public class ShopProductListTable : ITableData
	{
		public 1001 id = new 1001();
		public gold_1 name = new gold_1();
		public 1 shopId = new 1();
		public 1 buyType = new 1();
		public 10 buyPrice = new 10();
		public 1002 itemId = new 1002();

		public void Serialize(Dictionary<string, string> data)
		{
			if (data.ContainsKey("id") == true) { id = (1001)Enum.Parse(typeof(1001), data["id"]); }
			if (data.ContainsKey("name") == true) { name = (gold_1)Enum.Parse(typeof(gold_1), data["name"]); }
			if (data.ContainsKey("shopId") == true) { shopId = (1)Enum.Parse(typeof(1), data["shopId"]); }
			if (data.ContainsKey("buyType") == true) { buyType = (1)Enum.Parse(typeof(1), data["buyType"]); }
			if (data.ContainsKey("buyPrice") == true) { buyPrice = (10)Enum.Parse(typeof(10), data["buyPrice"]); }
			if (data.ContainsKey("itemId") == true) { itemId = (1002)Enum.Parse(typeof(1002), data["itemId"]); }
		}
	}
}
