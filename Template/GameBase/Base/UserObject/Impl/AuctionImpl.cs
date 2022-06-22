using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class AuctionImpl
    {
        public ImplObject _obj;
        public ServerType _serverType;
        public AuctionImpl(ImplObject obj)
        {
            _obj = obj;
        }
        public AuctionImpl(ServerType type)
        {
            _serverType = type;
        }
    }
}
