using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public abstract class QueryBaseValidate : QueryBase
    {
        private UserObject _callUser;

        private Int64 _callerUserDBKey;
        private Int64 _callerSessionKey;
        private Int64 _callerPlayerDBKey;

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
            return true;
        }

        public override string vGetName()
        {
            new NotImplementedException("vGetName");
            return "";
        }

        UserObject GetCallUser() { return _callUser; }
        Int64 GetCallerUserDBKey() { return _callerUserDBKey; }
        Int64 GetCallerPlayerDBKey() { return _callerPlayerDBKey; }
    }
}
