#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;
using System.Net.Sockets;

namespace GameBase.Template.Account.GameBaseAccount
{
	public partial class GameBaseAccountTemplate
	{
		public void ON_GC_HELLO_NOTI_CALLBACK(ImplObject userObject, PACKET_GC_HELLO_NOTI packet)
		{
			PACKET_CG_CHECK_AUTH_REQ sendData = new PACKET_CG_CHECK_AUTH_REQ();
			sendData.ProtocolGUID = PacketDefine.PROTOCOL_GUID;
			sendData.Ver = "1.0.0";
			sendData.Passport = userObject.GetAccountImpl<GameBaseAccountClientImpl>()._Passport;
			userObject.GetSession().SendPacket(sendData.Serialize());
        }
    }
}
