using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class AdminImpl : BaseImpl
    {
        public AdminImpl(ImplObject obj) : base(obj) { }
        public AdminImpl(ServerType type) : base(type) { }
    }
}
