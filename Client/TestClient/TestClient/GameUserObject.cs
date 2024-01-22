using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using GameBase.Template.GameBase;

namespace TestClient.TestClient
{
    public class GameUserObject : ImplObject
    {
        public GameUserObject()
        {
            _objectID = (int)ObjectType.User;
        }


    }
}
