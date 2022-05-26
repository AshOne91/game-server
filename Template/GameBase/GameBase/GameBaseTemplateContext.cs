using GameBase.Base;
using Service.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBase.Common
{
    public enum ETemplateType
    {
        None,
        Internal
    }
    public static class GameBaseTemplateContext
    {
        static Dictionary<ETemplateType, GameBaseTemplate> _templates = new Dictionary<ETemplateType, GameBaseTemplate>();

        static GameBaseInternalTemplate _internal = null;
        public static GameBaseInternalTemplate Internal
        {
            get
            {
                if (_internal == null)
                {
                    _internal = GetTemplate<GameBaseInternalTemplate>(ETemplateType.Internal);
                }
                return _internal;
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
