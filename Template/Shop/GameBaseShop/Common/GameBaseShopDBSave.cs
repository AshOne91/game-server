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
		private void _Run_SaveUser_DBShopTable(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			try
			{
				_dbSlotContainer_DBShopTable.ForEach((DBSlot_DBShopTable slot) =>
				{
					QueryBuilder query = new QueryBuilder("call gp_player_dbshoptable_save(?,?,?,?,?,?,?,?)");
					query.SetInputParam("@p_player_db_key", player_db_key);

					if (!slot._isDeleted)
					{
						query.SetInputParam("@p_slot", slot._nSlot);
						query.SetInputParam("@p_deleted", 0);
						query.SetInputParam("@p_create_time", slot._DBData.create_time);
						query.SetInputParam("@p_update_time", DateTime.UtcNow);

						query.SetInputParam("@p_shop_index", slot._DBData.shop_index);
						query.SetInputParam("@p_shop_product_index", slot._DBData.shop_product_index);
						query.SetInputParam("@p_buy_count", slot._DBData.buy_count);
					}
					else
					{
						query.SetInputParam("@p_slot", slot._nSlot);
						query.SetInputParam("@p_deleted", 1);
						query.SetInputParam("@p_create_time", DateTime.UtcNow);
						query.SetInputParam("@p_update_time", DateTime.UtcNow);

						query.SetInputParam("@p_shop_index", default(int));
						query.SetInputParam("@p_shop_product_index", default(int));
						query.SetInputParam("@p_buy_count", default(int));
					}
					adoDB.ExecuteNoRecords(query);
				}, true);
			}
			catch (Exception e)
			{
				throw new Exception("[gp_player_dbshoptable_save]" + e.Message);
			}
		}
		public override void SaveRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			_Run_SaveUser_DBShopTable(adoDB, user_db_key, player_db_key);
		}
	}
}
