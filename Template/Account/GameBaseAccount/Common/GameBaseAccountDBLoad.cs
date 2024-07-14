using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Account.GameBaseAccount.Common
{
	public partial class GameBaseAccountUserDB
	{
		private void _Run_LoadUser_player(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			try
			{
				string strResult;
				QueryBuilder query = new QueryBuilder("call gp_player_player_load(?,?)");
				query.SetInputParam("@p_player_db_key", player_db_key);
				query.SetInputParam("@p_user_db_key", user_db_key);

				adoDB.Execute(query);

				if (adoDB.RecordNotEOF())
				{
					player rplayer = _dbBaseContainer_player.GetWriteData(false)._DBData;

					rplayer.player_db_key = adoDB.RecordGetValue("player_db_key");
					rplayer.user_db_key = adoDB.RecordGetValue("user_db_key");
					rplayer.create_time = adoDB.RecordGetTimeValue("create_time");
					rplayer.update_time = adoDB.RecordGetTimeValue("update_time");

					rplayer.login_time = adoDB.RecordGetTimeValue("login_time");
					rplayer.logout_time = adoDB.RecordGetTimeValue("logout_time");
					rplayer.is_login = adoDB.RecordGetValue("is_login");
					rplayer.newbie = adoDB.RecordGetValue("newbie");
					rplayer.serial_allocator = adoDB.RecordGetValue("serial_allocator");
					rplayer.player_name = adoDB.RecordGetStrValue("player_name");
					rplayer.level = adoDB.RecordGetValue("level");
					rplayer.exp = adoDB.RecordGetValue("exp");
				}
				else
				{
					strResult = "[gp_player_player_load] No Result!";
				}
				adoDB.RecordEnd();
			}
			catch (Exception e)
			{
				adoDB.RecordEnd();
				throw new Exception("[gp_player_player_load]" + e.Message);
			}
		}
		public override void LoadRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			_Run_LoadUser_player(adoDB, user_db_key, player_db_key);
		}
	}
}
