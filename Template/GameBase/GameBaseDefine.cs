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
        ERROR_NO_GAME_SERVER,
        INVALID_SITE_USER_ID,
        DBError,
        Withdraw,
        EternalBlock,
        PeriodBlock,
        TempBlock,
        LongTimeBlock
    }

    public enum EBlockStatus
    {
        None = 0,
        EternalBlock,
        PeriodBlock,
        TempBlock,
        ChattingBlock,
        Max
    }

    public class ConnectInfo
    {
        public int ConnType = (int)ConnectType.Normal;
        public int ServerId = -1;
        public string Ip = "";
        public ushort Port = 0;
        public UInt64 Location = 0;
    }

    public class AuthInfo
    {
        public ulong _accountDBKey = 0;
        public ulong _userDBKey = 0;
        public string _encodeAccountId = string.Empty;
        public int _platformType = 0;
        public bool _isGoogleLink = false;
        public bool _isAppleLink = false;
        public bool _isFacebook = false;
        public bool _Auth = false;
    }
}
