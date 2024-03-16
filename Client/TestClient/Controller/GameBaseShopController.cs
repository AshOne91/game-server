using System;
using System.Collections.Generic;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Shop.GameBaseShop;
using GameBase.Template.Shop.GameBaseShop.Common;

namespace TestClient
{
	public static class ShopController
	{
		static Dictionary<ulong, GameBaseShopProtocol> _protocolByUid = new Dictionary<ulong, GameBaseShopProtocol>();

		public static void AddShopController(ulong uid)
		{
			GameBaseShopTemplate template = GameBaseTemplateContext.GetTemplate<GameBaseShopTemplate>(uid, ETemplateType.Shop);

			if (_protocolByUid.ContainsKey(uid) == true)
			{
				throw new Exception("Duplication AddShopController");
			}
			GameBaseShopProtocol protocol = new GameBaseShopProtocol();
			protocol.ON_CG_SHOP_INFO_REQ_CALLBACK = template.ON_CG_SHOP_INFO_REQ_CALLBACK;
			protocol.ON_CG_SHOP_INFO_RES_CALLBACK = template.ON_CG_SHOP_INFO_RES_CALLBACK;
			protocol.ON_CG_SHOP_BUY_REQ_CALLBACK = template.ON_CG_SHOP_BUY_REQ_CALLBACK;
			protocol.ON_CG_SHOP_BUY_RES_CALLBACK = template.ON_CG_SHOP_BUY_RES_CALLBACK;
			_protocolByUid.Add(uid, protocol);
		}

		public static void RemoveShopController(ulong uid)
		{
			if (_protocolByUid.ContainsKey(uid) == true)
			{
				_protocolByUid.Remove(uid);
			}
		}

		public static bool OnPacket(ImplObject obj, ushort protocolId, Packet packet)
		{
			ulong uid = obj.GetSession().GetUid();
			if (_protocolByUid.ContainsKey(uid) == false)
			{
				return false;
			}
			var Protocol = _protocolByUid[uid];
			return Protocol.OnPacket(obj, protocolId, packet);
		}
	}
}
