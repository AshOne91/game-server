using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;

namespace GameServer
{
	public sealed class GameServerEntry
	{
		public static GameServerApp serverApp = new GameServerApp();
		public static GameServerApp GetApp() { return serverApp; }

		static void Main(string[] args)
		{
			try
			{
				Console.Title = "GameServer : " + System.Diagnostics.Process.GetCurrentProcess().Id;

				ServerConfig config = new ServerConfig();
				config.PeerConfig.UseSessionEventQueue = true;

				Logger.Default = new Logger();
				Logger.Default.Create(true, "GameServer");
				Logger.Default.Log(ELogLevel.Always, "Start GameServer...");

				bool result = serverApp.Create(config);
				if (result == false)
				{
					Logger.Default.Log(ELogLevel.Fatal, "Failed Create GameServer.");
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
