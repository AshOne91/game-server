using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class BuildingImpl : BaseImpl
    {
        public BuildingImpl(ImplObject obj) : base(obj) { }
        public BuildingImpl(ServerType type) : base(type) { }
    }
}
