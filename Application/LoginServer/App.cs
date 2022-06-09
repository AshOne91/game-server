using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Service.Core;
using Service.Net;
using GameBase;
using System.Net.Sockets;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;
using LoginServer.Controllers;

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

            GameBaseTemplateContext.AddTemplate(ETemplateType.Admin, new GameBaseAccountTemplate());

            TemplateConfig templateConfig = new TemplateConfig();
            GameBaseTemplateContext.InitTemplate(templateConfig);
            GameBaseTemplateContext.LoadDataTable(templateConfig);

            PerformanceCounter._WarningEvent += OnPerfWarning;
            return result;
        }

        public bool ConnectToMaster()
        {
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
                session.SetUserObject(obj);
                obj.SetSocketSession(session);

                GameBaseTemplateContext.AddTemplate<UserObject>(obj, ETemplateType.Account, new GameBaseAccountTemplate());
                AccountController.AddAccountController(session.GetUid());

                obj.OnAccept(localEP);
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

        public override void OnPacket(SocketSession session, Packet packet)
        {
            AccountController.OnPacket(session.GetUserObject(), packet.GetId(), packet);
        }
    }
}
