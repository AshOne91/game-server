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

			GameBaseAccountUserImpl Impl = userObject.GetAccountImpl<GameBaseAccountUserImpl>();
			DBGameUserLoad query = new DBGameUserLoad(userObject);
			query._user_db_key = userObject.UserDBKey;
			query._player_db_key = userObject.PlayerDBKey;
			query._encode_account_id = Impl._AuthInfo._encodeAccountId;
			query._gm_level = Impl._AuthInfo._gm_level;
			GameBaseTemplateContext.GetDBManager().PushQueryGame(userObject.UserDBKey, userObject.GameDBIdx, 0, query, () =>
			{
				if (query.IsSuccess())
                {
					if (!UserLoadComplete(Impl, query._userDB))
                    {
						PACKET_CG_PLAYER_SELECT_RES sendPacket = new PACKET_CG_PLAYER_SELECT_RES();
						sendPacket.ErrorCode = (int)GServerCode.PlayerCreateFail;
						_obj.GetSession().SendPacket(sendPacket.Serialize());
					}
                }
				else
                {
					PACKET_CG_PLAYER_SELECT_RES sendPacket = new PACKET_CG_PLAYER_SELECT_RES();
					sendPacket.ErrorCode = (int)GServerCode.DBNotFound;
					_obj.GetSession().SendPacket(sendPacket.Serialize());
				}
			});
		}
		public void ON_CG_PLAYER_SELECT_RES_CALLBACK(ImplObject userObject, PACKET_CG_PLAYER_SELECT_RES packet)
		{
			if (packet.ErrorCode != (int)GServerCode.SUCCESS)
			{
                userObject.ClientCallback("PacketError", packet.ToString());
				return;
			}

            userObject.ClientCallback("SelectPlayer", packet.Player);
		}

		public bool UserLoadComplete(GameBaseAccountUserImpl impl, UserDB srcDB)
        {
			_obj.GetUserDB().Copy(srcDB, false);


			/*
			//�ű� �÷��̾� ����
			CPlayer* pPlayer = CObjectManager::Instance()->CreatePlayer(m_pThis);
			WRITELOG_RETURN_FALSE(pPlayer);

			m_pThis->SetPlayer(pPlayer);

			//DB�κ��� ���� �ű� ������ �� Agent���� �ڷᱸ���� �����.
			if (!m_pThis->OnPlayerSelect_Prepare())
			{
				CObjectManager::Instance()->DestroyObject(pPlayer);
				m_pThis->SetPlayer(nullptr);
				return false;
			}*/

			if (_obj.GetUserDB().GetReadUserDB<GameBaseAccountUserDB>(ETemplateType.Account)._dbBaseContainer_player.GetReadData()._DBData.newbie == true)
			{
				GameBaseTemplateContext.SetNewbie(impl._obj.GetSession().GetUid());
				_obj.GetUserDB().GetWriteUserDB<GameBaseAccountUserDB>(ETemplateType.Account)._dbBaseContainer_player.GetWriteData()._DBData.newbie = false;
			}

			if (!GameBaseTemplateContext.PlayerSelectPrepare(impl._obj.GetSession().GetUid()))
			{
				return false;
			}


			DBGlobal_Player_Login query = new DBGlobal_Player_Login();
			query._account_db_key = impl._obj.AccountDBKey;
			query._user_db_key = impl._obj.UserDBKey;
			query._player_db_key = impl._obj.PlayerDBKey;
			query._server_id = GetGameBaseAccountImpl()._ServerId;
			query._player_name = impl._PlayerInfo.PlayerName;
			query._player_level = 1;
			query._player_class_type = 0;
			GameBaseTemplateContext.GetDBManager().PushQueryGlobal(impl._obj.UserDBKey, query);

			PACKET_CG_PLAYER_SELECT_RES sendPacket = new PACKET_CG_PLAYER_SELECT_RES();
			sendPacket.Player = impl._PlayerInfo;
			impl._obj.GetSession().SendPacket(sendPacket.Serialize());

			return true;
        }
	}
}
