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
			GameBaseAccountUserImpl loginUserImpl = loginUserObject.GetAccountImpl<GameBaseAccountUserImpl>();
			if (Found)
            {
				loginUserImpl._connInfo.Location = RoomIdx;
				loginUserImpl._connInfo.ConnType = (int)ConnectType.Reconnect;

				if (state == (int)SessionState.PendingDisconnect || 
					state == (int)SessionState.PendingLogout)
                {
					Logger.Default.Log(ELogLevel.Trace, "Id {0} PendingState {1}", loginUserImpl._SiteUserId, state);
					PACKET_CL_CHECK_AUTH_RES sendPacket = new PACKET_CL_CHECK_AUTH_RES();
					sendPacket.ErrorCode = (int)GServerCode.PENDING_ERROR;
					loginUserObject.GetSession().SendPacket(sendPacket.Serialize());
					return;
				}

				if (state != (int)SessionState.Logout &&
					state != (int)SessionState.Disconnect)
                {
					//재접속 확인하기 /FIXME
					Logger.Default.Log(ELogLevel.Err, "Duplicate Login State:{0}", state.ToString());
					PACKET_LM_DUPLICATE_LOGIN_NOTI sendData = new PACKET_LM_DUPLICATE_LOGIN_NOTI();
					sendData.StieUserId = loginUserImpl._SiteUserId;
					masterClientObject.GetSession().SendPacket(sendData.Serialize());

					PACKET_CL_CHECK_AUTH_RES sendPacket = new PACKET_CL_CHECK_AUTH_RES();
					sendPacket.ErrorCode = (int)GServerCode.DUPLICATED_LOGIN;
					loginUserObject.GetSession().SendPacket(sendPacket.Serialize());

					//문제있을시 확인필요
					loginUserObject.GetSession().ShutDown();
					return;
				}
            }

			GameServerInfo info = GetGameBaseAccountImpl().GetGameServerInfo(new List<int> { loginUserImpl._WantedServerId, ServerId });

			if (info == null)
            {
				PACKET_CL_CHECK_AUTH_RES sendData = new PACKET_CL_CHECK_AUTH_RES();
				sendData.ErrorCode = (int)GServerCode.ERROR_NO_GAME_SERVER;
				loginUserObject.GetSession().SendPacket(sendData.Serialize());
			}
			else
            {
				loginUserImpl._connInfo.ConnType = (int)ConnectType.Normal;
				loginUserImpl._connInfo.Location = 0;
				loginUserImpl._connInfo.Ip = info.Ip;
				loginUserImpl._connInfo.Port = info.Port;
				loginUserImpl._connInfo.ServerId = info.ServerId;
				SendAuthResult(loginUserObject);
			}
        }

		void SendAuthResult(LoginUserObject loginUserObject)
        {
			string id;
			string extra;

			GameBaseAccountUserImpl loginUserImpl = loginUserObject.GetAccountImpl<GameBaseAccountUserImpl>();
			id = loginUserImpl._SiteUserId;
			DateTime now = DateTime.UtcNow;
			extra = String.Format("{0}{1}{2}{3}{4}{5};{6};{7};{8}", now.Year - 2000, now.Month, now.Hour, now.Minute, now.Second, now.Millisecond,
															(int)loginUserImpl._connInfo.ConnType, loginUserImpl._connInfo.Location, loginUserImpl._connInfo.ServerId);

			string passport = Passport.Encrypt(id, extra);

			PACKET_CL_CHECK_AUTH_RES sendData = new PACKET_CL_CHECK_AUTH_RES();
			sendData.ErrorCode = (int)GServerCode.SUCCESS;
			sendData.Passport = passport;
			sendData.IP = loginUserImpl._connInfo.Ip;
			sendData.Port = loginUserImpl._connInfo.Port;
			sendData.ServerId = loginUserImpl._connInfo.ServerId;
			loginUserObject.GetSession().SendPacket(sendData.Serialize());

			Logger.Default.Log(ELogLevel.Trace, "PACKET_AC_CHECK_AUTH_RES Success id:{0} extra:{1} passport:{2}", id, extra, passport);
			//문제있을시 확인필요
			loginUserObject.GetSession().ShutDown();
		}
	}
}
