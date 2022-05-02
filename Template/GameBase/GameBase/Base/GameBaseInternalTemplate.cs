using GameBase.Common;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Base
{
    public class GameBaseInternalTemplate : GameBaseTemplate
    {
        public void ON_LC_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_LC_HELLO_NOTI packet)
        {

        }
        public void ON_CL_HEART_BEAT_REQ_CALLBACK(UserObject userObject, PACKET_CL_HEART_BEAT_REQ packet)
        {

        }
        public void ON_LC_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_LC_HEART_BEAT_RES packet)
        {

        }
        public void ON_CL_CHECK_VERSION_REQ_CALLBACK(UserObject userObject, PACKET_CL_CHECK_VERSION_REQ packet)
        {

        }
        public void ON_LC_CHECK_VERSION_RES_CALLBACK(UserObject userObject, PACKET_LC_CHECK_VERSION_RES packet)
        {

        }
        public void ON_CL_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_CL_CHECK_AUTH_REQ packet)
        {

        }
        public void ON_LC_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_LC_CHECK_AUTH_RES packet)
        {

        }
        public void ON_CL_GEN_GUEST_ID_REQ_CALLBACK(UserObject userObject, PACKET_CL_GEN_GUEST_ID_REQ packet)
        {

        }
        public void ON_LC_GEN_GUEST_ID_RES_CALLBACK(UserObject userObject, PACKET_LC_GEN_GUEST_ID_RES packet)
        {

        }
        public void ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK(UserObject userObject, PACKET_LC_DUPLICATE_LOGIN_NOTI packet)
        {

        }
        public void ON_GC_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_GC_HELLO_NOTI packet)
        {

        }
        public void ON_CG_HEARTBEAT_REQ_CALLBACK(UserObject userObject, PACKET_CG_HEARTBEAT_REQ packet)
        {

        }
        public void ON_GC_HEARTBEAT_RES_CALLBACK(UserObject userObject, PACKET_GC_HEARTBEAT_RES packet)
        {

        }
        public void ON_GC_HEARTBEAT_NOTI_CALLBACK(UserObject userObject, PACKET_GC_HEARTBEAT_NOTI packet)
        {

        }
        public void ON_CG_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_CG_CHECK_AUTH_REQ packet)
        {

        }
        public void ON_GC_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_GC_CHECK_AUTH_RES packet)
        {

        }
        public void ON_MG_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_MG_HELLO_NOTI packet)
        {

        }
        public void ON_GM_HEART_BEAT_REQ_CALLBACK(UserObject userObject, PACKET_GM_HEART_BEAT_REQ packet)
        {

        }
        public void ON_MG_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_MG_HEART_BEAT_RES packet)
        {

        }
        public void ON_GM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_GM_CHECK_AUTH_REQ packet)
        {

        }
        public void ON_MG_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_MG_CHECK_AUTH_RES packet)
        {

        }
        public void ON_ML_HELLO_NOTI_CALLBACK(UserObject userObject, PACKET_ML_HELLO_NOTI packet)
        {
            PACKET_LM_CHECK_AUTH_REQ sendData = new PACKET_LM_CHECK_AUTH_REQ();
            sendData.ServerGUID = PacketDefine.SERVER_GUID;
            sendData.Ver = "1.0.0";
            sendData.HostIP = LoginServerEntry.GetConfig()._LoginIP;
            sendData.HostPort = LoginServerEntry.GetConfig()._LoginPort;
            userObject.GetSession().SendPacket(sendData.Serialize());
            
        }
        public void ON_LM_HEART_BEAT_REQ_CALLBACK(UserObject userObject, PACKET_LM_HEART_BEAT_REQ packet)
        {

        }
        public void ON_ML_HEART_BEAT_RES_CALLBACK(UserObject userObject, PACKET_ML_HEART_BEAT_RES packet)
        {

        }
        public void ON_LM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_REQ packet)
        {

        }
        public void ON_ML_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_ML_CHECK_AUTH_RES packet)
        {
            if (packet.ErrorCode == GServerCode.SUCCESS)
            {
                MasterOb
            }
            else
            {

            }
        }
    }
}
