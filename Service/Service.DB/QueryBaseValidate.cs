using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public abstract class QueryBaseValidate : QueryBase
    {
        private UserObject _callUser;

        private ulong _callerUserDBKey;
        private ulong _callerSessionKey;

        public QueryBaseValidate(UserObject userObject) : base()
        {
            _callUser = userObject;

            /*if (m_pCallUser)
            {
                m_CallerUserDBKey = m_pCallUser->GetUserDBKey();
                m_CallerGateSessionKey = m_pCallUser->GetGateSessionKey();
                m_CallerGTUserSessionKey = m_pCallUser->GetGTUserSessionKey();
            }
            if (m_pCallPlayer)
            {
                m_CallerPlayerDBKey = m_pCallPlayer->GetPlayerDBKey();
            }*/
            if (_callUser != null)
            {
                _callerSessionKey = _callUser.GetSession().GetUid();
                _callerUserDBKey = _callUser.GetUserDBKey();
            }

        }
        ~QueryBaseValidate() { }

        public override bool IsValidCheck()
        {
            /*if (m_pCallUser == NULL)
            {
                return true;
            }
            CGUser* pGUser = CGUserManager::Instance()->FindGUserBySessionKey(m_CallerGateSessionKey, m_CallerGTUserSessionKey);
            if (pGUser == m_pCallUser)
            {
                if (m_pCallPlayer)
                {
                    if (pGUser->GetPlayer() == m_pCallPlayer)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;*/
            if (_callUser == null)
            {
                return true;
            }
            if (_callUser.GetSession() == null)
            {
                return true;
            }
            var session = _callUser.GetSession().GetSessionManager().FindActiveSession(_callUser.GetSession().GetUid());
            if (_callUser == session.GetUserObject())
            {
                return true;
            }
            return false;
        }

        public override string vGetName()
        {
            new NotImplementedException("vGetName");
            return "";
        }

        UserObject GetCallUser() { return _callUser; }
        ulong GetCallerUserDBKey() { return _callerUserDBKey; }
    }
}
