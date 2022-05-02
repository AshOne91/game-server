using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Common
{
    public partial class GameBaseProtocol
    {
        public static Dictionary<UInt16, ControllerDelegate> MessageControllers = new Dictionary<ushort, ControllerDelegate>();

        public GameBaseProtocol()
        {
            Init();
        }

        void Init()
        {
            MessageControllers.Add(PACKET_LC_HELLO_NOTI.ProtocolId, LC_HELLO_NOTI_CONTROLLER);
            MessageControllers.Add(PACKET_CL_HEART_BEAT_REQ.ProtocolId, CL_HEART_BEAT_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_LC_HEART_BEAT_RES.ProtocolId, LC_HEART_BEAT_RES_CONTROLLER);
            MessageControllers.Add(PACKET_CL_CHECK_VERSION_REQ.ProtocolId, CL_CHECK_VERSION_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_LC_CHECK_VERSION_RES.ProtocolId, LC_CHECK_VERSION_RES_CONTROLLER);
            MessageControllers.Add(PACKET_CL_CHECK_AUTH_REQ.ProtocolId, CL_CHECK_AUTH_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_LC_CHECK_AUTH_RES.ProtocolId, LC_CHECK_AUTH_RES_CONTROLLER);
            MessageControllers.Add(PACKET_CL_GEN_GUEST_ID_REQ.ProtocolId, CL_GEN_GUEST_ID_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_LC_GEN_GUEST_ID_RES.ProtocolId, LC_GEN_GUEST_ID_RES_CONTROLLER);
            MessageControllers.Add(PACKET_LC_DUPLICATE_LOGIN_NOTI.ProtocolId, LC_DUPLICATE_LOGIN_NOTI_CONTROLLER);

            MessageControllers.Add(PACKET_GC_HELLO_NOTI.ProtocolId, GC_HELLO_NOTI_CONTROLLER);
            MessageControllers.Add(PACKET_CG_HEARTBEAT_REQ.ProtocolId, CG_HEARTBEAT_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_GC_HEARTBEAT_RES.ProtocolId, GC_HEARTBEAT_RES_CONTROLLER);
            MessageControllers.Add(PACKET_GC_HEARTBEAT_NOTI.ProtocolId, GC_HEARTBEAT_NOTI_CONTROLLER);
            MessageControllers.Add(PACKET_CG_CHECK_AUTH_REQ.ProtocolId, CG_CHECK_AUTH_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_GC_CHECK_AUTH_RES.ProtocolId, GC_CHECK_AUTH_RES_CONTROLLER);

            MessageControllers.Add(PACKET_MG_HELLO_NOTI.ProtocolId, MG_HELLO_NOTI_CONTROLLER);
            MessageControllers.Add(PACKET_GM_HEART_BEAT_REQ.ProtocolId, GM_HEART_BEAT_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_MG_HEART_BEAT_RES.ProtocolId, MG_HEART_BEAT_RES_CONTROLLER);
            MessageControllers.Add(PACKET_GM_CHECK_AUTH_REQ.ProtocolId, GM_CHECK_AUTH_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_MG_CHECK_AUTH_RES.ProtocolId, MG_CHECK_AUTH_RES_CONTROLLER);

            MessageControllers.Add(PACKET_ML_HELLO_NOTI.ProtocolId, ML_HELLO_NOTI_CONTROLLER);
            MessageControllers.Add(PACKET_LM_HEART_BEAT_REQ.ProtocolId, LM_HEART_BEAT_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_ML_HEART_BEAT_RES.ProtocolId, ML_HEART_BEAT_RES_CONTROLLER);
            MessageControllers.Add(PACKET_LM_CHECK_AUTH_REQ.ProtocolId, LM_CHECK_AUTH_REQ_CONTROLLER);
            MessageControllers.Add(PACKET_ML_CHECK_AUTH_RES.ProtocolId, ML_CHECK_AUTH_RES_CONTROLLER);
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
        public delegate void CL_HEART_BEAT_REQ(UserObject userObject, PACKET_CL_HEART_BEAT_REQ packet);
        public CL_HEART_BEAT_REQ ON_CL_HEART_BEAT_REQ_CALLBACK;
        public void CL_HEART_BEAT_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_CL_HEART_BEAT_REQ recvPacket = new PACKET_CL_HEART_BEAT_REQ();
            recvPacket.Deserialize(packet);
            ON_CL_HEART_BEAT_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void LC_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_LC_HEART_BEAT_RES packet);
        public LC_HEART_BEAT_RES_CALLBACK ON_LC_HEART_BEAT_RES_CALLBACK;
        public void LC_HEART_BEAT_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_LC_HEART_BEAT_RES recvPacket = new PACKET_LC_HEART_BEAT_RES();
            recvPacket.Deserialize(packet);
            ON_LC_HEART_BEAT_RES_CALLBACK(obj, recvPacket);
        }
        public delegate void CL_CHECK_VERSION_REQ_CALLBACK(UserObject userObject, PACKET_CL_CHECK_VERSION_REQ packet);
        public CL_CHECK_VERSION_REQ_CALLBACK ON_CL_CHECK_VERSION_REQ_CALLBACK;
        public void CL_CHECK_VERSION_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_CL_CHECK_VERSION_REQ recvPacket = new PACKET_CL_CHECK_VERSION_REQ();
            recvPacket.Deserialize(packet);
            ON_CL_CHECK_VERSION_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void LC_CHECK_VERSION_RES_CALLBACK(UserObject userObject, PACKET_LC_CHECK_VERSION_RES packet);
        public LC_CHECK_VERSION_RES_CALLBACK ON_LC_CHECK_VERSION_RES_CALLBACK;
        public void LC_CHECK_VERSION_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_LC_CHECK_VERSION_RES recvPacket = new PACKET_LC_CHECK_VERSION_RES();
            recvPacket.Deserialize(packet);
            ON_LC_CHECK_VERSION_RES_CALLBACK(obj, recvPacket);
        }
        public delegate void CL_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_CL_CHECK_AUTH_REQ packet);
        public CL_CHECK_AUTH_REQ_CALLBACK ON_CL_CHECK_AUTH_REQ_CALLBACK;
        public void CL_CHECK_AUTH_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_CL_CHECK_AUTH_REQ recvPacket = new PACKET_CL_CHECK_AUTH_REQ();
            recvPacket.Deserialize(packet);
            ON_CL_CHECK_AUTH_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void LC_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_LC_CHECK_AUTH_RES packet);
        public LC_CHECK_AUTH_RES_CALLBACK ON_LC_CHECK_AUTH_RES_CALLBACK;
        public void LC_CHECK_AUTH_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_LC_CHECK_AUTH_RES recvPacket = new PACKET_LC_CHECK_AUTH_RES();
            recvPacket.Deserialize(packet);
            ON_LC_CHECK_AUTH_RES_CALLBACK(obj, recvPacket);
        }
        public delegate void CL_GEN_GUEST_ID_REQ_CALLBACK(UserObject userObject, PACKET_CL_GEN_GUEST_ID_REQ packet);
        public CL_GEN_GUEST_ID_REQ_CALLBACK ON_CL_GEN_GUEST_ID_REQ_CALLBACK;
        public void CL_GEN_GUEST_ID_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_CL_GEN_GUEST_ID_REQ recvPacket = new PACKET_CL_GEN_GUEST_ID_REQ();
            recvPacket.Deserialize(packet);
            ON_CL_GEN_GUEST_ID_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void LC_GEN_GUEST_ID_RES_CALLBACK(UserObject userObject, PACKET_LC_GEN_GUEST_ID_RES packet);
        public LC_GEN_GUEST_ID_RES_CALLBACK ON_LC_GEN_GUEST_ID_RES_CALLBACK;
        public void LC_GEN_GUEST_ID_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_LC_GEN_GUEST_ID_RES recvPacket = new PACKET_LC_GEN_GUEST_ID_RES();
            recvPacket.Deserialize(packet);
            ON_LC_GEN_GUEST_ID_RES_CALLBACK(obj, recvPacket);
        }
        public delegate void LC_DUPLICATE_LOGIN_NOTI_CALLBACK(UserObject userObject, PACKET_LC_DUPLICATE_LOGIN_NOTI packet);
        public LC_DUPLICATE_LOGIN_NOTI_CALLBACK ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK;
        public void LC_DUPLICATE_LOGIN_NOTI_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_LC_DUPLICATE_LOGIN_NOTI recvPacket = new PACKET_LC_DUPLICATE_LOGIN_NOTI();
            recvPacket.Deserialize(packet);
            ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK(obj, recvPacket);
        }
        public delegate void GC_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_GC_HELLO_NOTI packet);
        public GC_HELLO_NOTI_CALLBACK ON_GC_HELLO_NOTI_CALLBACK;
        public void GC_HELLO_NOTI_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_GC_HELLO_NOTI recvPacket = new PACKET_GC_HELLO_NOTI();
            recvPacket.Deserialize(packet);
            ON_GC_HELLO_NOTI_CALLBACK(obj, recvPacket);
        }
        public delegate void CG_HEARTBEAT_REQ_CALLBACK(UserObject userObject, PACKET_CG_HEARTBEAT_REQ packet);
        public CG_HEARTBEAT_REQ_CALLBACK ON_CG_HEARTBEAT_REQ_CALLBACK;
        public void CG_HEARTBEAT_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_CG_HEARTBEAT_REQ recvPacket = new PACKET_CG_HEARTBEAT_REQ();
            recvPacket.Deserialize(packet);
            ON_CG_HEARTBEAT_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void GC_HEARTBEAT_RES_CALLBACK(UserObject userObject, PACKET_GC_HEARTBEAT_RES packet);
        public GC_HEARTBEAT_RES_CALLBACK ON_GC_HEARTBEAT_RES_CALLBACK;
        public void GC_HEARTBEAT_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_GC_HEARTBEAT_RES recvPacket = new PACKET_GC_HEARTBEAT_RES();
            recvPacket.Deserialize(packet);
            ON_GC_HEARTBEAT_RES_CALLBACK(obj, recvPacket);
        }
        public delegate void GC_HEARTBEAT_NOTI_CALLBACK(UserObject userObject, PACKET_GC_HEARTBEAT_NOTI packet);
        public GC_HEARTBEAT_NOTI_CALLBACK ON_GC_HEARTBEAT_NOTI_CALLBACK;
        public void GC_HEARTBEAT_NOTI_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_GC_HEARTBEAT_NOTI recvPacket = new PACKET_GC_HEARTBEAT_NOTI();
            recvPacket.Deserialize(packet);
            ON_GC_HEARTBEAT_NOTI_CALLBACK(obj, recvPacket);
        }
        public delegate void CG_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_CG_CHECK_AUTH_REQ packet);
        public CG_CHECK_AUTH_REQ_CALLBACK ON_CG_CHECK_AUTH_REQ_CALLBACK;
        public void CG_CHECK_AUTH_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_CG_CHECK_AUTH_REQ recvPacket = new PACKET_CG_CHECK_AUTH_REQ();
            recvPacket.Deserialize(packet);
            ON_CG_CHECK_AUTH_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void GC_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_GC_CHECK_AUTH_RES packet);
        public GC_CHECK_AUTH_RES_CALLBACK ON_GC_CHECK_AUTH_RES_CALLBACK;
        public void GC_CHECK_AUTH_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_GC_CHECK_AUTH_RES recvPacket = new PACKET_GC_CHECK_AUTH_RES();
            recvPacket.Deserialize(packet);
            ON_GC_CHECK_AUTH_RES_CALLBACK(obj, recvPacket);
        }

        public delegate void MG_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_MG_HELLO_NOTI packet);
        public MG_HELLO_NOTI_CALLBACK ON_MG_HELLO_NOTI_CALLBACK;
        public void MG_HELLO_NOTI_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_MG_HELLO_NOTI recvPacket = new PACKET_MG_HELLO_NOTI();
            recvPacket.Deserialize(packet);
            ON_MG_HELLO_NOTI_CALLBACK(obj, recvPacket);
        }
        public delegate void GM_HEART_BEAT_REQ_CALLBACK(UserObject userObject, PACKET_GM_HEART_BEAT_REQ packet);
        public GM_HEART_BEAT_REQ_CALLBACK ON_GM_HEART_BEAT_REQ_CALLBACK;
        public void GM_HEART_BEAT_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_GM_HEART_BEAT_REQ recvPacket = new PACKET_GM_HEART_BEAT_REQ();
            recvPacket.Deserialize(packet);
            ON_GM_HEART_BEAT_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void MG_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_MG_HEART_BEAT_RES packet);
        public MG_HEART_BEAT_RES_CALLBACK ON_MG_HEART_BEAT_RES_CALLBACK;
        public void MG_HEART_BEAT_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_MG_HEART_BEAT_RES recvPacket = new PACKET_MG_HEART_BEAT_RES();
            recvPacket.Deserialize(packet);
            ON_MG_HEART_BEAT_RES_CALLBACK(obj, recvPacket);
        }
        public delegate void GM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_GM_CHECK_AUTH_REQ packet);
        public GM_CHECK_AUTH_REQ_CALLBACK ON_GM_CHECK_AUTH_REQ_CALLBACK;
        public void GM_CHECK_AUTH_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_GM_CHECK_AUTH_REQ recvPacket = new PACKET_GM_CHECK_AUTH_REQ();
            recvPacket.Deserialize(packet);
            ON_GM_CHECK_AUTH_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void MG_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_MG_CHECK_AUTH_RES packet);
        public MG_CHECK_AUTH_RES_CALLBACK ON_MG_CHECK_AUTH_RES_CALLBACK;
        public void MG_CHECK_AUTH_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_MG_CHECK_AUTH_RES recvPacket = new PACKET_MG_CHECK_AUTH_RES();
            recvPacket.Deserialize(packet);
            ON_MG_CHECK_AUTH_RES_CALLBACK(obj, recvPacket);
        }

        public delegate void ML_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_ML_HELLO_NOTI packet);
        public ML_HELLO_NOTI_CALLBACK ON_ML_HELLO_NOTI_CALLBACK;
        public void ML_HELLO_NOTI_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_ML_HELLO_NOTI recvPacket = new PACKET_ML_HELLO_NOTI();
            recvPacket.Deserialize(packet);
            ON_ML_HELLO_NOTI_CALLBACK(obj, recvPacket);
        }
        public delegate void LM_HEART_BEAT_REQ_CALLBACK(UserObject userObject, PACKET_LM_HEART_BEAT_REQ packet);
        public LM_HEART_BEAT_REQ_CALLBACK ON_LM_HEART_BEAT_REQ_CALLBACK;
        public void LM_HEART_BEAT_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_LM_HEART_BEAT_REQ recvPacket = new PACKET_LM_HEART_BEAT_REQ();
            recvPacket.Deserialize(packet);
            ON_LM_HEART_BEAT_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void ML_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_ML_HEART_BEAT_RES packet);
        public ML_HEART_BEAT_RES_CALLBACK ON_ML_HEART_BEAT_RES_CALLBACK;
        public void ML_HEART_BEAT_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_ML_HEART_BEAT_RES recvPacket = new PACKET_ML_HEART_BEAT_RES();
            recvPacket.Deserialize(packet);
            ON_ML_HEART_BEAT_RES_CALLBACK(obj, recvPacket);
        }
        public delegate void LM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_REQ packet);
        public LM_CHECK_AUTH_REQ_CALLBACK ON_LM_CHECK_AUTH_REQ_CALLBACK;
        public void LM_CHECK_AUTH_REQ_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_LM_CHECK_AUTH_REQ recvPacket = new PACKET_LM_CHECK_AUTH_REQ();
            recvPacket.Deserialize(packet);
            ON_LM_CHECK_AUTH_REQ_CALLBACK(obj, recvPacket);
        }
        public delegate void ML_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_ML_CHECK_AUTH_RES packet);
        public ML_CHECK_AUTH_RES_CALLBACK ON_ML_CHECK_AUTH_RES_CALLBACK;
        public void ML_CHECK_AUTH_RES_CONTROLLER(UserObject obj, Packet packet)
        {
            PACKET_ML_CHECK_AUTH_RES recvPacket = new PACKET_ML_CHECK_AUTH_RES();
            recvPacket.Deserialize(packet);
            ON_ML_CHECK_AUTH_RES_CALLBACK(obj, recvPacket);
        }
    }
}
