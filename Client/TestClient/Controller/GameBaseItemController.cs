using System;
using System.Collections.Generic;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.Item.GameBaseItem;
using GameBase.Template.Item.GameBaseItem.Common;

namespace TestClient
{
	public static class ItemController
	{
		static Dictionary<ulong, GameBaseItemProtocol> _protocolByUid = new Dictionary<ulong, GameBaseItemProtocol>();

		public static void AddItemController(ulong uid)
		{
			GameBaseItemTemplate template = GameBaseTemplateContext.GetTemplate<GameBaseItemTemplate>(uid, ETemplateType.Item);

			if (_protocolByUid.ContainsKey(uid) == true)
			{
				throw new Exception("Duplication AddItemController");
			}
			GameBaseItemProtocol protocol = new GameBaseItemProtocol();
			protocol.ON_CG_ITEM_INFO_REQ_CALLBACK = template.ON_CG_ITEM_INFO_REQ_CALLBACK;
			protocol.ON_CG_ITEM_INFO_RES_CALLBACK = template.ON_CG_ITEM_INFO_RES_CALLBACK;
			_protocolByUid.Add(uid, protocol);
		}

		public static void RemoveItemController(ulong uid)
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
