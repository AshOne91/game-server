using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Service.Net
{
    using TimerID = UInt64;
    using TimerType = UInt32;
    public enum EServerMode
    {
        Login = 1,
        Game,
        Master,
        Proxy
    }

    public enum ObjectType
    {
        Master = 100,
        User = 200,
        Login = 300,
        Game = 400
    }

    public abstract class ServerApp : TimeDispatcher
    {
        public static long _totalSendBytes = 0;
        public static long _totalRecvBytes = 0;
        public static long _totalSendCount = 0;
        public static long _totalRecvCount = 0;

        public ELogLevel _logLevel = ELogLevel.Always;
        public EServerMode _serverMode = EServerMode.Login;
        public ServerConfig _config;

        public ILogger _logger = null;
        protected SessionManager _sessionManager = null;
        protected FlipQueue<SessionEvent> _eventQueue = null;
        protected EventWaitHandle _eventWait = null;
        protected PacketPool _packetPool = null;
        protected SessionEventPool _evtPool = null;
        protected FrameSkip _frameSkip = null;
        protected TimeProcessor _timeProcessor = new TimeProcessor();
        bool _running = false;
        protected List<TcpListener> _listeners = new List<TcpListener>();

        int _lastTick = 0;
        int _frame = 0;
        int _frameDelay = 0;



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

        public virtual bool Create(ServerConfig config, int frame = 30)
        {
            _config = config;

            _sessionManager = new SessionManager(this, config);
            _eventQueue = new FlipQueue<SessionEvent>();

            _eventWait = new EventWaitHandle(false, EventResetMode.AutoReset);

            _packetPool = new PacketPool();
            _packetPool.Initialize(_config.PeerConfig.PacketPoolCount);

            _evtPool = new SessionEventPool();
            _evtPool.Initialize(_config.PeerConfig.PacketPoolCount);

            _frameSkip = new FrameSkip();
            _running = true;

            _lastTick = Environment.TickCount;
            _frame = frame;
            _frameDelay = 1000 / frame;
            _frameSkip.SetFramePerSec(_frame);

            if (_logger == null)
            {
                Logger logger = new Logger();
                logger.Create(true, "");
                _logger = logger;
            }
            return true;
        }

        public virtual void Destroy()
        {
            if (_listeners != null)
            {
                foreach (var listener in _listeners)
                {
                    listener.Stop();
                }
                _listeners.Clear();
            }

            _running = false;
            _eventWait.Set();
        }

        public virtual void OnUpdate(float dt)
        {

        }

        public bool BeginAcceptor(IPEndPoint ep)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, ep.Port);
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _config.PeerConfig.ReuseAddress);
            listener.Start(_config.PeerConfig.PendingConnectionBacklog);
            listener.BeginAcceptTcpClient(new AsyncCallback(_OnAcceptCallback), listener);
            _listeners.Add(listener);

            return true;
        }

        private void _OnAcceptCallback(IAsyncResult IAR)
        {
            TcpListener listener = (TcpListener)IAR.AsyncState;

            try
            {
                TcpClient tcpClient = listener.EndAcceptTcpClient(IAR);
                if (tcpClient.Connected)
                {
                    SocketSession session = null;

                    bool result = _sessionManager.ActiveSession(tcpClient.Client, out session);
                }
            }
            catch (Exception e)
            {
                OnError(e.Message);
            }
            finally
            {
                listener.BeginAcceptTcpClient(new AsyncCallback(_OnAcceptCallback), listener);
            }
        }

        public SocketSession OpenConnection(IPEndPoint ep)
        {
            if (_sessionManager != null)
            {
                SocketSession session = _sessionManager.ActiveSession(ep);
                return session;
            }

            return null;
        }

        public virtual void Join(int frame)
        {
            while (_running)
            {
                int curTick = Environment.TickCount;
                int deltaTick = curTick - _lastTick;

                if (deltaTick > 0)
                {
                    float dt = deltaTick * 0.001f;

                    if (_frameSkip.Update(dt))
                    {
                        OnUpdate(dt);
                    }

                    _timeProcessor.ProcessTimer();
                }

                if (deltaTick <= _frameDelay)
                {
                    int sleepTick = _frameDelay - deltaTick;
                    if (_eventQueue.Count() > 0)
                    {
                        ProcessEvent();
                    }
                    else
                    {
                        if (_eventWait.WaitOne(sleepTick, true))
                            ProcessEvent();
                    }
                }

                _lastTick = curTick;
            }
        }

        public virtual void ExternalUpdate()
        {
            int curTick = Environment.TickCount;
            int deltaTick = curTick - _lastTick;

            if (deltaTick > 0)
            {
                float dt = deltaTick * 0.001f;

                if (_frameSkip.Update(dt))
                {
                    OnUpdate(dt);
                }

                _timeProcessor.ProcessTimer();
            }

            if (deltaTick <= _frameDelay)
            {
                int sleepTick = _frameDelay - deltaTick;
                if (_eventQueue.Count() > 0)
                {
                    ProcessEvent();
                }
                else
                {
                    if (_eventWait.WaitOne(sleepTick, true))
                        ProcessEvent();
                }
            }

            _lastTick = curTick;
        }

        public void ProcessEvent()
        {
            Queue<SessionEvent> queue = _eventQueue.GetQueue();

            foreach (SessionEvent evt in queue)
            {
                try
                {
                    switch (evt.evtType)
                    {
                        case SessionEvent.EvtType.RecvPacket:
                            OnPacket(evt.session, evt.packet);
                            evt.packet.Dispose();
                            _packetPool.Return(evt.packet);
                            break;
                        case SessionEvent.EvtType.SendComplete:
                            OnSendComplete(evt.session, evt.transBytes);
                            break;
                        case SessionEvent.EvtType.Accept:
                            OnAccept(evt.session, evt.localEP, evt.remoteEP);
                            break;
                        case SessionEvent.EvtType.Connect:
                            OnConnect(evt.session, evt.remoteEP);
                            break;
                        case SessionEvent.EvtType.ConnectFailed:
                            OnConnectFailed(evt.session, evt.msg);
                            break;
                        case SessionEvent.EvtType.LocalDisconnected:
                            OnDisconnected(evt.session, false, evt.msg);
                            OnClose(evt.session);
                            break;
                        case SessionEvent.EvtType.RemoteDisconnected:
                            OnDisconnected(evt.session, true, evt.msg);
                            OnClose(evt.session);
                            break;
                        case SessionEvent.EvtType.SocketError:
                            OnSocketError(evt.session, evt.msg);
                            OnClose(evt.session);
                            break;
                        case SessionEvent.EvtType.Timer:
                            break;
                        case SessionEvent.EvtType.AsyncTask:
                            OnAsyncTask(evt.task);
                            break;
                    }
                }
                catch (Exception e)
                {
                    _logger?.Log(ELogLevel.Always, e.ToString());
                    throw e;
                }
                finally
                {
                    _evtPool.Return(evt);
                }
            }

            queue.Clear();
        }

        public void EnqueueSessionEvent(SessionEvent evt)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (_eventQueue.Count() < _config.PeerConfig.EvtQueueSize)
                {
                    _eventQueue.Enqueue(evt);
                    _eventWait.Set();
                    return;
                }
                Thread.Sleep(32);
            }
        }

        public TimerID AddTimer(TimerType timerType, UInt32 interval, object extraObject)
        {
            return _timeProcessor.AddTimer(timerType, this, interval, extraObject);
        }

        public PacketPool GetPacketPool() { return _packetPool; }
        public SessionEventPool GetEventPool() { return _evtPool; }
        public virtual void OnAccept(SocketSession session, IPEndPoint localEP, IPEndPoint remoteEP) {}
        public virtual void OnConnect(SocketSession session, IPEndPoint ep) { }
        public virtual void OnConnectFailed(SocketSession session, string e) { }
        public virtual void OnDisconnected(SocketSession session, bool bRemote, string e) { }
        public virtual void OnClose(SocketSession session) { }
        public virtual void OnSocketError(SocketSession session, string e) { }
        public virtual void OnUserEvent(SocketSession session) { }
        public virtual void OnAsyncTask(AsyncTaskObject task) { }
        public virtual void OnPacket(SocketSession session, Packet packet) { }
        public virtual void OnSendComplete(SocketSession session, int transBytes) { }
        public virtual void OnAddSendQueue(SocketSession session, ushort protocol, int transBytes) { }
        public virtual void OnPacketError(SocketSession session, Packet packet) { }
        public virtual void OnTimer(TimerHandle timer) { }
        public virtual void OnError(string errorMsg) { }
        public static void AddSendBytes(long bytes)
        {
            Interlocked.Add(ref _totalSendBytes, bytes);
        }
        public static void AddRecvBytes(long bytes)
        {
            Interlocked.Add(ref _totalRecvBytes, bytes);
        }
        public static void AddSendCount()
        {
            Interlocked.Increment(ref _totalSendCount);
        }
        public static void AddRecvCount()
        {
            Interlocked.Increment(ref _totalRecvCount);
        }
        public static void ClearIOInfo()
        {
            Interlocked.Exchange(ref _totalSendBytes, 0);
            Interlocked.Exchange(ref _totalRecvBytes, 0);
            Interlocked.Exchange(ref _totalSendCount, 0);
            Interlocked.Exchange(ref _totalRecvCount, 0);
        }
    }
}
