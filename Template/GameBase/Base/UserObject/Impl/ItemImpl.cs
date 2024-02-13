using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class ItemImpl : BaseImpl
    {
        public ItemImpl(ImplObject obj) : base(obj) { }
        public ItemImpl(ServerType type) : base(type) { }
    }
}
