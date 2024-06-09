using System;
using System.Collections.Generic;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.Account.GameBaseAccount.Common;

namespace GameServer
{
    public static class AccountController
    {
        static Dictionary<ulong, GameBaseAccountProtocol> _protocolByUid = new Dictionary<ulong, GameBaseAccountProtocol>();

        public static void AddAccountController(ulong uid)
        {
            GameBaseAccountTemplate template = GameBaseTemplateContext.GetTemplate<GameBaseAccountTemplate>(uid, ETemplateType.Account);

            if (_protocolByUid.ContainsKey(uid) == true)
            {
                throw new Exception("Duplication AddAccountController");
            }
            GameBaseAccountProtocol protocol = new GameBaseAccountProtocol();
            protocol.ON_LC_HELLO_NOTI_CALLBACK = template.ON_LC_HELLO_NOTI_CALLBACK;
            protocol.ON_CL_CHECK_VERSION_REQ_CALLBACK = template.ON_CL_CHECK_VERSION_REQ_CALLBACK;
            protocol.ON_CL_CHECK_VERSION_RES_CALLBACK = template.ON_CL_CHECK_VERSION_RES_CALLBACK;
            protocol.ON_CL_CHECK_AUTH_REQ_CALLBACK = template.ON_CL_CHECK_AUTH_REQ_CALLBACK;
            protocol.ON_CL_CHECK_AUTH_RES_CALLBACK = template.ON_CL_CHECK_AUTH_RES_CALLBACK;
            protocol.ON_CL_GEN_GUEST_ID_REQ_CALLBACK = template.ON_CL_GEN_GUEST_ID_REQ_CALLBACK;
            protocol.ON_CL_GEN_GUEST_ID_RES_CALLBACK = template.ON_CL_GEN_GUEST_ID_RES_CALLBACK;
            protocol.ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK = template.ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK;
            protocol.ON_LM_CHECK_AUTH_REQ_CALLBACK = template.ON_LM_CHECK_AUTH_REQ_CALLBACK;
            protocol.ON_LM_CHECK_AUTH_RES_CALLBACK = template.ON_LM_CHECK_AUTH_RES_CALLBACK;
            protocol.ON_LM_SESSION_INFO_REQ_CALLBACK = template.ON_LM_SESSION_INFO_REQ_CALLBACK;
            protocol.ON_LM_SESSION_INFO_RES_CALLBACK = template.ON_LM_SESSION_INFO_RES_CALLBACK;
            protocol.ON_LM_DUPLICATE_LOGIN_NOTI_CALLBACK = template.ON_LM_DUPLICATE_LOGIN_NOTI_CALLBACK;
            protocol.ON_ML_GAMESERVER_INFO_NOTI_CALLBACK = template.ON_ML_GAMESERVER_INFO_NOTI_CALLBACK;
            protocol.ON_MG_HELLO_NOTI_CALLBACK = template.ON_MG_HELLO_NOTI_CALLBACK;
            protocol.ON_GM_HEART_BEAT_REQ_CALLBACK = template.ON_GM_HEART_BEAT_REQ_CALLBACK;
            protocol.ON_GM_HEART_BEAT_RES_CALLBACK = template.ON_GM_HEART_BEAT_RES_CALLBACK;
            protocol.ON_GM_CHECK_AUTH_REQ_CALLBACK = template.ON_GM_CHECK_AUTH_REQ_CALLBACK;
            protocol.ON_GM_CHECK_AUTH_RES_CALLBACK = template.ON_GM_CHECK_AUTH_RES_CALLBACK;
            protocol.ON_GM_STATE_INFO_NOTI_CALLBACK = template.ON_GM_STATE_INFO_NOTI_CALLBACK;
            protocol.ON_GM_SESSION_INFO_NOTI_CALLBACK = template.ON_GM_SESSION_INFO_NOTI_CALLBACK;
            protocol.ON_MG_FORCE_LOGOUT_NOTI_CALLBACK = template.ON_MG_FORCE_LOGOUT_NOTI_CALLBACK;
            protocol.ON_ML_HELLO_NOTI_CALLBACK = template.ON_ML_HELLO_NOTI_CALLBACK;
            protocol.ON_LM_HELLO_HEART_BEAT_REQ_CALLBACK = template.ON_LM_HELLO_HEART_BEAT_REQ_CALLBACK;
            protocol.ON_LM_HELLO_HEART_BEAT_RES_CALLBACK = template.ON_LM_HELLO_HEART_BEAT_RES_CALLBACK;
            protocol.ON_LM_STATE_INFO_REQ_CALLBACK = template.ON_LM_STATE_INFO_REQ_CALLBACK;
            protocol.ON_LM_STATE_INFO_RES_CALLBACK = template.ON_LM_STATE_INFO_RES_CALLBACK;
            protocol.ON_CL_HEART_BEAT_REQ_CALLBACK = template.ON_CL_HEART_BEAT_REQ_CALLBACK;
            protocol.ON_CL_HEART_BEAT_RES_CALLBACK = template.ON_CL_HEART_BEAT_RES_CALLBACK;
            protocol.ON_GC_HELLO_NOTI_CALLBACK = template.ON_GC_HELLO_NOTI_CALLBACK;
            protocol.ON_CG_HEARTBEAT_REQ_CALLBACK = template.ON_CG_HEARTBEAT_REQ_CALLBACK;
            protocol.ON_CG_HEARTBEAT_RES_CALLBACK = template.ON_CG_HEARTBEAT_RES_CALLBACK;
            protocol.ON_GC_HEARTBEAT_NOTI_CALLBACK = template.ON_GC_HEARTBEAT_NOTI_CALLBACK;
            protocol.ON_CG_CHECK_AUTH_REQ_CALLBACK = template.ON_CG_CHECK_AUTH_REQ_CALLBACK;
            protocol.ON_CG_CHECK_AUTH_RES_CALLBACK = template.ON_CG_CHECK_AUTH_RES_CALLBACK;
            protocol.ON_CG_CREATE_PLAYER_REQ_CALLBACK = template.ON_CG_CREATE_PLAYER_REQ_CALLBACK;
            protocol.ON_CG_CREATE_PLAYER_RES_CALLBACK = template.ON_CG_CREATE_PLAYER_RES_CALLBACK;
            protocol.ON_CG_PLAYERLIST_REQ_CALLBACK = template.ON_CG_PLAYERLIST_REQ_CALLBACK;
            protocol.ON_CG_PLAYERLIST_RES_CALLBACK = template.ON_CG_PLAYERLIST_RES_CALLBACK;
            protocol.ON_CG_PLAYER_SELECT_REQ_CALLBACK = template.ON_CG_PLAYER_SELECT_REQ_CALLBACK;
            protocol.ON_CG_PLAYER_SELECT_RES_CALLBACK = template.ON_CG_PLAYER_SELECT_RES_CALLBACK;
            _protocolByUid.Add(uid, protocol);
        }

        public static void RemoveAccountController(ulong uid)
        {
            if (_protocolByUid.ContainsKey(uid) == true)
            {
                _protocolByUid.Remove(uid);
            }
        }

        public static bool OnPacket(ImplObject obj, ushort protocolId, Packet packet)
        {
            ulong uid = obj.GetSession().GetUid();
            if (_protocolByUid.ContainsKey(uid) == false)
            {
                return false;
            }
            var Protocol = _protocolByUid[uid];
            return Protocol.OnPacket(obj, protocolId, packet);
        }
    }
}
