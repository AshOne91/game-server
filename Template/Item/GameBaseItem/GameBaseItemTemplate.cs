#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Item.GameBaseItem.Common;

namespace GameBase.Template.Item.GameBaseItem
{
	public partial class GameBaseItemTemplate : ItemTemplate
	{
		ImplObject _obj = null;
		static GameBaseItemImpl _Impl = null;
		public override void Init(TemplateConfig config, ServerType type)
		{
			base.Init(config, type);
			_Impl = new GameBaseItemImpl(type);
			//OnLoadData(config)
			// TODO : 서버 기동시 실행할 템플릿 관련 로직을 아래에 작성
		}

		public override void OnLoadData(TemplateConfig config)
		{
			// TODO : 로드할 데이터를 연결
		}

		public override void OnClientCreate(ImplObject userObject)
		{
			_obj = userObject;
			switch ((ObjectType)userObject.GetObjectID())
			{
				case ObjectType.Master:
					{
						_obj.ItemImpl = new GameBaseItemMasterImpl(_obj);
					}
					break;
				case ObjectType.User:
					{
						_obj.ItemImpl = new GameBaseItemUserImpl(_obj);
					}
					break;
				case ObjectType.Login:
					{
						_obj.ItemImpl = new GameBaseItemLoginImpl(_obj);
					}
					break;
				case ObjectType.Game:
					{
						_obj.ItemImpl = new GameBaseItemGameImpl(_obj);
					}
					break;
			}
			// TODO : 유저의 최초 생성시 필요한 DB관련 로직을 작성
		}

		public T GetGameBaseItemImpl<T>() where T : ItemImpl
		{
			return _obj.ItemImpl as T;
		}

		public static GameBaseItemImpl GetGameBaseItemImpl()
		{
			return _Impl;
		}

		public override void OnTemplateUpdate(float dt)
		{
			// TODO : 템플릿 업데이트 사항 작성(유저X)
		}

		public override void OnClientUpdate(float dt)
		{
			// TODO : 템플릿 업데이트 사항 작성
		}

		public override void OnClientDelete(ImplObject userObject)
		{
			// TODO : 계정 초기화시 사용 템플릿에 보유한 내역듣 삭제
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnAddItem(ImplObject userObject, int itemId, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnDeleteItem(ImplObject userObject, int itemId, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) AddRandomReward(ImplObject userObject, int classId, int grade, int kind, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override bool OnHasItemId(ImplObject userObject, int itemId)
		{
			return false;
		}

		public override bool OnHasItemType(ImplObject userObject, int itemType)
		{
			return false;
		}

		public override bool OnHasItemSubType(ImplObject userObject, int subType)
		{
			return false;
		}
	}
}
