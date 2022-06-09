using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
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
}
