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
		private void _Run_SaveUser_player(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			try
			{
				if (_dbBaseContainer_player.GetReadData()._isChanged == false)
				{
					return;
				}

				player rplayer = _dbBaseContainer_player.GetReadData()._DBData;

				QueryBuilder query = new QueryBuilder("call gp_player_player_save(?,?,?,?,?,?,?,?,?,?,?)");
				query.SetInputParam("@p_player_db_key", player_db_key);
				query.SetInputParam("@p_create_time", rplayer.create_time);
				query.SetInputParam("@p_update_time", rplayer.update_time);

				query.SetInputParam("@p_login_time", rplayer.login_time);
				query.SetInputParam("@p_logout_time", rplayer.logout_time);
				query.SetInputParam("@p_is_login", rplayer.is_login);
				query.SetInputParam("@p_newbie", rplayer.newbie);
				query.SetInputParam("@p_serial_allocator", rplayer.serial_allocator);
				query.SetInputParam("@p_player_name", rplayer.player_name);
				query.SetInputParam("@p_level", rplayer.level);
				query.SetInputParam("@p_exp", rplayer.exp);

				adoDB.ExecuteNoRecords(query);
			}
			catch (Exception e)
			{
				throw new Exception("[gp_player_player_save]" + e.Message);
			}
		}
		public override void SaveRun(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key)
		{
			_Run_SaveUser_player(adoDB, user_db_key, player_db_key);
		}
	}
}
