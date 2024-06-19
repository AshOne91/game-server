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
using System.Reflection;

namespace TestClient.TestClient
{
    public partial class NetworkManager : SceneSubSystem<NetworkManager>
    {
        private AppConfig _appConfig = null;
        public AppConfig AppConfig
        {
            get { return _appConfig; }
            set { _appConfig = value; }
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

            Logger.Default = new Logger();
            Logger.Default.Create(false, "TestClient");
            Logger.Default.Log(ELogLevel.Always, "Start TestClient...");

            string solutionPath = "../../../Client/TestClient/";
            string projectDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var configPath = Path.Combine(projectDirectory, "AppConfig.json");
            var solutionConfigPath = Path.Combine(solutionPath, "AppConfig.json");

            if (File.Exists(configPath) == false)
            {
                AppConfig appConfig = new AppConfig();
                string jsonConfig = JsonConvert.SerializeObject(appConfig);
                File.WriteAllText(configPath, jsonConfig);
            }

            if (File.Exists(solutionConfigPath) == true)
            {
                File.Copy(solutionConfigPath, configPath, true);
            }

            using (StreamReader reader = new StreamReader(configPath))
            {
                AppConfig = JsonConvert.DeserializeObject<AppConfig>(reader.ReadToEnd());
            }

            _agentApp = new AgentApp();
            _agentApp.Create(AppConfig);
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
                    gameUserObject.FnCall = this.ClientAction;
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
                    gameUserObject.FnCall = this.ClientAction;
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

        public void ClientAction(ImplObject obj, string action, object extraInfo)
        {
            switch (action)
            {
                case "AuthComplete":
                    {
                        EventManager.Instance.PostNotifycation("AuthComplete", NotifyType.BroadCast, 0, 0, 0, false, obj);
                    }
                    break;
                case "PacketError":
                    {
                        Logger.Default.Log(ELogLevel.Err, "packet error{0}", extraInfo.ToString());
                    }
                    break;
                case "PlayerList":
                    {
                        EventManager.Instance.PostNotifycation("PlayerList", NotifyType.BroadCast, 0, 0, 0, false, extraInfo);
                    }
                    break;
                case "CreatePlayer":
                    {
                        EventManager.Instance.PostNotifycation("CreatePlayer", NotifyType.BroadCast, 0, 0, 0, false, extraInfo);
                    }
                    break;
                case "SelectPlayer":
                    {
                        EventManager.Instance.PostNotifycation("SelectPlayer", NotifyType.BroadCast, 0, 0, 0, false, extraInfo);
                    }
                    break;
                case "ItemInfo":
                    {
                        EventManager.Instance.PostNotifycation("ItemInfo", NotifyType.BroadCast, 0, 0, 0, false, extraInfo);
                    }
                    break;
                case "ShopInfo":
                    {
                        EventManager.Instance.PostNotifycation("ShopInfo", NotifyType.BroadCast, 0, 0, 0, false, extraInfo);
                    }
                    break;
                case "ShopBuy":
                    {
                        EventManager.Instance.PostNotifycation("ShopBuy", NotifyType.BroadCast, 0, 0, 0, false, extraInfo);
                    }
                    break;
            } 
        }

    }
}
