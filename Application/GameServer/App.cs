using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;

namespace GameServer
{
	public class GameServerApp : ServerApp
	{
		public GameServerApp()
		{
		}

		~GameServerApp()
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
			UserObject obj = new UserObject();
			session.SetUserObject(obj);
			obj.SetSocketSession(session);

			GameBaseTemplateContext.AddTemplate<UserObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
			AccountController.AddAccountController(session.GetUid());
			GameBaseTemplateContext.CreateClient(session.GetUid());
			obj.OnAccept(localEP);
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
	}
}
