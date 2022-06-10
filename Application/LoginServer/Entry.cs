using System;
using System.Collections.Generic;
using System.Text;
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

                ServerConfig config = new ServerConfig();
                config.PeerConfig.UseSessionEventQueue = true;

                Logger.Default = new Logger();
                Logger.Default.Create(true, "login");
                Logger.Default.Log(ELogLevel.Always, "Start LoginServer...");

                serverApp.Create(config, 1);
                bool result = serverApp.ConnectToMaster();

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
            }
            finally
            {
                serverApp.Destroy();
            }

            return;
        }
    }
}
