using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.Item.GameBaseItem;
using GameBase.Template.Shop.GameBaseShop;
using System.Diagnostics;
using PerformanceCounter = Service.Core.PerformanceCounter;
using Service.DB;
using GameBase.Template.GameBase.Common;

namespace GameServer
{
	public class GameServerApp : ServerApp
	{
		private AppConfig _appConfig = null;
		public AppConfig AppConfig
		{
			get { return _appConfig; }
			set 
			{
				_appConfig = value;
				GameBaseTemplateContext.AppConfig = _appConfig;
            }
		}
		public GameServerApp()
		{
		}

		~GameServerApp()
		{
			Destroy();
		}

		/*public override bool Create(ServerConfig config, int frame = 30)
		{
			bool result = base.Create(config, frame);

			GameBaseTemplateContext.AddTemplate(ETemplateType.Account, new GameBaseAccountTemplate());
			GameBaseTemplateContext.AddTemplate(ETemplateType.Item, new GameBaseItemTemplate());
			GameBaseTemplateContext.AddTemplate(ETemplateType.Shop, new GameBaseShopTemplate());
			GameBaseTemplateContext.InitTemplate(templateConfig, ServerType.Game);
			GameBaseTemplateContext.LoadDataTable(templateConfig);

            Service.Core.PerformanceCounter._WarningEvent += OnPerfWarning;
			return result;
		}*/

		public bool Create(AppConfig config, int frame = 30)
		{
			bool result = Create(config.serverConfig, frame);
            GameBaseTemplateContext.AddTemplate(ETemplateType.Account, new GameBaseAccountTemplate());
            GameBaseTemplateContext.AddTemplate(ETemplateType.Item, new GameBaseItemTemplate());
            GameBaseTemplateContext.AddTemplate(ETemplateType.Shop, new GameBaseShopTemplate());
            GameBaseTemplateContext.InitTemplate(config.templateConfig, ServerType.Game);
            GameBaseTemplateContext.LoadDataTable(config.templateConfig);

            Service.Core.PerformanceCounter._WarningEvent += OnPerfWarning;
            return result;
		}

		public bool ConnectToMaster()
        {
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse(AppConfig.serverConfig.MasterIP), AppConfig.serverConfig.MasterPort);

			Logger.Default.Log(ELogLevel.Always, "Try Connect to MasterServer {0}:{1}", AppConfig.serverConfig.MasterIP, AppConfig.serverConfig.MasterPort);

			SocketSession ss = OpenConnection(ep);
			if (ss != null)
			{
				return true;
			}

			return false;
		}

		public void OnPerfWarning(int tick)
		{
			Logger.Default.Log(ELogLevel.Warn, "OnPerfWarning");
		}

		public override void Destroy()
		{
			base.Destroy();

			PerformanceCounter.Print();
			Logger.Default.Destroy();
		}

		public override void OnAccept(SocketSession session, IPEndPoint localEP, IPEndPoint remoteEP)
		{
			if (localEP.Port == AppConfig.serverConfig.Port)
			{
				GameUserObject obj = new GameUserObject();
				session.SetUserObject(obj);
				obj.SetSocketSession(session);

				GameBaseTemplateContext.AddTemplate<GameUserObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
				GameBaseTemplateContext.AddTemplate<GameUserObject>(obj, ETemplateType.Item, new GameBaseItemTemplate());
				GameBaseTemplateContext.AddTemplate<GameUserObject>(obj, ETemplateType.Shop, new GameBaseShopTemplate());
				AccountController.AddAccountController(session.GetUid());
				ItemController.AddItemController(session.GetUid());
				ShopController.AddShopController(session.GetUid());

				obj.OnAccept(localEP);
                GameBaseTemplateContext.CreateClient(session.GetUid());
                Logger.Default.Log(ELogLevel.Trace, "Client onaccept called, Address-{0}, Port-{1}", localEP.Address.ToString(), localEP.Port);
				GameBaseTemplateContext.GetTemplate<GameBaseAccountTemplate>(obj.GetSession().GetUid(), ETemplateType.Account).GC_HELLO_NOTI();
            }
		}

		private bool _bListenState = false;
		private void ListenUsers(bool bNewState)
		{
			if (_bListenState == bNewState) return;

			_bListenState = bNewState;
			if (_bListenState)
			{
				Logger.Default.Log(ELogLevel.Always, "Start Listen {0} ", AppConfig.serverConfig.Port);
				IPEndPoint epClient = new IPEndPoint(IPAddress.Any, AppConfig.serverConfig.Port);
				BeginAcceptor(epClient);
			}
			else
			{
				if (_listeners != null)
				{
					foreach (TcpListener listener in _listeners)
					{
						listener.Stop();
					}
					_listeners.Clear();
				}
			}
		}

		public int cnt = 0;
		public Int64 wholeSendCount = 0;
		public Int64 wholeRecvCount = 0;
		public int repeatCount = 0;

		public void PrintIO()
		{
			Logger.Default.Log(ELogLevel.Always, "Connection Count:{0}", this._sessionManager.GetActiveSessionCount());
			Logger.Default.Log(ELogLevel.Always, "SendBytes:{0}", _totalSendBytes);
			Logger.Default.Log(ELogLevel.Always, "RecvBytes:{0}", _totalRecvBytes);
			Logger.Default.Log(ELogLevel.Always, "SendCount:{0}", _totalSendCount);
			Logger.Default.Log(ELogLevel.Always, "RecvCount:{0}", _totalRecvCount);


			if (_totalSendCount != 0 && _totalRecvCount != 0)
			{
				repeatCount++;
				wholeSendCount += _totalSendCount;
				wholeRecvCount += _totalRecvCount;

				Logger.Default.Log(ELogLevel.Always, "Avg SendCount:{0}", wholeSendCount / repeatCount);
				Logger.Default.Log(ELogLevel.Always, "Avg RecvCount:{0}", wholeRecvCount / repeatCount);
			}
			Logger.Default.Log(ELogLevel.Always, "Loop:{0}", cnt);
			cnt = 0;
		}

        public override void OnConnect(SocketSession session, IPEndPoint ep)
        {
			Logger.Default.Log(ELogLevel.Always, "OnConnect {0}", ep.ToString());

			if (ep.Port == AppConfig.serverConfig.MasterPort)
			{
				ImplObject obj = new MasterClientObject();
                session.SetUserObject(obj);
                obj.SetSocketSession(session);

				GameBaseTemplateContext.AddTemplate<ImplObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
				GameBaseTemplateContext.AddTemplate<ImplObject>(obj, ETemplateType.Item, new GameBaseItemTemplate());
				GameBaseTemplateContext.AddTemplate<ImplObject>(obj, ETemplateType.Shop, new GameBaseShopTemplate());
				AccountController.AddAccountController(session.GetUid());
				ItemController.AddItemController(session.GetUid());
				ShopController.AddShopController(session.GetUid());
				obj.OnConnect(ep);
				GameBaseTemplateContext.CreateClient(session.GetUid());
				ListenUsers(true);
			}
		}
        public override void OnConnectFailed(SocketSession session, string e)
        {
			Logger.Default.Log(ELogLevel.Always, "OnConnectFailed {0}", e);
			if (GameBaseTemplateContext.GetObjectCount((ulong)ObjectType.Master) <= 0)
			{
				AddTimer((uint)ObjectType.Master, 1000, null);
			}
		}
        public override void OnClose(SocketSession session)
		{
			UserObject userObj = session.GetUserObject();
			if (userObj != null)
			{
				GameUserObject gameUserObject = userObj as GameUserObject;
				if (gameUserObject != null)
				{
					if (gameUserObject.SessionData.SessionState != (int)SessionState.Logout
						&& gameUserObject.SessionData.SessionState != (int)SessionState.PendingLogout)
					{
						gameUserObject.SessionData.SessionState = (int)SessionState.PendingDisconnect;
						GameBaseTemplateContext.Account.UpdateSessionInfo(gameUserObject);

						Logger.Default.Log(ELogLevel.Trace, "PendingDisconnect {0}", gameUserObject.UserDBKey);
					}

					//DBSaveAll()
					DBGameUserSave query = new DBGameUserSave();
					query._isConnected = gameUserObject.GetSession().IsConnected();
					query._user_db_key = gameUserObject.UserDBKey;
					query._player_db_key = gameUserObject.PlayerDBKey;
					query._userDB.Copy(gameUserObject.GetUserDB(), true);
					GameBaseTemplateContext.GetDBManager().PushQueryGame(gameUserObject.UserDBKey, gameUserObject.GameDBIdx, 0, query);

					DBGlobal_User_Logout userLogoutQuery = new DBGlobal_User_Logout();
                    userLogoutQuery._encode_account_id = gameUserObject.GetAccountImpl<GameBaseAccountUserImpl>()._SiteUserId;
					userLogoutQuery._user_db_key = gameUserObject.UserDBKey;
					GameBaseTemplateContext.GetDBManager().PushQueryGlobal(gameUserObject.UserDBKey, userLogoutQuery, () =>
					{
						if (gameUserObject.SessionData.SessionState == (int)SessionState.PendingDisconnect)
						{
							gameUserObject.SessionData.SessionState = (int)SessionState.Disconnect;
                        }
						else if (gameUserObject.SessionData.SessionState == (int)SessionState.PendingLogout)
						{
							gameUserObject.SessionData.SessionState = (int)SessionState.Logout;
						}

						Logger.Default.Log(ELogLevel.Trace, "Disconnect", gameUserObject.UserDBKey);
						GameBaseTemplateContext.Account.UpdateSessionInfo(gameUserObject);
					});
				 }

				ObjectType type = (ObjectType)userObj.ObjectID;
				GameBaseTemplateContext.DeleteClient(userObj.GetSession().GetUid());
                AccountController.RemoveAccountController(userObj.GetSession().GetUid());
				ItemController.RemoveItemController(userObj.GetSession().GetUid());
				ShopController.RemoveShopController(userObj.GetSession().GetUid());
                userObj.OnClose();
				userObj.Dispose();
				session.SetUserObject(null);
				if (type == ObjectType.Master)
                {
					// 마스터서버에서 끊어졌을 때, 유저 접속을 받지 않으려면 아래 uncomment
					//ListenUsers(false);
					ConnectToMaster();
				}
			}
		}

		public override void OnSocketError(SocketSession session, string e)
		{
			Logger.Default.Log(ELogLevel.Err, "OnSocketError = {{0}}", e);
			session.Disconnect();
		}

		public override void OnPacket(SocketSession session, Packet packet)
		{
			try
			{
				ImplObject obj = session.GetUserObject() as ImplObject;
				if (obj != null)
				{
					AccountController.OnPacket(obj, packet.GetId(), packet);
					ItemController.OnPacket(obj, packet.GetId(), packet);
					ShopController.OnPacket(obj, packet.GetId(), packet);

				}
				else
				{
					Logger.Default.Log(ELogLevel.Err, "wrong session OnPacket");
				}
			}
			catch (FatalException e)
            {
				Logger.WriteExceptionLog(e);
				throw e;
            }
			catch (Exception e)
            {
				Logger.WriteExceptionLog(e);
				session.Disconnect();
            }
		}

        public override void OnSendComplete(SocketSession session, int transBytes)
        {
			var obj = session.GetUserObject();
			if (obj != null)
            {
				obj.OnSendComplete();
            }
        }

		public override void OnPacketError(SocketSession session, Packet packet)
		{
			Logger.Default.Log(ELogLevel.Err, "OnPacketError = {0}", packet.GetId());
			session.Disconnect();
		}

		public override void OnTimer(TimerHandle timer)
		{
			if (timer._TimerType == (uint)ObjectType.Master)
            {
				ConnectToMaster();
            }
		}

		public override void OnUpdate(float dt)
		{
			GameBaseTemplateContext.UpdateClient(dt);
			GameBaseTemplateContext.GetDBManager().OnLoop();
		}
	}
}
