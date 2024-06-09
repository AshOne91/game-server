using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{

    public class ServerConfig
    {
        public string IP;
        public int Port;
        public string MasterIP;
        public int MasterPort;
        public PeerConfig PeerConfig = new PeerConfig();
    }
}
