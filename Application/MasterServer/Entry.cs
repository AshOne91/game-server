using System;
using System.Collections.Generic;
using System.Net;
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

				ServerConfig config = new ServerConfig();
				config.PeerConfig.UseSessionEventQueue = true;

				Logger.Default = new Logger();
				Logger.Default.Create(true, "MasterServer");
				Logger.Default.Log(ELogLevel.Always, "Start MasterServer...");

				bool result = serverApp.Create(config, 1);
				if (result == false)
				{
					Logger.Default.Log(ELogLevel.Fatal, "Failed Create MasterServer.");
					return;
				}

				//FIXME
				IPEndPoint epGame = new IPEndPoint(IPAddress.Any, 3000);
				serverApp.BeginAcceptor(epGame);

				//FIXME
				IPEndPoint epLogin = new IPEndPoint(IPAddress.Any, 4000);
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
