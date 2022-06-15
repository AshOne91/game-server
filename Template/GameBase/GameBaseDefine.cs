using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GameBase.Template.GameBase
{
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
}
