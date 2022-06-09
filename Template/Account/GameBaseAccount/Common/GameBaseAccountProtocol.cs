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
	}
}
