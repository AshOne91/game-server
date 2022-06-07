using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Service.Net
{
    public sealed class SessionManager
    {
        private ServerApp _serverApp;
        private ServerConfig _serverConfig;

        private Dictionary<ulong, SocketSession> _activeSessionMap = new Dictionary<ulong, SocketSession>();
        private ulong _uidCnt = 0;
        public SessionManager(ServerApp serverApp, ServerConfig config)
        {
            _serverApp = serverApp;
            _serverConfig = config;
        }

        public SocketSession FindActiveSession(ulong uid)
        {
            lock (_activeSessionMap)
            {
                SocketSession retSession = null;
                try
                {
                    retSession = _activeSessionMap[uid];
                }
                catch
                {

                }
                return retSession;
            }

        }

        // OnAccept
        public bool ActiveSession(Socket sock, out SocketSession session)
        {
            lock (_activeSessionMap)
            {
                ++_uidCnt;
                session = new SocketSession(_uidCnt, _serverApp, _serverConfig.PeerConfig, this);

                bool result = _activeSessionMap.TryAdd(_uidCnt, session);
                if (result)
                {

                    IPEndPoint localIpEndPoint = sock.LocalEndPoint as IPEndPoint;
                    IPEndPoint remoteIpEndPoint = sock.RemoteEndPoint as IPEndPoint;

                    session.AttachSocket(sock);
                    session.OnAccept(localIpEndPoint, remoteIpEndPoint);
                }
                else
                {
                    _serverApp.OnError("bool ActiveSession Failed. :" + _uidCnt.ToString());
                    return false;
                }

                return true;
            }


        }
        // ConnectTo
        public SocketSession ActiveSession(IPEndPoint ep)
        {
            lock (_activeSessionMap)
            {
                ++_uidCnt;
                SocketSession session = new SocketSession(_uidCnt, _serverApp, _serverConfig.PeerConfig, this);

                bool result = _activeSessionMap.TryAdd(_uidCnt, session);
                if (result)
                {
                    session.ConnectTo(ep.Address.ToString(), ep.Port);
                }
                else
                {
                    _serverApp.OnError("NwTcpSocketSession ActiveSession Failed. :" + _uidCnt.ToString());
                    return session;
                }

                return session;
            }
        }

        public bool InactiveSession(SocketSession session)
        {
            lock (_activeSessionMap)
            {
                bool result = _activeSessionMap.Remove(session.GetUid());
                if (result == false)
                {
                    _serverApp.OnError("NwTcpSocketSession InActiveSession Failed. :" + _uidCnt.ToString());
                }
                session.Dispose();
                return result;
            }
        }

        public int GetActiveSessionCount()
        {
            lock (_activeSessionMap)
            {
                return _activeSessionMap.Count;
            }
        }
    }
}
