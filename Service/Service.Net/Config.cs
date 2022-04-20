using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public class Config
    {
        public int Mode;
        public string ShardDBConn;
        public string GlobalDBConn;
        public string IP;
        public int Port;
        public string MasterIP;
        public int MasterPort;
        public string CacheServerIP;
        public int CacheServerPort;
        public ServerConfig ServerConfig = new ServerConfig();
    }
}
