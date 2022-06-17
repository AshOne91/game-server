using System;
using System.Collections.Generic;
using System.Text;
using Service.Net;

namespace GameBase.Template.GameBase
{
    public class ImplObject : UserObject
    {
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
