using GameBase.Template.GameBase.Common;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
{
    public partial class GameBaseTemplate
    {
        static BaseImpl _baseImpl = null;
        static GameBaseTemplate()
        {
            _baseImpl = new BaseImpl(ServerType.None);
        }
        public virtual void Init(TemplateConfig config, ServerType type)
        {

        }

        public virtual void OnLoadData(TemplateConfig config)
        {
        }


        public virtual void OnClientCreate(ImplObject userObject)
        {
        }

        public virtual void OnTemplateUpdate(float dt)
        {
        }

        public virtual void OnClientUpdate(float dt)
        {
        }

        public virtual void OnClientDelete(ImplObject userObject)
        {
        }

        public virtual (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnAddItem(ImplObject userObject, int itemId, long value, int parentItemId, int groupIndex)
        {
            return (null, null);
        }


        public virtual (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnDeleteItem(ImplObject userObject, int itemId, long value, int parentItemId, int groupIndex)
        {
            return (null, null);
        }


        public virtual (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) AddRandomReward(ImplObject userObject, int classId, int grade, int kind, long value, int parentItemId, int groupIndex)
        {
            return (null, null);
        }


        public virtual bool OnHasItemId(ImplObject userObject, int itemId)
        {
            return false;
        }

        public virtual bool OnHasItemType(ImplObject userObject, int itemType)
        {
            return false;
        }

        public virtual bool OnHasItemSubType(ImplObject userObject, int subType)
        {
            return false;
        }
    }
}
