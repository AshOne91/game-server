using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class UserImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public UserImpl(ImplObject obj)
        {
            _obj = obj;
        }
    }
}
