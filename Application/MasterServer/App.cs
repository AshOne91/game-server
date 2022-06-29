using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;

namespace MasterServer
{
	public class MasterServerApp : ServerApp
	{
		public MasterServerApp()
		{
		}

		~MasterServerApp()
		{
			Destroy();
		}

		public override bool Create(ServerConfig config, int frame = 30)
		{
			bool result = base.Create(config, frame);

			GameBaseTemplateContext.AddTemplate(ETemplateType.Account, new GameBaseAccountTemplate());

			TemplateConfig templateConfig = new TemplateConfig();
			GameBaseTemplateContext.InitTemplate(templateConfig, ServerType.Master);
			GameBaseTemplateContext.LoadDataTable(templateConfig);

			PerformanceCounter._WarningEvent += OnPerfWarning;
			return result;
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
			ImplObject obj = null;
			int idx = 0;
			//FIXME
			if (localEP.Port == 30000)
            {
				idx = AllocServerIdx(ObjectType.Game);
				obj = new GameServerObject();
				GameBaseAccountTemplate.GetGameBaseAccountImpl()._GameServerObjMap.Add(idx, (GameServerObject)obj);
			}
			else if (localEP.Port == 40000)
            {
				idx = AllocServerIdx(ObjectType.Login);
				obj = new LoginServerObject();
				GameBaseAccountTemplate.GetGameBaseAccountImpl()._LoginServerObjMap.Add(idx, (LoginServerObject)obj);
			}

			session.SetUserObject(obj);
			obj.SetSocketSession(session);
			GameBaseTemplateContext.AddTemplate<ImplObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
			AccountController.AddAccountController(session.GetUid());
			GameBaseTemplateContext.CreateClient(session.GetUid());
			obj.OnAccept(localEP);

			if (localEP.Port == 30000)
            {
				obj.GetAccountImpl<GameBaseAccountLoginImpl>()._ServerId = idx;
				GetAccountTemplate(obj).MG_HELLO_NOTI();
			}
			else if (localEP.Port == 40000)
            {
				obj.GetAccountImpl<GameBaseAccountGameImpl>()._Info.ServerId = idx;
				GetAccountTemplate(obj).ML_HELLO_NOTI();
			}
		}

		private bool _bListenState = false;
		private void ListenUsers(bool bNewState)
		{
			if (_bListenState == bNewState) return;

			_bListenState = bNewState;
			if (_bListenState)
			{
				Logger.Default.Log(ELogLevel.Always, "Start Listen {0} ", 10000/*TODO : 설정파일 읽는거로 변경하기*/);
				IPEndPoint epClient = new IPEndPoint(IPAddress.Any, 10000);
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

		public override void OnClose(SocketSession session)
		{
			UserObject userObj = session.GetUserObject();
			if (userObj != null)
			{
				if (ObjectType.Game == (ObjectType)userObj.GetObjectID())
                {
					GameServerObject gameObj = session.GetUserObject() as GameServerObject;
					GameBaseAccountTemplate.GetGameBaseAccountImpl()._GameServerObjMap.Remove(gameObj.GetAccountImpl<GameBaseAccountGameImpl>()._Info.ServerId, out gameObj);

					//FIXME UpdateSessionInfo 완성후 넣기
					//MasterServerEntry.GetUserSessionManager().RemoveSessionByServerID(_Info.ServerId);
				}
				else if (ObjectType.Login == (ObjectType)userObj.GetObjectID())
                {
					LoginServerObject loginObj = session.GetUserObject() as LoginServerObject;
					GameBaseAccountTemplate.GetGameBaseAccountImpl()._LoginServerObjMap.Remove(loginObj.GetAccountImpl<GameBaseAccountLoginImpl>()._ServerId, out loginObj);
				}
				GameBaseTemplateContext.DeleteClient(userObj.GetSession().GetUid());
				userObj.OnClose();
				userObj.Dispose();
				session.SetUserObject(null);
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

		public override void OnTimer(TimerHandle timer)
		{
		}

		public override void OnUpdate(float dt)
		{
			GameBaseTemplateContext.UpdateClient(dt);
			GameBaseTemplateContext.UpdateTemplate(dt);
		}

		public int AllocServerIdx(ObjectType type)
        {
			switch(type)
            {
				case ObjectType.Login:
                    {
						for (int i = (int)ObjectType.Login * 10; i < ((int)ObjectType.Login + 100) * 10; ++i)
                        {
							bool result = GameBaseAccountTemplate.GetGameBaseAccountImpl()._GameServerObjMap.ContainsKey(i);
							if (result == false)
                            {
								return i;
                            }
                        }
                    }
					break;
				case ObjectType.Game:
                    {
						for (int i = (int)ObjectType.Game * 10; i < ((int)ObjectType.Game + 100) * 10; ++i)
                        {
							bool result = GameBaseAccountTemplate.GetGameBaseAccountImpl()._LoginServerObjMap.ContainsKey(i);
							if (result == false)
                            {
								return i;
                            }
                        }
                    }
					break;
            }
			throw new Exception("Can't not Alloc Index");
        }

		public GameBaseAccountTemplate GetAccountTemplate(UserObject obj)
        {
			return GameBaseTemplateContext.GetTemplate<GameBaseAccountTemplate>(obj.GetSession().GetUid(), ETemplateType.Account);
		}
	}
}
