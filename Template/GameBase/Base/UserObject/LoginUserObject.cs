using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class LoginUserObject : ImplObject
    {
        public LoginUserObject()
        {
            _objectID = (int)ObjectType.User;
        }
    }
}
