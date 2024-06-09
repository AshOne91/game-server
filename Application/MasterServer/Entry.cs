using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using GameBase.Template.GameBase;
using Newtonsoft.Json;
using Service.Core;
using Service.Net;

namespace MasterServer
{
	public sealed class MasterServerEntry
	{
		public static MasterServerApp serverApp = new MasterServerApp();
		public static MasterServerApp GetApp() { return serverApp; }

		static void Main(string[] args)
		{
			try
			{
				Console.Title = "MasterServer : " + System.Diagnostics.Process.GetCurrentProcess().Id;

				//ServerConfig config = new ServerConfig();
				//config.PeerConfig.UseSessionEventQueue = true;

				Logger.Default = new Logger();
				Logger.Default.Create(true, "MasterServer", "/log/MS" + System.Diagnostics.Process.GetCurrentProcess().Id);
				Logger.Default.Log(ELogLevel.Always, "Start MasterServer...");

				string solutionPath = "../../../Application/MasterServer/";
				string projectDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

#if (!DEBUG)
				var configPath = Path.Combine(projectDirectory, "masterserver-config.json");
				var solutionConfigPath = Path.Combine(solutionPath, "masterserver-config.json");
#else
                var configPath = Path.Combine(projectDirectory, "masterserver-config_debug.json");
				var solutionConfigPath = Path.Combine(solutionPath, "masterserver-config_debug.json");
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
					Logger.Default.Log(ELogLevel.Fatal, "Failed Create MasterServer.");
					return;
				}

				IPEndPoint epGame = new IPEndPoint(IPAddress.Parse(serverApp.AppConfig.masterServerConfig.GameIP), serverApp.AppConfig.masterServerConfig.GamePort);
				serverApp.BeginAcceptor(epGame);

				IPEndPoint epLogin = new IPEndPoint(IPAddress.Parse(serverApp.AppConfig.masterServerConfig.LoginIP), serverApp.AppConfig.masterServerConfig.LoginPort);
				serverApp.BeginAcceptor(epLogin);

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
