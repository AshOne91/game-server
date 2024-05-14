using System;
using System.Collections.Generic;
using System.Text;
using GameBase.Template.GameBase.Common;
using Service.Core;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class GameUserObject : ImplObject
    {
        private UserSessionData _sessionData = new UserSessionData();
        private TimeCounter _updateSessionTick = new TimeCounter();
        private int _updateSessionInterval = 60000;
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
            if (_updateSessionTick.IsFinished())
            {
                GameBaseTemplateContext.Account.UpdateSessionInfo(this);
                _updateSessionTick.Start(_updateSessionInterval);
            }
        }
    }
}
