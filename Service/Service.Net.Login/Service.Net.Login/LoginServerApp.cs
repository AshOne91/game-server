using GameBase.Base;
using GameBase.Common;
using GameBase.Template.GameBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Service.Net.Login
{
    public class LoginServerConfig
    {
        public string _Ver;
        public string _MasterIP;
        public ushort _MasterPort;

        public string _LoginIP;
        public ushort _LoginPort;
        public void LoadConfig()
        {
            //json에서 읽을 수 있게 수정하기
            _Ver = "1.0.0";//처음 구동할 때 s3에서 얻게 수정하기
            _MasterIP = "127.0.0.1";
            _MasterPort = 30000;

            _LoginIP = "127.0.0.1";
            _LoginPort = 10000;
        }
    }
    public class LoginServerApp : ServerApp
    {
        private LoginServerConfig _serverConfig;
        private Dictionary<ulong, LoginUserObject> _loginUserObjMap;
        private MasterClientObject _masterClientObject;

        private List<GameServerInfo> _gameServerInfoArray = new List<GameServerInfo>();
        private int _forceGuideIdx = -1;
        private int _minGameServerUserCount = 100;
        private int _maxGameServerUserCount = 5000;
        public LoginServerConfig GetConfig() { return _serverConfig; }

        public LoginServerApp()
        {

        }

        ~LoginServerApp()
        {
            Destroy();
        }


        public override bool StartUp(ELogLevel logLevel, EServerMode serverMode, string configPath)
        {
            if(!base.StartUp(logLevel, serverMode, configPath))
            {
                return true;
            }


            return true;
        }

        public override bool Create(ServerConfig config)
        {
            LoginServerConfig _SeverConfig = new LoginServerConfig();
            _loginUserObjMap = new Dictionary<ulong, LoginUserObject>();
            bool result = base.Create(config);

            PerformanceCounter._WarningEvent += OnPerfWarning;
            //ConnectToMateAgent();
            GameBaseTemplateContext.AddTemplate(ETemplateType.Internal, new GameBaseInternalTemplate());
            GameBaseTemplateContext.LoadDataTable(null);
            GameBaseTemplateContext.InitTemplate(null);
            return result;
        }

        

        public bool ConnectToMaster()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_config.MasterIP), _serverConfig._MasterPort);

            Logger.Default.Log(ELogLevel.Always, "Try Connect to MasterServer {0}:{1}", _serverConfig._MasterIP, _serverConfig._MasterPort);
            SocketSession ss = OpenConnection(ep);
            if (ss != null)
            {
                return true;
            }

            return false;
        }
        public void OnPerfWarning(int tick)
        {
            Logger.Default.Log(ELogLevel.Warn, "OnPerfWarning");
        }
        public void WaitForShutdown()
        {

        }
        public override void Destroy()
        {
            base.Destroy();

            PerformanceCounter.Print();

            Logger.Default.Destroy();
        }
        public override void OnAccept(SocketSession session, IPEndPoint localEP, IPEndPoint remoteEP)
        {
            if (localEP.Port == _serverConfig._LoginPort)
            {
                LoginUserObject obj = new LoginUserObject();
                session.SetUserObject(obj);
                obj.SetTcpSession(session);

                _AddLoginUserObject(session.GetUid(), obj);

                obj.OnAccept(localEP);
            }
        }

        public int cnt = 0;
        public Int64 wholeSendCount = 0;
        public Int64 wholeRecvCount = 0;
        public int repeatCount = 0;
        public void PrintIO()
        {
            Logger.Default.Log(ELogLevel.Always, "Connection Count:{0}", this._sessionManager.GetActiveSessionCount());
            Logger.Default.Log(ELogLevel.Always, "SendBytes:{0}", _totalSendBytes);
            Logger.Default.Log(ELogLevel.Always, "RecvBytes:{0}", _totalRecvBytes);
            Logger.Default.Log(ELogLevel.Always, "SendCount:{0}", _totalSendCount);
            Logger.Default.Log(ELogLevel.Always, "RecvCount:{0}", _totalRecvCount);

            if (_totalSendCount != 0 && _totalRecvCount != 0)
            {
                repeatCount++;
                wholeSendCount += _totalSendCount;
                wholeRecvCount += _totalRecvCount;

                Logger.Default.Log(ELogLevel.Always, "Avg SendCount:{0}", wholeSendCount / repeatCount);
                Logger.Default.Log(ELogLevel.Always, "Avg RecvCount:{0}", wholeRecvCount / repeatCount);
            }
            Logger.Default.Log(ELogLevel.Always, "Loop:{0}", cnt);
            cnt = 0;
        }

        private bool _bListenState = false;
        private void ListenUsers(bool bNewState)
        {
            if (_bListenState == bNewState) return;

            _bListenState = bNewState;
            if (_bListenState)
            {
                Logger.Default.Log(ELogLevel.Always, "Start Listen {0} ", GetConfig()._LoginPort);
                IPEndPoint epClient = new IPEndPoint(IPAddress.Any, GetConfig()._LoginPort);
                BeginAcceptor(epClient);
            }
            else
            {
                if (_listeners != null)
                {
                    foreach (TcpListener listener in _listeners)
                    {
                        listener.Stop();
                    }
                    _listeners.Clear();
                }
            }
        }
        public override void OnConnect(SocketSession session, IPEndPoint ep)
        {
            Logger.Default.Log(ELogLevel.Always, "OnConnect {0}", ep.ToString());

            if (ep.Port == GetConfig()._LoginPort)
            {
                MasterClientObject obj = new MasterClientObject();
                session.SetUserObject(obj);
                obj.SetTcpSession(session);

                Debug.Assert(_masterClientObject == null);
                _masterClientObject = obj;

                obj.OnConnect(ep);

                ListenUsers(true);
            }
        }
        public override void OnConnectFailed(SocketSession session, string e)
        {
            Logger.Default.Log(ELogLevel.Always, "OnConnectFailed {0}", e);
            if (_masterClientObject == null)
            {
                AddTimer((uint)ObjectType.Master, 1000, null);
            }
        }
        public override void OnClose(SocketSession session)
        {
            UserObject userObj = session.GetUserObject();
            if (userObj != null)
            {
                if (userObj.GetObjectID() == (int)ObjectType.User)
                {
                    _RemoveLoginUserObject(session.GetUid());
                }
                else if (userObj.GetObjectID() == (int)ObjectType.Master)
                {
                    _masterClientObject = null;
                    ConnectToMaster();
                }

                userObj.OnClose();
                userObj.Dispose();
                session.SetUserObject(null);
            }
        }
        public override void OnSocketError(SocketSession session, string e)
        {
            Logger.Default.Log(ELogLevel.Err, "OnSocketError = {0}", e);
            session.Disconnect();
        }
        public override void OnUserEvent(SocketSession session) { }
        public override void OnAsyncTask(AsyncTaskObject task)
        {


        }
        public override void OnPacket(SocketSession session, Packet packet)
        {
            UserObject userObj = session.GetUserObject();
            if (userObj != null)
            {
                userObj.OnPacket(packet);
            }
        }
        public override void OnSendComplete(SocketSession session, int transBytes)
        {
            UserObject userObj = session.GetUserObject();
            if (userObj != null)
            {
                userObj.OnSendComplete();
            }
        }
        public override void OnAddSendQueue(SocketSession session, ushort protocol, int transBytes) { }
        public override void OnPacketError(SocketSession session, Packet packet)
        {
            Logger.Default.Log(ELogLevel.Err, "OnPacketError = {0}", packet.GetId());
            session.Disconnect();
        }
        public override void OnTimer(TimerHandle timer)
        {
            if (timer._TimerType == (uint)ObjectType.Master)
            {
                ConnectToMaster();
            }
        }
        public override void OnError(string errorMsg)
        {
            Logger.Default.Log(ELogLevel.Err, "OnError In AuthServer => {0}", errorMsg);
        }
        public override void OnUpdate(float dt)
        {
            //OnUpdateAMate(dt);
        }
        public void SetGameServerInfo(List<GameServerInfo> gameServerInfoArray)
        {
            _gameServerInfoArray = gameServerInfoArray;
        }
        public GameServerInfo GetGameServerInfo(List<int> wantedServerIds)
        {
            int ForceServerId = _forceGuideIdx;

            if (_forceGuideIdx > -1)
            {
                // 0번 칸에 삽입해줌
                wantedServerIds.Insert(0, _forceGuideIdx);
            }

            foreach (int testId in wantedServerIds)
            {
                if (testId == -1) continue;

                foreach (GameServerInfo info in _gameServerInfoArray)
                {
                    if (info.Alive == true && info.ServerId == testId)
                    {
                        return info;
                    }
                }
            }

            // 최소인원이 안채워진 채널 부터 채운다
            foreach (GameServerInfo info in _gameServerInfoArray)
            {
                if (info.Alive == true && info.CurrentUserCount < _minGameServerUserCount)
                {
                    return info;
                }
            }
            // 최소인원이 전부 다 채워져있다면  최대인원이 안채워진 채널로 채움
            foreach (GameServerInfo info in _gameServerInfoArray)
            {
                if (info.Alive == true && info.CurrentUserCount < _maxGameServerUserCount)
                {
                    return info;
                }
            }

            return null;
        }

        public void SendPacketToMaster(Packet packet)
        {
            if (_masterClientObject != null)
            {
                _masterClientObject.SendPacket(packet);
            }
        }

        private void _AddLoginUserObject(ulong uId, LoginUserObject obj)
        {
            bool result = _loginUserObjMap.TryAdd(uId, obj);
            if (result == false)
            {
                Logger.Default.Log(ELogLevel.Fatal, "AddLoginUserObject is failed - Uid: {0}", uId);
            }
        }

        private void _RemoveLoginUserObject(ulong uId)
        {
            bool result = _loginUserObjMap.Remove(uId);
            if (result == false)
            {
                Logger.Default.Log(ELogLevel.Fatal, "RemoveAuthUserObject is failed - Uid:{0}", uId);
            }
        }
    }
}
