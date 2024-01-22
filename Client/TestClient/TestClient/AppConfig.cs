using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.TestClient
{
    public class AppConfig
    {
        public ClientConfig clientConfig;
    }

    public class ClientConfig
    {
        public string loginServerIP;
        public int loginServerPort;
    }
}
