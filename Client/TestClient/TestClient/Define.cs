using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.TestClient
{
    // 서버종류
    public enum ConnectType
    {
        None,
        Login,
        Game
    }

    // 서버접속상태
    public enum ServerState
    {
        ConnectionError = -1,
        Disconnect,
        Connecting,
        Connection,
    }
}
