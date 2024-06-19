using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Account.GameBaseAccount;
using GameBase.Template.Item.GameBaseItem;
using Service.Core;
using GameBase.Template.Shop.GameBaseShop;

namespace TestClient.TestClient
{
    public class AgentApp : ServerApp
    {
        /*public sealed override bool Create(ServerConfig config, int frame = 30)
        {
            bool result = base.Create(config, frame);

            GameBaseTemplateContext.AddTemplate(ETemplateType.Account, new GameBaseAccountTemplate());
            GameBaseTemplateContext.AddTemplate(ETemplateType.Item, new GameBaseItemTemplate());
            GameBaseTemplateContext.AddTemplate(ETemplateType.Shop, new GameBaseShopTemplate());


            TemplateConfig templateConfig = new TemplateConfig();
            GameBaseTemplateContext.InitTemplate(templateConfig, ServerType.Client);
            GameBaseTemplateContext.LoadDataTable(templateConfig);
            return result;
        }*/

        public bool Create(AppConfig config, int frame = 30)
        {
            bool result = Create(config.serverConfig, frame);

            GameBaseTemplateContext.AddTemplate(ETemplateType.Account, new GameBaseAccountTemplate());
            GameBaseTemplateContext.AddTemplate(ETemplateType.Item, new GameBaseItemTemplate());
            GameBaseTemplateContext.AddTemplate(ETemplateType.Shop, new GameBaseShopTemplate());

            GameBaseTemplateContext.InitTemplate(config.templateConfig, ServerType.Client);
            GameBaseTemplateContext.LoadDataTable(config.templateConfig);

            return result;
        }

        public sealed override void OnConnect(SocketSession session, IPEndPoint ep)
        {
            GameUserObject userObject = (GameUserObject)session.GetUserObject();
            userObject.OnConnect(ep);
        }
        public sealed override void OnConnectFailed(SocketSession session, string e)
        {
            GameUserObject userObject = (GameUserObject)session.GetUserObject();
            userObject.OnConnectFailed();
        }
        public sealed override void OnDisconnected(SocketSession session, bool bRemote, string e)
        {

        }
        public sealed override void OnClose(SocketSession session)
        {
            UserObject userObject = session.GetUserObject();
            userObject.OnClose();

            /*UserObject obj = session.GetUserObject();
            if (obj != null)
            {
                GameBaseTemplateContext.DeleteClient(obj.GetSession().GetUid());
                obj.OnClose();
                obj.Dispose();
                session.SetUserObject(null);
            }*/
        }
        public override void OnSocketError(SocketSession session, string e) 
        {
            session.Disconnect();
        }
        public override void OnUserEvent(SocketSession session) { }
        public override void OnPacket(SocketSession session, Packet packet)
        {
            try
            {
                ImplObject obj = session.GetUserObject() as ImplObject;
                if (obj != null)
                {
                    AccountController.OnPacket(obj, packet.GetId(), packet);
                    ItemController.OnPacket(obj, packet.GetId(), packet);
                    ShopController.OnPacket(obj, packet.GetId(), packet);
                }
                else
                {
                    Logger.Default.Log(ELogLevel.Err, "wrong session OnPacket");
                }
            }
            catch (FatalException e)
            {
                Logger.Default.Log(ELogLevel.Err, e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                Logger.Default.Log(ELogLevel.Err, e.ToString());
                session.Disconnect();
            }
        }
        public override void OnUpdate(float dt)
        {
            GameBaseTemplateContext.UpdateClient(dt);
            GameBaseTemplateContext.UpdateTemplate(dt);
        }
    }
}
