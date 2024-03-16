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
			var shopProduct = userObject.GetUserDB().GetReadUserDB<GameBaseShopUserDB>(ETemplateType.Shop)._dbSlotContainer_DBShopTable.Find(slot => slot._DBData.shop_index == packet.shopId && slot._DBData.shop_product_index == packet.shopProductId);
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

			var resProduct = UpdateProduct;
			var resRewardItem = CreateRewardItem;
        }
		public void ON_CG_SHOP_BUY_RES_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_BUY_RES packet)
		{
		}
	}
}
