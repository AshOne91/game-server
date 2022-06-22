using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class ItemImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public ItemImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public ItemImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
