using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class AdminImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public AdminImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public AdminImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
