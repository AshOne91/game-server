using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Service.Core;
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
				for (int i = 0; i < 100; ++i)
				{
					serverMtx = new Mutex(false, string.Format("Global\\GameServer_{0}", i));
					if (serverMtx.WaitOne(1))
					{
						//FIXME
						//serverApp.GetConfig()._ClientPort += (ushort)i;
						//Console.WriteLine("GameServer_{0}, _ClientPort:{1}, _DediPort:{2}", i, serverApp.GetConfig()._ClientPort, serverApp.GetConfig()._DediPort);
						break;
					}
				}

				Console.Title = "GameServer : " + System.Diagnostics.Process.GetCurrentProcess().Id;

				ServerConfig config = new ServerConfig();
				config.PeerConfig.UseSessionEventQueue = true;

				Logger.Default = new Logger();
				Logger.Default.Create(true, "GameServer", "/log/GS" + System.Diagnostics.Process.GetCurrentProcess().Id);
				Logger.Default.Log(ELogLevel.Always, "Start GameServer...");

				bool result = serverApp.Create(config);
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
