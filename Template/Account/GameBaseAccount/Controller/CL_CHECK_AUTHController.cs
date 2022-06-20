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
		public void ON_CL_CHECK_AUTH_REQ_CALLBACK(ImplObject userObject, PACKET_CL_CHECK_AUTH_REQ packet)
		{
			if (userObject.GetAccountImpl<GameBaseAccountUserImpl>()._CheckVersion == false)
            {
				Logger.Default.Log(ELogLevel.Trace, "_CheckVersion is false");
				userObject.GetSession().Disconnect();
				return;
            }

			userObject.GetAccountImpl<GameBaseAccountUserImpl>()._SiteUserId = packet.SiteUserId;
			userObject.GetAccountImpl<GameBaseAccountUserImpl>()._WantedServerId = packet.WantedServerId;

			PACKET_LM_SESSION_INFO_REQ sendData = new PACKET_LM_SESSION_INFO_REQ();
			sendData.SiteUserId = userObject.GetAccountImpl<GameBaseAccountUserImpl>()._SiteUserId;
			sendData.Uid = userObject.GetSession().GetUid();
			ImplObject masterObj = GameBaseTemplateContext.FindUserObjFromType<MasterClientObject>((ulong)ObjectType.Master);
			if (masterObj == null)
            {
				Logger.Default.Log(ELogLevel.Always, "_MasterServer Down!");
				userObject.GetSession().Disconnect();
				return;
            }
			masterObj.GetSession().SendPacket(sendData.Serialize());
		}
		public void ON_CL_CHECK_AUTH_RES_CALLBACK(ImplObject userObject, PACKET_CL_CHECK_AUTH_RES packet)
		{
		}
	}
}
