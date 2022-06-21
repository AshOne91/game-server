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
		public ulong Uid = new ulong();
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
		public ulong Uid = new ulong();
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

#if SERVER
	public sealed class PACKET_MG_HELLO_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 19;
		
		public PACKET_MG_HELLO_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
#endif

#if SERVER
	public sealed class PACKET_GM_HEART_BEAT_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 21;
		public PACKET_GM_HEART_BEAT_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
	public sealed class PACKET_GM_HEART_BEAT_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 22;
		public PACKET_GM_HEART_BEAT_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	};
#endif

#if SERVER
	public sealed class PACKET_GM_CHECK_AUTH_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 23;
		/// <summary>
		/// 
		/// </summary>
		public string ServerGUID = string.Empty;
		/// <summary>
		/// 
		/// </summary>
		public string Ver = string.Empty;
		/// <summary>
		/// 
		/// </summary>
		public string IP = string.Empty;
		/// <summary>
		/// 
		/// </summary>
		public ushort Port = new ushort();
		public PACKET_GM_CHECK_AUTH_REQ():base(ProtocolId){}
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
	public sealed class PACKET_GM_CHECK_AUTH_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 24;
		/// <summary>
		/// 
		/// </summary>
		public int ServerId = new int();
		public PACKET_GM_CHECK_AUTH_RES():base(ProtocolId){}
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
	public sealed class PACKET_GM_STATE_INFO_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 25;
		
		/// <summary>
		/// 
		/// </summary>
		public int CurrentUserCount = new int();
		public PACKET_GM_STATE_INFO_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(CurrentUserCount);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref CurrentUserCount);
		}
	}
#endif

#if SERVER
	public sealed class PACKET_GM_SESSION_INFO_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 27;
		
		/// <summary>
		/// 
		/// </summary>
		public int LogoutType = new int();
		/// <summary>
		/// 
		/// </summary>
		public Int64 PlayerIdx = new Int64();
		public PACKET_GM_SESSION_INFO_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(LogoutType);
			packet.Write(PlayerIdx);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref LogoutType);
			packet.Read(ref PlayerIdx);
		}
	}
#endif

#if SERVER
	public sealed class PACKET_MG_FORCE_LOGOUT_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 29;
		
		/// <summary>
		/// 세션 데이터
		/// </summary>
		public UserSessionData Data = new UserSessionData();
		public PACKET_MG_FORCE_LOGOUT_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(Data);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(Data);
		}
	}
#endif

#if SERVER
	public sealed class PACKET_ML_HELLO_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 31;
		
		public PACKET_ML_HELLO_NOTI():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
#endif

#if SERVER
	public sealed class PACKET_LM_HELLO_HEART_BEAT_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 33;
		public PACKET_LM_HELLO_HEART_BEAT_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
	public sealed class PACKET_LM_HELLO_HEART_BEAT_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 34;
		public PACKET_LM_HELLO_HEART_BEAT_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	};
#endif

#if SERVER
	public sealed class PACKET_LM_STATE_INFO_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 37;
		/// <summary>
		/// 
		/// </summary>
		public string SiteUserId = string.Empty;
		/// <summary>
		/// 
		/// </summary>
		public ulong Uid = new ulong();
		public PACKET_LM_STATE_INFO_REQ():base(ProtocolId){}
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
	public sealed class PACKET_LM_STATE_INFO_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 38;
		/// <summary>
		/// 
		/// </summary>
		public bool Found = new bool();
		/// <summary>
		/// 
		/// </summary>
		public ulong Uid = new ulong();
		/// <summary>
		/// 
		/// </summary>
		public int State = new int();
		/// <summary>
		/// 
		/// </summary>
		public int ServerId = new int();
		public PACKET_LM_STATE_INFO_RES():base(ProtocolId){}
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

	public sealed class PACKET_CL_HEART_BEAT_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 39;
		public PACKET_CL_HEART_BEAT_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
	public sealed class PACKET_CL_HEART_BEAT_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 40;
		public PACKET_CL_HEART_BEAT_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	};
	public sealed class PACKET_GC_HELLO_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 41;
		
		public PACKET_GC_HELLO_NOTI():base(ProtocolId){}
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
		public static readonly ushort ProtocolId = 43;
		public PACKET_CG_HEARTBEAT_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
	public sealed class PACKET_CG_HEARTBEAT_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 44;
		/// <summary>
		/// 시간 동기화 용
		/// </summary>
		public DateTime Time = new DateTime();
		public PACKET_CG_HEARTBEAT_RES():base(ProtocolId){}
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
	};
	public sealed class PACKET_GC_HEARTBEAT_NOTI : PacketBaseNotification
	{
		public static readonly ushort ProtocolId = 45;
		
		public PACKET_GC_HEARTBEAT_NOTI():base(ProtocolId){}
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
		public static readonly ushort ProtocolId = 47;
		/// <summary>
		/// 프로토콜 GUID
		/// </summary>
		public string ProtocolGUID = string.Empty;
		/// <summary>
		/// 앱버전
		/// </summary>
		public string Ver = string.Empty;
		/// <summary>
		/// 패스포트
		/// </summary>
		public string Passport = string.Empty;
		public PACKET_CG_CHECK_AUTH_REQ():base(ProtocolId){}
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
	public sealed class PACKET_CG_CHECK_AUTH_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 48;
		/// <summary>
		/// 플레이어 네임
		/// </summary>
		public string PlayerName = string.Empty;
		/// <summary>
		/// 유저 DB고유 인덱스
		/// </summary>
		public ulong PlayerIdx = new ulong();
		/// <summary>
		/// 사이트 유저 아이디
		/// </summary>
		public string SiteUserId = string.Empty;
		public PACKET_CG_CHECK_AUTH_RES():base(ProtocolId){}
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
	};
	public sealed class PACKET_CG_CREATE_PLAYER_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 49;
		/// <summary>
		/// 유저 닉네임
		/// </summary>
		public string PlayerName = string.Empty;
		public PACKET_CG_CREATE_PLAYER_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(PlayerName);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref PlayerName);
		}
	}
	public sealed class PACKET_CG_CREATE_PLAYER_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 50;
		/// <summary>
		/// 플레이어 인덱스
		/// </summary>
		public ulong PlayerIdx = new ulong();
		/// <summary>
		/// 유저 닉네임
		/// </summary>
		public string PlayerName = string.Empty;
		public PACKET_CG_CREATE_PLAYER_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(PlayerIdx);
			packet.Write(PlayerName);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref PlayerIdx);
			packet.Read(ref PlayerName);
		}
	};
}
