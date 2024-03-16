#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Shop.GameBaseShop.Common;

namespace GameBase.Template.Shop.GameBaseShop
{
	public partial class GameBaseShopTemplate
	{
		public void ON_CG_SHOP_INFO_REQ_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_INFO_REQ packet)
		{
			PACKET_CG_SHOP_INFO_RES sendData = new PACKET_CG_SHOP_INFO_RES();
			Dictionary<int/*shop_index*/, ShopInfo> shopInfoList = new Dictionary<int , ShopInfo>();
			userObject.GetUserDB().GetReadUserDB<GameBaseShopUserDB>(ETemplateType.Shop)._dbSlotContainer_DBShopTable.ForEach(slot =>
			{
				ShopInfo shopInfo = null;
				if (shopInfoList.TryGetValue(slot._DBData.shop_index, out shopInfo) == false)
				{
                    shopInfo.shopId = slot._DBData.shop_index;
                    shopInfo.resetRemainType = 0;
                    shopInfo.listProductInfo = new List<ShopProductInfo>();
                    shopInfo.pointResetCount = 0;
                    shopInfo.ProductStatusList = new List<ProductStatus>();
					shopInfoList.Add(shopInfo.shopId, shopInfo);
                }
				ShopProductInfo productInfo = new ShopProductInfo();
				productInfo.shopProductId = slot._DBData.shop_product_index;
				productInfo.buyCount = (byte)slot._DBData.buy_count;
				shopInfo.listProductInfo.Add(productInfo);
			});
			foreach (var info in shopInfoList)
			{
				sendData.listShopInfo.Add(info.Value);
			}
            userObject.GetSession().SendPacket(sendData.Serialize());
		}
		public void ON_CG_SHOP_INFO_RES_CALLBACK(ImplObject userObject, PACKET_CG_SHOP_INFO_RES packet)
		{
			GameBaseShopClientImpl Impl = userObject.GetShopImpl<GameBaseShopClientImpl>();
			if (packet.ErrorCode != (int)GServerCode.SUCCESS)
			{
				Impl.ClientCallback("PacketError", packet.ToString());
			}
			Impl.ClientCallback("ShopInfo", packet.listShopInfo);
        }
	}
}
