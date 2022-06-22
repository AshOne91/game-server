using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public class ImplObject : UserObject
    {
        public BaseImpl GameBaseImpl = null;
        public AccountImpl AccountImpl = null;
        public AdminImpl AdminImpl = null;
        public AdvertImpl AdvertImpl = null;
        public AttendanceImpl AttendanceImpl = null;
        public AuctionImpl AuctionImpl = null;
        public BattleImpl BattleImpl = null;
        public BuildingImpl BuildingImpl = null;
        public CharacterImpl CharacterImpl = null;
        public ItemImpl ItemImpl = null;
        public MailBoxImpl MailBoxImpl = null;
        public MatchingImpl MatchingImpl = null;
        public NoticeImpl NoticeImpl = null;
        public QuestImpl QuestImpl = null;
        public RankImpl RankImpl = null;
        public ReportImpl ReportImpl = null;
        public SeasonImpl SeasonImpl = null;
        public UserImpl UserImpl = null;
        public T GetGameBaseImpl<T>() where T : BaseImpl
        {
            return this.GameBaseImpl as T;
        }
        public T GetAccountImpl<T>() where T : AccountImpl
        {
            return this.AccountImpl as T;
        }
        public T GetAdminImpl<T>() where T : AdminImpl
        {
            return this.AdminImpl as T;
        }
        public T GetAdvertImpl<T>() where T : AdvertImpl
        {
            return this.AdvertImpl as T;
        }
        public T GetAttendanceImpl<T>() where T : AttendanceImpl
        {
            return this.AttendanceImpl as T;
        }
        public T GetAuctionImpl<T>() where T : AuctionImpl
        {
            return this.AuctionImpl as T;
        }
        public T GetBattleImpl<T>() where T : BattleImpl
        {
            return this.BattleImpl as T;
        }
        public T GetBuildingImpl<T>() where T : BuildingImpl
        {
            return this.BuildingImpl as T;
        }
        public T GetCharacterImpl<T>() where T : CharacterImpl
        {
            return this.CharacterImpl as T;
        }
        public T GetItemImpl<T>() where T : ItemImpl
        {
            return this.ItemImpl as T;
        }
        public T GetMailBoxImpl<T>() where T : MailBoxImpl
        {
            return this.MailBoxImpl as T;
        }
        public T GetMatchingImpl<T>() where T : MatchingImpl
        {
            return this.MatchingImpl as T;
        }
        public T GetNoticeImpl<T>() where T : NoticeImpl
        {
            return this.NoticeImpl as T;
        }
        public T GetQuestImpl<T>() where T : QuestImpl
        {
            return this.QuestImpl as T;
        }
        public T GetRankImpl<T>() where T : RankImpl
        {
            return this.RankImpl as T;
        }
        public T GetReportImpl<T>() where T : ReportImpl
        {
            return this.ReportImpl as T;
        }
        public T GetSeasonImpl<T>() where T : SeasonImpl
        {
            return this.SeasonImpl as T;
        }
        public T GetUserImpl<T>() where T : UserImpl
        {
            return this.UserImpl as T;
        }
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public override void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            try
            {
                if (disposing)
                {
                }
            }
            catch
            {

            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
