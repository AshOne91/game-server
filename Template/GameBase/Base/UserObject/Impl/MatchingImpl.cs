using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class MatchingImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public MatchingImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public MatchingImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
