using Service.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Service.Net.Master
{
    public enum ELockPriority
    {
        GameServerObjectMap = 10000,
        LoginServerObjectMap = 5000,
        SessionManager = 1000
    }

    public class MasterServerConfig
    {
        public string _Ver;
        public ushort _GamePort;
        public ushort _LoginPort;

        public MasterServerConfig()
        {
            /*_Ini = new NwIni("master_config.ini");*/
            LoadConfig();
        }
        public void LoadConfig()
        {
            _Ver = "1.0.0";//_Ini.GetValue("version", "server", "1.0.0");
            _GamePort = 17000;//ushort.Parse(_Ini.GetValue("game_port", "server", "17000"));
            _LoginPort = 20000;//ushort.Parse(_Ini.GetValue("auth_port", "server", "20000"));
        }
    }


    public sealed partial class MasterServerApp : ServerApp
    {
        private MasterServerConfig _serverConfig;

        private Dictionary<int, GameServerObject> _gameServerObjMap;
        private PriorityLock _gameServerObjMapLock = new PriorityLock((int)ELockPriority.GameServerObjectMap);

        private Dictionary<int, LoginServerObject> _loginServerObjMap;
        private PriorityLock _loginServerObjMapLock = new PriorityLock((int)ELockPriority.LoginServerObjectMap);

        private UserSessionManager _userSessionManager = new UserSessionManager();

        private TimeCounter _debugTimer = new TimeCounter();

        public MasterServerApp()
        {

        }

        ~MasterServerApp()
        {
            Destroy();
        }

        public override bool Create(ServerConfig config)
        {
            _serverConfig = new MasterServerConfig();
            _gameServerObjMap = new Dictionary<int, GameServerObject>();
            _loginServerObjMap = new Dictionary<int, LoginServerObject>();
            _debugTimer.Start(5000);

            bool result = base.Create(config);
            PerformanceCounter._WarningEvent += OnPerfWarning;

            return result;
        }

        public override void Destroy()
        {
            base.Destroy();

            PerformanceCounter.Print();
            Logger.Default.Destroy();
        }

        public override void OnAccept(SocketSession session, IPEndPoint localEP, IPEndPoint remoteEP)
        {
            if (localEP.Port == _serverConfig._GamePort)
            {
                int idx = AllocServerIdx(ObjectType.Game);
                GameServerObject obj = new GameServerObject(idx);
                session.SetUserObject(obj);
                obj.SetSession(session);

                _gameServerObjMapLock.Enter("OnAccept");
                try
                {
                    _gameServerObjMap.TryAdd(idx, obj);
                }
                finally
                {
                    _gameServerObjMapLock.Exit();
                }

                obj.OnAccept(localEP);
                SendRegionServerIPs();
            }
            else if(localEP.Port == _serverConfig._LoginPort)
            {
                int idx = AllocServerIdx(ObjectType.Login);
                LoginServerObject obj = new LoginServerObject(idx);
                session.SetUserObject(obj);
                obj.SetSession(session);

                _loginServerObjMapLock.Enter("OnAccept");
                try
                {
                    _loginServerObjMap.TryAdd(idx, obj);
                }
                finally
                {
                    _loginServerObjMapLock.Exit();
                }

                obj.OnAccept(localEP);
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
        }
        public override void OnConnectFailed(SocketSession session, string e)
        {

        }
        public override void OnDisconnected(SocketSession session, bool bRemote, string e)
        {

        }
        public override void OnClose(SocketSession session)
        {
            UserObject userObj = session.GetUserObject();
            if (userObj != null)
            {
                if(userObj.GetObjectID() == (int)ObjectType.Game)
                {
                    GameServerObject gameObj = (GameServerObject)session.GetUserObject();
                    _gameServerObjMapLock.Enter("OnClose");
                    try
                    {
                        _gameServerObjMap.Remove(gameObj.GetInfo().ServerId, out gameObj);
                    }
                    finally
                    {
                        _gameServerObjMapLock.Exit();
                    }
                    SendRegionServerIPs();
                }
            }
            else if (userObj.GetObjectID() == (int)ObjectType.Login)
            {
                LoginServerObject loginObj = (LoginServerObject)session.GetUserObject();
                _loginServerObjMapLock.Enter("OnClose");
                try
                {
                    _loginServerObjMap.Remove(loginObj.GetServerId(), out loginObj);
                }
                finally
                {
                    _loginServerObjMapLock.Exit();
                }
            }

            userObj.OnClose();
            userObj.Dispose();
            session.SetUserObject(null);
        }

        public override void OnSocketError(SocketSession session, string e)
        {
            Logger.Default.Log(ELogLevel.Err, "OnSocketError = {0}", e);
            session.Disconnect();
        }
        public override void OnUserEvent(SocketSession session) { }
        public override void OnAsyncTask(AsyncTaskObject task) { }
        public override void OnPacket(SocketSession session, Packet packet)
        {
            //FIXMEFIXME
        }


    }
}
