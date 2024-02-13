using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class CharacterImpl : BaseImpl
    {
        public CharacterImpl(ImplObject obj) : base(obj) { }
        public CharacterImpl(ServerType type) : base(type) { }
    }
}
