using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Shop.GameBaseShop.Common
{
	public partial class GameBaseShopUserDB
	{
		private void _Run_LoadUser_DBShopTable(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			try
			{
				QueryBuilder query = new QueryBuilder("call gp_player_dbshoptable_load(?)");
				query.SetInputParam("@p_player_db_key", player_db_key);

				adoDB.Execute(query);

				while (adoDB.RecordWhileNotEOF())
				{
					short nSlot = adoDB.RecordGetValue("slot");

					DBSlot_DBShopTable slot = _dbSlotContainer_DBShopTable.Insert(nSlot, false);

					slot._isDeleted = adoDB.RecordGetValue("deleted");
					slot._DBData.player_db_key = adoDB.RecordGetValue("player_db_key");
					slot._DBData.create_time = adoDB.RecordGetTimeValue("create_time");
					slot._DBData.update_time = adoDB.RecordGetTimeValue("update_time");

					slot._DBData.shop_index = adoDB.RecordGetValue("shop_index");
					slot._DBData.shop_product_index = adoDB.RecordGetValue("shop_product_index");
					slot._DBData.buy_count = adoDB.RecordGetValue("buy_count");
				}
				adoDB.RecordEnd();
			}
			catch (Exception e)
			{
				adoDB.RecordEnd();
				throw new Exception("[gp_player_dbshoptable_load]" + e.Message);
			}
		}
		public override void LoadRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			_Run_LoadUser_DBShopTable(adoDB, user_db_key, player_db_key);
		}
	}
}
