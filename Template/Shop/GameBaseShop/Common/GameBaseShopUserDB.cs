using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Shop.GameBaseShop.Common
{
	public partial class GameBaseShopUserDB : GameBaseUserDB
	{
		public DBSlotContainer_DBShopTable _dbSlotContainer_DBShopTable = new DBSlotContainer_DBShopTable();

		public override void Copy(UserDB userSrc, bool isChanged)
		{
			GameBaseShopUserDB userDB = userSrc.GetReadUserDB<GameBaseShopUserDB>(ETemplateType.Shop);
			_dbSlotContainer_DBShopTable.Copy(userDB._dbSlotContainer_DBShopTable, isChanged);
		}
	}
}
