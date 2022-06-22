using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GameBase.Template.GameBase
{
    public enum ServerType : Byte
    {
        None,
        Login,
        Master,
        Game
    }
    public enum ConnectType : Byte
    {
        Normal = 1,
        Reconnect = 2,
        Invite = 3
    }

    public enum SessionState
    {
        BeforeAuth = 0,
        Login = 1,
        PendingLogout,
        Logout,
        Lobby,
        Playing,
        PendingDisconnect,
        Disconnect,
    }

    public enum GServerCode : int
    {
        SUCCESS = 0,
        INVALID_GUID,
        PENDING_ERROR,
        DUPLICATED_LOGIN,
        ERROR_NO_GAME_SERVER
    }

    public class ConnectInfo
    {
        public int ConnType = (int)ConnectType.Normal;
        public int ServerId = -1;
        public string Ip = "";
        public ushort Port = 0;
        public UInt64 Location = 0;
    }
}
