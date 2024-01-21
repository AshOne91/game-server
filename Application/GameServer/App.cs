using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;
using System.Diagnostics;
using PerformanceCounter = Service.Core.PerformanceCounter;
using Service.DB;

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
			GameBaseTemplateContext.InitTemplate(templateConfig, ServerType.Game);
			GameBaseTemplateContext.LoadDataTable(templateConfig);

            Service.Core.PerformanceCounter._WarningEvent += OnPerfWarning;
			return result;
		}

		public bool ConnectToMaster()
        {
			//FIXME
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);

			Logger.Default.Log(ELogLevel.Always, "Try Connect to MasterServer {0}:{1}", "127.0.0.1", 20000);

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
			//FixMe
			if (localEP.Port == 30000)
			{
				GameUserObject obj = new GameUserObject();
				session.SetUserObject(obj);
				obj.SetSocketSession(session);

				GameBaseTemplateContext.AddTemplate<GameUserObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
				AccountController.AddAccountController(session.GetUid());
				GameBaseTemplateContext.CreateClient(session.GetUid());
				obj.OnAccept(localEP);
				/*Log(LogLevel.TRACE, "Client onaccept called, Address-{0}, Port-{1}", ep.Address.ToString(), ep.Port);
				NwPacket sendPacket = new NwPacket((ushort)EGPacketProtocol.GC_HELLO_NOTI);
				_session.SendPacket(sendPacket);*/
			}
		}

		private bool _bListenState = false;
		private void ListenUsers(bool bNewState)
		{
			if (_bListenState == bNewState) return;

			_bListenState = bNewState;
			if (_bListenState)
			{
				//FIXME
				Logger.Default.Log(ELogLevel.Always, "Start Listen {0} ", 30000/*TODO : 설정파일 읽는거로 변경하기*/);
				IPEndPoint epClient = new IPEndPoint(IPAddress.Any, 30000);
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
			//FixMe
			Logger.Default.Log(ELogLevel.Always, "OnConnect {0}", ep.ToString());

			if (ep.Port == 20000)
			{
				ImplObject obj = new MasterClientObject();
				obj.SetSocketSession(session);

				GameBaseTemplateContext.AddTemplate<ImplObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
				AccountController.AddAccountController(session.GetUid());
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
				ObjectType type = (ObjectType)userObj.ObjectID;
				GameBaseTemplateContext.DeleteClient(userObj.GetSession().GetUid());
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
