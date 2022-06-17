using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class GameUserObject : ImplObject
    {
        public GameUserObject()
        {
            _objectID = (int)ObjectType.User;
        }
    }
}
