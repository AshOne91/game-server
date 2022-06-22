using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class AttendanceImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public AttendanceImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public AttendanceImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
