#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.Shop.GameBaseShop.Common
{
	public sealed class PACKET_CG_SHOP_INFO_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 1001;
		public PACKET_CG_SHOP_INFO_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
	public sealed class PACKET_CG_SHOP_INFO_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 1002;
		/// <summary>
		/// 상점 정보
		/// </summary>
		public List<ShopInfo> listShopInfo = new List<ShopInfo>();
		public PACKET_CG_SHOP_INFO_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			int lengthlistShopInfo = (listShopInfo == null) ? 0 : listShopInfo.Count;
			packet.Write(lengthlistShopInfo);
			for (int i = 0; i < lengthlistShopInfo; ++i)
			{
				packet.Write(listShopInfo[i]);
			}
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			int lengthlistShopInfo = (listShopInfo == null) ? 0 : listShopInfo.Count;
			packet.Read(ref lengthlistShopInfo);
			for (int i = 0; i < lengthlistShopInfo; ++i)
			{
				ShopInfo element = new ShopInfo();
				packet.Read(element);
				listShopInfo.Add(element);
			}
		}
	};
	public sealed class PACKET_CG_SHOP_BUY_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 1003;
		/// <summary>
		/// 상점 아이디
		/// </summary>
		public int shopId = new int();
		/// <summary>
		/// 상품 아이디
		/// </summary>
		public int shopProductId = new int();
		public PACKET_CG_SHOP_BUY_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(shopId);
			packet.Write(shopProductId);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref shopId);
			packet.Read(ref shopProductId);
		}
	}
	public sealed class PACKET_CG_SHOP_BUY_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 1004;
		/// <summary>
		/// 상점 아이디
		/// </summary>
		public int shopId = new int();
		/// <summary>
		/// 상품 정보
		/// </summary>
		public ShopProductInfo shopProductInfo = new ShopProductInfo();
		/// <summary>
		/// 변경 상품 정보
		/// </summary>
		public ShopProductInfo changeProductInfo = new ShopProductInfo();
		/// <summary>
		/// 삭제 아이템 정보
		/// </summary>
		public ItemBaseInfo deleteItemInfo = new ItemBaseInfo();
		/// <summary>
		/// 보상 아이템 정보
		/// </summary>
		public List<ItemBaseInfo> listRewardInfo = new List<ItemBaseInfo>();
		/// <summary>
		/// 업데이트 퀘스트 데이터
		/// </summary>
		public List<QuestData> listQuestData = new List<QuestData>();
		public PACKET_CG_SHOP_BUY_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			packet.Write(shopId);
			packet.Write(shopProductInfo);
			packet.Write(changeProductInfo);
			packet.Write(deleteItemInfo);
			int lengthlistRewardInfo = (listRewardInfo == null) ? 0 : listRewardInfo.Count;
			packet.Write(lengthlistRewardInfo);
			for (int i = 0; i < lengthlistRewardInfo; ++i)
			{
				packet.Write(listRewardInfo[i]);
			}
			int lengthlistQuestData = (listQuestData == null) ? 0 : listQuestData.Count;
			packet.Write(lengthlistQuestData);
			for (int i = 0; i < lengthlistQuestData; ++i)
			{
				packet.Write(listQuestData[i]);
			}
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			packet.Read(ref shopId);
			packet.Read(shopProductInfo);
			packet.Read(changeProductInfo);
			packet.Read(deleteItemInfo);
			int lengthlistRewardInfo = (listRewardInfo == null) ? 0 : listRewardInfo.Count;
			packet.Read(ref lengthlistRewardInfo);
			for (int i = 0; i < lengthlistRewardInfo; ++i)
			{
				ItemBaseInfo element = new ItemBaseInfo();
				packet.Read(element);
				listRewardInfo.Add(element);
			}
			int lengthlistQuestData = (listQuestData == null) ? 0 : listQuestData.Count;
			packet.Read(ref lengthlistQuestData);
			for (int i = 0; i < lengthlistQuestData; ++i)
			{
				QuestData element = new QuestData();
				packet.Read(element);
				listQuestData.Add(element);
			}
		}
	};
}
