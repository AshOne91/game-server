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
		public void ON_LM_CHECK_AUTH_REQ_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_REQ packet)
		{
		}
		public void ON_LM_CHECK_AUTH_RES_CALLBACK(UserObject userObject, PACKET_LM_CHECK_AUTH_RES packet)
		{
		}
	}
}