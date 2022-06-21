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
		public void ON_GM_CHECK_AUTH_REQ_CALLBACK(ImplObject userObject, PACKET_GM_CHECK_AUTH_REQ packet)
		{
			//MasterServer
			//GameServerObject
			GameBaseAccountGameImpl gameServerImpl = userObject.GetAccountImpl<GameBaseAccountGameImpl>();
			if (packet.ServerGUID != PacketDefine.SERVER_GUID)
            {
				Logger.Default.Log(ELogLevel.Err, "Invalid GUID GS:{0} MS:{1}", packet.ServerGUID, PacketDefine.SERVER_GUID);
				userObject.GetSession().Disconnect();
				return;
            }

			PACKET_GM_CHECK_AUTH_RES sendData = new PACKET_GM_CHECK_AUTH_RES();
			sendData.ServerId = gameServerImpl._Info.ServerId;
			userObject.GetSession().SendPacket(packet.Serialize());

			gameServerImpl._Info.Ip = packet.IP;
			gameServerImpl._Info.Port = packet.Port;
			gameServerImpl._Auth = true;

			Logger.Default.Log(ELogLevel.Always, "Success GM_CHECK_AUTH_REQ ServerID:{0} IP:{1} Port:{2}", gameServerImpl._Info.ServerId, gameServerImpl._Info.Ip, gameServerImpl._Info.Port);
		}
		public void ON_GM_CHECK_AUTH_RES_CALLBACK(ImplObject userObject, PACKET_GM_CHECK_AUTH_RES packet)
		{
			//GameServer
			//MasterClientObject
			GameBaseAccountMasterImpl masterClientImpl = userObject.GetAccountImpl<GameBaseAccountMasterImpl>();
			if (packet.ErrorCode == (int)GServerCode.SUCCESS)
            {
				// FIXME//TODO
				// �����ͼ����� ������ �� �翬�� �Ǹ� ���� ID�� ���� �ο��Ǵµ�, ���⼭ �̷����� ������ ����� ����
				// ���Ӽ����� �ѹ� �ο��� ���� ID�� �ٲ��� �ʴ°� ���?

				//GameServer�� �����ӽ� ���� �������� ������Ʈ �ϴ� ��Ŷ �ʿ�
				masterClientImpl._ServerId = packet.ServerId;

				Logger.Default.Log(ELogLevel.Always, "SUCCESS PACKET_GM_CHECK_AUTH_RES Result {0}", packet.ErrorCode);
			}
			else
            {

            }
		}
	}
}
