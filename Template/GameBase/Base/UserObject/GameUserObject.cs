using System;
using System.Collections.Generic;
using System.Text;
using GameBase.Template.GameBase.Common;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class GameUserObject : ImplObject
    {
        private UserSessionData _sessionData = new UserSessionData();
        public UserSessionData SessionData 
        {
            get => _sessionData; 
            set => _sessionData = value;
        }
        public GameUserObject()
        {
            _objectID = (int)ObjectType.User;
        }

        public override void OnUpdate(float dt)
        {
            
        }
    }
}
