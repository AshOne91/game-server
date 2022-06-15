#define SERVER
using System;
using System.Collections.Generic;
using System.Numerics;
using Service.Net;
using Service.Core;

namespace GameBase.Template.GameBase.Common
{
	public class ItemBaseInfo : IPacketSerializable
	{
		/// <sumary>
		/// 부모 아이템 아이디
		/// </sumary>
		public int ParentItemId = new int();
		/// <sumary>
		/// 아이템 그룹 인덱스(아이템이 속한 그룹을 구분짓기 위한 인덱스값)
		/// </sumary>
		public int GroupIndex = new int();
		/// <sumary>
		/// 아이템 타입
		/// </sumary>
		public int ItemType = new int();
		/// <sumary>
		/// 아이템 아이디
		/// </sumary>
		public int ItemId = new int();
		/// <sumary>
		/// 아이템 레벨
		/// </sumary>
		public int ItemLevel = new int();
		/// <sumary>
		/// 아이템 업데이트 수량
		/// </sumary>
		public long Value = new long();
		/// <sumary>
		/// 아이템 총수량
		/// </sumary>
		public long TotalValue = new long();
		/// <sumary>
		/// 남은 시간(초단위, -1: 무제한)
		/// </sumary>
		public int RemainTime = new int();
		public void Serialize(Packet packet)
		{
			packet.Write(ParentItemId);
			packet.Write(GroupIndex);
			packet.Write(ItemType);
			packet.Write(ItemId);
			packet.Write(ItemLevel);
			packet.Write(Value);
			packet.Write(TotalValue);
			packet.Write(RemainTime);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref ParentItemId);
			packet.Read(ref GroupIndex);
			packet.Read(ref ItemType);
			packet.Read(ref ItemId);
			packet.Read(ref ItemLevel);
			packet.Read(ref Value);
			packet.Read(ref TotalValue);
			packet.Read(ref RemainTime);
		}
	}
	public class QuestData : IPacketSerializable
	{
		/// <sumary>
		/// 퀘스트 데이터 아이디
		/// </sumary>
		public int QuestId = new int();
		/// <sumary>
		/// 퀘스트 수치
		/// </sumary>
		public long Value = new long();
		/// <sumary>
		/// 퀘스트 상태
		/// </sumary>
		public byte Status = new byte();
		public void Serialize(Packet packet)
		{
			packet.Write(QuestId);
			packet.Write(Value);
			packet.Write(Status);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref QuestId);
			packet.Read(ref Value);
			packet.Read(ref Status);
		}
	}
	public class QuestCompleteParam : IPacketSerializable
	{
		/// <sumary>
		/// 퀘스트 완료 타입
		/// </sumary>
		public int QuestType = new int();
		/// <sumary>
		/// 퀘스트 완료 아이디
		/// </sumary>
		public int EndId = new int();
		/// <sumary>
		/// 퀘스트 수치
		/// </sumary>
		public long Value = new long();
		public void Serialize(Packet packet)
		{
			packet.Write(QuestType);
			packet.Write(EndId);
			packet.Write(Value);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref QuestType);
			packet.Read(ref EndId);
			packet.Read(ref Value);
		}
	}
	public class QuestInfo : IPacketSerializable
	{
		/// <sumary>
		/// 퀘스트 그룹 리스트
		/// </sumary>
		public List<QuestGroup> listQuestGroup = new List<QuestGroup>();
		/// <sumary>
		/// 일일 보상 정보
		/// </sumary>
		public DailyRewardInfo DailyRewardInfo = new DailyRewardInfo();
		public void Serialize(Packet packet)
		{
			int lengthlistQuestGroup = (listQuestGroup == null) ? 0 : listQuestGroup.Count;
			packet.Write(lengthlistQuestGroup);
			for (int i = 0; i < lengthlistQuestGroup; ++i)
			{
				packet.Write(listQuestGroup[i]);
			}
			packet.Write(DailyRewardInfo);
		}
		public void Deserialize(Packet packet)
		{
			int lengthlistQuestGroup = (listQuestGroup == null) ? 0 : listQuestGroup.Count;
			packet.Read(ref lengthlistQuestGroup);
			for (int i = 0; i < lengthlistQuestGroup; ++i)
			{
				QuestGroup element = new QuestGroup();
				packet.Read(element);
				listQuestGroup.Add(element);
			}
			packet.Read(DailyRewardInfo);
		}
	}
	public class QuestGroup : IPacketSerializable
	{
		/// <sumary>
		/// 퀘스트 기간 타입
		/// </sumary>
		public int PeriodType = new int();
		/// <sumary>
		/// 만료까지 남은 시간(초단위)
		/// </sumary>
		public int ExpireTime = new int();
		/// <sumary>
		/// 초기화 횟수
		/// </sumary>
		public int ResetCount = new int();
		/// <sumary>
		/// 퀘스트 데이터 리스트
		/// </sumary>
		public List<QuestData> listQuestData = new List<QuestData>();
		public void Serialize(Packet packet)
		{
			packet.Write(PeriodType);
			packet.Write(ExpireTime);
			packet.Write(ResetCount);
			int lengthlistQuestData = (listQuestData == null) ? 0 : listQuestData.Count;
			packet.Write(lengthlistQuestData);
			for (int i = 0; i < lengthlistQuestData; ++i)
			{
				packet.Write(listQuestData[i]);
			}
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref PeriodType);
			packet.Read(ref ExpireTime);
			packet.Read(ref ResetCount);
			int lengthlistQuestData = (listQuestData == null) ? 0 : listQuestData.Count;
			packet.Read(ref lengthlistQuestData);
			for (int i = 0; i < lengthlistQuestData; ++i)
			{
				QuestData element = new QuestData();
				packet.Read(element);
				listQuestData.Add(element);
			}
		}
	}
	public class DailyRewardSlot : IPacketSerializable
	{
		/// <sumary>
		/// 슬롯 인덱스
		/// </sumary>
		public byte SlotIndex = new byte();
		/// <sumary>
		/// 아이템 아이디
		/// </sumary>
		public int ItemId = new int();
		/// <sumary>
		/// 아이템 수량
		/// </sumary>
		public int ItemCount = new int();
		/// <sumary>
		/// 아이템 지급 상태
		/// </sumary>
		public bool IsAcquired = new bool();
		public void Serialize(Packet packet)
		{
			packet.Write(SlotIndex);
			packet.Write(ItemId);
			packet.Write(ItemCount);
			packet.Write(IsAcquired);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref SlotIndex);
			packet.Read(ref ItemId);
			packet.Read(ref ItemCount);
			packet.Read(ref IsAcquired);
		}
	}
	public class DailyRewardInfo : IPacketSerializable
	{
		/// <sumary>
		/// 보상 슬롯 정보 
		/// </sumary>
		public List<DailyRewardSlot> listSlotInfo = new List<DailyRewardSlot>();
		/// <sumary>
		/// 만료 시간 
		/// </sumary>
		public int ExpireTime = new int();
		public void Serialize(Packet packet)
		{
			int lengthlistSlotInfo = (listSlotInfo == null) ? 0 : listSlotInfo.Count;
			packet.Write(lengthlistSlotInfo);
			for (int i = 0; i < lengthlistSlotInfo; ++i)
			{
				packet.Write(listSlotInfo[i]);
			}
			packet.Write(ExpireTime);
		}
		public void Deserialize(Packet packet)
		{
			int lengthlistSlotInfo = (listSlotInfo == null) ? 0 : listSlotInfo.Count;
			packet.Read(ref lengthlistSlotInfo);
			for (int i = 0; i < lengthlistSlotInfo; ++i)
			{
				DailyRewardSlot element = new DailyRewardSlot();
				packet.Read(element);
				listSlotInfo.Add(element);
			}
			packet.Read(ref ExpireTime);
		}
	}
	public class UserBuilding : IPacketSerializable
	{
		/// <sumary>
		/// 건물 고유 ID
		/// </sumary>
		public int id = new int();
		/// <sumary>
		/// 빌딩 데이터 테이블 ID
		/// </sumary>
		public int buildingDataId = new int();
		/// <sumary>
		/// 건물 레벨
		/// </sumary>
		public int level = new int();
		/// <sumary>
		/// 최근 보상을 요청해서 받은 시간
		/// </sumary>
		public DateTime lastReceiveDateTime = new DateTime();
		/// <sumary>
		/// 건설 완료시간
		/// </sumary>
		public DateTime constructEndTime = new DateTime();
		/// <sumary>
		/// 건물 건설 완료 상태 (false는 건설은 되었지만 완료 처리가 안된경우)
		/// </sumary>
		public bool isConstructComplete = new bool();
		public void Serialize(Packet packet)
		{
			packet.Write(id);
			packet.Write(buildingDataId);
			packet.Write(level);
			packet.Write(lastReceiveDateTime);
			packet.Write(constructEndTime);
			packet.Write(isConstructComplete);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref id);
			packet.Read(ref buildingDataId);
			packet.Read(ref level);
			packet.Read(ref lastReceiveDateTime);
			packet.Read(ref constructEndTime);
			packet.Read(ref isConstructComplete);
		}
	}
	public class MailText : IPacketSerializable
	{
		/// <sumary>
		/// 보낸이(key:언어코드, value:내용)
		/// </sumary>
		public Dictionary<string, string> textSender = new Dictionary<string, string>();
		/// <sumary>
		/// 제목(key:언어코드, value:내용)
		/// </sumary>
		public Dictionary<string, string> textTitle = new Dictionary<string, string>();
		/// <sumary>
		/// 내용(key:언어코드, value:내용)
		/// </sumary>
		public Dictionary<string, string> textContent = new Dictionary<string, string>();
		public void Serialize(Packet packet)
		{
			packet.Write(textSender.Count);
			foreach (var pair in textSender)
			{
				packet.Write(pair.Key);
				packet.Write(pair.Value);
			}
			packet.Write(textTitle.Count);
			foreach (var pair in textTitle)
			{
				packet.Write(pair.Key);
				packet.Write(pair.Value);
			}
			packet.Write(textContent.Count);
			foreach (var pair in textContent)
			{
				packet.Write(pair.Key);
				packet.Write(pair.Value);
			}
		}
		public void Deserialize(Packet packet)
		{
			int lengthtextSender = 0;
			packet.Read(ref lengthtextSender);
			for (int i = 0; i < lengthtextSender; ++i)
			{
				string tempKey = string.Empty;
				string tempValue = string.Empty;
				packet.Read(ref tempKey);
				packet.Read(ref tempValue);
				textSender.Add(tempKey, tempValue);
			}
			int lengthtextTitle = 0;
			packet.Read(ref lengthtextTitle);
			for (int i = 0; i < lengthtextTitle; ++i)
			{
				string tempKey = string.Empty;
				string tempValue = string.Empty;
				packet.Read(ref tempKey);
				packet.Read(ref tempValue);
				textTitle.Add(tempKey, tempValue);
			}
			int lengthtextContent = 0;
			packet.Read(ref lengthtextContent);
			for (int i = 0; i < lengthtextContent; ++i)
			{
				string tempKey = string.Empty;
				string tempValue = string.Empty;
				packet.Read(ref tempKey);
				packet.Read(ref tempValue);
				textContent.Add(tempKey, tempValue);
			}
		}
	}
	public class CharacterProfile : IPacketSerializable
	{
		/// <sumary>
		/// 캐릭터 아이디
		/// </sumary>
		public int charId = new int();
		/// <sumary>
		/// 레벨
		/// </sumary>
		public int level = new int();
		public void Serialize(Packet packet)
		{
			packet.Write(charId);
			packet.Write(level);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref charId);
			packet.Read(ref level);
		}
	}
	public class RankerInfo : IPacketSerializable
	{
		/// <sumary>
		/// 유저 등수
		/// </sumary>
		public long rank = new long();
		/// <sumary>
		/// 유저 닉네임
		/// </sumary>
		public string userName = string.Empty;
		/// <sumary>
		/// 유저 클래스
		/// </sumary>
		public int classId = new int();
		/// <sumary>
		/// 유저 점수
		/// </sumary>
		public long score = new long();
		/// <sumary>
		/// 연동 플랫폼 정보(key: 플랫폼타입(EPlatformType참조), value: 플랫폼 아이디)
		/// </sumary>
		public Dictionary<int, string> linkedPlatformInfo = new Dictionary<int, string>();
		/// <sumary>
		/// 캐릭터 프로필
		/// </sumary>
		public List<CharacterProfile> listCharacterProfile = new List<CharacterProfile>();
		public void Serialize(Packet packet)
		{
			packet.Write(rank);
			packet.Write(userName);
			packet.Write(classId);
			packet.Write(score);
			packet.Write(linkedPlatformInfo.Count);
			foreach (var pair in linkedPlatformInfo)
			{
				packet.Write(pair.Key);
				packet.Write(pair.Value);
			}
			int lengthlistCharacterProfile = (listCharacterProfile == null) ? 0 : listCharacterProfile.Count;
			packet.Write(lengthlistCharacterProfile);
			for (int i = 0; i < lengthlistCharacterProfile; ++i)
			{
				packet.Write(listCharacterProfile[i]);
			}
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref rank);
			packet.Read(ref userName);
			packet.Read(ref classId);
			packet.Read(ref score);
			int lengthlinkedPlatformInfo = 0;
			packet.Read(ref lengthlinkedPlatformInfo);
			for (int i = 0; i < lengthlinkedPlatformInfo; ++i)
			{
				int tempKey = new int();
				string tempValue = string.Empty;
				packet.Read(ref tempKey);
				packet.Read(ref tempValue);
				linkedPlatformInfo.Add(tempKey, tempValue);
			}
			int lengthlistCharacterProfile = (listCharacterProfile == null) ? 0 : listCharacterProfile.Count;
			packet.Read(ref lengthlistCharacterProfile);
			for (int i = 0; i < lengthlistCharacterProfile; ++i)
			{
				CharacterProfile element = new CharacterProfile();
				packet.Read(element);
				listCharacterProfile.Add(element);
			}
		}
	}
	public class SeasonInfo : IPacketSerializable
	{
		/// <sumary>
		/// 시즌 아이디
		/// </sumary>
		public int SeasonId = new int();
		/// <sumary>
		/// 시즌 상태
		/// </sumary>
		public int SeasonState = new int();
		/// <sumary>
		/// 종료까지 남은 시간(초단위)
		/// </sumary>
		public int EndRemainTime = new int();
		/// <sumary>
		/// 정산 남은 시간(초단위)
		/// </sumary>
		public int PrepareRemainTime = new int();
		/// <sumary>
		/// 시즌 포인트
		/// </sumary>
		public int SeasonPoint = new int();
		/// <sumary>
		/// 시즌 패스 보유 여부
		/// </sumary>
		public bool HasSeasonPass = new bool();
		/// <sumary>
		/// 시즌 보상 아이디
		/// </sumary>
		public List<int> SeasonRewardIds = new List<int>();
		/// <sumary>
		/// 오픈 보상 아이디
		/// </sumary>
		public int OpenRewardId = new int();
		public void Serialize(Packet packet)
		{
			packet.Write(SeasonId);
			packet.Write(SeasonState);
			packet.Write(EndRemainTime);
			packet.Write(PrepareRemainTime);
			packet.Write(SeasonPoint);
			packet.Write(HasSeasonPass);
			int lengthSeasonRewardIds = (SeasonRewardIds == null) ? 0 : SeasonRewardIds.Count;
			packet.Write(lengthSeasonRewardIds);
			for (int i = 0; i < lengthSeasonRewardIds; ++i)
			{
				packet.Write(SeasonRewardIds[i]);
			}
			packet.Write(OpenRewardId);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref SeasonId);
			packet.Read(ref SeasonState);
			packet.Read(ref EndRemainTime);
			packet.Read(ref PrepareRemainTime);
			packet.Read(ref SeasonPoint);
			packet.Read(ref HasSeasonPass);
			int lengthSeasonRewardIds = (SeasonRewardIds == null) ? 0 : SeasonRewardIds.Count;
			packet.Read(ref lengthSeasonRewardIds);
			for (int i = 0; i < lengthSeasonRewardIds; ++i)
			{
				int element = new int();
				packet.Read(ref element);
				SeasonRewardIds.Add(element);
			}
			packet.Read(ref OpenRewardId);
		}
	}
	public class GameServerInfo : IPacketSerializable
	{
		/// <sumary>
		/// 서버 고유아이디
		/// </sumary>
		public int ServerId = new int();
		/// <sumary>
		/// 가동여부
		/// </sumary>
		public bool Alive = new bool();
		/// <sumary>
		/// 접속 유저 수
		/// </sumary>
		public int UserCount = new int();
		/// <sumary>
		/// 서버 아이피
		/// </sumary>
		public string Ip = string.Empty;
		/// <sumary>
		/// 게임서버 포트
		/// </sumary>
		public ushort Port = new ushort();
		public void Serialize(Packet packet)
		{
			packet.Write(ServerId);
			packet.Write(Alive);
			packet.Write(UserCount);
			packet.Write(Ip);
			packet.Write(Port);
		}
		public void Deserialize(Packet packet)
		{
			packet.Read(ref ServerId);
			packet.Read(ref Alive);
			packet.Read(ref UserCount);
			packet.Read(ref Ip);
			packet.Read(ref Port);
		}
	}
}
