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
            if (_callUser != null)
            {
                _callerSessionKey = _callUser.GetSession().GetUid();
                _callerUserDBKey = _callUser.GetUserDBKey();
            }

        }
        ~QueryBaseValidate() { }

        public override bool IsValidCheck()
        {
            if (_callUser == null)
            {
                return true;
            }

            var session = _callUser.GetSession().GetSessionManager().FindActiveSession(_callerSessionKey);
            if (session == null)
            {
                return false;
            }

            return true;
        }

        public override string vGetName()
        {
            new NotImplementedException("vGetName");
            return "";
        }

        public UserObject GetCallUser() { return _callUser; }
        public ulong GetCallerUserDBKey() { return _callerUserDBKey; }
    }
}
