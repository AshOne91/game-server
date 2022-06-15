using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public partial class LoginServerObject : UserObject
    {
        public LoginServerObject(int serverId)
        {
            _objectID = (int)ObjectType.Login;
            _ServerId = serverId;
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
        private int _ServerId;
    }
}
