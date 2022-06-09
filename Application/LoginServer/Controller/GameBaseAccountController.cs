using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.Account.GameBaseAccount.Common;


namespace LoginServer.Controllers
{
    public static class AccountController
    {
        static Dictionary<ulong, GameBaseAccountProtocol> _protocolByUid = new Dictionary<ulong, GameBaseAccountProtocol>();
        public static void AddAccountController(ulong uid)
        {
            GameBaseAccountTemplate template = GameBaseTemplateContext.GetTemplate<GameBaseAccountTemplate>(uid, ETemplateType.Account);

            if (_protocolByUid.ContainsKey(uid) == true)
            {
                throw new Exception("Duplicate AddAccountController");
            }
            GameBaseAccountProtocol protocol = new GameBaseAccountProtocol();
            protocol.ON_LC_HELLO_NOTI_CALLBACK = template.ON_LC_HELLO_NOTI_CALLBACK;
            protocol.ON_CL_CHECK_VERSION_REQ_CALLBACK = template.ON_CL_CHECK_VERSION_REQ_CALLBACK;
            protocol.ON_CL_CHECK_VERSION_RES_CALLBACK = template.ON_CL_CHECK_VERSION_RES_CALLBACK;
            protocol.ON_CL_CHECK_AUTH_REQ_CALLBACK = template.ON_CL_CHECK_AUTH_REQ_CALLBACK;
            protocol.ON_CL_CHECK_AUTH_RES_CALLBACK = template.ON_CL_CHECK_AUTH_RES_CALLBACK;
            protocol.ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK = template.ON_LC_DUPLICATE_LOGIN_NOTI_CALLBACK;
            _protocolByUid.Add(uid, protocol);
        }

        public static bool OnPacket(UserObject obj, ushort protocolId, Packet packet)
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
