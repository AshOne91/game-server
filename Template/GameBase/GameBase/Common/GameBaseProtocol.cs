using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Common
{
        public enum ELGameBaseProtocol
        {
            CL_BASE = 1,
            LC_HELLO_NOTI,
            CL_HEART_BEAT_REQ,
            LC_HEART_BEAT_RES,
            CL_CHECK_VERSION_REQ = 10,
            LC_CHECK_VERSION_RES,
            CL_CHECK_AUTH_REQ,
            LC_CHECK_AUTH_RES,
            CL_GEN_GUEST_ID_REQ,
            LC_GEN_GUEST_ID_RES,
            LC_DUPLICATE_LOGIN_NOTI,
        }

        public enum EGGameBaseProtocol
        {
            GC_BASE = 1000,
            GC_HELLO_NOTI,
            CG_HEARTBEAT_REQ,
            GC_HEARTBEAT_RES,
            GC_HEARTBEAT_NOTI,
            CG_CHECK_AUTH_REQ,
            GC_CHECK_AUTH_RES
        }

        public enum EMPacketProtocl
        {
            MG_BASE = 2000,
            MG_HELLO_NOTI,
            GM_HEART_BEAT_REQ,
            MG_HEART_BEAT_RES,
            GM_CHECK_AUTH_REQ,
            MG_CHECK_AUTH_RES,

            ML_BASE = 3000,
            ML_HELLO_NOTI,
            LM_HEART_BEAT_REQ,
            ML_HEART_BEAT_RES,
            LM_CHECK_AUTH_REQ,
            ML_CHECK_AUTH_RES
        }
}
