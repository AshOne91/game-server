using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;

namespace GameBase.Template.Account.GameBaseAccount
{
    public class GameBaseAccountImpl : AccountImpl
    {
        //GameServerObj
        private GameServerInfo _info = new GameServerInfo();
        private bool _auth = false;

        //LoginUserObj
        public ConnectInfo _connInfo = new ConnectInfo();
        public int _WantedServerId = -1;

        public GameBaseAccountImpl(ImplObject obj) : base(obj)
        {

        }
    }
}
