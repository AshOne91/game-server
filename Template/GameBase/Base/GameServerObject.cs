using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.GameBase
{
    public partial class GameServerObject : UserObject
    {
        private GameServerInfo _info = new GameServerInfo();
        private bool _auth = false;

        public GameServerObject(int serverId)
        {
            _objectID = (int)ObjectType.Game;
            _info.ServerId = serverId;

            _timeOverInterval = 5000;
            _maxTimerOverCount = 3;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            try
            {
                if (disposing)
                {
                }
            }
            catch
            {

            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
