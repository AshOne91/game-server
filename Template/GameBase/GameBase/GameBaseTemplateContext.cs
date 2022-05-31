using GameBase.Base;
using GameBase.Common;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase
{
    public enum ETemplateType
    {
        None,
        Account,
        Admin,
        Advert,
        Attendance,
        Auction,
        Battle,
        Building,
        Character,
        Internal,
        Item,
        MailBox,
        Matching,
        Notice,
        Quest,
        Rank,
        Report,
        Season,
        Shop
    }
    public static class GameBaseTemplateContext
    {
        static Dictionary<ETemplateType, GameBaseTemplate> _templates = new Dictionary<ETemplateType, GameBaseTemplate>();

        static AccountTemplate _account = null;
        public static AccountTemplate Account
        {
            get
            {
                if (_account == null)
                {
                    _account = GetTemplate<AccountTemplate>(ETemplateType.Account);
                }
                return _account;
            }
        }

        static AdminTemplate _admin = null;
        public static AdminTemplate Admin
        {
            get
            {
                if (_admin == null)
                {
                    _admin = GetTemplate<AdminTemplate>(ETemplateType.Admin);
                }
                return _admin;
            }
        }

        static AdvertTemplate _advert = null;
        public static AdvertTemplate Advert
        {
            get
            {
                if (_advert == null)
                {
                    _advert = GetTemplate<AdvertTemplate>(ETemplateType.Advert);
                }
                return _advert;
            }
        }

        static AttendanceTemplate _attendance = null;
        public static AttendanceTemplate Attendance
        {
            get
            {
                if (_attendance == null)
                {
                    _attendance = GetTemplate<AttendanceTemplate>(ETemplateType.Attendance);
                }
                return _attendance;
            }
        }

        static AuctionTemplate _auction = null;
        public static AuctionTemplate Auction
        {
            get
            {
                if (_auction == null)
                {
                    _auction = GetTemplate<AuctionTemplate>(ETemplateType.Auction);
                }
                return _auction;
            }
        }

        static BattleTemplate _battle = null;
        public static BattleTemplate Battle
        {
            get
            {
                if (_battle == null)
                {
                    _battle = GetTemplate<BattleTemplate>(ETemplateType.Battle);
                }
                return _battle;
            }
        }

        static BuildingTemplate _building = null;
        public static BuildingTemplate Building
        {
            get
            {
                if (_building == null)
                {
                    _building = GetTemplate<BuildingTemplate>(ETemplateType.Building);
                }
                return _building;
            }
        }

        static CharacterTemplate _character = null;
        public static CharacterTemplate Character
        {
            get
            {
                if (_character == null)
                {
                    _character = GetTemplate<CharacterTemplate>(ETemplateType.Character);
                }
                return _character;
            }
        }

        static InternalTemplate _internal = null;
        public static InternalTemplate Internal
        {
            get
            {
                if (_internal == null)
                {
                    _internal = GetTemplate<InternalTemplate>(ETemplateType.Internal);
                }
                return _internal;
            }
        }

        static ItemTemplate _item = null;
        public static ItemTemplate Item
        {
            get
            {
                if (_item == null)
                {
                    _item = GetTemplate<ItemTemplate>(ETemplateType.Item);
                }
                return _item;
            }
        }

        static MailBoxTemplate _mailBox = null;
        public static MailBoxTemplate MailBox
        {
            get
            {
                if (_mailBox == null)
                {
                    _mailBox = GetTemplate<MailBoxTemplate>(ETemplateType.MailBox);
                }
                return _mailBox;
            }
        }

        static MatchingTemplate _matching = null;
        public static MatchingTemplate Matching
        {
            get
            {
                if (_matching == null)
                {
                    _matching = GetTemplate<MatchingTemplate>(ETemplateType.Matching);
                }
                return _matching;
            }
        }

        static NoticeTemplate _notice = null;
        public static NoticeTemplate Notice
        {
            get
            {
                if (_notice == null)
                {
                    _notice = GetTemplate<NoticeTemplate>(ETemplateType.Notice);
                }
                return _notice;
            }
        }

        static QuestTemplate _quest = null;
        public static QuestTemplate Quest
        {
            get
            {
                if (_quest == null)
                {
                    _quest = GetTemplate<QuestTemplate>(ETemplateType.Quest);
                }
                return _quest;
            }
        }

        static RankTemplate _rank = null;
        public static RankTemplate Rank
        {
            get
            {
                if (_rank == null)
                {
                    _rank = GetTemplate<RankTemplate>(ETemplateType.Rank);
                }
                return _rank;
            }
        }

        static ReportTemplate _report = null;
        public static ReportTemplate Report
        {
            get
            {
                if (_report == null)
                {
                    _report = GetTemplate<ReportTemplate>(ETemplateType.Report);
                }
                return _report;
            }
        }

        static SeasonTemplate _season = null;
        public static SeasonTemplate Season
        {
            get
            {
                if (_season == null)
                {
                    _season = GetTemplate<SeasonTemplate>(ETemplateType.Season);
                }
                return _season;
            }
        }

        static ShopTemplate _shop = null;
        public static ShopTemplate Shop
        {
            get
            {
                if (_shop == null)
                {
                    _shop = GetTemplate<ShopTemplate>(ETemplateType.Shop);
                }
                return _shop;
            }
        }

        public static void Clear()
        {
            _templates.Clear();
        }

        public static bool AddTemplate(ETemplateType key, GameBaseTemplate value)
        {
            if (_templates.ContainsKey(key) == true)
            {
                return false;
            }

            _templates.Add(key, value);
            return true;
        }

        public static T GetTemplate<T>(ETemplateType key) where T : GameBaseTemplate
        {
            GameBaseTemplate value;
            if (_templates.TryGetValue(key, out value) == false)
            {
                return null;
            }

            return value as T;
        }

        public static void RemoveTemplate(ETemplateType key)
        {
            if (_templates.ContainsKey(key) == true)
            {
                _templates.Remove(key);
            }
        }

        public static void InitTemplate(TemplateConfig config)
        {
            foreach (var t in _templates.Values)
            {
                t.Init(config);
            }
        }

        public static void LoadDataTable(TemplateConfig config)
        {
            //DataTable<string, TDataCommonGameData>.Instance.Init(config.localPath + "/CommonGameData.csv", false);
            //DataTable<int, TDataItemList>.Instance.Init(config.localPath + "/ItemList.csv", false);

            foreach (var t in _templates.Values)
            {
                t.OnLoadData(config);
            }
        }



        public static void CreateClient(UserObject clientSession)
        {
            foreach (var t in _templates.Values)
            {
                t.OnClientCreate(clientSession);
            }
        }


        public static void UpdateClient(UserObject clientSession)
        {
            foreach (var t in _templates.Values)
            {
                t.OnClientUpdate(clientSession);
            }
        }

        public static void DeleteClient(string userId)
        {
            foreach (var t in _templates.Values)
            {
                t.OnClientDelete(userId);
            }
        }

        public static (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) AddItem(UserObject userObject, int itemId, long value, int parentItemId = -1, int groupIndex = 0)
        {
            List<ItemBaseInfo> listItemInfo = new List<ItemBaseInfo>();
            List<QuestCompleteParam> listQuestCompleteParam = new List<QuestCompleteParam>();

            //Dictionary<string, string> hashValues = null;
            foreach (var t in _templates.Values)
            {
                var res = t.OnAddItem(userObject, itemId, value, parentItemId, groupIndex);
                if (res.listItemInfo != null)
                {
                    listItemInfo.AddRange(res.listItemInfo);

                    // 최상위 함수에서만 아이템 추적 로그를 전송한다.
                    /*if (parentItemId == -1)
                    {
                        if (hashValues == null)
                        {
                            CacheService.UserHash.Get(userId, new List<string> { "api_path", "app_version", "os", "country" }, out hashValues);
                        }

                        foreach (var itemInfo in res.listItemInfo)
                        {
                            EventService.WriteEventLog("item_trace", userId,
                                hashValues["app_version"],
                                hashValues["os"],
                                hashValues["country"],
                                new Dictionary<string, string>
                                {
                                    { hashValues["api_path"], new JObject {
                                        {"event_type", 0},
                                        {"item_type", itemInfo.ItemType.ToString()},
                                        {"item_id", itemInfo.ItemId.ToString()},
                                        {"before_count", (itemInfo.TotalValue - itemInfo.Value).ToString()},
                                        {"after_count", itemInfo.TotalValue.ToString()},
                                    }.ToString()}
                                });
                        }
                    }*/
                }

                if (res.listQuestCompleteParam != null)
                {
                    listQuestCompleteParam.AddRange(res.listQuestCompleteParam);
                }
            }

            return (listItemInfo, listQuestCompleteParam);
        }

        public static (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) DeleteItem(UserObject userObject, int itemId, long value, int parentItemId = -1, int groupIndex = 0)
        {
            List<ItemBaseInfo> listItemInfo = new List<ItemBaseInfo>();
            List<QuestCompleteParam> listQuestCompleteParam = new List<QuestCompleteParam>();

            Dictionary<string, string> hashValues = null;
            foreach (var t in _templates.Values)
            {
                var res = t.OnDeleteItem(userObject, itemId, value, parentItemId, groupIndex);
                /*if (res.listItemInfo != null)
                {
                    listItemInfo.AddRange(res.listItemInfo);

                    // 최상위 함수에서만 아이템 추적 로그를 전송한다.
                    if (parentItemId == -1)
                    {
                        if (hashValues == null)
                        {
                            CacheService.UserHash.Get(userId, new List<string> { "api_path", "app_version", "os", "country" }, out hashValues);
                        }

                        foreach (var itemInfo in res.listItemInfo)
                        {
                            EventService.WriteEventLog("item_trace", userId,
                                hashValues["app_version"],
                                hashValues["os"],
                                hashValues["country"],
                                new Dictionary<string, string>
                                {
                                    { hashValues["api_path"], new JObject {
                                        {"event_type", 1},
                                        {"item_type", itemInfo.ItemType.ToString()},
                                        {"item_id", itemInfo.ItemId.ToString()},
                                        {"before_count", (itemInfo.TotalValue - itemInfo.Value).ToString()},
                                        {"after_count", itemInfo.TotalValue.ToString()},
                                    }.ToString()}
                                });
                        }
                    }
                }*/

                if (res.listQuestCompleteParam != null)
                {
                    listQuestCompleteParam.AddRange(res.listQuestCompleteParam);
                }
            }

            return (listItemInfo, listQuestCompleteParam);
        }


        public static bool HasItemId(UserObject userObject, int itemId)
        {
            foreach (var template in _templates.Values)
            {
                if (template.OnHasItemId(userObject, itemId) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasItemType(UserObject userObject, int itemType)
        {
            foreach (var template in _templates.Values)
            {
                if (template.OnHasItemType(userObject, itemType) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasItemSubType(UserObject userObject, int itemSubType)
        {
            foreach (var template in _templates.Values)
            {
                if (template.OnHasItemSubType(userObject, itemSubType) == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
