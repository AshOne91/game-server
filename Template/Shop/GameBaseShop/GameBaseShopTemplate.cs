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
	public partial class GameBaseShopTemplate : ShopTemplate
	{
		ImplObject _obj = null;
		static GameBaseShopImpl _Impl = null;

		public override GameBaseUserDB CreateUserDB()
		{
			return base.CreateUserDB();
		}

		public override void Init(TemplateConfig config, ServerType type)
		{
			base.Init(config, type);
			_Impl = new GameBaseShopImpl(type);
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
			switch ((ObjectType)userObject.ObjectID)
			{
				case ObjectType.Master:
					{
						_obj.ShopImpl = new GameBaseShopMasterImpl(_obj);
					}
					break;
				case ObjectType.User:
					{
						_obj.ShopImpl = new GameBaseShopUserImpl(_obj);
					}
					break;
				case ObjectType.Login:
					{
						_obj.ShopImpl = new GameBaseShopLoginImpl(_obj);
					}
					break;
				case ObjectType.Game:
					{
						_obj.ShopImpl = new GameBaseShopGameImpl(_obj);
					}
					break;
				case ObjectType.Client:
					{
						_obj.ShopImpl = new GameBaseShopClientImpl(_obj);
					}
					break;
			}
			// TODO : 유저의 최초 생성시 필요한 DB관련 로직을 작성
		}

		public override void OnSetNewbie(ImplObject userObject)
		{
		}

		public override bool OnPlayerSelectPrepare(ImplObject userObject)
		{
			return true;
		}

		public T GetGameBaseShopImpl<T>() where T : ShopImpl
		{
			return _obj.ShopImpl as T;
		}

		public static GameBaseShopImpl GetGameBaseShopImpl()
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
