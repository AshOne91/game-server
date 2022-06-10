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
		public void ON_CL_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_CL_CHECK_AUTH_REQ packet)
		{
			Logger.Default.Log(ELogLevel.Always, packet.SiteUserId);
			Logger.Default.Log(ELogLevel.Always, packet.WantedServerId.ToString());
			PACKET_CL_CHECK_AUTH_RES resultPacket = new PACKET_CL_CHECK_AUTH_RES();
			resultPacket.IP = "127.0.0.1";
			resultPacket.Passport = "test";
			resultPacket.Port = 20000;
			resultPacket.ServerId = 1;
			userObject.GetSession().SendPacket(resultPacket.Serialize());
		}
		public void ON_CL_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_CL_CHECK_AUTH_RES packet)
		{
		}
	}
}
