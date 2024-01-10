using Service.Net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace TestClient.TestClient
{
    public class AgentApp : ServerApp
    {
        public sealed override bool Create(ServerConfig config, int frame = 30)
        {
            return base.Create(config, frame);
        }
        public sealed override void OnAccept(SocketSession session, IPEndPoint localEP, IPEndPoint remoteEP)
        {

        }
        public sealed override void OnConnect(SocketSession session, IPEndPoint ep)
        {

        }
        public sealed override void OnConnectFailed(SocketSession session, string e)
        {
            
        }
        public sealed override void OnDisconnected(SocketSession session, bool bRemote, string e)
        {
            
        }
        public sealed override void OnClose(SocketSession session)
        {
            
        }
        public override void OnSocketError(SocketSession session, string e) { }
        public override void OnUserEvent(SocketSession session) { }
        public override void OnPacket(SocketSession session, Packet packet)
        {

        }
        public override void OnSendComplete(SocketSession session, int transBytes) { }
        public override void OnAddSendQueue(SocketSession session, ushort protocol, int transBytes) { }
        public override void OnPacketError(SocketSession session, Packet packet) { }
        public override void OnUpdate(float dt)
        {

        }
    }
}
