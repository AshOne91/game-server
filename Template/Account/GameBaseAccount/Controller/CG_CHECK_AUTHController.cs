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
		public void ON_CG_CHECK_AUTH_REQ_CALLBACK(ImplObject userObject, PACKET_CG_CHECK_AUTH_REQ packet)
		{
			//GameServer
			//UserObject
			if (packet.ProtocolGUID != PacketDefine.PROTOCOL_GUID)
            {
				Logger.Default.Log(ELogLevel.Err, "Invalid GUID Recv {0} : {1}", packet.ProtocolGUID, PacketDefine.PROTOCOL_GUID);

				PACKET_CG_CHECK_AUTH_RES sendData = new PACKET_CG_CHECK_AUTH_RES();
				sendData.ErrorCode = (int)GServerCode.INVALID_GUID;
				userObject.GetSession().SendPacket(sendData.Serialize());
				return;
			}

			string id = "";
			string extra = "";
			bool result = Passport.Vertify(packet.Passport, out id, out extra);
			if (result == false)
            {
				Logger.Default.Log(ELogLevel.Err, "Failed Passport.Vertify {0}", packet.Passport);
				userObject.GetSession().Disconnect();
				return;
            }
		}
		public void ON_CG_CHECK_AUTH_RES_CALLBACK(ImplObject userObject, PACKET_CG_CHECK_AUTH_RES packet)
		{
		}
	}
}
