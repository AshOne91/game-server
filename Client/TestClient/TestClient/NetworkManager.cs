using System;
using System.Collections.Generic;
using System.Text;
using TestClient.FrameWork;
using System.Net;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using Service.Net;
using Service.Core;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;

namespace TestClient.TestClient
{
    public partial class NetworkManager : SceneSubSystem<NetworkManager>
    {
        private AppConfig _config = null;
        public AppConfig Config
        {
            get { return _config; }
        }
        private AgentApp _agentApp = null;
        private ulong _authUID = 0;
        public ulong AuthUID
        {
            get { return _authUID; }
            private set { _authUID = value; }
        }
        private Dictionary<ulong, GameUserObject> _userObjectList = new Dictionary<ulong, GameUserObject>();
        public GameUserObject GetUserObject(ulong uid)
        {
            _userObjectList.TryGetValue(uid, out GameUserObject userObject);
            return userObject;
        }


        public sealed override void DoUpdate()
        {
            _agentApp.ExternalUpdate();
        }
        public sealed override void OnEnable()
        {

        }
        public sealed override void OnDisable()
        {

        }
        public sealed override void OnInit()
        {
            Console.Title = "TestClient : " + System.Diagnostics.Process.GetCurrentProcess().Id;

            ServerConfig AppConfig = new ServerConfig();
            AppConfig.PeerConfig.UseSessionEventQueue = true;

            Logger.Default = new Logger();
            Logger.Default.Create(true, "TestClient");

            _agentApp = new AgentApp();
            _agentApp.Create(AppConfig);
            string defaultPath = "../../../";
            using (StreamReader reader = new StreamReader(defaultPath + "AppConfig.json"))
            {
                AppConfig config = JsonConvert.DeserializeObject<AppConfig>(reader.ReadToEnd());
            }
        }

        public sealed override void OnRelease()
        {
            _agentApp = null;
        }

        public bool LoginConnect(string ip, int port, ulong uid = 0)
        {
            IPEndPoint endPoinst = new IPEndPoint(IPAddress.Parse(ip), port);
            SocketSession session = _agentApp.OpenConnection(endPoinst);
            {
                GameUserObject gameUserObject = null;
                if (_userObjectList.TryGetValue(uid, out gameUserObject) == false)
                {
                    gameUserObject = new GameUserObject();
                    gameUserObject.FnServerState = this.OnServerState;
                    _userObjectList.Add(gameUserObject.UId, gameUserObject);
                }
                session.SetUserObject(gameUserObject);
                gameUserObject.SetSocketSession(session);
                gameUserObject._connectType = ConnectType.Login;
                gameUserObject._serverData.IP = ip;
                gameUserObject._serverData.PORT = port;
                gameUserObject.ServerStateCallback(ServerState.Connecting);
            }

            return true;
        }

        public bool GameConnect(string ip, int port, ulong uid = 0)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            SocketSession session = _agentApp.OpenConnection(endPoint);
            {
                GameUserObject gameUserObject = null;
                if (_userObjectList.TryGetValue(uid, out gameUserObject) == false)
                {
                    gameUserObject = new GameUserObject();
                    gameUserObject.FnServerState = this.OnServerState;
                    _userObjectList.Add(gameUserObject.UId, gameUserObject);
                }
                session.SetUserObject(gameUserObject);
                gameUserObject.SetSocketSession(session);
                gameUserObject._connectType = ConnectType.Game;
                gameUserObject._serverData.IP = ip;
                gameUserObject._serverData.PORT = port;
                gameUserObject.ServerStateCallback(ServerState.Connecting);
            }
            return true;
        }

        public void OnServerState(GameUserObject gameUserObject, ConnectType connectType, ServerState serverState)
        {
            switch (serverState)
            {

                case ServerState.ConnectionError:
                    EventManager.Instance.PostNotifycation("ConnectionError", NotifyType.BroadCast, 0, 0, 0, false, gameUserObject);
                    break;
                case ServerState.Disconnect:
                    EventManager.Instance.PostNotifycation("Disconnect", NotifyType.BroadCast, 0, 0, 0, false, gameUserObject);
                    if (gameUserObject.GetAccountImpl<GameBaseAccountClientImpl>()._LoginAuth == true)
                    {
                        AuthUID = gameUserObject.UId;
                    }
                    break;
                case ServerState.Connecting:
                    EventManager.Instance.PostNotifycation("Connecting", NotifyType.BroadCast, 0, 0, 0, false, gameUserObject);
                    break;
                case ServerState.Connection:
                    EventManager.Instance.PostNotifycation("Connection", NotifyType.BroadCast, 0, 0, 0, false, gameUserObject);
                    break;
            }
        }

    }
}
