using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class SeasonImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public SeasonImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public SeasonImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
