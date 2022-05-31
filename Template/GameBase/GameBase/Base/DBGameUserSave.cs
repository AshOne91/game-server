﻿using Service.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Base
{
    public partial class DBGameUserSave : QueryBase
    {
        public bool _isConnected;
        public UInt64 _partitionKey_1;
        public UInt64 _partitionKey_2;
        public UserDB _userDB;

        public DBGameUserSave()
        {
            _userDB = new UserDB();
        }
        ~DBGameUserSave()
        {

        }


        public override void vRun(AdoDB adoDB)
        {
            try
            {
                AccountRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                AdvertRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                AttendanceRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                AuctionRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                BattleRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                BuildingRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                CharacterRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                InternalRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                ItemRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                MailBoxRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                MatchingRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                NoticeRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                QuestRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                RankRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                ReportRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                SeasonRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                ShopRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
                UserRun(_userDB, adoDB, _partitionKey_1, _partitionKey_2);
            }
            catch (Exception Error)
            {
                _strResult = Error.Message;
            }
        }
        public override string vGetName() { return "DBGameUserSave"; }
        partial void AccountRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void AdvertRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void AttendanceRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void AuctionRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void BattleRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void BuildingRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void CharacterRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void InternalRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void ItemRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void MailBoxRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void MatchingRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void NoticeRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void QuestRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void RankRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void ReportRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void SeasonRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void ShopRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
        partial void UserRun(UserDB userDB, AdoDB adoDB, UInt64 partitionKey_1, UInt64 partitionKey_2);
    }
}
