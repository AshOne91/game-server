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
		public void ON_LM_SESSION_INFO_REQ_CALLBACK(ImplObject userObject, PACKET_LM_SESSION_INFO_REQ packet)
		{
			UserSessionData session = null;
			bool result = GameBaseTemplateContext._SessionManager.GetUserSession(packet.SiteUserId, out session);

			PACKET_LM_SESSION_INFO_RES sendData = new PACKET_LM_SESSION_INFO_RES();
			sendData.Found = result;
			sendData.Uid = packet.Uid;
			if (result && session != null)
            {
				sendData.State = (int)session.SessionState;
				sendData.ServerId = session.ServerIdx;
				//sendData.RoomIdx = session.RoomIdx; //FIXME!
            }
			userObject.GetSession().SendPacket(sendData.Serialize());
		}
		public void ON_LM_SESSION_INFO_RES_CALLBACK(ImplObject userObject, PACKET_LM_SESSION_INFO_RES packet)
		{
			LoginUserObject loginUserObject = GameBaseTemplateContext.FindUserObj<LoginUserObject>(packet.Uid);
			if (loginUserObject != null)
            {
				OnSessionInfo(loginUserObject, userObject as MasterClientObject, packet.Found, 0/*FixMe*/, packet.ServerId, packet.State);
			}
		}

		public void OnSessionInfo(LoginUserObject loginUserObject, MasterClientObject masterClientObject, bool Found, UInt64 RoomIdx/*FixMe*/, int ServerId, int state)
        {
			if (Found)
            {
				loginUserObject._connInfo.Location = RoomIdx;
				loginUserObject._connInfo.ConnType = (int)ConnectType.Reconnect;

				if (state == (int)SessionState.PendingDisconnect || 
					state == (int)SessionState.PendingLogout)
                {
					Logger.Default.Log(ELogLevel.Trace, "Id {0} PendingState {1}", _SiteUserId, state);
					PACKET_CL_CHECK_AUTH_RES sendPacket = new PACKET_CL_CHECK_AUTH_RES();
					sendPacket.ErrorCode = (int)GServerCode.PENDING_ERROR;
					loginUserObject.GetSession().SendPacket(sendPacket.Serialize());
					return;
				}

				if (state != (int)SessionState.Logout &&
					state != (int)SessionState.Disconnect)
                {
					//������ Ȯ���ϱ� /FIXME
					Logger.Default.Log(ELogLevel.Err, "Duplicate Login State:{0}", state.ToString());
					PACKET_LM_DUPLICATE_LOGIN_NOTI sendData = new PACKET_LM_DUPLICATE_LOGIN_NOTI();
					sendData.StieUserId = _SiteUserId;
					masterClientObject.GetSession().SendPacket(sendData.Serialize());

					PACKET_CL_CHECK_AUTH_RES sendPacket = new PACKET_CL_CHECK_AUTH_RES();
					sendPacket.ErrorCode = (int)GServerCode.DUPLICATED_LOGIN;
					loginUserObject.GetSession().SendPacket(sendPacket.Serialize());

					//���������� Ȯ���ʿ�
					loginUserObject.GetSession().ShutDown();
					return;
				}
            }

			GameServerInfo info = GameBaseTemplateContext.GetTemplate<GameBaseAccountTemplate>(ETemplateType.Account).GetGameServerInfo(new List<int> { loginUserObject._WantedServerId, ServerId });

			if (info == null)
            {
				PACKET_CL_CHECK_AUTH_RES sendData = new PACKET_CL_CHECK_AUTH_RES();
				sendData.ErrorCode = (int)GServerCode.ERROR_NO_GAME_SERVER;
				loginUserObject.GetSession().SendPacket(sendData.Serialize());
			}
			else
            {
				loginUserObject._connInfo.ConnType = (int)ConnectType.Normal;
				loginUserObject._connInfo.Location = 0;
				loginUserObject._connInfo.Ip = info.Ip;
				loginUserObject._connInfo.Port = info.Port;
				loginUserObject._connInfo.ServerId = info.ServerId;
			}
        }

		void SendAuthResult(LoginUserObject loginUserObject)
        {
			string id;
			string extra;

			id = loginUserObject._Si
        }
	}
}
