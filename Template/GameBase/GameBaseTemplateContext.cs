﻿using GameBase.Template.GameBase;
using GameBase.Template.GameBase.Common;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Template.GameBase
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
        static Dictionary<ulong/*uid*/, UserObject> _objByUid = new Dictionary<ulong, UserObject>();
        static Dictionary<ETemplateType, GameBaseTemplate> _templates = new Dictionary<ETemplateType, GameBaseTemplate>();
        static Dictionary<ulong, Dictionary<ETemplateType, GameBaseTemplate>> _templateByUid = new Dictionary<ulong, Dictionary<ETemplateType, GameBaseTemplate>>();


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
            _objByUid.Clear();
            _templateByUid.Clear();
        }

        public static void Remove(ulong uid)
        {
            if ( _objByUid.Remove(uid) == true)
            {
                _templateByUid.Remove(uid);
            }
        }

        public static void RemoveAll()
        {
            _objByUid.Clear();
            _templateByUid.Clear();
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

        public static bool AddTemplate<T>(T userObject, ETemplateType key, GameBaseTemplate value) where T : UserObject
        {
            ulong uid = userObject.GetSession().GetUid();
            if (_objByUid.ContainsKey(uid) == false)
            {
                _objByUid.Add(uid, userObject);
                _templateByUid.Add(uid, new Dictionary<ETemplateType, GameBaseTemplate>());
            }
            
            if (_templateByUid[uid].ContainsKey(key) == true)
            {
                return false;
            }

            _templateByUid[uid].Add(key, value);
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

        public static T GetTemplate<T>(ulong uid, ETemplateType key) where T : GameBaseTemplate
        {
            GameBaseTemplate value;
            Dictionary<ETemplateType, GameBaseTemplate> userTemplate = null;
            if (_templateByUid.TryGetValue(uid, out userTemplate) == false)
            {
                return null;
            }

            if (userTemplate.TryGetValue(key, out value) == false)
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

        public static void RemoveTemplate(ulong uid, ETemplateType key)
        {
            if (_objByUid.ContainsKey(uid) == true)
            {
                _objByUid.Remove(uid);
                _templateByUid[uid].Remove(key);
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

        public static T FindUserObj<T>(ulong uid) where T : UserObject
        {
            UserObject obj;
            if (_objByUid.TryGetValue(uid, out obj) == false)
            {
                return null;
            }
            return obj as T;
        }

        public static void CreateClient(ulong uid)
        {
            UserObject obj = FindUserObj<UserObject>(uid);
            foreach (var t in _templateByUid[uid].Values)
            {
                t.OnClientCreate(obj);
            }
        }


        public static void UpdateClient(ulong uid)
        {
            UserObject obj = FindUserObj<UserObject>(uid);
            foreach (var t in _templateByUid[uid].Values)
            {
                t.OnClientUpdate(obj);
            }
        }

        public static void DeleteClient(ulong uid)
        {
            UserObject obj = FindUserObj<UserObject>(uid);
            foreach (var t in _templateByUid[uid].Values)
            {
                t.OnClientDelete(obj);
            }
            Remove(uid);
        }

        public static (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) AddItem(ulong uid, int itemId, long value, int parentItemId = -1, int groupIndex = 0)
        {
            List<ItemBaseInfo> listItemInfo = new List<ItemBaseInfo>();
            List<QuestCompleteParam> listQuestCompleteParam = new List<QuestCompleteParam>();
            UserObject obj = FindUserObj<UserObject>(uid);

            //Dictionary<string, string> hashValues = null;
            foreach (var t in _templateByUid[uid].Values)
            {
                var res = t.OnAddItem(obj, itemId, value, parentItemId, groupIndex);
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

        public static (List<ItemBaseInfo> listItemInfo, List<QuestCompleteParam> listQuestCompleteParam) DeleteItem(ulong uid, int itemId, long value, int parentItemId = -1, int groupIndex = 0)
        {
            List<ItemBaseInfo> listItemInfo = new List<ItemBaseInfo>();
            List<QuestCompleteParam> listQuestCompleteParam = new List<QuestCompleteParam>();
            UserObject obj = FindUserObj<UserObject>(uid);

            //Dictionary<string, string> hashValues = null;
            foreach (var t in _templateByUid[uid].Values)
            {
                var res = t.OnDeleteItem(obj, itemId, value, parentItemId, groupIndex);
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


        public static bool HasItemId(ulong uid, int itemId)
        {
            UserObject obj = FindUserObj<UserObject>(uid);
            foreach (var template in _templateByUid[uid].Values)
            {
                if (template.OnHasItemId(obj, itemId) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasItemType(ulong uid, int itemType)
        {
            UserObject obj = FindUserObj<UserObject>(uid);
            foreach (var template in _templateByUid[uid].Values)
            {
                if (template.OnHasItemType(obj, itemType) == true)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasItemSubType(ulong uid, int itemSubType)
        {
            UserObject obj = FindUserObj<UserObject>(uid);
            foreach (var template in _templateByUid[uid].Values)
            {
                if (template.OnHasItemSubType(obj, itemSubType) == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}