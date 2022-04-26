using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Common
{
    public sealed class PACKET_LC_HELLO_NOTI : PacketBaseNotification
    {
        public PACKET_LC_HELLO_NOTI() : base(1) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }
    public sealed class PACKET_CL_HEART_BEAT_REQ : PacketBaseRequest
    {
        public PACKET_CL_HEART_BEAT_REQ() : base(2) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }
    public sealed class PACKET_LC_HEART_BEAT_RES : PacketBaseResponse
    {
        public PACKET_LC_HEART_BEAT_RES() : base(3) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }
    public sealed class PACKET_CL_CHECK_VERSION_REQ : PacketBaseRequest
    {
        public PACKET_CL_CHECK_VERSION_REQ() : base(4) { }
        public string ProtocolGUID = "";
        public string Ver = "";
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(ProtocolGUID);
            packet.Write(Ver);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref ProtocolGUID);
            packet.Read(ref Ver);
        }
    }
    public sealed class PACKET_LC_CHECK_VERSION_RES : PacketBaseResponse
    {
        public PACKET_LC_CHECK_VERSION_RES() : base(5) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }
    public sealed class PACKET_CL_CHECK_AUTH_REQ : PacketBaseRequest
    {
        public PACKET_CL_CHECK_AUTH_REQ() : base(6) { }
        public string SiteUserId = "";
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(SiteUserId);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref SiteUserId);
        }
    }
    public sealed class PACKET_LC_CHECK_AUTH_RES : PacketBaseResponse
    {
        public PACKET_LC_CHECK_AUTH_RES() : base(7) { }
        public string Passport = "";
        public string IP = "";
        public ushort Port = 0;
        public int ServerId = -1;
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(Passport);
            packet.Write(IP);
            packet.Write(Port);
            packet.Write(ServerId);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref Passport);
            packet.Read(ref IP);
            packet.Read(ref Port);
            packet.Read(ref ServerId);
        }
    }

    public sealed class PACKET_CL_GEN_GUEST_ID_REQ : PacketBaseRequest
    {
        public PACKET_CL_GEN_GUEST_ID_REQ() : base(8) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_LC_GEN_GUEST_ID_RES : PacketBaseResponse
    {
        public PACKET_LC_GEN_GUEST_ID_RES() : base(9) { }
        public string SiteUserId = "";
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(SiteUserId);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref SiteUserId);
        }
    }

    public sealed class PACKET_LC_DUPLICATE_LOGIN_NOTI : PacketBaseNotification
    {
        public PACKET_LC_DUPLICATE_LOGIN_NOTI() : base(10) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }




    public sealed class PACKET_GC_HELLO_NOTI : PacketBaseNotification
    {
        public PACKET_GC_HELLO_NOTI() : base(1001) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_CG_HEARTBEAT_REQ : PacketBaseRequest
    {
        public PACKET_CG_HEARTBEAT_REQ() : base(1002) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_GC_HEARTBEAT_RES : PacketBaseResponse
    {
        public PACKET_GC_HEARTBEAT_RES() : base(1003) { }
        public DateTime Time = new DateTime();
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(Time);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref Time);
        }
    }

    public sealed class PACKET_GC_HEARTBEAT_NOTI : PacketBaseNotification
    {
        public PACKET_GC_HEARTBEAT_NOTI() : base(1004) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_CG_CHECK_AUTH_REQ : PacketBaseRequest
    {
        public PACKET_CG_CHECK_AUTH_REQ() : base(1005) { }
        public string ProtocolGUID = "";
        public string Ver = "";
        public string Passport = "";
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(ProtocolGUID);
            packet.Write(Ver);
            packet.Write(Passport);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref ProtocolGUID);
            packet.Read(ref Ver);
            packet.Read(ref Passport);
        }
    }

    public sealed class PACKET_GC_CHECK_AUTH_RES : PacketBaseResponse
    {
        public PACKET_GC_CHECK_AUTH_RES() : base(1006) { }
        public string PlayerName = "";
        public Int64 PlayerIdx = 0;
        public string SiteUserId = "";
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(PlayerName);
            packet.Write(PlayerIdx);
            packet.Write(SiteUserId);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref PlayerName);
            packet.Read(ref PlayerIdx);
            packet.Read(ref SiteUserId);
        }
    }


    public sealed class PACKET_MG_HELLO_NOTI : PacketBaseNotification
    {
        public PACKET_MG_HELLO_NOTI() : base(1007) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_GM_HEART_BEAT_REQ : PacketBaseRequest
    {
        public PACKET_GM_HEART_BEAT_REQ() : base(1008) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_MG_HEART_BEAT_RES : PacketBaseResponse
    {
        public PACKET_MG_HEART_BEAT_RES() : base(1009) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_GM_CHECK_AUTH_REQ : PacketBaseRequest
    {
        public PACKET_GM_CHECK_AUTH_REQ() : base(1010) { }
        public string ServerGUID = "";
        public string Ver = "";
        public string IP = "127.0.0.1";
        public ushort Port = 0;
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(ServerGUID);
            packet.Write(Ver);
            packet.Write(IP);
            packet.Write(Port);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref ServerGUID);
            packet.Read(ref Ver);
            packet.Read(ref IP);
            packet.Read(ref Port);
        }
    }
    public sealed class PACKET_MG_CHECK_AUTH_RES : PacketBaseResponse
    {
        public PACKET_MG_CHECK_AUTH_RES() : base(1011) { }
        public int ServerId = 0;
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(ServerId);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref ServerId);
        }
    }


    public sealed class PACKET_ML_HELLO_NOTI : PacketBaseNotification
    {
        public PACKET_ML_HELLO_NOTI() : base(3001) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_LM_HEART_BEAT_REQ : PacketBaseRequest
    {
        public PACKET_LM_HEART_BEAT_REQ() : base(3002) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_ML_HEART_BEAT_RES : PacketBaseResponse
    {
        public PACKET_ML_HEART_BEAT_RES() : base(3003) { }

        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_LM_CHECK_AUTH_REQ : PacketBaseRequest
    {
        public PACKET_LM_CHECK_AUTH_REQ() : base(3004) { }
        public string ServerGUID = "";
        public string Ver = "";
        public string HostIP = "127.0.0.1";
        public ushort HostPort = 7777;
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
        }
    }

    public sealed class PACKET_ML_CHECK_AUTH_RES : PacketBaseResponse
    {
        public PACKET_ML_CHECK_AUTH_RES() : base(3005) { }
        public int ServerId = 0;
        public override void Serialize(Packet packet)
        {
            base.Serialize(packet);
            packet.Write(ServerId);
        }
        public override void Deserialize(Packet packet)
        {
            base.Deserialize(packet);
            packet.Read(ref ServerId);
        }
    }

}
