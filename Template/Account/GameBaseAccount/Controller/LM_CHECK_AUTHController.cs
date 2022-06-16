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
		public void ON_LM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_REQ packet)
		{
			//MasterServer
			if (packet.ServerGUID != PacketDefine.SERVER_GUID)
            {
				Logger.Default.Log(ELogLevel.Err, "Invalid GUID LS:{0} MS:{1}", packet.ServerGUID, PacketDefine.SERVER_GUID);
				userObject.GetSession().Disconnect();
				return;
            }

			_LoginHostIP = packet.HostIP;
			_LoginHostPort = packet.HostPort;

			Logger.Default.Log(ELogLevel.Always, "Success LM_CHECK_AUTH_REQ ServerID:{0} IP:{1} Port:{2}", _ServerId, _LoginHostIP, _LoginHostPort);

			PACKET_LM_CHECK_AUTH_RES sendData = new PACKET_LM_CHECK_AUTH_RES();
			sendData.ServerId = _ServerId;
			userObject.GetSession().SendPacket(sendData.Serialize());
		}
		public void ON_LM_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_RES packet)
		{
			//LoginServer
			if (packet.ErrorCode == (int)GServerCode.SUCCESS)
            {
				_ServerId = packet.ServerId;
				Logger.Default.Log(ELogLevel.Always, "SUCCESS LM_CHECK_AUTH_RES Result {0}", packet.ErrorCode);
            }
			else
            {
				Logger.Default.Log(ELogLevel.Fatal, "ERROR LM_CHECK_AUTH_RES Result {0}", packet.ErrorCode);
            }
		}

		//MasterServerParam
		public string _LoginHostIP;
		public ushort _LoginHostPort;
		public bool _Auth = false;

		//common
		public int _ServerId = -1;
	}
}
