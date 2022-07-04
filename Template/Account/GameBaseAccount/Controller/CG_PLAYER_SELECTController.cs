#define SERVER
using System;
using System.Collections.Generic;
using Service.Core;
using Service.Net;
using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using GameBase.Template.Account.GameBaseAccount.Common;

namespace GameBase.Template.Account.GameBaseAccount
{
	public partial class GameBaseAccountTemplate
	{
		public void ON_CG_PLAYER_SELECT_REQ_CALLBACK(ImplObject userObject, PACKET_CG_PLAYER_SELECT_REQ packet)
		{
			//����üũ �κ����� ���

			//ȯ���������� üũ

			//���� ��ü ���� �ε� ��!!! FIXME
			DBGameUserLoad query = new DBGameUserLoad(userObject);
			query._partitionKey_1 = userObject.GetUserDBKey();
			query._partitionKey_2 = userObject.GetPlayerDBKey();
			query._encode_account_id = userObject.GetAccountImpl<GameBaseAccountUserImpl>()._AuthInfo._encodeAccountId;
			query._gm_level = userObject.GetAccountImpl<GameBaseAccountUserImpl>()._AuthInfo._gm_level;
			GameBaseTemplateContext.GetDBManager().PushQueryGame(userObject.GetUserDBKey(), userObject.GetGameDBIdx(), 0, query, () =>
			{

			});
		}
		public void ON_CG_PLAYER_SELECT_RES_CALLBACK(ImplObject userObject, PACKET_CG_PLAYER_SELECT_RES packet)
		{
		}
	}
}
