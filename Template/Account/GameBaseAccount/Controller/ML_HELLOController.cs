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
		public void ON_ML_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_ML_HELLO_NOTI packet)
		{
			PACKET_LM_CHECK_AUTH_REQ sendData = new PACKET_LM_CHECK_AUTH_REQ();
			sendData.ServerGUID = PacketDefine.SERVER_GUID;
			sendData.Ver = "1.0.0";
			sendData.HostIP = "127.0.0.1"; //FIXME
			sendData.HostPort = 10000;//FIXME

			userObject.GetSession().SendPacket(sendData.Serialize());
		}
	}
}
