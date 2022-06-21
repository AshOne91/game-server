using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Net;

namespace Service.DB
{
    using RankingPair_t = System.Collections.Generic.KeyValuePair<string/*key*/, double/*score*/>;
    using RankingPairList_t = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string/*key*/, double/*score*/>>;
    using StringList_t = System.Collections.Generic.List<string>;
    using StringPair_t = System.Collections.Generic.KeyValuePair<string/*key*/, string/*field*/>;
    using StringPairList_t = System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string/*key*/, string/*field*/>>;
    using StringPairDic_t = System.Collections.Generic.Dictionary<string/*key*/, string/*field*/>;
    public class RedisWraper
    {
        private ConnectionMultiplexer _redisConnection;
        private IDatabase _redisDatabase;
        private IServer _redisCurrentServer;
        private ConfigurationOptions _redisServer;
        private bool _isConnected;

        private ITransaction _redisTransaction;

        public RedisWraper()
        {
            _redisConnection = null;
            _redisDatabase = null;
            _redisCurrentServer = null;
            _redisServer = new ConfigurationOptions();
            _isConnected = false;

            _redisTransaction = null;
        }
        ~RedisWraper()
        {
            if (_redisConnection != null)
            {
                _redisConnection.Close();
                _redisConnection = null;
            }
        }

        public void AddServer(string host, int port)
        {
            _redisServer.EndPoints.Add(host, port);

            foreach (var conn in _redisServer.EndPoints)
            {
                if (((IPEndPoint)conn).Address.ToString() == host && ((IPEndPoint)conn).Port.ToString() == port.ToString())
                {
                    return;
                }
            }
        }
        public bool Connect()
        {
            if (_redisServer.EndPoints.Count == 0)
            {
                return false;
            }

            try
            {
                if (_redisConnection != null)
                {
                    _redisConnection.Close();
                    _redisConnection = null;
                }
                _redisConnection = ConnectionMultiplexer.Connect(_redisServer);
                if (_redisConnection.IsConnected == false)
                {
                    _redisConnection.Close();
                    _redisConnection = null;
                    return false;
                }
                _isConnected = true;
                _redisDatabase = _redisConnection.GetDatabase();
                _redisConnection.GetServer(_redisServer.EndPoints[0]);
            }
            catch (RedisException e)
            {
                return false;
            }
            return true;
        }
        public bool Connect(string host, int port)
        {
            _redisServer = null;
            _redisServer = new ConfigurationOptions();
            AddServer(host, port);
            return Connect();
        }
        public void Select(int dbindex)
        {
            if (_redisConnection == null)
            {
                return;
            }

            try
            {
                _redisDatabase = _redisConnection.GetDatabase(dbindex);
                _redisCurrentServer = _redisConnection.GetServer(_redisServer.EndPoints[dbindex]);
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public void FlushDB()
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisCurrentServer.FlushDatabase();
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public void FlushAll()
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisCurrentServer.FlushAllDatabases();
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }

        //멀티(트랜잭션) 롤백기능은 지원하지 않아 성공한거는 들어감
        public void Multi()
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisTransaction = _redisDatabase.CreateTransaction();
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public void Exec(string key)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisTransaction.Execute();
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public void Append(string key, string value)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.StringAppend(key, value);
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public string Get(string key)
        {
            if (_redisDatabase == null)
            {
                return "";
            }

            try
            {
                return _redisDatabase.StringGet(key);
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return "";
        }
        public void Set(string key, string value)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.StringSet(key, value);
            }
            catch (Exception e)
            {
                StackFrame stackFrame = new StackFrame(true);
                _isConnected = false;
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public string GetSet(string key, string value)
        {
            string rtnValue = value;
            do
            {
                if (_redisDatabase == null)
                {
                    break;
                }

                try
                {
                    value = _redisDatabase.StringGetSet(key, value);
                }
                catch (Exception e)
                {
                    StackFrame stackFrame = new StackFrame(true);
                    _isConnected = false;
                    throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
                }
            }
            while (false);
            return rtnValue;
        }
        public int Keys(string pattern, StringList_t valueList)
        {
            int resultCount = 0;
            do
            {
                if (_redisDatabase == null)
                {
                    break;
                }

                try
                {
                    foreach (var key in _redisCurrentServer.Keys(0, pattern))
                    {
                        valueList.Add(key);
                    }
                    resultCount = valueList.Count;
                }
                catch (Exception e)
                {
                    _isConnected = false;
                }
            }
            while (false);
            return resultCount;
        }
        public bool Exists(string key)
        {
            if (_redisDatabase == null)
            {
                return false;
            }

            try
            {
                return _redisDatabase.KeyExists(key);
            }
            catch (Exception e)
            {

            }
            return false;
        }
        public bool Del(string key)
        {
            if (_redisDatabase == null)
            {
                return false;
            }

            try
            {
                return _redisDatabase.KeyDelete(key);
            }
            catch (Exception e)
            {

            }
            return false;
        }
        public long Incr(string key)
        {
            if (_redisDatabase == null)
            {
                return 0;
            }

            try
            {
                return _redisDatabase.StringIncrement(key);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return 0;
        }
        public long IncrBy(string key, long by)
        {
            if (_redisDatabase == null)
            {
                return 0;
            }

            try
            {
                return _redisDatabase.StringIncrement(key, by);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return 0;
        }
        public long Decr(string key)
        {
            if (_redisDatabase == null)
            {
                return 0;
            }

            try
            {
                return _redisDatabase.StringDecrement(key);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return 0;
        }
        public long DecrBy(string key, long by)
        {
            if (_redisDatabase == null)
            {
                return 0;
            }

            try
            {
                return _redisDatabase.StringDecrement(key, by);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return 0;
        }

        public void HSet(string key, string field, string value)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.HashSet(key, field, value);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public string HGet(string key, string field)
        {
            if (_redisDatabase == null)
            {
                return "";
            }

            try
            {
                return _redisDatabase.HashGet(key, field);
            }
            catch (Exception e)
            {

            }
            return "";
        }
        public StringPairList_t HGetAll(string key)
        {
            StringPairList_t resultStringList = new StringPairList_t();
            do
            {
                if (_redisDatabase == null)
                {
                    break;
                }

                try
                {
                    var rtn = _redisDatabase.HashGetAll(key);
                    foreach (var pair in rtn)
                    {
                        resultStringList.Add(new StringPair_t(pair.Name, pair.Value));
                    }
                }
                catch (Exception e)
                {

                }
            }
            while (false);
            return resultStringList;
        }

        public void ZAdd(string key, string member, double score)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.SortedSetAdd(key, member, score);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public void ZRem(string key, string member)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.SortedSetRemove(key, member);
            }
            catch (Exception e)
            {
            }
        }
        public double ZScore(string key, string member)
        {
            if (_redisDatabase == null)
            {
                return 0.0;
            }

            try
            {
                double? score = _redisDatabase.SortedSetScore(key, member);
                if (score.HasValue == false)
                {
                    return 0.0;
                }
                return (double)score;
            }
            catch (Exception e)
            {
            }
            return 0.0;
        }
        public long ZRank(string key, string member)
        {
            if (_redisDatabase == null)
            {
                return -1;
            }

            try
            {
                long? rank = _redisDatabase.SortedSetRank(key, member);
                if (rank.HasValue == false)
                {
                    return -1;
                }
                return (long)rank;
            }
            catch (Exception e)
            {

            }
            return -1;
        }
        public long ZRevRank(string key, string member)
        {
            if (_redisDatabase == null)
            {
                return -1;
            }

            try
            {
                long? revRank = _redisDatabase.SortedSetRank(key, member, Order.Descending);
                if (revRank.HasValue == false)
                {
                    return -1;
                }
                return (long)revRank;
            }
            catch (Exception e)
            {

            }
            return -1;
        }
        public RankingPairList_t ZRange(string key, long start, long end)
        {
            RankingPairList_t resultRankingList = new RankingPairList_t();
            do
            {
                if (_redisDatabase == null)
                {
                    break;
                }

                try
                {
                    var sortSetEntry = _redisDatabase.SortedSetRangeByRankWithScores(key, start, end);
                    foreach (var entry in sortSetEntry)
                    {
                        resultRankingList.Add(new RankingPair_t(entry.Element, entry.Score));
                    }

                }
                catch (Exception e)
                {

                }
            }
            while (false);
            return resultRankingList;
        }
        public RankingPairList_t ZRevRange(string key, long start, long end)
        {
            RankingPairList_t resultRankingList = new RankingPairList_t();
            do
            {
                if (_redisDatabase == null)
                {
                    break;
                }

                try
                {
                    var sortSetEntry = _redisDatabase.SortedSetRangeByRankWithScores(key, start, end, Order.Descending);
                    foreach (var entry in sortSetEntry)
                    {
                        resultRankingList.Add(new RankingPair_t(entry.Element, entry.Score));
                    }

                }
                catch (Exception e)
                {

                }
            }
            while (false);
            return resultRankingList;
        }
        public StringList_t ZRangeByScore(string key, double min, double max, int offset = 0, int maxCount = -1)
        {
            StringList_t resultStringList = new StringList_t();
            do
            {
                if (_redisDatabase == null)
                {
                    break;
                }

                try
                {
                    var stringList = _redisDatabase.SortedSetRangeByScore(key, min, max, Exclude.None, Order.Ascending, offset, maxCount);
                    foreach (var value in stringList)
                    {
                        resultStringList.Add(value);
                    }
                }
                catch (Exception e)
                {

                }
            }
            while (false);
            return resultStringList;
        }
        public StringList_t ZRevRangeByScore(string key, double max, double min, int offset = 0, int maxCount = -1)
        {
            StringList_t resultStringList = new StringList_t();
            do
            {
                if (_redisDatabase == null)
                {
                    break;
                }

                try
                {
                    var stringList = _redisDatabase.SortedSetRangeByScore(key, min, max, Exclude.None, Order.Descending, offset, maxCount);
                    foreach (var value in stringList)
                    {
                        resultStringList.Add(value);
                    }
                }
                catch (Exception e)
                {

                }
            }
            while (false);
            return resultStringList;
        }
        public double ZIncrBy(string key, string member, double increment)
        {
            if (_redisDatabase == null)
            {
                return 0.0;
            }

            try
            {
                return _redisDatabase.SortedSetIncrement(key, member, increment);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);

            }
            return 0.0;
        }
        public long ZCard(string key)
        {
            if (_redisDatabase == null)
            {
                return 0;
            }

            try
            {
                return _redisDatabase.SortedSetLength(key);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return 0;
        }

        public void Expire(string key, uint secs)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.KeyExpire(key, TimeSpan.FromSeconds(secs));
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public int TTL(string key)
        {
            if (_redisDatabase == null)
            {
                return 0;
            }

            try
            {
                TimeSpan? ttl = _redisDatabase.KeyTimeToLive(key);
                if (ttl.HasValue == false)
                {
                    return 0;
                }
                return ((TimeSpan)ttl).Seconds;
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return 0;
        }

        public void LPush(string key, string value)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.ListLeftPush(key, value);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public string LPop(string key)
        {
            if (_redisDatabase == null)
            {
                return "";
            }

            string value = string.Empty;
            try
            {
                value = _redisDatabase.ListLeftPop(key);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return value;
        }
        public void RPush(string key, string value)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                _redisDatabase.ListRightPush(key, value);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public string RPop(string key)
        {
            if (_redisDatabase == null)
            {
                return "";
            }

            string value = string.Empty;
            try
            {
                value = _redisDatabase.ListRightPop(key);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
            return value;
        }

        /*public void ExecTransaction(List<List<string>> commands)
        {
            if (_redisDatabase == null)
            {
                return;
            }

            try
            {
                var transaction = _redisDatabase.CreateTransaction();
                transaction.com
                transaction.Execute()
            }
            catch (Exception e)
            {

            }
        }*/

        public void BGSave()
        {
            if (_redisCurrentServer == null)
            {
                return;
            }

            try
            {
                _redisCurrentServer.Save(SaveType.BackgroundSave);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }
        public void Save()
        {
            if (_redisCurrentServer == null)
            {
                return;
            }

            try
            {
                _redisCurrentServer.Save(SaveType.ForegroundSave);
            }
            catch (Exception e)
            {
                _isConnected = false;
                StackFrame stackFrame = new StackFrame(true);
                throw new Exception("[" + stackFrame.GetMethod().Name + "]" + e.Message);
            }
        }

        public bool IsConnected() { return _isConnected; }
    }
}
