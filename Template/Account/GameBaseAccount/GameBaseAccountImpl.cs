using System;
using System.Collections.Generic;
using System.Text;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;

namespace GameBase.Template.Account.GameBaseAccount
{
    public class GameBaseAccountImpl : AccountImpl
    {
        //LoginServer
        public int _LastHeartBeatTick = Environment.TickCount;
        private static List<GameServerInfo> _GameServerInfoList = new List<GameServerInfo>();
        private static int _ForceGuidIdx = -1;
        private static int _MinGameServerUserCount = 100;
        private static int _MaxGameServerUserCount = 500;

        //MasterServer
        public Dictionary<int, GameServerObject> _GameServerObjMap = new Dictionary<int, GameServerObject>();
        public Dictionary<int, LoginServerObject> _LoginServerObjMap = new Dictionary<int, LoginServerObject>();
        public GameBaseSessionManager _UserSessionManager = new GameBaseSessionManager();
        public TimeCounter _DebugTimer = new TimeCounter();

        //GameServer
        public int _ServerId = -1;

        public void SetGameServerInfo(List<GameServerInfo> gameServerInfoList)
        {
            _GameServerInfoList = gameServerInfoList;
        }

        public GameServerInfo GetGameServerInfo(List<int> wantedServerIds)
        {
            if (_ForceGuidIdx > -1)
            {
                wantedServerIds.Insert(0, _ForceGuidIdx);
            }

            foreach (var serverId in wantedServerIds)
            {
                if (serverId == -1) continue;

                foreach (var info in _GameServerInfoList)
                {
                    if (info.Alive == true && info.ServerId == serverId)
                    {
                        return info;
                    }
                }
            }

            // 최소 인원이 안 채워진 채널 부터 채움
            foreach (var info in _GameServerInfoList)
            {
                if (info.Alive == true && info.UserCount < _MinGameServerUserCount)
                {
                    return info;
                }
            }

            // 최소인원이 전부 다 채워져있다면 최대인원이 안채워진 채널로 채움
            foreach (var info in _GameServerInfoList)
            {
                if (info.Alive == true && info.UserCount < _MaxGameServerUserCount)
                {
                    return info;
                }
            }

            return null;
        }

        public void Update(float dt)
        {
            if (_serverType == ServerType.Master)
            {
                //MasterServer
                SendGameServerInfo();
                CheckKeepAlive_Game();
                CheckKeepAlive_Auth();

                if (_DebugTimer.IsFinished())
                {
                    _UserSessionManager.DebugPrintSessions();
                    _DebugTimer.Start(5000);
                }

                _UserSessionManager.RemoveSessionByTimeout();
                //OnUpdateAMate(dt);
            }
        }
        public GameBaseAccountImpl(ServerType type) : base(type)
        {

        }
        public void CheckKeepAlive_Game()
        {
            var enumerator = _GameServerObjMap.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GameServerObject obj = enumerator.Current;
                if (obj != null)
                {
                    obj.CheckKeepALive();
                }
            }
        }
        public void CheckKeepAlive_Auth()
        {
            var enumerator = _LoginServerObjMap.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                LoginServerObject obj = enumerator.Current;
                if (obj != null)
                {
                    obj.CheckKeepALive();
                }
            }
        }
        public void SendGameServerInfo()
        {
            List<GameServerInfo> gameServerInfoArray = new List<GameServerInfo>();
            foreach (KeyValuePair<int, GameServerObject> kv in _GameServerObjMap)
            {

                gameServerInfoArray.Add(kv.Value.GetAccountImpl<GameBaseAccountGameImpl>()._Info);
            }

            PACKET_ML_GAMESERVER_INFO_NOTI sendToLoginData = new PACKET_ML_GAMESERVER_INFO_NOTI();
            sendToLoginData.GameServerInfoList = gameServerInfoArray;
            BroadcastToServers(ServerType.Login, sendToLoginData.Serialize());

        }
        public void BroadcastToServers(ServerType type, Packet packet)
        {
            switch (type)
            {
                case ServerType.Game:
                    {
                        foreach (KeyValuePair<int, GameServerObject> kv in _GameServerObjMap)
                        {
                            kv.Value.GetSession().SendPacket(packet);
                        }
                    }
                    break;
                case ServerType.Login:
                    {
                        foreach (KeyValuePair<int, LoginServerObject> kv in _LoginServerObjMap)
                        {
                            kv.Value.GetSession().SendPacket(packet);
                        }
                    }
                    break;
            }
        }
    }

    public class GameBaseAccountMasterImpl : AccountImpl
    {
        public int _ServerId = -1;
        public GameBaseAccountMasterImpl(ImplObject obj) : base(obj)
        {

        }
    }

    public class GameBaseAccountUserImpl : AccountImpl
    {
        //LoginUserObj
        public ConnectInfo _connInfo = new ConnectInfo();
        public int _WantedServerId = -1;
        public int _PlatformType = -1;
        public bool _CheckVersion = false;
        public string _SiteUserId = string.Empty;

        //GameUserObj
        public PlayerInfo _PlayerInfo = new PlayerInfo();
        public string[] _PassportExtra = { };

        public GameBaseAccountUserImpl(ImplObject obj) : base(obj)
        {

        }
    }

    public class GameBaseAccountLoginImpl : AccountImpl
    {
        public string _HostIP = string.Empty;
        public ushort _HostPort;
        public int _ServerId = -1;

        public GameBaseAccountLoginImpl(ImplObject obj) : base(obj)
        {

        }
    }

    public class GameBaseAccountGameImpl : AccountImpl
    {
        public GameServerInfo _Info = new GameServerInfo();
        public bool _Auth = false;
        public GameBaseAccountGameImpl(ImplObject obj) : base(obj)
        {

        }
    }
}
