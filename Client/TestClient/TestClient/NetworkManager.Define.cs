using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.TestClient
{
    public partial class NetworkManager
    {
        // 서버종류
        public enum ServerType
        {
            None,
            Login,
            GameServer
        }

        // 서버접속상태
        public enum ServerState
        {
            ConnectionError = -1,
            Disconnect,
            Connecting,
            Connection
        }
        public enum ObjectType
        {
            Auth = 100,
            Game = 200
        }

    }
}
