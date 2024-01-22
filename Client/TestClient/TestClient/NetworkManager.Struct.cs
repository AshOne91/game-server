using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using TestClient.FrameWork;

namespace TestClient.TestClient
{
    public partial class NetworkManager
    {
        public class ServerData
        {
            private float _time = 0.0f;
            private string _ip = string.Empty;
            private int _port = 0;
            public string IP { get { return _ip; } set { _ip = value; } }
            public int PORT { get { return _port; } set { _port = value; } }

            private ServerType _serverType = ServerType.None;
            public ServerType ServerType
            {
                get { return _serverType; }
                protected set { _serverType = value; }
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
                if (_serverType != ServerType.GameServer)
                {
                    return false;
                }
                if (_serverState != ServerState.Connection)
                {
                    return false;
                }
                _time += TimerManager.Instance.SecondPerFrame;
                if (_time <= NetworkManager.Instance.HeartBeat)
                {
                    return false;
                }
                _time = 0.0f;
                return true;
            }
        }

    }
}
