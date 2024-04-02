#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Item.GameBaseItem.Common;

namespace GameBase.Template.Item.GameBaseItem
{
	public partial class GameBaseItemTemplate
	{
		public void ON_CG_ITEM_INFO_REQ_CALLBACK(ImplObject userObject, PACKET_CG_ITEM_INFO_REQ packet)
		{
			PACKET_CG_ITEM_INFO_RES sendData = new PACKET_CG_ITEM_INFO_RES();
			List<ItemBaseInfo> listItemBaseInfo = new List<ItemBaseInfo>();
			var DBItemList = userObject.GetUserDB().GetReadUserDB<GameBaseItemUserDB>(ETemplateType.Item)._dbSlotContainer_DBItemTable;
			DBItemList.ForEach(slot => 
			{
				ItemBaseInfo itemInfo = new ItemBaseInfo();
				itemInfo.ItemId = slot._DBData.item_id;
				itemInfo.Value = slot._DBData.item_count;
				itemInfo.TotalValue = slot._DBData.item_count;
				sendData.listItemInfo.Add(itemInfo);
            });

			userObject.GetSession().SendPacket(sendData.Serialize());
		}
		public void ON_CG_ITEM_INFO_RES_CALLBACK(ImplObject userObject, PACKET_CG_ITEM_INFO_RES packet)
		{
			GameBaseItemClientImpl Impl = userObject.GetItemImpl<GameBaseItemClientImpl>();
			if (packet.ErrorCode != (int)GServerCode.SUCCESS)
			{
				Impl.ClientCallback("PacketError", packet.ToString());
				return;
			}
			Impl.ClientCallback("ItemInfo", packet.listItemInfo);
		}
	}
}
