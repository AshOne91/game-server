using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Service.Core;
using Service.Net;
using GameBase;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;

namespace LoginServer
{
    public class LoginServerApp : ServerApp
    {
        public LoginServerApp()
        {

        }

        ~LoginServerApp()
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
            ConnectToMaster();
            return result;
        }

        public bool ConnectToMaster()
        {
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
            if (localEP.Port == 10000)
            {
                UserObject obj = new UserObject();
                obj.SetObjectId((ulong)(ObjectType.User));
                session.SetUserObject(obj);
                obj.SetSocketSession(session);

                GameBaseTemplateContext.AddTemplate<UserObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
                AccountController.AddAccountController(session.GetUid());

                obj.OnAccept(localEP);
                GameBaseTemplateContext.CreateClient(session.GetUid());
                GameBaseTemplateContext.GetTemplate<GameBaseAccountTemplate>(obj.GetSession().GetUid(), ETemplateType.Account).HelloNoti();
            }
        }

        private bool _bListenState = false;
        private void ListenUsers(bool bNewState)
        {
            if (_bListenState == bNewState) return;

            _bListenState = bNewState;
            if (_bListenState)
            {
                Logger.Default.Log(ELogLevel.Always, "Start Listen {0} ", 10000);
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

        public override void OnConnect(SocketSession session, IPEndPoint ep)
        {
            Logger.Default.Log(ELogLevel.Always, "OnConnect {0}", ep.ToString());

            if (ep.Port == 20000)
            {
                UserObject obj = new UserObject();
                obj.SetObjectID((ulong)ObjectType.Master);
                obj.SetSocketSession(session);

                GameBaseTemplateContext.AddTemplate<UserObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
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
                if (userObj.GetObjectID() == (ulong)ObjectType.Master)
                {
                    ConnectToMaster();
                }
                GameBaseTemplateContext.DeleteClient(userObj.GetSession().GetUid());
                userObj.OnClose();
                userObj.Dispose();
                session.SetUserObject(null);
            }
        }

        public override void OnSocketError(SocketSession session, string e)
        {
            Logger.Default.Log(ELogLevel.Err, "OnSocketError = {0}", e);
            session.Disconnect();
        }

        public override void OnPacket(SocketSession session, Packet packet)
        {
            UserObject userObject = session.GetUserObject();
            if (userObject != null)
            {
                AccountController.OnPacket(session.GetUserObject(), packet.GetId(), packet);
            }
            else
            {
                Logger.Default.Log(ELogLevel.Always, "session Disconnect but OnPacket");
            }
        }

        public override void OnSendComplete(SocketSession session, int transBytes)
        {
            UserObject userObj = session.GetUserObject();
            if (userObj != null)
            {
                userObj.OnSendComplete();
            }
        }

        public override void OnPacketError(SocketSession session, Packet packet)
        {
            Logger.Default.Log(ELogLevel.Err, "OnPacketError = {0}", packet.GetId());
            session.Disconnect();
        }

        public override void OnTimer(TimerHandle timer)
        {
            if (timer._TimerType == (ulong)ObjectType.Master)
            {
                ConnectToMaster();
            }
        }

        public override void OnError(string errorMsg)
        {
            Logger.Default.Log(ELogLevel.Err, "OnError In LoginServer => {0}", errorMsg);
        }

        public override void OnUpdate(float dt)
        {
            GameBaseTemplateContext.UpdateClient(dt);
        }


    }
}
