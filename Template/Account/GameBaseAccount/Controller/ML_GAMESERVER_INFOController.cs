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
		public void ON_ML_GAMESERVER_INFO_NOTI_CALLBACK(ImplObject userObject, PACKET_ML_GAMESERVER_INFO_NOTI packet)
		{
			//LoginServer
			//MasterClientObject
			GameBaseAccountTemplate.GetGameBaseAccountImpl().SetGameServerInfo(packet.GameServerInfoList);
		}
	}
}
