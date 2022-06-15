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

    [Serializable]
    public class UserSessionData
    {
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

        public Int64 PlayerIdx = 0;
        public string PlayerName = "";
        public string SiteUserId = "";
        public int ServerIdx = 0;
        public UInt64 RoomIdx = 0;
        public int State = 0;
        public string RemoteIP = "";
        public UInt16 RemotePort = 0;
        public DateTime LastUpdateTime = DateTime.Now;

        public string GetLog()
        {
            string log = "";
            FieldInfo[] fields = this.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                log += string.Format("{0}={1}\r\n", field.Name, field.GetValue(this).ToString());
            }
            return log;
        }
    }
}
