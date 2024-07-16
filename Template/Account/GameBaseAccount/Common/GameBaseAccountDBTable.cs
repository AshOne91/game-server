using System;
using System.Collections.Generic;
using Service.Net;
using Service.Core;
using Service.DB;
using GameBase.Template.GameBase;

namespace GameBase.Template.Account.GameBaseAccount.Common
{
	public class player : BaseDBClass
	{
		/// <sumary>
		/// 파티션키_1
		/// <sumary>
		public UInt64 player_db_key;
		/// <sumary>
		/// 파티션키_2
		/// <sumary>
		public UInt64 user_db_key;
		/// <sumary>
		/// 생성시간
		/// <sumary>
		public DateTime create_time;
		/// <sumary>
		/// 업데이트 시간
		/// <sumary>
		public DateTime update_time;
		/// <sumary>
		/// 로그인 시간
		/// <sumary>
		public DateTime login_time;
		/// <sumary>
		/// 로그아웃 시간
		/// <sumary>
		public DateTime logout_time;
		/// <sumary>
		/// 접속여부
		/// <sumary>
		public bool is_login;
		/// <sumary>
		/// 첫생성자 여부
		/// <sumary>
		public bool newbie;
		/// <sumary>
		/// 시리얼번호 생성기
		/// <sumary>
		public Int64 serial_allocator;
		/// <sumary>
		/// 플레이어 이름
		/// <sumary>
		public string player_name;
		/// <sumary>
		/// 레벨
		/// <sumary>
		public short level;
		/// <sumary>
		/// 경험치
		/// <sumary>
		public Int64 exp;
		public player() { Reset(); }
		~player() { Reset(); }
		public override void Reset()
		{
			player_db_key = default(UInt64);
			user_db_key = default(UInt64);
			create_time = DateTime.UtcNow;
			update_time = DateTime.UtcNow;
			login_time = default(DateTime);
			logout_time = default(DateTime);
			is_login = default(bool);
			newbie = default(bool);
			serial_allocator = default(Int64);
			player_name = string.Empty;
			level = default(short);
			exp = default(Int64);
		}
		public override void Copy(BaseDBClass srcDBData)
		{
			player srcplayer = (player)srcDBData;
			player_db_key = srcplayer.player_db_key;
			user_db_key = srcplayer.user_db_key;
			create_time = srcplayer.create_time;
			update_time = srcplayer.update_time;
			login_time = srcplayer.login_time;
			logout_time = srcplayer.logout_time;
			is_login = srcplayer.is_login;
			newbie = srcplayer.newbie;
			serial_allocator = srcplayer.serial_allocator;
			player_name = srcplayer.player_name;
			level = srcplayer.level;
			exp = srcplayer.exp;
		}
	}
	public class DBBase_player : DBBase<player>{}
	public class DBBaseContainer_player : DBBaseContainer<DBBase_player, player>{}
}
