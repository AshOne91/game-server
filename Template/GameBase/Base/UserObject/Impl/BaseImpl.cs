using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class BaseImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public BaseImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public BaseImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
