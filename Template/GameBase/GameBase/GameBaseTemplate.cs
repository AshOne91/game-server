using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Common
{
    public partial class GameBaseTemplate
    {
        public virtual void Init(TemplateConfig config)
        {
        }

        public virtual void OnLoadData(TemplateConfig config)
        {
        }


        /*public virtual void OnClientCreate(IDatabaseClient dbClient, ClientSession clientSession)
        {
        }


        public virtual void OnClientUpdate(IDatabaseClient dbClient, ClientSession clientSession)
        {
        }

        public virtual void OnClientDelete(IDatabaseClient dbClient, string userId)
        {
        }*/

        public virtual void /*(List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam)*/ OnAddItem(/*IDatabaseItem dbItem, int itemId, long value, int parentItemId, int groupIndex*/)
        {
            //return (null, null);
        }


        public virtual void /*(List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam)*/ OnDeleteItem(/*IDatabaseItem dbItem, int itemId, long value, int parentItemId, int groupIndex*/)
        {
            //return (null, null);
        }


        public virtual void /*(List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam)*/ AddRandomReward(/*IDatabaseItem dbItem, int classId, int grade, int kind, long value, int parentItemId, int groupIndex*/)
        {
            //return (null, null);
        }


        public virtual bool OnHasItemId(/*IDatabaseItem dbItem, int itemId*/)
        {
            return false;
        }

        public virtual bool OnHasItemType(/*IDatabaseItem dbItem, ItemType itemType*/)
        {
            return false;
        }

        public virtual bool OnHasItemSubType(/*IDatabaseItem dbItem, EItemSubType subType*/)
        {
            return false;
        }


        /*public RankerInfo GetRankerInfo(string rankKey, long rankNum, long score)
        {
            Dictionary<string, string> valuePairs;
            CacheService.UserHash.GetAll(rankKey, out valuePairs);
            if (valuePairs.ContainsKey("user_name") == false)
            {
                return null;
            }


            RankerInfo rankerInfo = new RankerInfo();
            rankerInfo.rank = rankNum;
            rankerInfo.score = score;
            rankerInfo.userName = valuePairs["user_name"];

            if (valuePairs.ContainsKey("class") == true && string.IsNullOrEmpty(valuePairs["class"]) == false)
            {
                rankerInfo.classId = int.Parse(valuePairs["class"]);
            }

            if (valuePairs.ContainsKey("platform") == true)
            {
                rankerInfo.linkedPlatformInfo = JsonConvert.DeserializeObject<Dictionary<int, string>>(valuePairs["platform"]);
            }

            if (valuePairs.ContainsKey("char_profile") == true)
            {
                rankerInfo.listCharacterProfile = JsonConvert.DeserializeObject<List<CharacterProfile>>(valuePairs["char_profile"]);
            }

            return rankerInfo;
        }*/
    }
}
