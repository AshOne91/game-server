#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using GameBase.Template.GameBase;

namespace GameBase.Template.Shop.GameBaseShop.Common
{
	public partial class GameBaseShopProtocol
	{
		Dictionary<ushort, ControllerDelegate> MessageControllers = new Dictionary<ushort, ControllerDelegate>();

		public GameBaseShopProtocol()
		{
			Init();
		}

		void Init()
		{
			MessageControllers.Add(PACKET_CG_SHOP_INFO_REQ.ProtocolId, CG_SHOP_INFO_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_CG_SHOP_INFO_RES.ProtocolId, CG_SHOP_INFO_RES_CONTROLLER);
			MessageControllers.Add(PACKET_CG_SHOP_BUY_REQ.ProtocolId, CG_SHOP_BUY_REQ_CONTROLLER);
			MessageControllers.Add(PACKET_CG_SHOP_BUY_RES.ProtocolId, CG_SHOP_BUY_RES_CONTROLLER);
		}

		public virtual bool OnPacket(ImplObject userObject, ushort protocolId, Packet packet)
		{
			ControllerDelegate controllerCallback;
			if(MessageControllers.TryGetValue(protocolId, out controllerCallback) == false)
			{
				return false;
			}
			controllerCallback(userObject, packet);
			return true;
		}

		public delegate void CG_SHOP_INFO_REQ_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_INFO_REQ packet);
		public CG_SHOP_INFO_REQ_CALLBACK ON_CG_SHOP_INFO_REQ_CALLBACK;
		public void CG_SHOP_INFO_REQ_CONTROLLER(ImplObject obj, Packet packet)
		{
			PACKET_CG_SHOP_INFO_REQ recvPacket = new PACKET_CG_SHOP_INFO_REQ();
			recvPacket.Deserialize(packet);
			ON_CG_SHOP_INFO_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void CG_SHOP_INFO_RES_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_INFO_RES packet);
		public CG_SHOP_INFO_RES_CALLBACK ON_CG_SHOP_INFO_RES_CALLBACK;
		public void CG_SHOP_INFO_RES_CONTROLLER(ImplObject obj, Packet packet)
		{
			PACKET_CG_SHOP_INFO_RES recvPacket = new PACKET_CG_SHOP_INFO_RES();
			recvPacket.Deserialize(packet);
			ON_CG_SHOP_INFO_RES_CALLBACK(obj, recvPacket);
		}
		public delegate void CG_SHOP_BUY_REQ_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_BUY_REQ packet);
		public CG_SHOP_BUY_REQ_CALLBACK ON_CG_SHOP_BUY_REQ_CALLBACK;
		public void CG_SHOP_BUY_REQ_CONTROLLER(ImplObject obj, Packet packet)
		{
			PACKET_CG_SHOP_BUY_REQ recvPacket = new PACKET_CG_SHOP_BUY_REQ();
			recvPacket.Deserialize(packet);
			ON_CG_SHOP_BUY_REQ_CALLBACK(obj, recvPacket);
		}
		public delegate void CG_SHOP_BUY_RES_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_BUY_RES packet);
		public CG_SHOP_BUY_RES_CALLBACK ON_CG_SHOP_BUY_RES_CALLBACK;
		public void CG_SHOP_BUY_RES_CONTROLLER(ImplObject obj, Packet packet)
		{
			PACKET_CG_SHOP_BUY_RES recvPacket = new PACKET_CG_SHOP_BUY_RES();
			recvPacket.Deserialize(packet);
			ON_CG_SHOP_BUY_RES_CALLBACK(obj, recvPacket);
		}
	}
}
