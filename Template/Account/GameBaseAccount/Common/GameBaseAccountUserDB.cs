using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Account.GameBaseAccount.Common
{
	public partial class GameBaseAccountUserDB : GameBaseUserDB
	{
		public DBBaseContainer_player _dbBaseContainer_player = new DBBaseContainer_player();

		public override void Copy(UserDB userSrc, bool isChanged)
		{
			GameBaseAccountUserDB userDB = userSrc.GetUserDB<GameBaseAccountUserDB>(ETemplateType.Account);
			_dbBaseContainer_player.Copy(userDB._dbBaseContainer_player, isChanged);
		}
	}
}
