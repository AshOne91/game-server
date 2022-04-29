using GameBase.Base;
using GameBase.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Controller
{
    public class GameBaseController
    {
        GameBaseProtocol _protocol;

        public GameBaseController()
        {
            GameBaseInternalTemplate template = GameBaseTemplateContext.GetTemplate<GameBaseInternalTemplate>(ETemplateType.InternalGame);

            _protocol = new GameBaseProtocol();
            _protocol.ON_LC_HELLO_NOTI_CALLBACK = template.ON_LC_HELLO_NOTI_CALLBACK;
            _protocol.ON_CL_HEART_BEAT_REQ_CALLBACK = template.ON_CL_HEART_BEAT_REQ_CALLBACK;
            _protocol.ON_LC_HEART_BEAT_RES_CALLBACK = template.ON_LC_HEART_BEAT_RES_CALLBACK;
            _protocol.ON_CL_CHECK_VERSION_REQ_CALLBACK = template.ON_CL_CHECK_VERSION_REQ_CALLBACK;
            _protocol.ON_LC_CHECK_VERSION_RES_CALLBACK = template.ON_LC_CHECK_VERSION_RES_CALLBACK;
            _protocol.ON_CL_CHECK_AUTH_REQ_CALLBACK = template.ON_CL_CHECK_AUTH_REQ_CALLBACK;
            _protocol.ON_LC_CHECK_AUTH_RES_CALLBACK = template.ON_LC_CHECK_AUTH_RES_CALLBACK;
            _protocol.ON_CL_GEN_GUEST_ID_REQ_CALLBACK = template.ON_CL_GEN_GUEST_ID_REQ_CALLBACK;
            _protocol.ON_LC_GEN_GUEST_ID_RES_CALLBACK = template.ON_LC_GEN_GUEST_ID_RES_CALLBACK;
            _protocol.ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK = template.ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK;
            _protocol.ON_GC_HELLO_NOTI_CALLBACK = template.ON_GC_HELLO_NOTI_CALLBACK;
            _protocol.ON_CG_HEARTBEAT_REQ_CALLBACK = template.ON_CG_HEARTBEAT_REQ_CALLBACK;
            _protocol.ON_GC_HEARTBEAT_RES_CALLBACK = template.ON_GC_HEARTBEAT_RES_CALLBACK;
            _protocol.ON_GC_HEARTBEAT_NOTI_CALLBACK = template.ON_GC_HEARTBEAT_NOTI_CALLBACK;
            _protocol.ON_CG_CHECK_AUTH_REQ_CALLBACK = template.ON_CG_CHECK_AUTH_REQ_CALLBACK;
            _protocol.ON_GC_CHECK_AUTH_RES_CALLBACK = template.ON_GC_CHECK_AUTH_RES_CALLBACK;
            _protocol.ON_MG_HELLO_NOTI_CALLBACK = template.ON_MG_HELLO_NOTI_CALLBACK;
            _protocol.ON_GM_HEART_BEAT_REQ_CALLBACK = template.ON_GM_HEART_BEAT_REQ_CALLBACK;
            _protocol.ON_MG_HEART_BEAT_RES_CALLBACK = template.ON_MG_HEART_BEAT_RES_CALLBACK;
            _protocol.ON_GM_CHECK_AUTH_REQ_CALLBACK = template.ON_GM_CHECK_AUTH_REQ_CALLBACK;
            _protocol.ON_MG_CHECK_AUTH_RES_CALLBACK = template.ON_MG_CHECK_AUTH_RES_CALLBACK;
            _protocol.ON_ML_HELLO_NOTI_CALLBACK = template.ON_ML_HELLO_NOTI_CALLBACK;
            _protocol.ON_LM_HEART_BEAT_REQ_CALLBACK = template.ON_LM_HEART_BEAT_REQ_CALLBACK;
            _protocol.ON_ML_HEART_BEAT_RES_CALLBACK = template.ON_ML_HEART_BEAT_RES_CALLBACK;
            _protocol.ON_LM_CHECK_AUTH_REQ_CALLBACK = template.ON_LM_CHECK_AUTH_REQ_CALLBACK;
            _protocol.ON_ML_CHECK_AUTH_RES_CALLBACK = template.ON_ML_CHECK_AUTH_RES_CALLBACK;
        }
    }
}
