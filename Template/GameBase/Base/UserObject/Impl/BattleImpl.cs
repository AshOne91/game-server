using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class BattleImpl : BaseImpl
    {
        public BattleImpl(ImplObject obj) : base(obj) { }
        public BattleImpl(ServerType type) : base(type) { }
    }
}
