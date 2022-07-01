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
		public void ON_CG_CREATE_PLAYER_REQ_CALLBACK(ImplObject userObject, PACKET_CG_CREATE_PLAYER_REQ packet)
		{
			//����üũ �߰�

			//�÷��̾� �̸� üũ

			//��ŸƮ ���Ǳ�

			//����ġ ����

			//��ȿ�� üũ ������ �̸� �ؾߵ�

			DBGlobal_Get_PlayerDBKey query = new DBGlobal_Get_PlayerDBKey(userObject);
			query._user_db_key = userObject.GetUserDBKey();
			query._player_name = packet.PlayerName;
			query._server_id = GetGameBaseAccountImpl()._ServerId;
			GameBaseTemplateContext.GetDBManager().PushQueryGlobal(query.GetCallerUserDBKey(), query, () =>
			{
				PACKET_CG_CREATE_PLAYER_RES sendPacket = new PACKET_CG_CREATE_PLAYER_RES();
				if (!query.IsSuccess()) //Ʈ����� ����
                {
					sendPacket.ErrorCode = (int)GServerCode.DuplicateName;
					_obj.GetSession().SendPacket(sendPacket.Serialize());
					return;
                }
				else if (query._player_db_key == 0) //�̸� �ߺ�
                {
					sendPacket.ErrorCode = (int)GServerCode.DuplicateName;
					_obj.GetSession().SendPacket(sendPacket.Serialize());
				}
				PlayerCreate_Complete(_obj, query._player_name, query._player_db_key, 1, 0);
			});
		}
		public void ON_CG_CREATE_PLAYER_RES_CALLBACK(ImplObject userObject, PACKET_CG_CREATE_PLAYER_RES packet)
		{
		}

		public void PlayerCreate_Complete(UserObject obj, string playerName, ulong playerDBKey, short playerLevel, ulong playerExp)
        {
			DBGame_Player_Create query = new DBGame_Player_Create(_obj);
			query._max_player_count = 1;//�ϴ� �Ѹ�����
			query._player_db_key = playerDBKey;
			query._player_name = playerName;
			query._player_level = playerLevel;
			query._player_exp = PlayerExp;

		}
	}
}
