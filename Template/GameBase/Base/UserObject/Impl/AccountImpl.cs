using System;
using System.Collections.Generic;
using System.Text;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.GameBase
{
    public class AccountImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public AccountImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public AccountImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
