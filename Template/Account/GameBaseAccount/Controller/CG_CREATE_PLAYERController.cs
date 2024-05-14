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
			//FIXME

			//����üũ �߰�

			//�÷��̾� �̸� üũ

			//��ŸƮ ���Ǳ�

			//����ġ ����

			//��ȿ�� üũ ������ �̸� �ؾߵ�

			DBGlobal_Get_PlayerDBKey query = new DBGlobal_Get_PlayerDBKey(userObject);
			query._user_db_key = userObject.UserDBKey;
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
				else
				{
					PlayerCreate_Complete(_obj, query._player_name, query._player_db_key, 1, 0);
				}
			});
		}
		public void ON_CG_CREATE_PLAYER_RES_CALLBACK(ImplObject userObject, PACKET_CG_CREATE_PLAYER_RES packet)
		{
            if (packet.ErrorCode != (int)GServerCode.SUCCESS)
			{
				userObject.ClientCallback("PacketError", packet.ToString());
				return;
            }

            userObject.ClientCallback("CreatePlayer", packet.Player);
        }

		public void PlayerCreate_Complete(UserObject obj, string playerName, ulong playerDBKey, short playerLevel, long playerExp)
        {
			DBGame_Player_Create query = new DBGame_Player_Create(_obj);
			query._max_player_count = 1;//�ϴ� �Ѹ�����
			query._player_db_key = playerDBKey;
			query._player_name = playerName;
			query._player_level = playerLevel;
			query._player_exp = playerExp;
			GameBaseTemplateContext.GetDBManager().PushQueryGame(obj.UserDBKey, obj.GameDBIdx, 0, query, () =>
			{
				PACKET_CG_CREATE_PLAYER_RES sendPacket = new PACKET_CG_CREATE_PLAYER_RES();
				GServerCode result = GServerCode.DBNotFound;
				if (query.IsSuccess())
				{
					if (query._result == DBGame_Player_Create.EResult.Success)
					{
						result = GServerCode.SUCCESS;
					}
					else if (query._result == DBGame_Player_Create.EResult.MaxCountOver)
                    {
						result = GServerCode.PlayerMaxCount;
                    }
					else if (query._result == DBGame_Player_Create.EResult.DuplicateName)
                    {
						result = GServerCode.DuplicateName;
                    }
					else if (query._result == DBGame_Player_Create.EResult.DuplicatePlayerDBKey)
                    {
						result = GServerCode.PlayerDBKeyInvalid;
                    }
				}

				if (result == GServerCode.SUCCESS)
                {
					sendPacket.Player = query._player;
					obj.GetSession().SendPacket(sendPacket.Serialize());
				}
				else
                {
					obj.GetSession().SendPacket(sendPacket.Serialize());
                }
			});

		}
	}
}
