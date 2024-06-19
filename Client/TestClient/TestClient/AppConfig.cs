using GameBase.Template.GameBase;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.TestClient
{
    public class AppConfig
    {
        public ClientConfig clientConfig = new ClientConfig();
        public ServerConfig serverConfig = new ServerConfig();
        public TemplateConfig templateConfig = new TemplateConfig();
    }

    public class ClientConfig
    {
        public string loginServerIP;
        public int loginServerPort;
        public int heartBeat;
    }
}
