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
			userObject.GetAccountImpl<GameBaseAccountUserImpl>()._PlatformType = packet.PlatformType;

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
			if ((GServerCode)packet.ErrorCode != GServerCode.SUCCESS)
			{
				Logger.Default.Log(ELogLevel.Trace, "Auth Check Error");
				return;
			}

			//userObject.GetAccountImpl<GameBaseAccountClientImpl>()._SiteUserId = packet.;
			userObject.GetAccountImpl<GameBaseAccountClientImpl>()._Passport = packet.Passport;
			userObject.GetAccountImpl<GameBaseAccountClientImpl>()._IP = packet.IP;
			userObject.GetAccountImpl<GameBaseAccountClientImpl>()._Port = packet.Port;
			userObject.GetAccountImpl<GameBaseAccountClientImpl>()._LoginAuth = true;
			userObject.GetAccountImpl<GameBaseAccountClientImpl>().ClientCallback("GameAuth");
        }
	}
}
