using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class BuildingImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public BuildingImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public BuildingImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
