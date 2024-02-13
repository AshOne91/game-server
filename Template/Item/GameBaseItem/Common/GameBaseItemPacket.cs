#define SERVER
using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;

namespace GameBase.Template.Item.GameBaseItem.Common
{
	public sealed class PACKET_CG_ITEM_INFO_REQ : PacketBaseRequest
	{
		public static readonly ushort ProtocolId = 30001;
		public PACKET_CG_ITEM_INFO_REQ():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
		}
	}
	public sealed class PACKET_CG_ITEM_INFO_RES : PacketBaseResponse
	{
		public static readonly ushort ProtocolId = 30002;
		/// <summary>
		/// 아이템 정보 리스트
		/// </summary>
		public List<ItemBaseInfo> listItemInfo = new List<ItemBaseInfo>();
		public PACKET_CG_ITEM_INFO_RES():base(ProtocolId){}
		public override void Serialize(Packet packet)
		{
			base.Serialize(packet);
			int lengthlistItemInfo = (listItemInfo == null) ? 0 : listItemInfo.Count;
			packet.Write(lengthlistItemInfo);
			for (int i = 0; i < lengthlistItemInfo; ++i)
			{
				packet.Write(listItemInfo[i]);
			}
		}
		public override void Deserialize(Packet packet)
		{
			base.Deserialize(packet);
			int lengthlistItemInfo = (listItemInfo == null) ? 0 : listItemInfo.Count;
			packet.Read(ref lengthlistItemInfo);
			for (int i = 0; i < lengthlistItemInfo; ++i)
			{
				ItemBaseInfo element = new ItemBaseInfo();
				packet.Read(element);
				listItemInfo.Add(element);
			}
		}
	};
}
