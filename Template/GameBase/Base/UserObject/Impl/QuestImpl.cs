using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class QuestImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public QuestImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public QuestImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
