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
			GameBaseTemplateContext.InitTemplate(templateConfig);
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
			UserObject obj = null;
			int idx = 0;
			if (localEP.Port == 30000)
            {
				idx = AllocServerIdx(ObjectType.Game);
				obj = new GameServerObject(idx);
				_GameServerObjMap.Add(idx, (GameServerObject)obj);
			}
			else if (localEP.Port == 40000)
            {
				idx = AllocServerIdx(ObjectType.Login);
				obj = new LoginServerObject(idx);
				_LoginServerObjMap.Add(idx, (LoginServerObject)obj);
			}

			session.SetUserObject(obj);
			obj.SetSocketSession(session);
			GameBaseTemplateContext.AddTemplate<UserObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
			AccountController.AddAccountController(session.GetUid());
			GameBaseTemplateContext.CreateClient(session.GetUid());
			obj.OnAccept(localEP);

			if (localEP.Port == 3000)
            {
				GetAccountTemplate(obj)._ServerId = idx;
			}
			else if (localEP.Port == 4000)
            {
				GetAccountTemplate(obj)._ServerId = idx;
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
			AccountController.OnPacket(session.GetUserObject(), packet.GetId(), packet);
		}

		public override void OnTimer(TimerHandle timer)
		{
		}

		public override void OnUpdate(float dt)
		{
			GameBaseTemplateContext.UpdateClient(dt);
		}

		public int AllocServerIdx(ObjectType type)
        {
			switch(type)
            {
				case ObjectType.Login:
                    {
						for (int i = (int)ObjectType.Login * 10; i < ((int)ObjectType.Login + 100) * 10; ++i)
                        {
							bool result = _GameServerObjMap.ContainsKey(i);
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
							bool result = _LoginServerObjMap.ContainsKey(i);
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

		Dictionary<int, GameServerObject> _GameServerObjMap = new Dictionary<int, GameServerObject>();
		Dictionary<int, LoginServerObject> _LoginServerObjMap = new Dictionary<int, LoginServerObject>();
	}
}
