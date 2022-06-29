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
		public void ON_CG_CHECK_AUTH_REQ_CALLBACK(ImplObject userObject, PACKET_CG_CHECK_AUTH_REQ packet)
		{
			//GameServer
			//UserObject
			if (packet.ProtocolGUID != PacketDefine.PROTOCOL_GUID)
            {
				Logger.Default.Log(ELogLevel.Err, "Invalid GUID Recv {0} : {1}", packet.ProtocolGUID, PacketDefine.PROTOCOL_GUID);

				PACKET_CG_CHECK_AUTH_RES sendData = new PACKET_CG_CHECK_AUTH_RES();
				sendData.ErrorCode = (int)GServerCode.INVALID_GUID;
				userObject.GetSession().SendPacket(sendData.Serialize());
				return;
			}

			string id = "";
			string extra = "";
			bool result = Passport.Vertify(packet.Passport, out id, out extra);
			if (result == false)
            {
				Logger.Default.Log(ELogLevel.Err, "Failed Passport.Vertify {0}", packet.Passport);
				userObject.GetSession().Disconnect();
				return;
            }

			if (id == string.Empty)
            {
				PACKET_CG_CHECK_AUTH_RES sendData = new PACKET_CG_CHECK_AUTH_RES();
				sendData.ErrorCode = (int)GServerCode.INVALID_SITE_USER_ID;
				userObject.GetSession().SendPacket(sendData.Serialize());
				return;
			}

			GameBaseAccountUserImpl userImpl = userObject.GetAccountImpl<GameBaseAccountUserImpl>();
			userImpl._PassportExtra = extra.Split(";");

			if (userImpl._PassportExtra.Length != 5)
            {
				Logger.Default.Log(ELogLevel.Err, "Passport ExtraValue Verify Failed : {0}", userImpl._PassportExtra);
				userObject.GetSession().Disconnect();
				return;
			}

			int passportServerId = int.Parse(userImpl._PassportExtra[3]);
			int platformType = int.Parse(userImpl._PassportExtra[4]);
			if (passportServerId == -1 || passportServerId != GetGameBaseAccountImpl()._ServerId)
            {
				Logger.Default.Log(ELogLevel.Err, "Passport Not Allowed for This Server : {0} != {1}", passportServerId, GetGameBaseAccountImpl()._ServerId);
				userObject.GetSession().Disconnect();
				return;
			}

			DBGlobal_PlatformAuth query = new DBGlobal_PlatformAuth(userObject);
			query._platform_type = platformType;
			query._site_user_id = id;
			GameBaseTemplateContext.GetDBManager().PushQueryGlobal(id, query, () => {
				if (query.IsSuccess())
                {

					Login_DBAuth_Complete(userImpl
						, query._account_db_key
						, query._encode_account_id
						, query._account_status
						, query._block_endtime
						, query._is_withdraw
						, query._withdraw_time
						, query._withdraw_cancel_count
						, query._is_google_link
						, query._is_apple_link
						, query._is_facebook_link);
				}
				else
                {
					PACKET_CG_CHECK_AUTH_RES sendPacket = new PACKET_CG_CHECK_AUTH_RES();
					sendPacket.ErrorCode = (int)GServerCode.DBError;
					userObject.GetSession().SendPacket(sendPacket.Serialize());
				}
			});
		}
		public void ON_CG_CHECK_AUTH_RES_CALLBACK(ImplObject userObject, PACKET_CG_CHECK_AUTH_RES packet)
		{
		}

		public void Login_DBAuth_Complete(GameBaseAccountUserImpl Impl
			, ulong accountDBKey
			, string encodeAccountId
			, string accountStatus
			, DateTime blockEndTime
			, bool isWithdraw
			, DateTime withdrawTime
			, int withdrawCancelCount
			, bool isGoogleLink
			, bool isAppleLink
			, bool isFacebookLink)
        {
			PACKET_CG_CHECK_AUTH_RES sendPacket = new PACKET_CG_CHECK_AUTH_RES();
			if (accountStatus == "Withdraw" || isWithdraw == true)
            {
				sendPacket.ErrorCode = (int)GServerCode.Withdraw;
				Impl._obj.GetSession().SendPacket(sendPacket.Serialize());
			}
			else if (accountStatus == "EternalBlock")
            {
				sendPacket.ErrorCode = (int)GServerCode.EternalBlock;
				Impl._obj.GetSession().SendPacket(sendPacket.Serialize());
			}
			else if (accountStatus == "PeriodBlock")
            {
				sendPacket.ErrorCode = (int)GServerCode.PeriodBlock;
				Impl._obj.GetSession().SendPacket(sendPacket.Serialize());
			}
			else if (accountStatus == "TempBlock")
            {
				sendPacket.ErrorCode = (int)GServerCode.TempBlock;
				Impl._obj.GetSession().SendPacket(sendPacket.Serialize());
			}
			else if (accountStatus == "LongTimeBlock")
            {
				sendPacket.ErrorCode = (int)GServerCode.LongTimeBlock;
				Impl._obj.GetSession().SendPacket(sendPacket.Serialize());
			}
			else
            {
				Impl._obj.SetAccountDBKey(accountDBKey);

				Impl._AuthInfo._accountDBKey = accountDBKey;
				Impl._AuthInfo._encodeAccountId = encodeAccountId;
				Impl._AuthInfo._isGoogleLink = isGoogleLink;
				Impl._AuthInfo._isAppleLink = isAppleLink;
				Impl._AuthInfo._isFacebook = isFacebookLink;

				sendPacket.
			}
		}
	}
}
