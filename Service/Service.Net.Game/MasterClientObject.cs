using GameBase.Controller;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Service.Net.Game
{
    public sealed class MasterClientObject : UserObject
    {
        private PacketAbuseChecker _packetAbuser;

        private int _serverId = -1;
        private TimeCounter _stateTimer = new TimeCounter();
        private int _stateInterval = 2000;

        private GameBaseController _baseController = null;

        public MasterClientObject()
        {
            _objectID = (int)ObjectType.Master;
            _packetAbuser = new PacketAbuseChecker();
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

        }
        public override void OnConnect(IPEndPoint ep)
        {
            _stateTimer.Start(_stateInterval);
        }
        public override void OnClose()
        {

        }
        public override void OnSendComplete()
        {
        }
        public void Disconnect(bool force = true)
        {
            if (force)
            {
                Log(ELogLevel.Err, "Force Disconnect!");
            }
            GetSession().Disconnect();
        }

        public void SendPacket(Packet packet, bool canDrop = false)
        {
            _session.SendPacket(packet, canDrop);
        }

        public void OnUpdate(float dt)
        {
            if (_stateTimer.IsFinished())
            {
                SendStateInfo();
                _stateTimer.Start(_stateInterval);
            }
        }

        public void Log(ELogLevel level, string format, params object[] args)
        {
            if (level < Logger.Default.GetLogLevel())
                return;


            string log = string.Format(format, args);
            Logger.Default.Log(level, log);
        }

        public override void OnPacket(NwPacket packet)
        {
            /*Log(LogLevel.TRACE, "OnPacket {0}", ((EMPacketProtocol)packet.GetId()).ToString());
            switch ((EMPacketProtocol)packet.GetId())
            {
                case EMPacketProtocol.MG_HELLO_NOTI:
                    MG_HELLO_NOTI(packet);
                    break;
                case EMPacketProtocol.MG_CHECK_AUTH_RES:
                    MG_CHECK_AUTH_RES(packet);
                    break;
                case EMPacketProtocol.MG_FORCE_LOGOUT_NOTI:
                    MG_FORCE_LOGOUT_NOTI(packet);
                    break;
                case EMPacketProtocol.MG_REGION_SERVER_INFO_NOTI:
                    MG_REGION_SERVER_INFO_NOTI(packet);
                    break;

            }*/
            Log(ELogLevel.Trace, "OnPacket {0}", (packet.GetId()).ToString());
            _baseController._protocol.OnPacket(this, packet.GetId(), packet);
        }

        /*private void MG_REGION_SERVER_INFO_NOTI(NwPacket packet)
        {
            PACKET_MG_REGION_SERVER_INFO_NOTI recvPacket = new PACKET_MG_REGION_SERVER_INFO_NOTI();
            recvPacket.Deserialize(packet);

            // todo fix
            //GameServerEntry.GetApp().RegionServerIPs = recvPacket.RegionServerIPs;
        }

        void MG_HELLO_NOTI(NwPacket packet)
        {
            PACKET_GM_CHECK_AUTH_REQ sendData = new PACKET_GM_CHECK_AUTH_REQ();
            sendData.ServerGUID = PacketDefine.SERVER_GUID;
            sendData.Ver = "1.0.0";
            sendData.Ip = GameServerEntry.GetConfig()._ClientIP;
            sendData.CliPort = GameServerEntry.GetConfig()._ClientPort;
            sendData.DediPort = GameServerEntry.GetConfig()._DediPort;

            Log(LogLevel.TRACE, "Send PACKET_GM_CHECK_AUTH_REQ {0}", sendData.GetLog());
            SendPacket(sendData.Serialize());
        }

        void MG_CHECK_AUTH_RES(NwPacket packet)
        {
            PACKET_MG_CHECK_AUTH_RES recvPacket = new PACKET_MG_CHECK_AUTH_RES();
            recvPacket.Deserialize(packet);

            if (recvPacket.Result == GServerCode.SUCCESS)
            {
                _ServerId = recvPacket.ServerId;

                // 마스터서버와 끊어진 후 재연결 되면 서버 ID가 새로 부여되는데, 여기서 이런저런 문제가 생길것 같다
                // 게임서버에 한번 부여된 서버 ID는 바뀌지 않는게 어떨지?
                GameServerEntry.GetApp().ServerId = _ServerId;

                Log(LogLevel.ALWAYS, "SUCCESS PACKET_MG_CHECK_AUTH_RES Result {0}", recvPacket.Result);

                GameServerEntry.GetApp().ConnectToMatching();
                GameServerEntry.GetApp().ConnectToDediPoolServer();
            }
            else
            {
                Log(LogLevel.FATAL, "ERROR PACKET_MG_CHECK_AUTH_RES Result {0}", recvPacket.Result);
            }
        }
        void MG_FORCE_LOGOUT_NOTI(NwPacket packet)
        {
            PACKET_MG_FORCE_LOGOUT_NOTI recvPacket = new PACKET_MG_FORCE_LOGOUT_NOTI();
            recvPacket.Deserialize(packet);


            GameUserObject target = GameServerEntry.GetApp().GetAuthUser(recvPacket.PlayerIdx);
            if (target != null)
            {
                target.OnForceLogout(recvPacket.LogoutType, "");
                Log(LogLevel.ALWAYS, "SUCCESS MG_FORCE_LOGOUT_NOTI PlayerIdx: {0} Name:{1}", recvPacket.PlayerIdx, target.GetPlayerInfo().PlayerName);
            }
        }*/

        void SendStateInfo()
        {
            PACKET_GM_STATE_INFO_NOTI sendData = new PACKET_GM_STATE_INFO_NOTI();
            sendData.CurrentUserCount = GameServerEntry.GetApp().GetUserCount();
            sendData.CurrentDediCount = GameServerEntry.GetApp().GetDediCount();
            SendPacket(sendData.Serialize());
        }
    }
}
