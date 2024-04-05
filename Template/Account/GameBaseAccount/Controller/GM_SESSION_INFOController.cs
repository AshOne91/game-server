#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;
using static System.Collections.Specialized.BitVector32;

namespace GameBase.Template.Account.GameBaseAccount
{
	public partial class GameBaseAccountTemplate
	{
		public void ON_GM_SESSION_INFO_NOTI_CALLBACK(ImplObject userObject, PACKET_GM_SESSION_INFO_NOTI packet)
		{
			UserSessionData sessionData = packet.sessionData;

			Logger.Default.Log(ELogLevel.Always, "GM_SESSION_INFO_NOTI {0}", sessionData.GetLog());

			switch((SessionState)sessionData.SessionState) 
			{
				case SessionState.BeforeAuth:
					{
						Logger.Default.Log(ELogLevel.Err, "BeforeAuth - {0}", sessionData.GetLog());
                    }
                    break;
                case SessionState.Login:
					{
						GameBaseAccountTemplate.GetGameBaseAccountImpl()._UserSessionManager.RemoveSession(sessionData.SiteUserId);
						GameBaseAccountTemplate.GetGameBaseAccountImpl()._UserSessionManager.AddSession(sessionData);
                    }
					break;
				case SessionState.Logout:
					{
						GameBaseAccountTemplate.GetGameBaseAccountImpl()._UserSessionManager.RemoveSession(sessionData.SiteUserId);
                    }
					break;
				case SessionState.PendingLogout:
				case SessionState.Lobby:
				case SessionState.Playing:
				case SessionState.PendingDisconnect:
				case SessionState.Disconnect:
					{
						UserSessionData resultSession = null;
						bool result = GameBaseAccountTemplate.GetGameBaseAccountImpl()._UserSessionManager.GetUserSession(sessionData.SiteUserId, out resultSession);
						if (result == true)
						{
							GameBaseAccountTemplate.GetGameBaseAccountImpl()._UserSessionManager.UpdateSession(resultSession);
                        }
						else
						{
							GameBaseAccountTemplate.GetGameBaseAccountImpl()._UserSessionManager.AddSession(resultSession);

                        }
                    }
					break;
				default:
					{
						Logger.Default.Log(ELogLevel.Fatal, "Wrong State {0}", sessionData);
					}
					break;
			}
		}
	}
}
