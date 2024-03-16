using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Item.GameBaseItem.Common
{
	public class DBItemTable : BaseDBClass
	{
		/// <sumary>
		/// 파티션키_1
		/// <sumary>
		public UInt64 user_db_key;
		/// <sumary>
		/// 파티션키_2
		/// <sumary>
		public UInt64 player_db_key;
		/// <sumary>
		/// 생성시간
		/// <sumary>
		public DateTime create_time;
		/// <sumary>
		/// 업데이트 시간
		/// <sumary>
		public DateTime update_time;
		/// <sumary>
		/// 아이템 타입
		/// <sumary>
		public int item_type;
		/// <sumary>
		/// 아이템 아이디
		/// <sumary>
		public int item_id;
		/// <sumary>
		/// 아이템 카운트
		/// <sumary>
		public long item_count;
		/// <sumary>
		/// 남은 충전 타입
		/// <sumary>
		public DateTime remain_charge_time;
		public DBItemTable() { Reset(); }
		~DBItemTable() { Reset(); }
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
	public class DBSlot_DBItemTable : DBSlot<DBItemTable>{}
	public class DBSlotContainer_DBItemTable : DBSlotContainer<DBSlot_DBItemTable, DBItemTable> {}
}
