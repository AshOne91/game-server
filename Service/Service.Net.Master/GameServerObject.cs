using GameBase.Common;
using GameBase.Controller;
using GameBase.Template.GameBase;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Service.Net.Master
{
    public sealed class GameServerObject : UserObject
    {
        public GameServerInfo GetInfo() { return _info; }
        private GameServerInfo _info = new GameServerInfo();
        private bool _login = false;
        private PacketAbuseChecker _packetAbuser = new PacketAbuseChecker();
        private List<string> _userLog = new List<string>();
        private GameBaseController _baseController = null;

        public GameServerObject(int serverId)
        {
            _objectID = (int)ObjectType.Game;
            _info.ServerId = serverId;

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
        public override void OnAccept(IPEndPoint ep)
        {
            PACKET_MG_HELLO_NOTI sendPacket = new PACKET_MG_HELLO_NOTI();
            _session.SendPacket(sendPacket.Serialize());
        }
        public override void OnConnect(IPEndPoint ep) {}
        public override void OnClose()
        {
            Log(ELogLevel.Always, "GameServerObject OnClose! ServerID:{0}", this._info.ServerId);
            MasterServerEntry.GetUserSessionManager().RemoveSessionByServerID(_info.ServerId);
        }
        public override void OnSendComplete() { }
        public override void OnFailedKeepAlive()
        {
            Log(ELogLevel.Err, "OnFailedKeepAlive OnClose! ServerID:{0}", this._info.ServerId);
            Disconnect();
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

        public override void OnPacket(Packet packet)
        {
            _lastCheckTick = Environment.TickCount;
            Log(ELogLevel.Trace, "OnPacket {0}", (packet.GetId()).ToString());
            _baseController._protocol.OnPacket(this, packet.GetId(), packet);
        }

        /*void GM_CHECK_AUTH_REQ(NwPacket packet)
        {
            PACKET_GM_CHECK_AUTH_REQ recvPacket = new PACKET_GM_CHECK_AUTH_REQ();
            recvPacket.Deserialize(packet);

            if (recvPacket.ServerGUID != PacketDefine.SERVER_GUID)
            {
                this.Log(LogLevel.ERR, "Invalid GUID AS:{0} MS:{1}", recvPacket.ServerGUID, PacketDefine.SERVER_GUID);
                Disconnect();
                return;
            }



            PACKET_MG_CHECK_AUTH_RES sendData = new PACKET_MG_CHECK_AUTH_RES();
            sendData.Result = GServerCode.SUCCESS;
            sendData.ServerId = _Info.ServerId;
            SendPacket(sendData);

            _Info.Ip = recvPacket.Ip;
            _Info.CliPort = recvPacket.CliPort;
            _Info.DediPort = recvPacket.DediPort;
            _Auth = true;

            this.Log(LogLevel.ALWAYS, "Success GM_CHECK_AUTH_REQ ServerID:{0} IP:{1} CliPort:{2} DediPort:{3}", _Info.ServerId, _Info.Ip, _Info.CliPort, _Info.DediPort);
        }

        void GM_STATE_INFO_NOTI(NwPacket packet)
        {
            PACKET_GM_STATE_INFO_NOTI recvPacket = new PACKET_GM_STATE_INFO_NOTI();
            recvPacket.Deserialize(packet);
            _Info.Alive = true;
            _Info.CurrentUserCount = recvPacket.CurrentUserCount;
            _Info.CurrentDediCount = recvPacket.CurrentDediCount;

            Log(LogLevel.ALWAYS, "GM_STATE_INFO_NOTI {0}", recvPacket.ToString());

        }
        void GM_SESSION_INFO_NOTI(NwPacket packet)
        {
            PACKET_GM_SESSION_INFO_NOTI recvPacket = new PACKET_GM_SESSION_INFO_NOTI();
            recvPacket.Deserialize(packet);

            UserSessionData session = recvPacket.Data;

            Log(LogLevel.ALWAYS, "GM_SESSION_INFO_NOTI {0}", session.GetLog());

            switch ((UserSessionData.SessionState)session.State)
            {
                case UserSessionData.SessionState.BeforeAuth:
                    {
                        Log(LogLevel.ERR, "The session need to get authentication - {0}", session.GetLog());
                    }
                    break;
                case UserSessionData.SessionState.Login:
                    {
                        MasterServerEntry.GetUserSessionManager().RemoveSession(session.SiteUserId);
                        MasterServerEntry.GetUserSessionManager().AddSession(session);
                    }
                    break;
                case UserSessionData.SessionState.Logout:
                    {
                        MasterServerEntry.GetUserSessionManager().RemoveSession(session.SiteUserId);
                    }
                    break;
                case UserSessionData.SessionState.Playing:
                case UserSessionData.SessionState.Lobby:
                case UserSessionData.SessionState.PendingDisconnect:
                case UserSessionData.SessionState.PendingLogout:
                case UserSessionData.SessionState.Disconnect:
                    {
                        UserSessionData existSession = null;
                        bool result = MasterServerEntry.GetUserSessionManager().GetUserSession(session.SiteUserId, out existSession);
                        if (result == true)
                        {
                            MasterServerEntry.GetUserSessionManager().UpdateSession(session);
                        }
                        else
                        {
                            MasterServerEntry.GetUserSessionManager().AddSession(session);
                        }
                    }
                    break;
                default:
                    {
                        Log(LogLevel.FATAL, "Unknown State GM_SESSION_INFO_NOTI {0}", session.GetLog());
                        break;
                    }
            }
        }*/
    }
}
