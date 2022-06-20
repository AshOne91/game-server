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
			_CheckVersion = true;
		}
		public void ON_CL_CHECK_VERSION_RES_CALLBACK(ImplObject userObject, PACKET_CL_CHECK_VERSION_RES packet)
		{

		}

		//public bool _CheckVersion = false;
	}
}
