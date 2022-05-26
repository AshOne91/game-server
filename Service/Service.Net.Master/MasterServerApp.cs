using GameBase.Common;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
            UserObject userObj = session.GetUserObject();
            if (userObj != null)
            {
                userObj.OnPacket(packet);
            }
        }
        public override void OnSendComplete(SocketSession session, int transBytes) { }
        public override void OnAddSendQueue(SocketSession session, ushort protocol, int transBytes) { }
        public override void OnPacketError(SocketSession session, Packet packet)
        {
            Logger.Default.Log(ELogLevel.Err, "OnPacketError = {0}", packet.GetId());
            session.Disconnect();
        }
        public override void OnError(string errorMsg)
        {
            Logger.Default.Log(ELogLevel.Err, "OnError In MasterServer => {0}", errorMsg);
        }
        public override void OnUpdate(float dt)
        {
            SendGameServerInfo();
            CheckKeepAlive_Game();
            CheckKeepAlive_Login();

            if (_debugTimer.IsFinished())
            {
                _userSessionManager.DebugPrintSessions();
                _debugTimer.Start(5000);
            }

            _userSessionManager.RemoveSessionByTimeout();
            OnUpdateAMate(dt);
        }

        void CheckKeepAlive_Game()
        {
            try
            {
                _gameServerObjMapLock.Enter("CheckKeepAlive_Game");
                var enumerator = _gameServerObjMap.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    GameServerObject obj = enumerator.Current as GameServerObject;
                    if (obj != null)
                    {
                        obj.CheckKeepAlive();
                    }
                }
            }
            finally
            {
                _gameServerObjMapLock.Exit();
            }
        }

        void SendGameServerInfo()
        {
            List<GameServerInfo> gameServerInfoArray = new List<GameServerInfo>();

            try
            {
                _gameServerObjMapLock.Enter("SendGameServerInfo");

                foreach (KeyValuePair<int, GameServerObject> kv in _gameServerObjMap)
                {
                    gameServerInfoArray.Add(kv.Value.GetInfo());
                }
            }
            finally
            {
                _gameServerObjMapLock.Exit();
            }

            PACKET_ML_GAMESERVER_INFO_NOTI sendToLoginData = new PACKET_ML_GAMESERVER_INFO_NOTI();
            sendToLoginData.GameServerInfoArray = gameServerInfoArray;
            BroadCastToServers(ObjectType.Login, sendToLoginData);
        }

        void BroadcastToServers(ObjectType serverType, GPacket packet)
        {
            PriorityLock curLock = null;
            try
            {
                switch (serverType)
                {
                    case ObjectType.Game:
                        {
                            curLock = _gameServerObjMapLock;
                            _gameServerObjMapLock.Enter("BroadCastToGameServers");
                            foreach (KeyValuePair<int, GameServerObject> kv in _gameServerObjMap)
                            {
                                kv.Value.SendPacket(packet);
                            }
                        }
                        break;
                    case ObjectType.Login:
                        {
                            curLock = _loginServerObjMapLock; ;
                            _loginServerObjMapLock.Enter("BroadCastToLoginServers");
                            foreach(KeyValuePair<int, LoginServerObject> kv in _loginServerObjMap)
                            {
                                kv.Value.SendPacket(packet);
                            }
                        }
                        break;
                }
            }
            finally
            {
                if (curLock != null)
                {
                    curLock.Exit();
                }
            }
        }
        public void SendPacketToGameServer(int serverId, GPacket packet)
        {
            try
            {
                _gameServerObjMapLock.Enter("SendPacketToGameServer");
                GameServerObject gameServer = null;
                bool result = _gameServerObjMap.TryGetValue(serverId, out gameServer);
                if (result && gameServer != null)
                {
                    gameServer.SendPacket(packet);
                }
            }
            finally
            {
                Logger.Default.Log(ELogLevel.Err, "SendPacketToGameServer Failed serverId:{0}", serverId);
            }
        }

        int AllocServerIdx(ObjectType type)
        {
            switch(type)
            {
                case ObjectType.Game:
                    {
                        try
                        {
                            _gameServerObjMapLock.Enter("AllocServerIdx_Game");
                            for (int i = 1; i < 1000; ++i)
                            {
                                bool result = _gameServerObjMap.ContainsKey(i);
                                if (result == false)
                                {
                                    return i;
                                }
                            }
                        }
                        finally
                        {
                            _gameServerObjMapLock.Exit();
                        }
                    }
                    break;
                case ObjectType.Login:
                    {
                        _loginServerObjMapLock.Enter("AllocServerIdx_Login");

                        for (int i = 1000; i < 2000; ++i)
                        {
                            bool 
                        }
                    }
                    break;
            }
            throw new Exception("");
            return -1;
        }

        public void SendRegionServerIPs()
        {
            PACKET_MG_REGION_SERVER_INFO_NOTI sendData = new PACKET_MG_REGION_SERVER_INFO_NOTI();
            sendData.RegionServerIPs = _GetUniqueRegionIPs();
            BroadcastToServers(ObjectType.Game, sendData);
        }

        private List<string> _GetUniqueRegionIPs()
        {
            List<string> uniqueRegionIPs = new List<string>();

            try
            {
                foreach (GameServerObject serverObj in _gameServerObjMap.Values.ToList())
                {
                    string regionIP = serverObj.GetSession().GetIP();

                    int splitIdx = regionIP.IndexOfAny(new char[] { ':' });
                    regionIP = regionIP.Substring(0, splitIdx);

                    if (regionIP.Equals("127.0.0.1"))
                    {
                        IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (IPAddress curIP in ipEntry.AddressList)
                        {
                            if (curIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                _AddUniqueGameServerIP(uniqueRegionIPs, curIP.ToString());
                                break;
                            }
                        }

                        string externalIP = new WebClient().DownloadString("https://api.ipify.org");
                        _AddUniqueGameServerIP(uniqueRegionIPs, externalIP);
                    }
                    else
                    {
                        _AddUniqueGameServerIP(uniqueRegionIPs, regionIP);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Default.Log(ELogLevel.Err, e.Message);
            }

            return uniqueRegionIPs;
        }

        private void _AddUniqueGameServerIP(List<string> result, string newIP)
        {
            if (result == null)
            {
                return;
            }

            if (result.Contains(newIP) == false)
            {
                result.Add(newIP);
            }
        }
    }
}
