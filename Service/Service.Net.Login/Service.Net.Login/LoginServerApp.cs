using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Service.Net.Login
{
    public class LoginServerConfig
    {
        public string _Ver;
        public string _MasterIP;
        public ushort _MasterPort;

        public string _LoginIP;
        public ushort _LoginPort;
        public void LoadConfig()
        {
            //json에서 읽을 수 있게 수정하기
            _Ver = "1.0.0";//처음 구동할 때 s3에서 얻게 수정하기
            _MasterIP = "127.0.0.1";
            _MasterPort = 30000;

            _LoginIP = "127.0.0.1";
            _LoginPort = 10000;
        }
    }
    public class LoginServerApp : ServerApp
    {
        public LoginServerApp()
        {

        }

        ~LoginServerApp()
        {
            Destroy();
        }


        public override bool StartUp(ELogLevel logLevel, EServerMode serverMode, string configPath)
        {
            if(!base.StartUp(logLevel, serverMode, configPath))
            {
                return true;
            }


            return true;
        }

        public override bool Create(ServerConfig config)
        {
            LoginServerConfig _SeverConfig = new LoginServerConfig();
            new Dictionary<long, LoginUserObject> _LoginUserObjMap = new Dictionary<ulong, LoginUserObject>();
            bool result = base.Create(config);

            PerformanceCounter._WarningEvent += OnPerfWarning;
            //ConnectTo
            return result;
        }

        public bool ConnectToMaster()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_config.MasterIP));

            Logger.Default.Log(ELogLevel.Always, "Try Connect to MasterServer {0}:{1}")
        }
    }
}
