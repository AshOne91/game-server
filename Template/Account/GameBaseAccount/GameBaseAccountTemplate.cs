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

		public override void OnClientCreate(ImplObject userObject)
		{
			// TODO : 유저의 최초 생성시 필요한 DB관련 로직을 작성
			_obj = userObject;
			_obj.AccountImpl = new AccountImpl(_obj);
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

		public void SetGameServerInfo(List<GameServerInfo> gameServerInfoList)
        {
			_GameServerInfoList = gameServerInfoList;
		}

		public GameServerInfo GetGameServerInfo(List<int> wantedServerIds)
        {
			if (_ForceGuidIdx > -1)
            {
				wantedServerIds.Insert(0, _ForceGuidIdx);
            }

			foreach (var serverId in wantedServerIds)
            {
				if (serverId == -1) continue;

				foreach (var info in _GameServerInfoList)
                {
					if (info.Alive == true && info.ServerId == serverId)
                    {
						return info;
                    }
                }
            }

			// 최소 인원이 안 채워진 채널 부터 채움
			foreach (var info in _GameServerInfoList)
            {
				if (info.Alive == true && info.UserCount < _MinGameServerUserCount)
                {
					return info;
                }
            }

			// 최소인원이 전부 다 채워져있다면 최대인원이 안채워진 채널로 채움
			foreach (var info in _GameServerInfoList)
            {
				if (info.Alive == true && info.UserCount < _MaxGameServerUserCount)
                {
					return info;
                }
            }

			return null;
        }

		//AuthServer
		private static List<GameServerInfo> _GameServerInfoList = new List<GameServerInfo>();
		private static int _ForceGuidIdx = -1;
		private static int _MinGameServerUserCount = 100;
		private static int _MaxGameServerUserCount = 500;
	}
}
