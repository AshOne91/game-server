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

			//상태체크 추가

			//플레이어 이름 체크

			//스타트 컨피그

			//경험치 셋팅

			//유효성 체크 위에서 미리 해야됨

			DBGlobal_Get_PlayerDBKey query = new DBGlobal_Get_PlayerDBKey(userObject);
			query._user_db_key = userObject.UserDBKey;
			query._player_name = packet.PlayerName;
			query._server_id = GetGameBaseAccountImpl()._ServerId;
			GameBaseTemplateContext.GetDBManager().PushQueryGlobal(query.GetCallerUserDBKey(), query, () =>
			{
				PACKET_CG_CREATE_PLAYER_RES sendPacket = new PACKET_CG_CREATE_PLAYER_RES();
				if (!query.IsSuccess()) //트랜잭션 실패
				{
					sendPacket.ErrorCode = (int)GServerCode.DuplicateName;
					_obj.GetSession().SendPacket(sendPacket.Serialize());
					return;
				}
				else if (query._player_db_key == 0) //이름 중복
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
			query._max_player_count = 1;//일단 한명으로
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
