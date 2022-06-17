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
		public void ON_GM_CHECK_AUTH_REQ_CALLBACK(ImplObject userObject, PACKET_GM_CHECK_AUTH_REQ packet)
		{
		}
		public void ON_GM_CHECK_AUTH_RES_CALLBACK(ImplObject userObject, PACKET_GM_CHECK_AUTH_RES packet)
		{
		}
	}
}
