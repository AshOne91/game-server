using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Service.Core;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class MasterClientObject : ImplObject
    {
        private TimeCounter _stateTimer = new TimeCounter();
        private int _stateInterval = 2000;

        public MasterClientObject()
        {
            _objectID = (int)ObjectType.Master;
        }

        public override void OnConnect(IPEndPoint ep)
        {
            base.OnConnect(ep);
            if (GameBaseTemplate.ServerType == ServerType.Game)
            {
                _stateTimer.Start(_stateInterval);
            }
        }

        public override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            if (_stateTimer.IsFinished())
            {
                GameBaseTemplateContext.Account.SendStateInfo(this);
                _stateTimer.Start(_stateInterval);
            }
        }
    }
}
