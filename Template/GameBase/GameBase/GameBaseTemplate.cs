using Service.Net;
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


        public virtual void OnClientCreate(UserObject userObject)
        {
        }


        public virtual void OnClientUpdate(UserObject userObject)
        {
        }

        public virtual void OnClientDelete(string userId)
        {
        }

        public virtual (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnAddItem(UserObject userObject, int itemId, long value, int parentItemId, int groupIndex)
        {
            return (null, null);
        }


        public virtual (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnDeleteItem(UserObject userObject, int itemId, long value, int parentItemId, int groupIndex)
        {
            return (null, null);
        }


        public virtual (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) AddRandomReward(UserObject userObject, int classId, int grade, int kind, long value, int parentItemId, int groupIndex)
        {
            return (null, null);
        }


        public virtual bool OnHasItemId(UserObject userObject, int itemId)
        {
            return false;
        }

        public virtual bool OnHasItemType(UserObject userObject, int itemType)
        {
            return false;
        }

        public virtual bool OnHasItemSubType(UserObject userObject, int subType)
        {
            return false;
        }
    }
}
