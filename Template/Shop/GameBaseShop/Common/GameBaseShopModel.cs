#define SERVER
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Numerics;
using Service.Net;
using Service.Core;

namespace GameBase.Template.Shop.GameBaseShop.Common
{
	public class ShopInfo : IPacketSerializable
	{
		/// <sumary>
		/// ShopInfo 아이디
		/// </sumary>
		public int shopId = new int();
		/// <sumary>
		/// 갱신 남은 초
		/// </sumary>
		public int resetRemainType = new int();
		/// <sumary>
		/// 상품 목록
		/// </sumary>
		public List<ShopProductInfo> listProductInfo = new List<ShopProductInfo>();
		/// <sumary>
		/// 재화 갱신 횟수
		/// </sumary>
		public byte pointResetCount = new byte();
		/// <sumary>
		/// 상품 상태 리스트
		/// </sumary>
		public List<ProductStatus> ProductStatusList = new List<ProductStatus>();
		public void Serialize(Packet packet)
		{
			packet.Write(shopId);
			packet.Write(resetRemainType);
			int lengthlistProductInfo = (listProductInfo == null) ? 0 : listProductInfo.Count;
			packet.Write(lengthlistProductInfo);
			for (int i = 0; i < lengthlistProductInfo; ++i)
			{
				packet.Write(listProductInfo[i]);
			}
			packet.Write(pointResetCount);
			int lengthProductStatusList = (ProductStatusList == null) ? 0 : ProductStatusList.Count;
			packet.Write(lengthProductStatusList);
			for (int i = 0; i < lengthProductStatusList; ++i)
			{
				packet.Write(ProductStatusList[i]);
			}
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref shopId);
			packet.Read(ref resetRemainType);
			int lengthlistProductInfo = (listProductInfo == null) ? 0 : listProductInfo.Count;
			packet.Read(ref lengthlistProductInfo);
			for (int i = 0; i < lengthlistProductInfo; ++i)
			{
				ShopProductInfo element = new ShopProductInfo();
				packet.Read(element);
				listProductInfo.Add(element);
			}
			packet.Read(ref pointResetCount);
			int lengthProductStatusList = (ProductStatusList == null) ? 0 : ProductStatusList.Count;
			packet.Read(ref lengthProductStatusList);
			for (int i = 0; i < lengthProductStatusList; ++i)
			{
				ProductStatus element = new ProductStatus();
				packet.Read(element);
				ProductStatusList.Add(element);
			}
		}
		public string GetLog()
		{
			string log = "";
			FieldInfo[] fields = this.GetType().GetFields();
			foreach (FieldInfo field in fields)
			{
				log += string.Format("{{0}}={{1}}\r\n", field.Name, field.GetValue(this).ToString());
			}
			return log;
		}
	}
	public class ShopProductInfo : IPacketSerializable
	{
		/// <sumary>
		/// 상품 아이디
		/// </sumary>
		public int shopProductId = new int();
		/// <sumary>
		/// 구매 횟수
		/// </sumary>
		public byte buyCount = new byte();
		public void Serialize(Packet packet)
		{
			packet.Write(shopProductId);
			packet.Write(buyCount);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref shopProductId);
			packet.Read(ref buyCount);
		}
		public string GetLog()
		{
			string log = "";
			FieldInfo[] fields = this.GetType().GetFields();
			foreach (FieldInfo field in fields)
			{
				log += string.Format("{{0}}={{1}}\r\n", field.Name, field.GetValue(this).ToString());
			}
			return log;
		}
	}
	public class ProductStatus : IPacketSerializable
	{
		/// <sumary>
		/// 상태 타입
		/// </sumary>
		public int statusType = new int();
		/// <sumary>
		/// 상태 종료 남은시간(초단위)
		/// </sumary>
		public int remainEndTime = new int();
		public void Serialize(Packet packet)
		{
			packet.Write(statusType);
			packet.Write(remainEndTime);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref statusType);
			packet.Read(ref remainEndTime);
		}
		public string GetLog()
		{
			string log = "";
			FieldInfo[] fields = this.GetType().GetFields();
			foreach (FieldInfo field in fields)
			{
				log += string.Format("{{0}}={{1}}\r\n", field.Name, field.GetValue(this).ToString());
			}
			return log;
		}
	}
}
