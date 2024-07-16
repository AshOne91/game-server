using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Shop.GameBaseShop.Common
{
	public class DBShopTable : BaseDBClass
	{
		/// <sumary>
		/// 파티션키_1
		/// <sumary>
		public UInt64 player_db_key;
		/// <sumary>
		/// 생성시간
		/// <sumary>
		public DateTime create_time;
		/// <sumary>
		/// 업데이트 시간
		/// <sumary>
		public DateTime update_time;
		/// <sumary>
		/// Shop인덱스
		/// <sumary>
		public int shop_index;
		/// <sumary>
		/// ShopProductList인덱스
		/// <sumary>
		public int shop_product_index;
		/// <sumary>
		/// 구매횟수
		/// <sumary>
		public int buy_count;
		public DBShopTable() { Reset(); }
		~DBShopTable() { Reset(); }
		public override void Reset()
		{
			player_db_key = default(UInt64);
			create_time = DateTime.UtcNow;
			update_time = DateTime.UtcNow;
			shop_index = default(int);
			shop_product_index = default(int);
			buy_count = default(int);
		}
		public override void Copy(BaseDBClass srcDBData)
		{
			DBShopTable srcDBShopTable = (DBShopTable)srcDBData;
			player_db_key = srcDBShopTable.player_db_key;
			create_time = srcDBShopTable.create_time;
			update_time = srcDBShopTable.update_time;
			shop_index = srcDBShopTable.shop_index;
			shop_product_index = srcDBShopTable.shop_product_index;
			buy_count = srcDBShopTable.buy_count;
		}
	}
	public class DBSlot_DBShopTable : DBSlot<DBShopTable>{}
	public class DBSlotContainer_DBShopTable : DBSlotContainer<DBSlot_DBShopTable, DBShopTable>{}
}
