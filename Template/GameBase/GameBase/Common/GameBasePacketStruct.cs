using Service.Net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GameBase.Common
{
    public partial class PacketDefine
    {
        //public static string GUID = "{09DCE851-CC6D-4816-89B3-4B484AE62AFD}";
        public static string SERVER_GUID = "{AE5F33E3-3C6E-46D3-8DCF-CBA2DFEEC37B}";

        // 아래는 static PacketDefine() 생성자에서 채워짐(PacketSerializationsGenerated.cs 에 있음)
        public static string PROTOCOL_GUID = "";

        public static UInt32 ProtocolHash32 = 0;   // 각 패킷 클래스의 멤버변수명, 멤버타입명 등을 가지고 해싱한 값
        public static int PacketGenerateCounter = 0; // PacketSerializationGenerated.cs 파일 generation 할 때 마다 1씩 증가하게 됨(파일을 삭제하면 다시 0부터)
    }
    public class GPacket
    {
        public GPacket(UInt16 protocol)
        {
            _protocol = protocol;
        }

        private GPacket() { }
        public virtual void Serialize(Packet packet)
        {
            throw new Exception("No Implementation");
        }
        public virtual void Deserialize(Packet packet)
        {
            throw new Exception("No Implementation");
        }
        public Packet Serialize()
        {
            Packet packet = new Packet((UInt16)_protocol);
            Serialize(packet);
            return packet;
        }
        public string GetLog()
        {
            string log = "";
            log += this.GetType().Name + "\r\n";
            log += _protocol.ToString() + "\r\n";
            FieldInfo[] fields = this.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                object val = field.GetValue(this);
                log += string.Format("{0}={1}\r\n", field.Name, val != null ? val.ToString() : "null");
            }
            return log;
        }
        public override string ToString()
        {
            return GetLog();
        }

        protected UInt16 _protocol;
        public UInt16 Protocol() { return _protocol; }
    }

    public class PacketBaseRequest : GPacket
    {
        //공통으로 추가될 사항 추가 REQ
        public PacketBaseRequest(UInt16 protocol) : base(protocol) { }
    }
    public class PacketBaseResponse : GPacket
    {
        public int ErrorCode = 0;
        public PacketBaseResponse(UInt16 protocol) : base(protocol) { }
        public override void Serialize(Packet packet)
        {
            packet.Write(ErrorCode);
        }
        public override void Deserialize(Packet packet)
        {
            packet.Read(ref ErrorCode);
        }
    }
    public class PacketBaseNotification : GPacket
    {
        public PacketBaseNotification(UInt16 protocol) : base(protocol) { }
    }
}
