using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class AdvertImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public AdvertImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public AdvertImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
