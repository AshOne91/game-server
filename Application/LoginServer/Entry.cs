using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using GameBase.Template.GameBase;
using Newtonsoft.Json;
using Service.Core;
using Service.Net;

namespace LoginServer
{
    public sealed class LoginServerEntry
    {
        public static LoginServerApp serverApp = new LoginServerApp();
        public static LoginServerApp GetApp() { return serverApp; }

        static void Main(string[] args)
        {
            try
            {
                Console.Title = "LoginServer : " + System.Diagnostics.Process.GetCurrentProcess().Id;

                //ServerConfig config = new ServerConfig();
                //config.PeerConfig.UseSessionEventQueue = true;

                Logger.Default = new Logger();
                Logger.Default.Create(true, "LoginServer", "/log/LS" + System.Diagnostics.Process.GetCurrentProcess().Id);
                Logger.Default.Log(ELogLevel.Always, "Start LoginServer...");

                string solutionPath = "../../../Application/LoginServer/";
                string projectDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

#if (!DEBUG)
                var configPath = Path.Combine(projectDirectory, "loginserver-config.json");
                var solutionConfigPath = Path.Combine(solutionPath, "loginserver-config.json")
#else
                var configPath = Path.Combine(projectDirectory, "loginserver-config_debug.json");
                var solutionConfigPath = Path.Combine(solutionPath, "loginserver-config_debug.json");
#endif
                if (File.Exists(configPath) == false)
                {
                    AppConfig appConfig = new AppConfig();
                    string jsonConfig = JsonConvert.SerializeObject(appConfig);
                    File.WriteAllText(configPath, jsonConfig);
                }

                if (File.Exists(solutionConfigPath) == true)
                {
                    File.Copy(solutionConfigPath, configPath, true);
                }

                using (StreamReader reader = new StreamReader(configPath)) 
                {
                    serverApp.AppConfig = JsonConvert.DeserializeObject<AppConfig>(reader.ReadToEnd());
                }

                serverApp.AppConfig.serverConfig.PeerConfig.UseSessionEventQueue = true;
                bool result = serverApp.Create(serverApp.AppConfig);
                if (result == false)
                {
                    Logger.Default.Log(ELogLevel.Fatal, "Failed Create LoginServer.");
                    return;
                }

                result = serverApp.ConnectToMaster();

                if (result == false)
                {
                    Logger.Default.Log(ELogLevel.Fatal, "Failed connect to master server.");
                    return;
                }

                Logger.Default.Log(ELogLevel.Always, "Start WaitForSessionEvent...");
                serverApp.Join();
            }
            catch (Exception e)
            {
                Logger.WriteExceptionLog(e);
                ObjectDumper.Write(GetApp());
                Debugger.Break();
            }
            finally
            {
                serverApp.Destroy();
            }

            return;
        }
    }
}
