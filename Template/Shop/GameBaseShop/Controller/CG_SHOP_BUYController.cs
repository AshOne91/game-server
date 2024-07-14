#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Shop.GameBaseShop.Common;
using GameBase.Template.GameBase.Table;

namespace GameBase.Template.Shop.GameBaseShop
{
	public partial class GameBaseShopTemplate
	{
		public void ON_CG_SHOP_BUY_REQ_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_BUY_REQ packet)
		{
			PACKET_CG_SHOP_BUY_RES sendData = new PACKET_CG_SHOP_BUY_RES();
            var listQuestCompleteParam = new List<QuestCompleteParam>();
            var shopProduct = userObject.UserDB.GetReadUserDB<GameBaseShopUserDB>(ETemplateType.Shop)._dbSlotContainer_DBShopTable.Find(slot => slot._DBData.shop_index == packet.shopId && slot._DBData.shop_product_index == packet.shopProductId);
			if (shopProduct == null) 
			{
				sendData.ErrorCode = (int)GServerCode.DBNotFound;
                userObject.GetSession().SendPacket(sendData.Serialize());
				return;
            }

			var productList = DataTable<int, ShopProductListTable>.Instance.GetData(packet.shopProductId);
            if (productList == null)
			{
				sendData.ErrorCode = (int)GServerCode.TableNotFound;
				userObject.GetSession().SendPacket(sendData.Serialize());
				return;
            }

			var resBuyItem = UpdateBuyItem(userObject, productList);
			if (resBuyItem.Item1 != GServerCode.SUCCESS)
			{
				sendData.ErrorCode = (int)resBuyItem.Item1;
				userObject.GetSession().SendPacket(sendData.Serialize());
				return;
			}
			listQuestCompleteParam.AddRange(resBuyItem.Item3);
			var resProduct = UpdateProduct(userObject, productList);
			listQuestCompleteParam.AddRange(resProduct.listQuestCompleteParam);
			var resRewardItem = CreateRewardItem(userObject, productList);
			listQuestCompleteParam.AddRange(resRewardItem.listQuest);

			listQuestCompleteParam.Add(new QuestCompleteParam { QuestType = (int)QuestType.BuyShop, EndId = -1, Value = 1 });

			if (GameBaseTemplateContext.Quest != null)
			{
				var listUpdateQuest = GameBaseTemplateContext.GetTemplate<QuestTemplate>(ETemplateType.Quest).UpdateQuest(userObject, listQuestCompleteParam);
				if (listUpdateQuest != null)
				{
					sendData.listQuestData.AddRange(listUpdateQuest);
				}
            }

			sendData.ErrorCode = (int)GServerCode.SUCCESS;
			sendData.shopId = packet.shopId;
			sendData.shopProductInfo = resProduct.productInfo;
			sendData.changeProductInfo = resProduct.changeProductInfo;
			if (resBuyItem.Item2 != null && resBuyItem.Item2.Count > 0)
			{
				sendData.deleteItemInfo = resBuyItem.Item2[0];
            }
			sendData.listRewardInfo = resRewardItem.listReward;
			userObject.GetSession().SendPacket(sendData.Serialize());
        }
		public void ON_CG_SHOP_BUY_RES_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_BUY_RES packet)
		{
			if (packet.ErrorCode != (int)GServerCode.SUCCESS)
			{
                userObject.ClientCallback("PacketError", packet.ToString());
				return;
			}
            userObject.ClientCallback("ShopBuy", packet);
		}
	}
}
