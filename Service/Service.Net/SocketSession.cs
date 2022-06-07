using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Service.Net
{
    public sealed class SocketSession : IDisposable
    {
        private readonly ServerApp _serverApp;
        private readonly PeerConfig _config;
        private readonly SessionManager _sessionManager;

        private ulong _uid = 0;
        private Socket _socket = null;

        private byte[] _recvBuffer = new byte[Packet.PACKETBUFFERSIZE];
        private byte[] _packetBuffer = new byte[Packet.PACKETBUFFERSIZE];
        private byte[] _tempBuffer = new byte[Packet.PACKETBUFFERSIZE];
        private int _maxSendBufferSize = Packet.PACKETBUFFERSIZE * 10;

        private List<byte> _sendBuffer = new List<byte>();

        private int _receivedSize = 0;
        private byte _cSeq = 0;
        private bool _firedOnClose = false;
        private string _remoteIP = "";

        private const int _none = 0;
        private const int _connecting = 1;
        private const int _connected = 2;
        private const int _disconnected = 4;
        private int _state = _none;

        private UserObject _userObject;

        public SocketSession(ulong uid, ServerApp serverApp, PeerConfig config, SessionManager manager)
        {
            _uid = uid;
            _serverApp = serverApp;
            _config = config;
            _sessionManager = manager;
        }

        SocketSession() { }
        ~SocketSession()
        {
            this.Dispose(false);
        }
        public ulong GetUid() { return _uid; }
        public void SetUserObject(UserObject userObj) { _userObject = userObj; }
        public UserObject GetUserObject() { return _userObject; }
        public ServerApp GetServerApp() { return _serverApp; }
        public (string ip, int port) GetRemoteAddr()
        {
            IPEndPoint remote = _socket.RemoteEndPoint as IPEndPoint;
            return (remote.Address.ToString(), remote.Port);
        }
        public bool IsConnected()
        {
            if (_socket != null)
            {
                if (_socket.Connected)
                {
                    return true;
                }
            }
            return false;
        }
        public void AttachSocket(Socket sock)
        {
            int origin = Interlocked.CompareExchange(ref _state, _connecting, _none);
            if (origin == _disconnected)
            {
                throw new ObjectDisposedException("This tcp socket session has been disposed when connecting.");
            }
            else if (origin != _none)
            {
                throw new InvalidOperationException("This tcp socket session is in invalid state when connecting.");
            }

            _socket = sock;
            SetSocketOptions();

            if (Interlocked.CompareExchange(ref _state, _connected, _connecting) != _connecting)
            {
                throw new ObjectDisposedException("This tcp socket session has been disposed after connected.");
            }
        }
        public void OnAccept(IPEndPoint localEp, IPEndPoint remoteEp)
        {
            FireAcceptEvent(localEp, remoteEp);
            _WaitForRecv();
            _remoteIP = remoteEp.Address.ToString();
        }

        public bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            try
            {
                if (disposing)
                {
                    this._socket.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                this.disposed = true;
            }

            this.disposed = true;
        }

        public void ConnectTo(string ip, int port)
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);
                _socket.BeginConnect(ipep, new AsyncCallback(_OnConnectCallBack), _socket);
            }
            catch (SocketException se)
            {
                FireConnectFailedEvent(se);
            }
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _state, _disconnected) == _disconnected)
            {
                return;
            }

            if (_socket != null && _socket.Connected)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();

                    if (_socket.Connected == false)
                    {
                        FireLocalDisconnectedEvent();
                    }
                    _socket = null;
                }
                catch (SocketException ex)
                {
                    FireSocketErrorEvent(ex);
                }
            }
        }

        public string GetIP()
        {
            if (_socket != null)
            {
                return _socket.RemoteEndPoint.ToString();
            }
            return _remoteIP;
        }

        private void OnClose()
        {
            if (!_firedOnClose)
            {
                if (_config.UseSessionEventQueue == false)
                {
                    _serverApp.OnClose(this);
                }

                _sessionManager.InactiveSession(this);
                _firedOnClose = true;
            }
        }

        public void SendPacket(Packet packet, bool bCanDrop = false)
        {
            _SendPacket(packet, bCanDrop);
        }

        private void _SendPacket(Packet packet, bool bCanDrop)
        {
            try
            {
                if (_state != _connected || _firedOnClose || _socket == null || !_socket.Connected)
                {
                    return;
                }

                packet.Encrypt(_cSeq);

                lock (_sendBuffer)
                {
                    if (bCanDrop && _sendBuffer.Count > _maxSendBufferSize)
                    {
                        return;
                    }

                    ArraySegment<byte> segment = new ArraySegment<byte>(packet.GetPacketBuffer(), 0, packet.GetPacketSize());
                    _sendBuffer.AddRange(segment);

                    _cSeq++;

                    if (_sendBuffer.Count == packet.GetPacketSize())
                    {
                        _Send();
                    }
                }
            }
            catch (SocketException e)
            {
                FireSocketErrorEvent(e);
            }
        }

        private void SetSocketOptions()
        {
            _socket.ReceiveBufferSize = _config.ReceiveBufferSize;
            _socket.SendBufferSize = _config.SendBufferSize;
            _socket.ReceiveTimeout = _config.ReceiveTimeout;
            _socket.SendTimeout = (int)_config.SendTimeout;

            _socket.ReceiveTimeout = 0;
            _socket.SendTimeout = 0;

            _socket.NoDelay = _config.NoDelay;
            _socket.LingerState = new LingerOption(_config.LingerState, 0);
            
            if (_config.KeepAlive)
            {
                _socket.SetSocketOption(
                    SocketOptionLevel.Socket,
                    SocketOptionName.KeepAlive,
                    _config.KeepAliveInterval
                    );
            }

            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, _config.ReceiveBufferSize);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, _config.SendBufferSize);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, _config.NoDelay);

            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _config.ReuseAddress);
        }

        private void _OnConnectCallBack(IAsyncResult IAR)
        {
            try
            {
                Socket tempSock = (Socket)IAR.AsyncState;
                IPEndPoint svrEP = (IPEndPoint)tempSock.RemoteEndPoint;
                tempSock.EndConnect(IAR);
                AttachSocket(tempSock);

                _remoteIP = svrEP.Address.ToString();
                this.FireConnectedEvent(svrEP);
                _WaitForRecv();


            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.NotConnected)
                {
                    FireConnectFailedEvent(se);
                }
            }
        }

        private void _OnReceiveCallBack(IAsyncResult IAR)
        {
            try
            {
                Socket tempSocket = (Socket)IAR.AsyncState;
                if (tempSocket.Connected == false)
                {
                    return;
                }
                int transBytes = tempSocket.EndReceive(IAR);

                if (transBytes > 0)
                {
                    Array.Copy(_recvBuffer, 0, _packetBuffer, _receivedSize, transBytes);
                    _receivedSize += transBytes;

                    while (_receivedSize > 0)
                    {
                        Packet receivedPacket = _serverApp.GetPacketPool().Take();
                        receivedPacket.CopyToBuffer(_packetBuffer, _receivedSize);

                        if (receivedPacket.IsValidPacket() == true &&
                            _receivedSize >= receivedPacket.GetPacketSize())
                        {
                            int dispatchSize = receivedPacket.GetPacketSize();
                            _receivedSize -= dispatchSize;

                            if (_receivedSize > 0)
                            {
                                Array.Clear(_tempBuffer, 0, _tempBuffer.Length);
                                Array.Copy(_packetBuffer, dispatchSize, _tempBuffer, 0, _receivedSize);
                                Array.Clear(_packetBuffer, 0, _packetBuffer.Length);
                                Array.Copy(_tempBuffer, 0, _packetBuffer, 0, _receivedSize);
                            }

                            receivedPacket.Decrypt();
                            DispatchPacket(receivedPacket);
                        }
                        else
                        {
                            break;
                        }
                        _WaitForRecv();
                    }

                }
                else
                {
                    FireRemoteDisconnectedEvent(null);
                }

            }
            catch(SocketException se)
            {
                if (se.SocketErrorCode == SocketError.ConnectionReset)
                {
                    FireRemoteDisconnectedEvent(se);
                }
                else
                {
                    FireSocketErrorEvent(se);
                }
            }
        }
        private void _OnSendCallBack(IAsyncResult IAR)
        {
            try
            {
                int sentBytes = _socket.EndSend(IAR);

                if (sentBytes > 0)
                {
                    ServerApp.AddSendBytes(sentBytes);
                    ServerApp.AddSendCount();

                    FireSentEvent(sentBytes);
                    lock (_sendBuffer)
                    {
                        _sendBuffer.RemoveRange(0, sentBytes);

                        if (_sendBuffer.Count > 0)
                        {
                            _Send();
                        }
                    }
                }
            }
            catch (SocketException se)
            {
                FireSocketErrorEvent(se);
            }
        }

        private void _Send()
        {
            byte[] sendArray = _sendBuffer.ToArray();
            _socket.BeginSend(sendArray, 0, sendArray.Length, SocketFlags.None, new AsyncCallback(_OnSendCallBack), null);
        }

        public void _WaitForRecv()
        {
            if (_state == _connected && _socket != null && _socket.Connected)
            {
                _socket.BeginReceive(_recvBuffer, 0, Packet.PACKETBUFFERSIZE - _receivedSize, SocketFlags.None, new AsyncCallback(_OnReceiveCallBack), _socket);
            }
        }
        public void DispatchPacket(Packet packet)
        {
            FirePacketEvent(packet);
        }

        private void FireAcceptEvent(IPEndPoint localEp, IPEndPoint remoteEp)
        {
            if (_config.UseSessionEventQueue)
            {
                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.evtType = SessionEvent.EvtType.Accept;
                evt.session = this;
                evt.localEP = localEp;
                evt.remoteEP = remoteEp;
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnAccept(this, localEp, remoteEp);
            }
        }
        private void FireConnectedEvent(IPEndPoint ep)
        {
            if (_config.UseSessionEventQueue)
            {
                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.evtType = SessionEvent.EvtType.Connect;
                evt.session = this;
                evt.remoteEP = ep;
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnConnect(this, ep);
            }
        }
        private void FireConnectFailedEvent(SocketException e)
        {
            if (_config.UseSessionEventQueue)
            {
                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.session = this;
                evt.evtType = SessionEvent.EvtType.ConnectFailed;
                evt.msg = e.ToString();
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnConnectFailed(this, e.ToString());
            }
            OnClose();
        }
        private void FireLocalDisconnectedEvent()
        {
            if (_config.UseSessionEventQueue)
            {

                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.session = this;
                evt.evtType = SessionEvent.EvtType.LocalDisconnected;
                evt.msg = "LocalDisconnected";
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnDisconnected(this, false, "LocalDisconnected");
            }
            OnClose();
        }
        private void FireRemoteDisconnectedEvent(SocketException e)
        {
            if (_config.UseSessionEventQueue)
            {
                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.evtType = SessionEvent.EvtType.RemoteDisconnected;
                evt.session = this;
                evt.msg = "RemoteDisconnectedEvent";
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnDisconnected(this, true, "RemoteDisconnectedEvent");
            }


            OnClose();
        }
        private void FirePacketEvent(Packet packet)
        {
            ServerApp.AddRecvBytes(packet.GetPacketSize());
            ServerApp.AddRecvCount();

            if (_config.UseSessionEventQueue)
            {
                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.evtType = SessionEvent.EvtType.RecvPacket;
                evt.session = this;
                evt.packet = packet;
                evt.transBytes = packet.GetPacketSize();
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnPacket(this, packet);
                packet.Dispose();
                _serverApp.GetPacketPool().Return(packet);
            }
        }
        private void FireSentEvent(int transBytes)
        {
            if (_config.UseSessionEventQueue)
            {
                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.evtType = SessionEvent.EvtType.SendComplete;
                evt.session = this;
                evt.transBytes = transBytes;
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnSendComplete(this, transBytes);
            }
        }
        private void FireSocketErrorEvent(SocketException e)
        {
            if (_config.UseSessionEventQueue)
            {
                SessionEvent evt = _serverApp.GetEventPool().Take();
                evt.evtType = SessionEvent.EvtType.SocketError;
                evt.session = this;
                evt.msg = e.ToString();
                _serverApp.EnqueueSessionEvent(evt);
            }
            else
            {
                _serverApp.OnSocketError(this, e.ToString());
            }
        } 
    }
}
