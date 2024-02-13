using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class BaseImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        private Action<ImplObject, string, object> _fnCall;
        public Action<ImplObject, string, object> FnCall
        {
            get { return _fnCall; }
            set { _fnCall = value; }
        }

        public void ClientCallback(string action, object extraInfo = null)
        {
            if (_fnCall != null)
            {
                _fnCall(_obj, action, extraInfo);
            }
        }
        public BaseImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public BaseImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
