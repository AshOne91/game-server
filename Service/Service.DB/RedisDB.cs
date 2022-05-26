using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    using RankingPair_t = System.Collections.Generic.KeyValuePair<string/*key*/, double/*score*/>;
    using RankingPairList_t = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string/*key*/, double/*score*/>>;
    using StringList_t = System.Collections.Generic.List<string>;
    using StringPair_t = System.Collections.Generic.KeyValuePair<string/*key*/, string/*field*/>;
    using StringPairList_t = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string/*key*/, string/*field*/>>;
    using StringPairDic_t = System.Collections.Generic.Dictionary<string/*key*/, string/*field*/>;
    //public struct RankingPair_t
    //{
    //    private readonly KeyValuePair<string/*key*/, double/*score*/> value;
    //    public RankingPair_t(KeyValuePair<string/*key*/, double/*score*/> value) { this.value = value;}
    //    public static implicit operator KeyValuePair<string/*key*/, double/*score*/>(RankingPair_t value)
    //    {
    //        return value;
    //    }
    //    public static implicit operator RankingPair_t(KeyValuePair<string/*key*/, double/*score*/> value)
    //    {
    //        return value;
    //    }

    //}
    //public struct RankingPairList_t
    //{
    //    private readonly List<KeyValuePair<string/*key*/, double/*score*/>> value;
    //    public RankingPairList_t(List<KeyValuePair<string/*key*/, double/*score*/>> value) { this.value = value; }
    //    public static implicit operator List<KeyValuePair<string/*key*/, double/*score*/>> (RankingPairList_t value)
    //    {
    //        return value;
    //    }
    //    public static implicit operator RankingPairList_t(List<KeyValuePair<string/*key*/, double/*score*/>> value)
    //    {
    //        return value;
    //    }
    //}
    //public struct StringList_t
    //{
    //    private readonly List<string> value;
    //    public StringList_t(List<string> value) { this.value = value; }
    //    public static implicit operator List<string>(StringList_t value)
    //    {
    //        return value;
    //    }
    //    public static implicit operator StringList_t(List<string> value)
    //    {
    //        return value;
    //    }
    //}
    //public struct StringPair_t
    //{
    //    private readonly KeyValuePair<string/*key*/, string/*field*/> value;
    //    public StringPair_t(System.Collections.Generic.KeyValuePair<string/*key*/, string/*field*/> value) { this.value = value; }
    //    public static implicit operator KeyValuePair<string/*key*/, string/*field*/>(StringPair_t value)
    //    {
    //        return value;
    //    }
    //    public static implicit operator StringPair_t(KeyValuePair<string/*key*/, string/*field*/> value)
    //    {
    //        return value;
    //    }
    //}
    //public struct StringPairList_t
    //{
    //    private readonly List<KeyValuePair<string/*key*/, string/*field*/>> value;
    //    public StringPairList_t(System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string/*key*/, string/*field*/>> value) { this.value = value; }
    //    public static implicit operator List<KeyValuePair<string/*key*/, string/*field*/>>(StringPairList_t value)
    //    {
    //        return value;
    //    }
    //    public static implicit operator StringPairList_t(List<KeyValuePair<string/*key*/, string/*field*/>> value)
    //    {
    //        return value;
    //    }
    //}
    //public struct StringPairDic_t
    //{
    //    private readonly System.Collections.Generic.Dictionary<string/*key*/, string/*field*/> value;
    //    public StringPairDic_t(System.Collections.Generic.Dictionary<string/*key*/, string/*field*/> value) { this.value = value; }
    //}

    public class RedisDB : DBBase
    {
        private RedisWraper _masterRedis;
        private RedisWraper _slaveRedis;

        public RedisDB(Logger writeErrorLog) : base(writeErrorLog)
        {
            _masterRedis = new RedisWraper();
            _slaveRedis = new RedisWraper();
        }
        ~RedisDB()
        {
        }

        public override bool IsOpen()
        {
            return (_masterRedis.IsConnected() && _slaveRedis.IsConnected() && base.IsOpen());
        }
        public override void Open(DBInfo rDBInfo, double reconnectTime)
        {
            try
            {
                base.SetDBInfo(rDBInfo);
                if (!_masterRedis.Connect(rDBInfo._dbIP, rDBInfo._dbPort))
                {
                    throw new Exception("Can't connect master redis server (" + rDBInfo._dbIP + ":" + rDBInfo._dbPort.ToString() + ")");
                }
                if (!_slaveRedis.Connect(rDBInfo._slavedbIP, rDBInfo._dbPort))
                {
                    throw new Exception("Can't connect slave redis server (" + rDBInfo._slavedbIP + ":" + rDBInfo._dbPort.ToString() + ")");
                }
                base.Open(rDBInfo, reconnectTime);
            }
            catch (Exception e)
            {
                _ThrowErrorMsg(e.Message);
            }
        }
        public override bool IsRedisDB() { return true; }

        public void Select(int dbIndex)
        {
            _masterRedis.Select(dbIndex);
            _slaveRedis.Select(dbIndex);
        }
        public void FlushDB()
        {
            _masterRedis.FlushDB();
        }
        public void Expire(string key, uint seconds)
        {
            _masterRedis.Expire(key, seconds);
        }
        public void Append(string key, string value)
        {
            _masterRedis.Append(key, value);
        }
        public string Get(string key)
        {
            return _slaveRedis.Get(key);
        }
        public void Set(string key, string value)
        {
            _masterRedis.Set(key, value);
        }
        public long IncrBy(string key, long increment)
        {
            return _masterRedis.IncrBy(key, increment);
        }
        public bool Exists(string key)
        {
            return _slaveRedis.Exists(key);
        }
        public bool Del(string key)
        {
            return _masterRedis.Del(key);
        }
        public void ZAdd(string key, string member, double score)
        {
            _masterRedis.ZAdd(key, member, score);
        }
        public void ZRem(string key, string member)
        {
            _masterRedis.ZRem(key, member);
        }
        public double ZScore(string key, string member)
        {
            return _slaveRedis.ZScore(key, member);
        }
        public long ZRank(string key, string member)
        {
            return _slaveRedis.ZRank(key, member);
        }
        public long ZRevRank(string key, string member)
        {
            return _slaveRedis.ZRevRank(key, member);
        }
        RankingPairList_t ZRange(string key, long start, long end)
        {
            return _slaveRedis.ZRange(key, start, end);
        }
        RankingPairList_t ZRevRange(string key, long start, long end)
        {
            return _slaveRedis.ZRevRange(key, start, end);
        }
        StringList_t ZRangeByScore(string key, double min, double max, int offset = 0, int maxCount = -1)
        {
            return _slaveRedis.ZRangeByScore(key, min, max, offset, maxCount);
        }
        StringList_t ZRevRangeByScore(string key, double max, double min, int offset = 0, int maxCount = -1)
        {
            return _slaveRedis.ZRevRangeByScore(key, max, min, offset, maxCount);
        }
        double ZIncrBy(string key, string member, double increment)
        {
            return _masterRedis.ZIncrBy(key, member, increment);
        }
        long ZCard(string key)
        {
            return _masterRedis.ZCard(key);
        }

        void HSet(string key, string field, string value)
        {
            _masterRedis.HSet(key, field, value);
        }
        string HGet(string key, string field)
        {
            return _slaveRedis.HGet(key, field);
        }
        StringPairDic_t HGetAll(string key)
        {
            StringPairDic_t stringPairDIc = new StringPairDic_t();
            StringPairList_t stringPairList = _slaveRedis.HGetAll(key);
            foreach (var pair in stringPairList)
            {
                stringPairDIc.Add(pair.Key, pair.Value);
            }
            return stringPairDIc;
        }

        protected override void _ThrowErrorMsg(string szMsg)
        {
            throw new Exception(szMsg);
        }
    }
}
