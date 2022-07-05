using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;

namespace GameBase.Template.Item.GameBaseItem.Common
{
    public class DBItemTable : BaseDBClass
    {
        public UInt64 _user_db_key;
        public UInt64 _player_db_key;
        public DateTime _create_time;
        public DateTime _update_time;
        public int _item_type;
        public int _item_id;
        public long _item_count;
        public DateTime _remain_charge_time;

        public DBItemTable() { Reset(); }
        public override void Reset()
        {
            _user_db_key = default(UInt64);
            _player_db_key = default(UInt64);
            _create_time = DateTime.UtcNow;
            _update_time = DateTime.UtcNow;
            _item_type = default(int);
            _item_id = default(int);
            _item_count = default(long);
            _remain_charge_time = default(DateTime);
        }
    }
    public class DBSlot_DBItemTable : DBSlot<DBItemTable> { }
    public class DBSlotContainer_DBItemTable : DBSlotContainer<DBSlot_DBItemTable> { }
}
