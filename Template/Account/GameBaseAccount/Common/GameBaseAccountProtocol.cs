#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;

namespace GameBase.Template.Account.GameBaseAccount.Common
{
	public partial class GameBaseAccountProtocol
	{
		Dictionary<ushort, ControllerDelegate> MessageControllers = new Dictionary<ushort, ControllerDelegate>();

		public GameBaseAccountProtocol()
		{
			Init();
		}

		void Init()
		{
			MessageControllers.Add(PACKET_LC_HELLO_NOTI.ProtocolId, LC_HELLO_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_CL_CHECK_VERSION_REQ.ProtocolId, CL_CHECK_VERSION_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_CL_CHECK_VERSION_RES.ProtocolId, CL_CHECK_VERSION_RES_CONTROLLER);
			MessageControllers.Add(PACKET_CL_CHECK_AUTH_REQ.ProtocolId, CL_CHECK_AUTH_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_CL_CHECK_AUTH_RES.ProtocolId, CL_CHECK_AUTH_RES_CONTROLLER);
			MessageControllers.Add(PACKET_CL_GEN_GUEST_ID_REQ.ProtocolId, CL_GEN_GUEST_ID_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_CL_GEN_GUEST_ID_RES.ProtocolId, CL_GEN_GUEST_ID_RES_CONTROLLER);
			MessageControllers.Add(PACKET_LC_DUPLICATE_LOGIN_NOTI.ProtocolId, LC_DUPLICATE_LOGIN_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_LM_CHECK_AUTH_REQ.ProtocolId, LM_CHECK_AUTH_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_LM_CHECK_AUTH_RES.ProtocolId, LM_CHECK_AUTH_RES_CONTROLLER);
			MessageControllers.Add(PACKET_LM_SESSION_INFO_REQ.ProtocolId, LM_SESSION_INFO_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_LM_SESSION_INFO_RES.ProtocolId, LM_SESSION_INFO_RES_CONTROLLER);
			MessageControllers.Add(PACKET_LM_DUPLICATE_LOGIN_NOTI.ProtocolId, LM_DUPLICATE_LOGIN_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_ML_GAMESERVER_INFO_NOTI.ProtocolId, ML_GAMESERVER_INFO_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_MG_HELLO_NOTI.ProtocolId, MG_HELLO_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_GM_HEART_BEAT_REQ.ProtocolId, GM_HEART_BEAT_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_GM_HEART_BEAT_RES.ProtocolId, GM_HEART_BEAT_RES_CONTROLLER);
			MessageControllers.Add(PACKET_GM_CHECK_AUTH_REQ.ProtocolId, GM_CHECK_AUTH_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_GM_CHECK_AUTH_RES.ProtocolId, GM_CHECK_AUTH_RES_CONTROLLER);
			MessageControllers.Add(PACKET_GM_STATE_INFO_NOTI.ProtocolId, GM_STATE_INFO_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_GM_SESSION_INFO_NOTI.ProtocolId, GM_SESSION_INFO_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_MG_FORCE_LOGOUT_NOTI.ProtocolId, MG_FORCE_LOGOUT_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_ML_HELLO_NOTI.ProtocolId, ML_HELLO_NOTI_CONTROLLER);
			MessageControllers.Add(PACKET_LM_HELLO_HEART_BEAT_REQ.ProtocolId, LM_HELLO_HEART_BEAT_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_LM_HELLO_HEART_BEAT_RES.ProtocolId, LM_HELLO_HEART_BEAT_RES_CONTROLLER);
			MessageControllers.Add(PACKET_LM_STATE_INFO_REQ.ProtocolId, LM_STATE_INFO_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_LM_STATE_INFO_RES.ProtocolId, LM_STATE_INFO_RES_CONTROLLER);
		}

		public virtual bool OnPacket(UserObject userObject, ushort protocolId, Packet packet)
		{
			ControllerDelegate controllerCallback;
			if(MessageControllers.TryGetValue(protocolId, out controllerCallback) == false)
			{
				return false;
			}
			controllerCallback(userObject, packet);
			return true;
		}

		public delegate void LC_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_LC_HELLO_NOTI packet);
		public LC_HELLO_NOTI_CALLBACK ON_LC_HELLO_NOTI_CALLBACK;
		public void LC_HELLO_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LC_HELLO_NOTI recvPacket = new PACKET_LC_HELLO_NOTI();
			recvPacket.Deserialize(packet);
			ON_LC_HELLO_NOTI_CALLBACK(obj, recvPacket);
		}
		public delegate void CL_CHECK_VERSION_REQ_CALLBACK(UserObject userObject, PACKET_CL_CHECK_VERSION_REQ packet);
		public CL_CHECK_VERSION_REQ_CALLBACK ON_CL_CHECK_VERSION_REQ_CALLBACK;
		public void CL_CHECK_VERSION_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_CL_CHECK_VERSION_REQ recvPacket = new PACKET_CL_CHECK_VERSION_REQ();
			recvPacket.Deserialize(packet);
			ON_CL_CHECK_VERSION_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void CL_CHECK_VERSION_RES_CALLBACK(UserObject userObject, PACKET_CL_CHECK_VERSION_RES packet);
		public CL_CHECK_VERSION_RES_CALLBACK ON_CL_CHECK_VERSION_RES_CALLBACK;
		public void CL_CHECK_VERSION_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_CL_CHECK_VERSION_RES recvPacket = new PACKET_CL_CHECK_VERSION_RES();
			recvPacket.Deserialize(packet);
			ON_CL_CHECK_VERSION_RES_CALLBACK(obj, recvPacket);
		}
		public delegate void CL_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_CL_CHECK_AUTH_REQ packet);
		public CL_CHECK_AUTH_REQ_CALLBACK ON_CL_CHECK_AUTH_REQ_CALLBACK;
		public void CL_CHECK_AUTH_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_CL_CHECK_AUTH_REQ recvPacket = new PACKET_CL_CHECK_AUTH_REQ();
			recvPacket.Deserialize(packet);
			ON_CL_CHECK_AUTH_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void CL_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_CL_CHECK_AUTH_RES packet);
		public CL_CHECK_AUTH_RES_CALLBACK ON_CL_CHECK_AUTH_RES_CALLBACK;
		public void CL_CHECK_AUTH_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_CL_CHECK_AUTH_RES recvPacket = new PACKET_CL_CHECK_AUTH_RES();
			recvPacket.Deserialize(packet);
			ON_CL_CHECK_AUTH_RES_CALLBACK(obj, recvPacket);
		}
		public delegate void CL_GEN_GUEST_ID_REQ_CALLBACK(UserObject userObject, PACKET_CL_GEN_GUEST_ID_REQ packet);
		public CL_GEN_GUEST_ID_REQ_CALLBACK ON_CL_GEN_GUEST_ID_REQ_CALLBACK;
		public void CL_GEN_GUEST_ID_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_CL_GEN_GUEST_ID_REQ recvPacket = new PACKET_CL_GEN_GUEST_ID_REQ();
			recvPacket.Deserialize(packet);
			ON_CL_GEN_GUEST_ID_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void CL_GEN_GUEST_ID_RES_CALLBACK(UserObject userObject, PACKET_CL_GEN_GUEST_ID_RES packet);
		public CL_GEN_GUEST_ID_RES_CALLBACK ON_CL_GEN_GUEST_ID_RES_CALLBACK;
		public void CL_GEN_GUEST_ID_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_CL_GEN_GUEST_ID_RES recvPacket = new PACKET_CL_GEN_GUEST_ID_RES();
			recvPacket.Deserialize(packet);
			ON_CL_GEN_GUEST_ID_RES_CALLBACK(obj, recvPacket);
		}
		public delegate void LC_DUPLICATE_LOGIN_NOTI_CALLBACK(UserObject userObject, PACKET_LC_DUPLICATE_LOGIN_NOTI packet);
		public LC_DUPLICATE_LOGIN_NOTI_CALLBACK ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK;
		public void LC_DUPLICATE_LOGIN_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LC_DUPLICATE_LOGIN_NOTI recvPacket = new PACKET_LC_DUPLICATE_LOGIN_NOTI();
			recvPacket.Deserialize(packet);
			ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK(obj, recvPacket);
		}
#if SERVER
		public delegate void LM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_REQ packet);
		public LM_CHECK_AUTH_REQ_CALLBACK ON_LM_CHECK_AUTH_REQ_CALLBACK;
		public void LM_CHECK_AUTH_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_CHECK_AUTH_REQ recvPacket = new PACKET_LM_CHECK_AUTH_REQ();
			recvPacket.Deserialize(packet);
			ON_LM_CHECK_AUTH_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void LM_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_RES packet);
		public LM_CHECK_AUTH_RES_CALLBACK ON_LM_CHECK_AUTH_RES_CALLBACK;
		public void LM_CHECK_AUTH_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_CHECK_AUTH_RES recvPacket = new PACKET_LM_CHECK_AUTH_RES();
			recvPacket.Deserialize(packet);
			ON_LM_CHECK_AUTH_RES_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void LM_SESSION_INFO_REQ_CALLBACK(UserObject userObject, PACKET_LM_SESSION_INFO_REQ packet);
		public LM_SESSION_INFO_REQ_CALLBACK ON_LM_SESSION_INFO_REQ_CALLBACK;
		public void LM_SESSION_INFO_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_SESSION_INFO_REQ recvPacket = new PACKET_LM_SESSION_INFO_REQ();
			recvPacket.Deserialize(packet);
			ON_LM_SESSION_INFO_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void LM_SESSION_INFO_RES_CALLBACK(UserObject userObject, PACKET_LM_SESSION_INFO_RES packet);
		public LM_SESSION_INFO_RES_CALLBACK ON_LM_SESSION_INFO_RES_CALLBACK;
		public void LM_SESSION_INFO_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_SESSION_INFO_RES recvPacket = new PACKET_LM_SESSION_INFO_RES();
			recvPacket.Deserialize(packet);
			ON_LM_SESSION_INFO_RES_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void LM_DUPLICATE_LOGIN_NOTI_CALLBACK(UserObject userObject, PACKET_LM_DUPLICATE_LOGIN_NOTI packet);
		public LM_DUPLICATE_LOGIN_NOTI_CALLBACK ON_LM_DUPLICATE_LOGIN_NOTI_CALLBACK;
		public void LM_DUPLICATE_LOGIN_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_DUPLICATE_LOGIN_NOTI recvPacket = new PACKET_LM_DUPLICATE_LOGIN_NOTI();
			recvPacket.Deserialize(packet);
			ON_LM_DUPLICATE_LOGIN_NOTI_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void ML_GAMESERVER_INFO_NOTI_CALLBACK(UserObject userObject, PACKET_ML_GAMESERVER_INFO_NOTI packet);
		public ML_GAMESERVER_INFO_NOTI_CALLBACK ON_ML_GAMESERVER_INFO_NOTI_CALLBACK;
		public void ML_GAMESERVER_INFO_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_ML_GAMESERVER_INFO_NOTI recvPacket = new PACKET_ML_GAMESERVER_INFO_NOTI();
			recvPacket.Deserialize(packet);
			ON_ML_GAMESERVER_INFO_NOTI_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void MG_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_MG_HELLO_NOTI packet);
		public MG_HELLO_NOTI_CALLBACK ON_MG_HELLO_NOTI_CALLBACK;
		public void MG_HELLO_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_MG_HELLO_NOTI recvPacket = new PACKET_MG_HELLO_NOTI();
			recvPacket.Deserialize(packet);
			ON_MG_HELLO_NOTI_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void GM_HEART_BEAT_REQ_CALLBACK(UserObject userObject, PACKET_GM_HEART_BEAT_REQ packet);
		public GM_HEART_BEAT_REQ_CALLBACK ON_GM_HEART_BEAT_REQ_CALLBACK;
		public void GM_HEART_BEAT_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_GM_HEART_BEAT_REQ recvPacket = new PACKET_GM_HEART_BEAT_REQ();
			recvPacket.Deserialize(packet);
			ON_GM_HEART_BEAT_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void GM_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_GM_HEART_BEAT_RES packet);
		public GM_HEART_BEAT_RES_CALLBACK ON_GM_HEART_BEAT_RES_CALLBACK;
		public void GM_HEART_BEAT_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_GM_HEART_BEAT_RES recvPacket = new PACKET_GM_HEART_BEAT_RES();
			recvPacket.Deserialize(packet);
			ON_GM_HEART_BEAT_RES_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void GM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_GM_CHECK_AUTH_REQ packet);
		public GM_CHECK_AUTH_REQ_CALLBACK ON_GM_CHECK_AUTH_REQ_CALLBACK;
		public void GM_CHECK_AUTH_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_GM_CHECK_AUTH_REQ recvPacket = new PACKET_GM_CHECK_AUTH_REQ();
			recvPacket.Deserialize(packet);
			ON_GM_CHECK_AUTH_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void GM_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_GM_CHECK_AUTH_RES packet);
		public GM_CHECK_AUTH_RES_CALLBACK ON_GM_CHECK_AUTH_RES_CALLBACK;
		public void GM_CHECK_AUTH_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_GM_CHECK_AUTH_RES recvPacket = new PACKET_GM_CHECK_AUTH_RES();
			recvPacket.Deserialize(packet);
			ON_GM_CHECK_AUTH_RES_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void GM_STATE_INFO_NOTI_CALLBACK(UserObject userObject, PACKET_GM_STATE_INFO_NOTI packet);
		public GM_STATE_INFO_NOTI_CALLBACK ON_GM_STATE_INFO_NOTI_CALLBACK;
		public void GM_STATE_INFO_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_GM_STATE_INFO_NOTI recvPacket = new PACKET_GM_STATE_INFO_NOTI();
			recvPacket.Deserialize(packet);
			ON_GM_STATE_INFO_NOTI_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void GM_SESSION_INFO_NOTI_CALLBACK(UserObject userObject, PACKET_GM_SESSION_INFO_NOTI packet);
		public GM_SESSION_INFO_NOTI_CALLBACK ON_GM_SESSION_INFO_NOTI_CALLBACK;
		public void GM_SESSION_INFO_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_GM_SESSION_INFO_NOTI recvPacket = new PACKET_GM_SESSION_INFO_NOTI();
			recvPacket.Deserialize(packet);
			ON_GM_SESSION_INFO_NOTI_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void MG_FORCE_LOGOUT_NOTI_CALLBACK(UserObject userObject, PACKET_MG_FORCE_LOGOUT_NOTI packet);
		public MG_FORCE_LOGOUT_NOTI_CALLBACK ON_MG_FORCE_LOGOUT_NOTI_CALLBACK;
		public void MG_FORCE_LOGOUT_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_MG_FORCE_LOGOUT_NOTI recvPacket = new PACKET_MG_FORCE_LOGOUT_NOTI();
			recvPacket.Deserialize(packet);
			ON_MG_FORCE_LOGOUT_NOTI_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void ML_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_ML_HELLO_NOTI packet);
		public ML_HELLO_NOTI_CALLBACK ON_ML_HELLO_NOTI_CALLBACK;
		public void ML_HELLO_NOTI_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_ML_HELLO_NOTI recvPacket = new PACKET_ML_HELLO_NOTI();
			recvPacket.Deserialize(packet);
			ON_ML_HELLO_NOTI_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void LM_HELLO_HEART_BEAT_REQ_CALLBACK(UserObject userObject, PACKET_LM_HELLO_HEART_BEAT_REQ packet);
		public LM_HELLO_HEART_BEAT_REQ_CALLBACK ON_LM_HELLO_HEART_BEAT_REQ_CALLBACK;
		public void LM_HELLO_HEART_BEAT_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_HELLO_HEART_BEAT_REQ recvPacket = new PACKET_LM_HELLO_HEART_BEAT_REQ();
			recvPacket.Deserialize(packet);
			ON_LM_HELLO_HEART_BEAT_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void LM_HELLO_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_LM_HELLO_HEART_BEAT_RES packet);
		public LM_HELLO_HEART_BEAT_RES_CALLBACK ON_LM_HELLO_HEART_BEAT_RES_CALLBACK;
		public void LM_HELLO_HEART_BEAT_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_HELLO_HEART_BEAT_RES recvPacket = new PACKET_LM_HELLO_HEART_BEAT_RES();
			recvPacket.Deserialize(packet);
			ON_LM_HELLO_HEART_BEAT_RES_CALLBACK(obj, recvPacket);
		}
#endif

#if SERVER
		public delegate void LM_STATE_INFO_REQ_CALLBACK(UserObject userObject, PACKET_LM_STATE_INFO_REQ packet);
		public LM_STATE_INFO_REQ_CALLBACK ON_LM_STATE_INFO_REQ_CALLBACK;
		public void LM_STATE_INFO_REQ_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_STATE_INFO_REQ recvPacket = new PACKET_LM_STATE_INFO_REQ();
			recvPacket.Deserialize(packet);
			ON_LM_STATE_INFO_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void LM_STATE_INFO_RES_CALLBACK(UserObject userObject, PACKET_LM_STATE_INFO_RES packet);
		public LM_STATE_INFO_RES_CALLBACK ON_LM_STATE_INFO_RES_CALLBACK;
		public void LM_STATE_INFO_RES_CONTROLLER(UserObject obj, Packet packet)
		{
			PACKET_LM_STATE_INFO_RES recvPacket = new PACKET_LM_STATE_INFO_RES();
			recvPacket.Deserialize(packet);
			ON_LM_STATE_INFO_RES_CALLBACK(obj, recvPacket);
		}
#endif

	}
}
