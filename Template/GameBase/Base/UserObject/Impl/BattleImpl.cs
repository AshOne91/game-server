using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class BattleImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public BattleImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public BattleImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
