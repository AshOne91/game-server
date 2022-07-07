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
		private void _Run_LoadUser_DBItemTable(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			try
			{
				QueryBuilder query = new QueryBuilder("call gp_player_dbitemtable_load(?,?)");
				query.SetInputParam("@p_user_db_key", user_db_key);
				query.SetInputParam("@p_player_db_key", player_db_key);

				adoDB.Execute(query);

				while (adoDB.RecordWhileNotEOF())
				{
					short nSlot = adoDB.RecordGetValue("slot");

					DBSlot_DBItemTable slot = _dbSlotContainer_DBItemTable.Insert(nSlot, false);

					slot._isDeleted = adoDB.RecordGetValue("deleted");
					slot._DBData.user_db_key = adoDB.RecordGetValue("user_db_key");
					slot._DBData.player_db_key = adoDB.RecordGetValue("player_db_key");
					slot._DBData.create_time = adoDB.RecordGetTimeValue("create_time");
					slot._DBData.update_time = adoDB.RecordGetTimeValue("update_time");

					slot._DBData.item_type = adoDB.RecordGetValue("item_type");
					slot._DBData.item_id = adoDB.RecordGetValue("item_id");
					slot._DBData.item_count = adoDB.RecordGetValue("item_count");
					slot._DBData.remain_charge_time = adoDB.RecordGetTimeValue("remain_charge_time");
				}
				adoDB.RecordEnd();
			}
			catch (Exception e)
			{
				adoDB.RecordEnd();
				throw new Exception("[gp_player_dbitemtable_load]" + e.Message);
			}
		}
	}
}
