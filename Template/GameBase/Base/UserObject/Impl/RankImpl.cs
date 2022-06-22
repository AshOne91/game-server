using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class RankImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public RankImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public RankImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
