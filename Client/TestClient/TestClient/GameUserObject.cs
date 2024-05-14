using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using GameBase.Template.GameBase;
using System.Net;
using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.Item.GameBaseItem;
using GameBase.Template.Shop.GameBaseShop;

namespace TestClient.TestClient
{
    public class GameUserObject : ImplObject
    {
        private float _heartBeat = 0.0f;
        public float HeartBeat { get => _heartBeat; }
        private Action<GameUserObject, ConnectType, ServerState> _fnServerState = null;
        public Action<GameUserObject, ConnectType, ServerState> FnServerState
        {
            get { return _fnServerState; }
            set { _fnServerState = value; }
        }

        private Dictionary<ConnectType, ServerData> _connectDatas = null;
        public ServerData _serverData = null;
        public ConnectType _connectType
        {
            get
            {
                if (_serverData == null)
                {
                    return ConnectType.None;
                }
                return _serverData._ConnectType;
            }
            set
            {
                if (_connectType == value)
                {
                    return;
                }
                if (_serverData != null)
                {
                    _serverData.ServerState = ServerState.Disconnect;
                }
                if(GetSession() != null)
                {
                    GetSession().Disconnect();
                }
                _serverData = null;
                if (_connectDatas.TryGetValue(value, out _serverData) == true)
                {
                    _serverData.ServerState = ServerState.Connecting;
                }
            }
        }

        public ServerState _ServerState
        {
            get 
            {
                if (_serverData == null)
                {
                    return ServerState.Disconnect;
                }
                return _serverData.ServerState;
            }
            set 
            { 
                if (_serverData != null)
                {
                    _serverData.ServerState = value;
                }
            }
        }

        public bool IsConnected
        {
            get
            {
                ServerState serverState = _ServerState;
                return (serverState == ServerState.Connecting || serverState == ServerState.Connection);
            }
        }

        public GameUserObject()
        {
            _objectID = (int)ObjectType.Client;
            _connectDatas = new Dictionary<ConnectType, ServerData>()
            {
                { ConnectType.Login, new ServerData(ConnectType.Login, this)},
                { ConnectType.Game, new ServerData(ConnectType.Game, this)}
            };
        }

        public void ServerStateCallback(ServerState serverState)
        {
            if (_fnServerState != null)
            {
                _fnServerState(this, _connectType, serverState);
            }
        }

        public sealed override void OnConnect(IPEndPoint ep)
        {
            _ServerState = ServerState.Connection;
            GameBaseTemplateContext.AddTemplate(this, ETemplateType.Account, new GameBaseAccountTemplate());
            GameBaseTemplateContext.AddTemplate(this, ETemplateType.Item, new GameBaseItemTemplate());
            GameBaseTemplateContext.AddTemplate(this, ETemplateType.Shop, new GameBaseShopTemplate());
            AccountController.AddAccountController(GetSession().GetUid());
            ItemController.AddItemController(GetSession().GetUid());
            ShopController.AddShopController(GetSession().GetUid());
            GameBaseTemplateContext.CreateClient(GetSession().GetUid());
            ServerStateCallback(_ServerState);
        }

        public sealed override void OnClose()
        {
            GameBaseTemplateContext.DeleteClient(GetSession().GetUid());
            GameBaseTemplateContext.RemoveTemplate(GetSession().GetUid(), ETemplateType.Account);
            GameBaseTemplateContext.RemoveTemplate(GetSession().GetUid(), ETemplateType.Item);
            GameBaseTemplateContext.RemoveTemplate(GetSession().GetUid(), ETemplateType.Shop);
            AccountController.RemoveAccountController(GetSession().GetUid());
            ItemController.RemoveItemController(GetSession().GetUid());
            ShopController.RemoveShopController(GetSession().GetUid());
            GetSession().SetUserObject(null);
            _ServerState = ServerState.Disconnect;
            ServerStateCallback(_ServerState);
        }

        public sealed override void OnConnectFailed()
        {
            _ServerState = ServerState.ConnectionError;
            ServerStateCallback(_ServerState);
        }

        public sealed override void OnDisconnected()
        {
            //_ServerState = ServerState.Disconnect;
            //ServerStateCallback(_ServerState);
        }
    }
}
