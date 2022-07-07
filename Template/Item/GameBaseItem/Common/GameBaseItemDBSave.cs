using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Item.GameBaseItem.Common
{
	public partial class GameBaseItemUserDB
	{
		private void _Run_SaveUser_DBItemTable(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			try
			{
				_dbSlotContainer_DBItemTable.ForEach((DBSlot_DBItemTable slot) =>
				{
					QueryBuilder query = new QueryBuilder("call gp_player_dbitemtable_save(?,?,?,?,?,?,?,?,?,?)");
					query.SetInputParam("@p_user_db_key", user_db_key);
					query.SetInputParam("@p_player_db_key", player_db_key);

					if (!slot._isDeleted)
					{
						query.SetInputParam("@p_slot", slot._nSlot);
						query.SetInputParam("@p_deleted", 0);
						query.SetInputParam("@p_create_time", slot._DBData.create_time);
						query.SetInputParam("@p_update_time", DateTime.UtcNow);

						query.SetInputParam("@p_item_type", slot._DBData.item_type);
						query.SetInputParam("@p_item_id", slot._DBData.item_id);
						query.SetInputParam("@p_item_count", slot._DBData.item_count);
						query.SetInputParam("@p_remain_charge_time", slot._DBData.remain_charge_time);
					}
					else
					{
						query.SetInputParam("@p_slot", slot._nSlot);
						query.SetInputParam("@p_deleted", 1);
						query.SetInputParam("@p_create_time", DateTime.UtcNow);
						query.SetInputParam("@p_update_time", DateTime.UtcNow);

						query.SetInputParam("@p_item_type", default(int));
						query.SetInputParam("@p_item_id", default(int));
						query.SetInputParam("@p_item_count", default(long));
						query.SetInputParam("@p_remain_charge_time", default(DateTime));
					}
					adoDB.ExecuteNoRecords(query);
				}, true);
			}
			catch (Exception e)
			{
				throw new Exception("[gp_player_dbitemtable_save]" + e.Message);
			}
		}
		public override void SaveRun(AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2)
		{
			_Run_SaveUser_DBItemTable(adoDB, partitionKey_1, partitionKey_2);
		}
	}
}
