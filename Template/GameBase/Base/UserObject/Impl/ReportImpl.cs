using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class ReportImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public ReportImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public ReportImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
