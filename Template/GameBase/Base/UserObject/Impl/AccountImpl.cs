using System;
using System.Collections.Generic;
using System.Text;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.GameBase
{
    public class AccountImpl : BaseImpl
    {
        public AccountImpl(ImplObject obj) : base(obj) { }
        public AccountImpl(ServerType type) : base(type) { }
    }
}
