using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Base
{
    public partial class UserDB
    {
        public bool _IsLoaded = false;
        public void Copy(UserDB userSrc, bool isChanged)
        {
            AccountCopy(userSrc, isChanged);
            AdvertCopy(userSrc, isChanged);
            AttendanceCopy(userSrc, isChanged);
            AuctionCopy(userSrc, isChanged);
            BattleCopy(userSrc, isChanged);
            BuildingCopy(userSrc, isChanged);
            CharacterCopy(userSrc, isChanged);
            InternalCopy(userSrc, isChanged);
            ItemCopy(userSrc, isChanged);
            MailBoxCopy(userSrc, isChanged);
            MatchingCopy(userSrc, isChanged);
            NoticeCopy(userSrc, isChanged);
            QuestCopy(userSrc, isChanged);
            RankCopy(userSrc, isChanged);
            ReportCopy(userSrc, isChanged);
            SeasonCopy(userSrc, isChanged);
            ShopCopy(userSrc, isChanged);
            UserCopy(userSrc, isChanged);
        }

        partial void AccountCopy(UserDB userSrc, bool isChanged);
        partial void AdvertCopy(UserDB userSrc, bool isChanged);
        partial void AttendanceCopy(UserDB userSrc, bool isChanged);
        partial void AuctionCopy(UserDB userSrc, bool isChanged);
        partial void BattleCopy(UserDB userSrc, bool isChanged);
        partial void BuildingCopy(UserDB userSrc, bool isChanged);
        partial void CharacterCopy(UserDB userSrc, bool isChanged);
        partial void InternalCopy(UserDB userSrc, bool isChanged);
        partial void ItemCopy(UserDB userSrc, bool isChanged);
        partial void MailBoxCopy(UserDB userSrc, bool isChanged);
        partial void MatchingCopy(UserDB userSrc, bool isChanged);
        partial void NoticeCopy(UserDB userSrc, bool isChanged);
        partial void QuestCopy(UserDB userSrc, bool isChanged);
        partial void RankCopy(UserDB userSrc, bool isChanged);
        partial void ReportCopy(UserDB userSrc, bool isChanged);
        partial void SeasonCopy(UserDB userSrc, bool isChanged);
        partial void ShopCopy(UserDB userSrc, bool isChanged);
        partial void UserCopy(UserDB userSrc, bool isChanged);
    }
}
