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
		public void ON_CL_GEN_GUEST_ID_REQ_CALLBACK(ImplObject userObject, PACKET_CL_GEN_GUEST_ID_REQ packet)
		{
			GameBaseAccountUserImpl loginImpl = userObject.GetAccountImpl<GameBaseAccountUserImpl>();
			if (loginImpl._CheckVersion == false)
            {
				Logger.Default.Log(ELogLevel.Trace, "CheckVersion is false");
				userObject.GetSession().Disconnect();
				return;
            }

			loginImpl._SiteUserId = "G" + Guid.NewGuid().ToString();

			PACKET_CL_GEN_GUEST_ID_RES sendData = new PACKET_CL_GEN_GUEST_ID_RES();
			sendData.SiteUserId = loginImpl._SiteUserId;
			userObject.GetSession().SendPacket(sendData.Serialize());
		}
		public void ON_CL_GEN_GUEST_ID_RES_CALLBACK(ImplObject userObject, PACKET_CL_GEN_GUEST_ID_RES packet)
		{
		}
	}
}
