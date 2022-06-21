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
		public void ON_MG_HELLO_NOTI_CALLBACK(ImplObject userObject, PACKET_MG_HELLO_NOTI packet)
		{
			PACKET_GM_CHECK_AUTH_REQ sendData = new PACKET_GM_CHECK_AUTH_REQ();
			sendData.ServerGUID = PacketDefine.SERVER_GUID;
			sendData.Ver = "1.0.0"; //FIXME
			sendData.IP = "127.0.0.1"; //FIXME
			sendData.Port = 30000; //FIXME

			Logger.Default.Log(ELogLevel.Trace, "Send PACKET_GM_CHECK_AUTH_REQ {0}", sendData.GetLog());
			userObject.GetSession().SendPacket(sendData.Serialize());
		}
	}
}
