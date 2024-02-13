using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class UserImpl : BaseImpl
    {
        public UserImpl(ImplObject obj) : base(obj) { }
        public UserImpl(ServerType type) : base(type) { }
    }
}
