using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.GameBase
{
    public partial class GameServerObject : ImplObject
    {
        public GameServerObject()
        {
            _objectID = (int)ObjectType.Game;

            _timeOverInterval = 5000;
            _maxTimerOverCount = 3;
        }
    }
}
