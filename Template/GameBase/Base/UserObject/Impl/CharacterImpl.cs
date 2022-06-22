using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class CharacterImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public CharacterImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public CharacterImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
