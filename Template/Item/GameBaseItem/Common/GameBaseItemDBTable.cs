using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;

namespace GameBase.Template.Item.GameBaseItem.Common
{
    public class DBItemTable : BaseDBClass
    {
        public UInt64 user_db_key;
        public UInt64 player_db_key;
        public DateTime create_time;
        public DateTime update_time;
        public int item_type;
        public int item_id;
        public long item_count;
        public DateTime remain_charge_time;

        public DBItemTable() { Reset(); }
        public override void Reset()
        {
            user_db_key = default(UInt64);
            player_db_key = default(UInt64);
            create_time = DateTime.UtcNow;
            update_time = DateTime.UtcNow;
            item_type = default(int);
            item_id = default(int);
            item_count = default(long);
            remain_charge_time = default(DateTime);
        }
    }
    public class DBSlot_DBItemTable : DBSlot<DBItemTable> { }
    public class DBSlotContainer_DBItemTable : DBSlotContainer<DBSlot_DBItemTable> { }
}
