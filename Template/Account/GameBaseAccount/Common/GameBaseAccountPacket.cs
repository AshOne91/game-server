#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.Account.GameBaseAccount.Common
{
	public sealed class PACKET_LC_HELLO_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 1;
		
		public PACKET_LC_HELLO_NOTI():base(ProtocolId){}
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
		public static readonly ushort ProtocolId = 3;
		/// <summary>
		/// 프로토콜 버전
		/// </summary>
		public string ProtocolGUID = string.Empty;
		/// <summary>
		/// 게임버전
		/// </summary>
		public string Ver = string.Empty;
		public PACKET_CL_CHECK_VERSION_REQ():base(ProtocolId){}
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
	public sealed class PACKET_CL_CHECK_VERSION_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 4;
		public PACKET_CL_CHECK_VERSION_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	};
	public sealed class PACKET_CL_CHECK_AUTH_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 5;
		/// <summary>
		/// 유저 아이디
		/// </summary>
		public string SiteUserId = string.Empty;
		/// <summary>
		/// -1이 아닌 경우 원하는 서버로 이동할 수 있음(다이렉트로 이동 가능 한 지 서버에서 체크(재접, 게임매칭 등등)
		/// </summary>
		public int WantedServerId = new int();
		public PACKET_CL_CHECK_AUTH_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(SiteUserId);
			packet.Write(WantedServerId);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref SiteUserId);
			packet.Read(ref WantedServerId);
		}
	}
	public sealed class PACKET_CL_CHECK_AUTH_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 6;
		/// <summary>
		/// 발급된 통행권
		/// </summary>
		public string Passport = string.Empty;
		/// <summary>
		/// 게임서버 아이피
		/// </summary>
		public string IP = string.Empty;
		/// <summary>
		/// 게임서버 포트
		/// </summary>
		public ushort Port = new ushort();
		/// <summary>
		/// 게임서버 아이디
		/// </summary>
		public int ServerId = new int();
		public PACKET_CL_CHECK_AUTH_RES():base(ProtocolId){}
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
	};
	public sealed class PACKET_CL_GEN_GUEST_ID_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 7;
		public PACKET_CL_GEN_GUEST_ID_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
	public sealed class PACKET_CL_GEN_GUEST_ID_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 8;
		/// <summary>
		/// 고유아이디
		/// </summary>
		public string SiteUserId = string.Empty;
		public PACKET_CL_GEN_GUEST_ID_RES():base(ProtocolId){}
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
	};
	public sealed class PACKET_LC_DUPLICATE_LOGIN_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 9;
		
		public PACKET_LC_DUPLICATE_LOGIN_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
#if SERVER
	public sealed class PACKET_LM_CHECK_AUTH_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 11;
		/// <summary>
		/// 서버GUID
		/// </summary>
		public string ServerGUID = string.Empty;
		/// <summary>
		/// 버전
		/// </summary>
		public string Ver = string.Empty;
		/// <summary>
		/// 아이피
		/// </summary>
		public string HostIP = string.Empty;
		/// <summary>
		/// 포트
		/// </summary>
		public ushort HostPort = new ushort();
		public PACKET_LM_CHECK_AUTH_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(ServerGUID);
			packet.Write(Ver);
			packet.Write(HostIP);
			packet.Write(HostPort);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref ServerGUID);
			packet.Read(ref Ver);
			packet.Read(ref HostIP);
			packet.Read(ref HostPort);
		}
	}
	public sealed class PACKET_LM_CHECK_AUTH_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 12;
		/// <summary>
		/// 서버아이디
		/// </summary>
		public int ServerId = new int();
		public PACKET_LM_CHECK_AUTH_RES():base(ProtocolId){}
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
	};
#endif

#if SERVER
	public sealed class PACKET_LM_SESSION_INFO_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 13;
		/// <summary>
		/// 유저 고유아이디
		/// </summary>
		public string SiteUserId = string.Empty;
		/// <summary>
		/// 세션아이디
		/// </summary>
		public Int64 Uid = new Int64();
		public PACKET_LM_SESSION_INFO_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(SiteUserId);
			packet.Write(Uid);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref SiteUserId);
			packet.Read(ref Uid);
		}
	}
	public sealed class PACKET_LM_SESSION_INFO_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 14;
		/// <summary>
		/// 존재여부
		/// </summary>
		public bool Found = new bool();
		/// <summary>
		/// 세션아이디
		/// </summary>
		public Int64 Uid = new Int64();
		/// <summary>
		/// 세션상태
		/// </summary>
		public int State = new int();
		/// <summary>
		/// 서버아이디
		/// </summary>
		public int ServerId = new int();
		public PACKET_LM_SESSION_INFO_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(Found);
			packet.Write(Uid);
			packet.Write(State);
			packet.Write(ServerId);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref Found);
			packet.Read(ref Uid);
			packet.Read(ref State);
			packet.Read(ref ServerId);
		}
	};
#endif

#if SERVER
	public sealed class PACKET_LM_DUPLICATE_LOGIN_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 15;
		
		/// <summary>
		/// 유저 아이디
		/// </summary>
		public string StieUserId = string.Empty;
		public PACKET_LM_DUPLICATE_LOGIN_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(StieUserId);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref StieUserId);
		}
	}
#endif

#if SERVER
	public sealed class PACKET_ML_GAMESERVER_INFO_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 17;
		
		/// <summary>
		/// 서버 정보
		/// </summary>
		public List<GameServerInfo> GameServerInfoList = new List<GameServerInfo>();
		public PACKET_ML_GAMESERVER_INFO_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			int lengthGameServerInfoList = (GameServerInfoList == null) ? 0 : GameServerInfoList.Count;
			packet.Write(lengthGameServerInfoList);
			for (int i = 0; i < lengthGameServerInfoList; ++i)
			{
				packet.Write(GameServerInfoList[i]);
			}
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			int lengthGameServerInfoList = (GameServerInfoList == null) ? 0 : GameServerInfoList.Count;
			packet.Read(ref lengthGameServerInfoList);
			for (int i = 0; i < lengthGameServerInfoList; ++i)
			{
				GameServerInfo element = new GameServerInfo();
				packet.Read(element);
				GameServerInfoList.Add(element);
			}
		}
	}
#endif

}
