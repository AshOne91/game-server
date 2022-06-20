using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;

namespace GameBase.Template.Account.GameBaseAccount
{
    public class GameBaseAccountImpl : AccountImpl
    {
        //GameServerObj
        private GameServerInfo _info = new GameServerInfo();
        private bool _auth = false;

        //LoginServer
        public int _LastHeartBeatTick = Environment.TickCount;
        private static List<GameServerInfo> _GameServerInfoList = new List<GameServerInfo>();
        private static int _ForceGuidIdx = -1;
        private static int _MinGameServerUserCount = 100;
        private static int _MaxGameServerUserCount = 500;

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

        public GameBaseAccountImpl(ImplObject obj) : base(obj)
        {

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
        public bool _CheckVersion = false;
        public string _SiteUserId = string.Empty;

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
        public int _ServerId = -1;
        public GameBaseAccountGameImpl(ImplObject obj) : base(obj)
        {

        }
    }
}
