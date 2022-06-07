using GameBase.Common;
using GameBase.Controller;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Service.Net.Master
{
    public sealed class LoginServerObject : UserObject
    {
        private bool _login = false;
        private int _serverId;
        private string _hostIP = "";
        private ushort _hostPort = 0;
        private PacketAbuseChecker _packetAbuser = new PacketAbuseChecker();
        private List<string> _userLog = new List<string>();

        private GameBaseController _baseController = null;

        public LoginServerObject(int serverId)
        {
            _objectID = (int)ObjectType.Login;
            _serverId = serverId;
            _timeOverInterval = 5000;
            _maxTimerOverCount = 3;

            _baseController = new GameBaseController(this);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public override void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            try
            {
                if (disposing)
                {
                }
            }
            catch
            {

            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public int GetServerId() { return _serverId; }
        public override void OnAccept(IPEndPoint ep)
        {
            PACKET_ML_HELLO_NOTI sendPacket = new PACKET_ML_HELLO_NOTI();
            _session.SendPacket(sendPacket.Serialize());
        }
        public override void OnConnect(IPEndPoint ep) {}
        public override void OnClose()
        {
            Log(ELogLevel.Always, "OnAuthServerObject OnClose! ServerID:{0}", this._serverId);
        }
        public void Disconnect(bool force = true)
        {
            if (force)
            {
                Log(ELogLevel.Err, "Force Disconnect!");
            }
            GetSession().Disconnect();
        }
        public void SendPacket(GPacket packet)
        {
            _session.SendPacket(packet.Serialize());
            Log((ELogLevel.Trace), packet.GetLog());
        }

        public void Log(ELogLevel level, string format, params object[] args)
        {
            if (level < Logger.Default.GetLogLevel())
                return;

            string log = string.Format(format, args);
            Logger.Default.Log(level, log);
            UserLog(log);
        }

        [Conditional("UserLog")]
        void UserLog(string log)
        {
            _userLog.Add(log);
            if (_userLog.Count > 200)
            {
                _userLog.RemoveAt(0);
            }
        }
        public override void OnFailedKeepAlive()
        {
            Log(ELogLevel.Err, "OnFailedKeepAlive OnClose! ServerID:{0}", this._serverId);
            Disconnect();
        }
        public override void OnPacket(Packet packet)
        {
            _lastCheckTick = Environment.TickCount;
            Log(ELogLevel.Trace, "OnPacket {0}", (packet.GetId()).ToString());
            _baseController._protocol.OnPacket(this, packet.GetId(), packet);
        }

        void AM_CHECK_AUTH_REQ(NwPacket packet)
        {
            PACKET_AM_CHECK_AUTH_REQ recvPacket = new PACKET_AM_CHECK_AUTH_REQ();
            recvPacket.Deserialize(packet);

            if (recvPacket.ServerGUID != PacketDefine.SERVER_GUID)
            {
                this.Log(LogLevel.ERR, "Invalid GUID AS:{0} MS:{1}", recvPacket.ServerGUID, PacketDefine.SERVER_GUID);
                Disconnect();
                return;
            }

            _HostIP = recvPacket.HostIP;
            _HostPort = recvPacket.HostPort;

            this.Log(LogLevel.ALWAYS, "Success AM_CHECK_AUTH_REQ ServerID:{0} IP:{1} Port:{2}", _ServerId, _HostIP, _HostPort);

            PACKET_MA_CHECK_AUTH_RES sendData = new PACKET_MA_CHECK_AUTH_RES();
            sendData.Result = GServerCode.SUCCESS;
            sendData.ServerId = _ServerId;
            SendPacket(sendData);

            _Auth = true;
        }
        void AM_SESSION_INFO_REQ(NwPacket packet)
        {
            PACKET_AM_SESSION_INFO_REQ recvPacket = new PACKET_AM_SESSION_INFO_REQ();
            recvPacket.Deserialize(packet);

            UserSessionData session = null;
            bool result = MasterServerEntry.GetUserSessionManager().GetUserSession(recvPacket.SiteUserId, out session);

            PACKET_MA_SESSION_INFO_RES sendData = new PACKET_MA_SESSION_INFO_RES();
            sendData.Found = result;
            sendData.Uid = recvPacket.Uid;
            if (result && session != null)
            {
                sendData.State = (int)session.State;
                sendData.ServerId = session.ServerIdx;
                sendData.RoomIdx = session.RoomIdx;
            }
            SendPacket(sendData);
        }

        void AM_DUPLICATE_LOGIN_NOTI(NwPacket packet)
        {
            PACKET_AM_DUPLICATE_LOGIN_NOTI recvPacket = new PACKET_AM_DUPLICATE_LOGIN_NOTI();
            recvPacket.Deserialize(packet);

            UserSessionData session = null;
            bool result = MasterServerEntry.GetUserSessionManager().GetUserSession(recvPacket.SiteUserId, out session);

            if (result && session != null)
            {
                PACKET_MG_FORCE_LOGOUT_NOTI sendData = new PACKET_MG_FORCE_LOGOUT_NOTI();
                sendData.LogoutType = (int)ForceLogoutType.DuplicateLogin;
                sendData.PlayerIdx = session.PlayerIdx;
                MasterServerEntry.GetApp().SendPacketToGameServer(session.ServerIdx, sendData);

            }
        }

    }
}
