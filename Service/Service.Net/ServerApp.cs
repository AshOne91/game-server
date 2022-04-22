﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Service.Net
{
    public enum EServerMode
    {
        Login = 1,
        Game,
        Master,
        Proxy
    }

    public abstract class ServerApp
    {
        public ELogLevel _logLevel = ELogLevel.Always;
        public EServerMode _serverMode = EServerMode.Login;
        public ServerConfig _config;

        public ILogger _logger = null;
        SessionManager _sessionManager = null;
        FlipQueue<SessionEvent> _eventQueue = null;
        EventWaitHandle _eventWait = null;
        PacketPool _packetPool = null;
        SessionEventPool _evtPool = null;
        FrameSkip _frameSkip = null;
        TimeProcessor _timeProcessor = new TimeProcessor();
        bool _running = false;
        protected List<TcpListener> _listeners = new List<TcpListener>();





        public virtual bool StartUp(ELogLevel logLevel, EServerMode serverMode, string configPath)
        {
            /*#if (!DEBUG)
            using (StreamReader reader = new StreamReader("gamebaseserver-config.json"))
#else
            using (StreamReader reader = new StreamReader("gamebaseserver-config_debug.json"))
#endif*/
            using (StreamReader reader = new StreamReader(configPath))
            {
                _logLevel = logLevel;
                _serverMode = serverMode;

                string jsonStr = reader.ReadToEnd();
                _config = JsonConvert.DeserializeObject<ServerConfig>(jsonStr);
            }
            //공용 설정파일 추가 예정
            Console.Title = "server:" + _serverMode.ToString() + System.Diagnostics.Process.GetCurrentProcess().Id;
            Logger.Default = new Logger();
            Logger.Default.Create(true, _serverMode.ToString());
            Logger.Default.Log(_logLevel, "Start " + _serverMode.ToString() + " Server");


            return true;
        }

        public virtual bool Create(ServerConfig config)
        {
            _config = config;

            _sessionManager = new SessionManager(this, config);
            _eventQueue = new FlipQueue<SessionEvent>();

            //_eventWait = 
            return true;

        }
    }
}
