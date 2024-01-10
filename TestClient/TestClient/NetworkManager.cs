using System;
using System.Collections.Generic;
using System.Text;
using TestClient.FrameWork;
using System.Net;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;

namespace TestClient.TestClient
{
    public partial class NetworkManager: SceneSubSystem<NetworkManager>
    {
        private float _heartBeat = 0.0f;
        private AgentApp _agentApp;
        private int _authObjectCount = 0;
        private int _gameObjectCount = 0;
        private ConcurrentDictionary<int, AgentAuthObject> _authObjectList;
        private ConcurrentDictionary<int, AgentGameObject> _gameObjectList;
        public float HeartBeat { get => _heartBeat; }
        public sealed override void DoUpdate()
        {

        }
        public sealed override void OnEnable()
        {

        }
        public sealed override void OnDisable()
        {

        }
        public sealed override void OnInit()
        {
            using (StreamReader reader = new StreamReader("AppConfig.json"))
            {
                //AppConfig config = JsonConvert.DeserializeObject<AppConfig>
            }
        }
        public sealed override void OnRelease()
        {

        }
    }
}
