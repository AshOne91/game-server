using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using GameBase.Template.GameBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using Service.DB;
using Service.Net;

namespace GameServer
{
	public sealed class GameServerEntry
	{
		public static GameServerApp serverApp = new GameServerApp();
		public static GameServerApp GetApp() { return serverApp; }

		private static Mutex serverMtx;
		private static Timer ticker;

		public static void TimerMethod(object state)
        {
			serverApp.PrintIO();
			GameServerApp.ClearIOInfo();
        }

		static void Main(string[] args)
		{
			try
			{
                Console.Title = "GameServer : " + System.Diagnostics.Process.GetCurrentProcess().Id;

				Logger.Default = new Logger();
				Logger.Default.Create(true, "GameServer", "/log/GS" + System.Diagnostics.Process.GetCurrentProcess().Id);
				Logger.Default.Log(ELogLevel.Always, "Start GameServer...");

				string solutionPath = "../../../Application/GameServer/";
                string projectDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
#if (!DEBUG)
				var configPath = Path.Combine(projectDirectory, "gameserver-config.json");
				var solutionConfigPath = Path.Combine(solutionPath, "gameserver-config.json");
#else
                var configPath = Path.Combine(projectDirectory, "gameserver-config_debug.json");
                var solutionConfigPath = Path.Combine(solutionPath, "gameserver-config_debug.json");
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

                for (int i = 0; i < 100; ++i)
				{
					serverMtx = new Mutex(false, string.Format("Global\\GameServer_{0}", i));
					if (serverMtx.WaitOne(1))
					{
                        serverApp.AppConfig.serverConfig.Port += (ushort)i;
						Console.WriteLine("GameServer_{0}, Port", i, serverApp.AppConfig.serverConfig.Port);
						break;
					}
				}

				bool result = serverApp.Create(serverApp.AppConfig);
				if (result == false)
				{
					Logger.Default.Log(ELogLevel.Fatal, "Failed Create GameServer.");
					return;
				}

				result = serverApp.ConnectToMaster();

				if (result == false)
				{
					Logger.Default.Log(ELogLevel.Fatal, "Failed connect to master server.");
					return;
				}

				ticker = new Timer(TimerMethod, null, 10000, 10000);

				Logger.Default.Log(ELogLevel.Always, "Start WaitForSessionEvent...");

				GameBaseTemplateContext.SetDBManager(new GameBaseDBManager(Logger.Default));



				//FIXME
				/*DBSimpleInfo test = new DBSimpleInfo();
				test._dbIndex = 0;
				test._dbIP = "127.0.0.1";
				test._dbPort = 3306;
				test._dbID = "ubf";
				test._dbName = "globaldb";
				test._threadCount = 2;
				test._slaveDBIP = "";
				test._serverID = 1;
				test._dbPW = "qjxjvmffkdl!@#";
				test._dbType = EDBType.Global;
				List<DBSimpleInfo> testSImpleInfo = new List<DBSimpleInfo>();
				testSImpleInfo.Add(test);*/

				GameBaseTemplateContext.GetDBManager().SetupDB(serverApp.AppConfig.dbInfo);

				GameBaseTemplateContext.GetDBManager().DBLoad_Request(1, () =>
				{
					Logger.Default.Log(ELogLevel.Always, "DBList Request Completed...");
				});

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
