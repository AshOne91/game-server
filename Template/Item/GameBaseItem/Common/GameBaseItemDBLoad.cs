using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;

namespace GameBase.Template.Item.GameBaseItem.Common
{
    public class GameBaseItemDBLoad : ItemDBLoad
    {
        private void _Run_LoadUser_DBItem(AdoDB adoDB, UInt64 user_db_key, UInt64 player_db_key, DBSlotContainer_DBItemTable container)
        {
            try
            {
                QueryBuilder query = new QueryBuilder("call gp_player_dbitemtable_load");

                query.SetInputParam("@p_user_db_key", user_db_key);
                query.SetInputParam("@p_player_db_key", player_db_key);

                adoDB.Execute(query);

                while (adoDB.RecordWhileNotEOF())
                {
                    short nSlot = adoDB.RecordGetValue("slot");

                    DBSlot_DBItemTable slot = container.Insert(nSlot, false);

                    slot._isDeleted = adoDB.RecordGetValue("deleted");
                    slot._DBData.user_db_key = adoDB.RecordGetValue("")
                }

            }
            catch (Exception e)
            {
                throw new Exception("[gp_player_dbitemtable_load]" + e.Message);
            }
        }
    }
}
