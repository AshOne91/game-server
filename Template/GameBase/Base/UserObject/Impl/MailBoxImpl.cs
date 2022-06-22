using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class MailBoxImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public MailBoxImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public MailBoxImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
