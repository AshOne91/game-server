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
		public void ON_LM_CHECK_AUTH_REQ_CALLBACK(ImplObject userObject, PACKET_LM_CHECK_AUTH_REQ packet)
		{
			//MasterServer
			if (packet.ServerGUID != PacketDefine.SERVER_GUID)
            {
				Logger.Default.Log(ELogLevel.Err, "Invalid GUID LS:{0} MS:{1}", packet.ServerGUID, PacketDefine.SERVER_GUID);
				userObject.GetSession().Disconnect();
				return;
            }

			var LoginImpl = userObject.GetAccountImpl<GameBaseAccountLoginImpl>();
			LoginImpl._HostIP = packet.HostIP;
			LoginImpl._HostPort = packet.HostPort;

			Logger.Default.Log(ELogLevel.Always, "Success LM_CHECK_AUTH_REQ ServerID:{0} IP:{1} Port:{2}", LoginImpl._ServerId, LoginImpl._HostIP, LoginImpl._HostPort);

			PACKET_LM_CHECK_AUTH_RES sendData = new PACKET_LM_CHECK_AUTH_RES();
			sendData.ServerId = LoginImpl._ServerId;
			userObject.GetSession().SendPacket(sendData.Serialize());
		}
		public void ON_LM_CHECK_AUTH_RES_CALLBACK(ImplObject userObject, PACKET_LM_CHECK_AUTH_RES packet)
		{
			//LoginServer
			if (packet.ErrorCode == (int)GServerCode.SUCCESS)
            {
				var MasterImpl = userObject.GetAccountImpl<GameBaseAccountMasterImpl>();
				MasterImpl._ServerId = packet.ServerId;
				Logger.Default.Log(ELogLevel.Always, "SUCCESS LM_CHECK_AUTH_RES Result {0}", packet.ErrorCode);
            }
			else
            {
				Logger.Default.Log(ELogLevel.Fatal, "ERROR LM_CHECK_AUTH_RES Result {0}", packet.ErrorCode);
            }
		}
	}
}
