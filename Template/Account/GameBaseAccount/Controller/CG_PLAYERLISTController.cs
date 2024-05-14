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
		public void ON_CG_PLAYERLIST_REQ_CALLBACK(ImplObject userObject, PACKET_CG_PLAYERLIST_REQ packet)
		{
			GameBaseAccountUserImpl Impl = userObject.GetAccountImpl<GameBaseAccountUserImpl>();
			DBGame_PlayerList_LoadAll query = new DBGame_PlayerList_LoadAll(userObject);
			query._encode_account_id = Impl._AuthInfo._encodeAccountId;
			query._gm_level = Impl._AuthInfo._gm_level;
			GameBaseTemplateContext.GetDBManager().PushQueryGame(userObject.UserDBKey, userObject.GameDBIdx, 0, query, () =>
			{
				PACKET_CG_PLAYERLIST_RES sendPacket = new PACKET_CG_PLAYERLIST_RES();
				if (query.IsSuccess())
                {
					sendPacket.PlayerInfoList = query._playerInfos;
					userObject.GetSession().SendPacket(sendPacket.Serialize());
				}
				else
                {
					sendPacket.ErrorCode = (int)GServerCode.DBError;
					userObject.GetSession().SendPacket(sendPacket.Serialize());
				}
			});
		}
		public void ON_CG_PLAYERLIST_RES_CALLBACK(ImplObject userObject, PACKET_CG_PLAYERLIST_RES packet)
		{
			if (packet.ErrorCode != (int)GServerCode.SUCCESS)
			{
                userObject.ClientCallback("PacketError", packet.ToString());
				return;
            }
            userObject.ClientCallback("PlayerList", packet.PlayerInfoList);
		}
	}
}
