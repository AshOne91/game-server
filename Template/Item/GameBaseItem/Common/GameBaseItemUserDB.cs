using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Item.GameBaseItem.Common
{
    public partial class GameBaseItemUserDB : GameBaseUserDB
    {
        public DBSlotContainer_DBItemTable _dbSlotContainer_DBItemTable = new DBSlotContainer_DBItemTable();

        public override void Copy(UserDB userSrc, bool isChanged)
        {
            GameBaseItemUserDB userDB = userSrc.GetUserDB<GameBaseItemUserDB>(ETemplateType.Item);
            _dbSlotContainer_DBItemTable.Copy(userDB._dbSlotContainer_DBItemTable, isChanged);
        }
    }
}
