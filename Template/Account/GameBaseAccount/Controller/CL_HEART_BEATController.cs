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
		public void ON_CL_HEART_BEAT_REQ_CALLBACK(ImplObject userObject, PACKET_CL_HEART_BEAT_REQ packet)
		{
			PACKET_CL_CHECK_AUTH_RES sendPacket = new PACKET_CL_CHECK_AUTH_RES();
			userObject.GetSession().SendPacket(sendPacket.Serialize());

			_LastHeartBeatTick = Environment.TickCount;
		}
		public void ON_CL_HEART_BEAT_RES_CALLBACK(ImplObject userObject, PACKET_CL_HEART_BEAT_RES packet)
		{
		}

		//LoginServer
		int _LastHeartBeatTick = Environment.TickCount;
	}
}
