using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;

namespace GameBase.Template.Account.GameBaseAccount
{
	public partial class GameBaseAccountTemplate : AccountTemplate
	{
		ImplObject _obj = null;
		static GameBaseAccountImpl _accountImpl = null; 
		public override void Init(TemplateConfig config, ServerType type)
		{
			base.Init(config, type);
			_accountImpl = new GameBaseAccountImpl(type);
			//OnLoadData(config)
			// TODO : 서버 기동시 실행할 템플릿 관련 로직을 아래에 작성
		}

		public override void OnLoadData(TemplateConfig config)
		{
			// TODO : 로드할 데이터를 연결
		}

		public override void OnClientCreate(ImplObject userObject)
		{
			// TODO : 유저의 최초 생성시 필요한 DB관련 로직을 작성
			_obj = userObject;
			switch ((ObjectType)userObject.ObjectID)
			{
				case ObjectType.Master:
					{
						_obj.AccountImpl = new GameBaseAccountMasterImpl(_obj);
					}
					break;
				case ObjectType.User:
					{
						_obj.AccountImpl = new GameBaseAccountUserImpl(_obj);
					}
					break;
				case ObjectType.Login:
					{
						_obj.AccountImpl = new GameBaseAccountLoginImpl(_obj);
					}
					break;
				case ObjectType.Game:
					{
						_obj.AccountImpl = new GameBaseAccountGameImpl(_obj);
					}
					break;
				case ObjectType.Client:
					{
                        _obj.AccountImpl = new GameBaseAccountClientImpl(_obj);
                    }
					break;
			}
		}

		public T GetGameBaseAccountImpl<T>() where T : AccountImpl
		{
			return _obj.AccountImpl as T;
		}

		public static GameBaseAccountImpl GetGameBaseAccountImpl()
        {
			return _accountImpl;
		}

        public override void OnTemplateUpdate(float dt)
        {
			_accountImpl.Update(dt);
        }

        public override void OnClientUpdate(float dt)
		{
			// TODO : 유저의 로그인시 필요한 DB관련 로직을 작성
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

		public void LC_HELLO_NOTI()
        {
			PACKET_LC_HELLO_NOTI packet = new PACKET_LC_HELLO_NOTI();
			_obj.GetSession().SendPacket(packet.Serialize());
		}

		public void ML_HELLO_NOTI()
        {
			PACKET_ML_HELLO_NOTI packet = new PACKET_ML_HELLO_NOTI();
			_obj.GetSession().SendPacket(packet.Serialize());
        }

		public void MG_HELLO_NOTI()
        {
			PACKET_MG_HELLO_NOTI packet = new PACKET_MG_HELLO_NOTI();
			_obj.GetSession().SendPacket(packet.Serialize());
        }

		public void GC_HELLO_NOTI()
		{
			PACKET_GC_HELLO_NOTI packet = new PACKET_GC_HELLO_NOTI();
			_obj.GetSession().SendPacket(packet.Serialize());
		}
	}
}
