using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Item.GameBaseItem.Common
{
    public class GameBaseItemUserDB : ItemUserDB
    {
        public DBSlotContainer_DBItemTable _dbSlotContainer_DBItemTable = new DBSlotContainer_DBItemTable();

        void override ItemCopy(UserDB userSrc, bool isChanged)
        {
            GameBaseItemUserDB _ItemUserDB = userSrc._ItemUserDB;
            _dbSlotContainer_DBItemTable.Copy(_ItemUserDB._dbSlotContainer_DBItemTable, isChanged);
        }
    }
}
