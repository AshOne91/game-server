using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Service.Net.Login
{
    
    public class LoginServerApp : ServerApp
    {
        public override bool StartUp(ELogLevel logLevel, EServerMode serverMode, string configPath)
        {
            if(!base.StartUp(logLevel, serverMode, configPath))
            {
                return true;
            }


            return true;
        }
    }
}
