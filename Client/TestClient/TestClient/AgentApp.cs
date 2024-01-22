using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.Item.GameBaseItem;

namespace TestClient.TestClient
{
    public class AgentApp : ServerApp
    {

        public sealed override bool Create(ServerConfig config, int frame = 30)
        {
            bool result = base.Create(config, frame);

            GameBaseTemplateContext.AddTemplate(ETemplateType.Account, new GameBaseAccountTemplate());
            GameBaseTemplateContext.AddTemplate(ETemplateType.Item, new GameBaseItemTemplate());


            TemplateConfig templateConfig = new TemplateConfig();
            GameBaseTemplateContext.InitTemplate(templateConfig, ServerType.Client);
            GameBaseTemplateContext.LoadDataTable(templateConfig);
            return result;
        }
        public sealed override void OnConnect(SocketSession session, IPEndPoint ep)
        {
            GameUserObject userObject = new GameUserObject();
            session.SetUserObject(userObject);
            userObject.SetSocketSession(session);
            userObject.OnConnect(ep);

            GameBaseTemplateContext.AddTemplate<GameUserObject>(userObject, ETemplateType.Account, new GameBaseAccountTemplate());
            GameBaseTemplateContext.AddTemplate<GameUserObject>(userObject, ETemplateType.Item, new GameBaseItemTemplate());
            AccountCon

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
