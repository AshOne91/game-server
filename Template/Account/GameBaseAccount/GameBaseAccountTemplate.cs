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
		UserObject _obj = null;
		public override void Init(TemplateConfig config)
		{
			base.Init(config);
			//OnLoadData(config)
			// TODO : 서버 기동시 실행할 템플릿 관련 로직을 아래에 작성
		}

		public override void OnLoadData(TemplateConfig config)
		{
			// TODO : 로드할 데이터를 연결
		}

		public override void OnClientCreate(UserObject userObject)
		{
			// TODO : 유저의 최초 생성시 필요한 DB관련 로직을 작성
			_obj = userObject;
		}

		public override void OnClientUpdate(float dt)
		{
			// TODO : 유저의 로그인시 필요한 DB관련 로직을 작성
		}

		public override void OnClientDelete(UserObject userObject)
		{
			// TODO : 계정 초기화시 사용 템플릿에 보유한 내역듣 삭제
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnAddItem(UserObject userObject, int itemId, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) OnDeleteItem(UserObject userObject, int itemId, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) AddRandomReward(UserObject userObject, int classId, int grade, int kind, long value, int parentItemId, int groupIndex)
		{
			return (null, null);
		}

		public override bool OnHasItemId(UserObject userObject, int itemId)
		{
			return false;
		}

		public override bool OnHasItemType(UserObject userObject, int itemType)
		{
			return false;
		}

		public override bool OnHasItemSubType(UserObject userObject, int subType)
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
	}
}
