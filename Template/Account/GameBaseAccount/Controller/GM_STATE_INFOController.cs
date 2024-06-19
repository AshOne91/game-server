#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;

namespace GameBase.Template.Account.GameBaseAccount
{
	public partial class GameBaseAccountTemplate
	{
		public void ON_GM_STATE_INFO_NOTI_CALLBACK(ImplObject userObject, PACKET_GM_STATE_INFO_NOTI packet)
		{
            GameBaseAccountGameImpl gameServerImpl = userObject.GetAccountImpl<GameBaseAccountGameImpl>();
			gameServerImpl._Info.Alive = true;
			gameServerImpl._Info.UserCount = packet.CurrentUserCount;
			Logger.Default.Log(ELogLevel.Always, "GM_STATE_INFO_NOTI {0}", packet.ToString());
        }
	}
}
