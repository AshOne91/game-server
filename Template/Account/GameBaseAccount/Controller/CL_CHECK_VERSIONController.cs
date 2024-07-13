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
		public void ON_CL_CHECK_VERSION_REQ_CALLBACK(ImplObject userObject, PACKET_CL_CHECK_VERSION_REQ packet)
		{
			if (packet.ProtocolGUID != PacketDefine.PROTOCOL_GUID)
            {
				Logger.Default.Log(ELogLevel.Err, "Invalid GUID LS:{0} MS:{1}", packet.ProtocolGUID, PacketDefine.PROTOCOL_GUID);

				PACKET_CL_CHECK_VERSION_RES sendResData = new PACKET_CL_CHECK_VERSION_RES();
				sendResData.ErrorCode = (int)GServerCode.INVALID_GUID;
				userObject.GetSession().SendPacket(sendResData.Serialize());
				userObject.GetSession().Disconnect();
				return;
			}

			PACKET_CL_CHECK_VERSION_RES sendData = new PACKET_CL_CHECK_VERSION_RES();
			sendData.ErrorCode = (int)GServerCode.SUCCESS;
			userObject.GetSession().SendPacket(sendData.Serialize());
			userObject.GetAccountImpl<GameBaseAccountUserImpl>()._CheckVersion = true;
		}
		public void ON_CL_CHECK_VERSION_RES_CALLBACK(ImplObject userObject, PACKET_CL_CHECK_VERSION_RES packet)
		{
			if (packet.ErrorCode == (int)GServerCode.SUCCESS)
			{
				PACKET_CL_CHECK_AUTH_REQ sendData = new PACKET_CL_CHECK_AUTH_REQ();
				userObject.GetAccountImpl<GameBaseAccountClientImpl>()._SiteUserId = userObject.UId.ToString();
				userObject.GetAccountImpl<GameBaseAccountClientImpl>()._PlatformType = (int)EPlatformType.Guest;
                sendData.SiteUserId = userObject.GetAccountImpl<GameBaseAccountClientImpl>()._SiteUserId;
				sendData.WantedServerId = -1;
				sendData.PlatformType = userObject.GetAccountImpl<GameBaseAccountClientImpl>()._PlatformType;
                userObject.GetSession().SendPacket(sendData.Serialize());
            }
			else
			{
				Logger.Default.Log(ELogLevel.Err, "CL_CHECK_VERSION_RES_CALLBACK ERR");
			}
		}
	}
}
