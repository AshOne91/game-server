using Service.DB;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public class AppConfig
    {
        public int serverId = 0;
        public ServerConfig serverConfig = new ServerConfig();
        public TemplateConfig templateConfig = new TemplateConfig();
        public DBInfo dbInfo = new DBInfo();
        public MasterServerConfig masterServerConfig = new MasterServerConfig();
    }

    public class TemplateConfig
    {
        public string gameId;
        public string env;
        public string localPath = string.Empty;
        public string bucketEnv = string.Empty;
        public string bucketUrl = string.Empty;
        public string bucketName = string.Empty;
        public string awsAccessKeyId = string.Empty;
        public string awsSecretAccessKey = string.Empty;
        public Dictionary<int, List<string>> allowApiForPlayState = new Dictionary<int, List<string>>();
        public Dictionary<string, int> updatePlayStateToApi = new Dictionary<string, int>();
    }

    public class MasterServerConfig
    {
        public string GameIP = string.Empty;
        public int GamePort = 0;
        public string LoginIP = string.Empty;
        public int LoginPort = 0;
    }
}
