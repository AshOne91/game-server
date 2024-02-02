using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public class ServerData
    {
        private GameUserObject _gameUserObject = null;
        private float _time = 0.0f;
        private string _ip = string.Empty;
        private int _port = 0;
        public string IP { get { return _ip; } set { _ip = value; } }
        public int PORT { get { return _port; } set { _port = value; } }

        public ServerData(ConnectType serverType, GameUserObject gameUserObject)
        {
            _connectType = serverType;
            _gameUserObject = gameUserObject;
        }

        private ConnectType _connectType = ConnectType.None;
        public ConnectType _ConnectType
        {
            get { return _connectType; }
            protected set { _connectType = value; }
        }
        private ServerState _serverState = ServerState.Disconnect;
        public ServerState ServerState
        {
            get { return _serverState; }
            set
            {
                _serverState = value;
            }
        }
        public bool IsHeartBeat()
        {
            if (_connectType != ConnectType.Game)
            {
                return false;
            }
            if (_serverState != ServerState.Connection)
            {
                return false;
            }
            _time += TimerManager.Instance.SecondPerFrame;
            if (_time <= _gameUserObject.HeartBeat)
            {
                return false;
            }
            _time = 0.0f;
            return true;
        }
    }
}
