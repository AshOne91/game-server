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
		public void ON_LM_HELLO_HEART_BEAT_REQ_CALLBACK(ImplObject userObject, PACKET_LM_HELLO_HEART_BEAT_REQ packet)
		{
		}
		public void ON_LM_HELLO_HEART_BEAT_RES_CALLBACK(ImplObject userObject, PACKET_LM_HELLO_HEART_BEAT_RES packet)
		{
		}
	}
}
