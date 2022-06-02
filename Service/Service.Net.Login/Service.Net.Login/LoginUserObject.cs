using GameBase.Common;
using GameBase.Controller;
using GameBase.Template.GameBase;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Service.Net.Login
{
    public sealed class LoginUserObject : UserObject
    {
        private PacketAbuseChecker _packetAbuser;
        private List<string> _userLog = new List<string>();
        private int _lastHeartBeatTick = 0;
        private bool _checkVersion = false;
        private string _siteUserId = "";
        private bool _needDisconnect = false;
        private int _wantedServerId = -1;
        private ConnectInfo _connInfo = new ConnectInfo();
        private GameBaseController _baseController = null;

        public class ConnectInfo
        {
            public int ConnType = (int)ConnectType.Normal;
            public int ServerId = -1;
            public string Ip = "";
            public ushort Port = 0;
            public UInt64 Location = 0;
        }

        public LoginUserObject()
        {
            _objectID = (int)ObjectType.User;
            _baseController = new GameBaseController(this);
            _packetAbuser = new PacketAbuseChecker();
        }

        public override void Dispose(bool disposing)//에러시 확인하기
        {
            if (!disposing)
            {
                base.Dispose(false);
            }
        }

        public override void OnAccept(IPEndPoint ep)
        {
            PACKET_LC_HEART_BEAT_RES sendPacket = new PACKET_LC_HEART_BEAT_RES();
            _session.SendPacket(sendPacket.Serialize());
        }

        public override void OnConnect(IPEndPoint ep) { }
        public override void OnClose() { }
        public override void OnSendComplete()
        {
            if (_needDisconnect)
            {
                Disconnect();
            }
        }
        public void Disconnect(bool force = true)
        {
            if (force)
            {
                Log(ELogLevel.Err, "Force Disconnect!");
            }
            GetSession().Disconnect();
        }

        public void SendPacket(GPacket packet, bool canDrop = false)
        {
            _session.SendPacket(packet.Serialize(), canDrop);
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

        public override void OnPacket(Packet packet)
        {
            _lastCheckTick = Environment.TickCount;
            Log(ELogLevel.Trace, "OnPacket {0}", (packet.GetId()).ToString());
            _baseController._protocol.OnPacket(this, packet.GetId(), packet);
        }

        public void OnSessionInfo(bool Found, int ServerId, UInt64 RoomIdx, int State)
        {
            // 기존 세션이 있다면

            if (Found)
            {
                _connInfo.Location = RoomIdx;
                _connInfo.ConnType = (int)ConnectType.Reconnect;

                if (State == (int)UserSessionData.SessionState.PendingDisconnect || State == (int)UserSessionData.SessionState.PendingLogout)
                {
                    Log(ELogLevel.Trace, "Id{0} PendingState {1}", _siteUserId, State);
                    PACKET_AC_CHECK_AUTH_RES sendPacket = new PACKET_AC_CHECK_AUTH_RES();
                    sendPacket.Error = GServerCode.PENDING_ERROR;  // 다른 서버에서 데이터 정리중이니 잠시 후에 재시도 하라는 의미
                    SendPacket(sendPacket);
                    return;
                }

                if (State != (int)UserSessionData.SessionState.Logout &&
                    State != (int)UserSessionData.SessionState.Disconnect)
                {
                    this.Log(LogLevel.ERR, "Duplicate Login State:{0}", State.ToString());
                    PACKET_LM_DUPLICATE_LOGIN_NOTI sendData = new PACKET_LM_DUPLICATE_LOGIN_NOTI();
                    sendData.SIteUserId = _siteUserId;
                    LoginServerApp.GetApp().SendPacketToMaster(sendData);

                    PACKET_LC_CHECK_AUTH_RES sendPacket = new PACKET_LC_CHECK_AUTH_RES();
                    sendPacket.ErrorCode = GServerCode.DUPLICATED_LOGIN;
                    SendPacket(sendPacket);

                    _needDisconnect = true;
                    return;
                }
            }

            GameServerInfo info = LoginServerEntry.GetApp().GetGameServerInfo(new List<int> { _wantedServerId, ServerId });

            if (info == null)
            {
                PACKET_AC_CHECK_AUTH_RES sendData = new PACKET_AC_CHECK_AUTH_RES();
                sendData.Result = GServerCode.ERROR_NO_GAME_SERVER;
                SendPacket(sendData);

                this.Log(ELogLevel.Warn, "ERR_NOGAME_SERVER");
            }
            else
            {
                _connInfo.ConnType = (int)ConnectType.Normal;
                _connInfo.Location = 0;
                _connInfo.Ip = info.Ip;
                _connInfo.Port = info.Port;
                _connInfo.ServerId = info.ServerId;
                SendAuthResult();
            }
        }

        void SendAuthResult()
        {
            string id;
            string extra;

            id = _siteUserId;
            DateTime now = DateTime.Now;
            extra = String.Format("{0}{1}{2}{3}{4}{5};{6};{7};{8}", now.Year - 2000, now.Month, now.Hour, now.Minute, now.Second, now.Millisecond,
                                                            (int)_connInfo.ConnType, _connInfo.Location, _connInfo.ServerId);

            string passport = Passport.Encrypt(id, extra);

            PACKET_AC_CHECK_AUTH_RES sendData = new PACKET_AC_CHECK_AUTH_RES();
            sendData.Result = GServerCode.SUCCESS;
            sendData.Passport = passport;
            sendData.Ip = _connInfo.Ip;
            sendData.CliPort = _connInfo.Port;
            sendData.ServerId = _connInfo.ServerId;
            SendPacket(sendData);

            Log(ELogLevel.Trace, "PACKET_AC_CHECK_AUTH_RES Success id:{0} extra:{1} passport:{2}", id, extra, passport);
            _needDisconnect = true;
        }
    }
}
