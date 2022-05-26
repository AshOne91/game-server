using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

//임시 해당파일 교채 필요
namespace GameBase.Common
{
    [Serializable]
    public class ItemBaseInfo
    {
        /// <summary>
        /// 부모 아이템 아이디
        /// </summary>
        public int ParentItemId;
        /// <summary>
        /// 아이템 그룹 인덱스(아이템이 속한 그룹을 구분짓기 위한 인덱스값)
        /// </summary>
        public int GroupIndex;
        /// <summary>
        /// 아이템 타입
        /// </summary>
        public int ItemType;
        /// <summary>
        /// 아이템 아이디
        /// </summary>
        public int ItemId;
        /// <summary>
        /// 아이템 레벨
        /// </summary>
        public int ItemLevel;
        /// <summary>
        /// 아이템 업데이트 수량
        /// </summary>
        public long Value;
        /// <summary>
        /// 아이템 총수량
        /// </summary>
        public long TotalValue;
        /// <summary>
        /// 남은 시간(초단위, -1: 무제한)
        /// </summary>
        public int RemainTime;

        public void BinarySerialize(BinaryWriter bw)
        {
        }

        public static ItemBaseInfo BinaryDeserialize(BinaryReader br)
        {
            var data = new ItemBaseInfo();
            return data;
        }

        public string JsonSerialize()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }

        public static ItemBaseInfo JsonDeserialize(string json)
        {
            return JsonConvert.DeserializeObject<ItemBaseInfo>(json);
        }
    }

    [Serializable]
    public class QuestData
    {
        /// <summary>
        /// 퀘스트 데이터 아이디
        /// </summary>
        public int QuestId;
        /// <summary>
        /// 퀘스트 수치
        /// </summary>
        public long Value;
        /// <summary>
        /// 퀘스트 상태
        /// </summary>
        public byte Status;

        public void BinarySerialize(BinaryWriter bw)
        {
        }

        public static QuestData BinaryDeserialize(BinaryReader br)
        {
            var data = new QuestData();
            return data;
        }

        public string JsonSerialize()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }

        public static QuestData JsonDeserialize(string json)
        {
            return JsonConvert.DeserializeObject<QuestData>(json);
        }
    }

    [Serializable]
    public class QuestCompleteParam
    {
        /// <summary>
        /// 퀘스트 완료 타입
        /// </summary>
        public int QuestType;
        /// <summary>
        /// 퀘스트 완료 아이디
        /// </summary>
        public int EndId;
        /// <summary>
        /// 퀘스트 수치
        /// </summary>
        public long Value;

        public void BinarySerialize(BinaryWriter bw)
        {
        }

        public static QuestCompleteParam BinaryDeserialize(BinaryReader br)
        {
            var data = new QuestCompleteParam();
            return data;
        }

        public string JsonSerialize()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }

        public static QuestCompleteParam JsonDeserialize(string json)
        {
            return JsonConvert.DeserializeObject<QuestCompleteParam>(json);
        }
    }
}
