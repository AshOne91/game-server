﻿using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public class DBConfig
    {
        public double _dbReconnectTime;
        public double _dbTroubleWaitTime;

        public Dictionary<EDBType, DBSimpleInfo> _dbSimpleInfoByDBType;

        public DBConfig()
        {
            _dbReconnectTime = 5;
            _dbTroubleWaitTime = 20;
        }
        ~DBConfig()
        {

        }

        public void LoadConfig(string dbConfig)
        {
            //FIXMEFIXMEFIXME
        }
        DBSimpleInfo GetDBSimpleInfo(EDBType type)
        {
            if (_dbSimpleInfoByDBType.ContainsKey(type) == false)
            {
                return null;
            }
            return _dbSimpleInfoByDBType[type];
        }
    }
    public class DBSimpleInfo
    {
        public short _dbIndex;
        public int _serverID;
        public EDBType _dbType;
        public string _dbName;
        public string _dbID;
        public string _dbPW;
        public string _dbIP;
        public string _slaveDBIP;
        public int _dbPort;
        public Byte _threadCount;
        public Dictionary<string, short> _redisDBIndex;

        public DBSimpleInfo()
        {
            Reset();
        }

        public void Reset()
        {
            _dbIndex = 0;
            _serverID = 0;
            _dbType = EDBType.Max;
            _dbName = string.Empty;
            _dbID = string.Empty;
            _dbPW = string.Empty;
            _dbIP = string.Empty;
            _slaveDBIP = string.Empty;
            _dbPort = 0;
            _threadCount = 0;
            _redisDBIndex.Clear();
        }
    }
    public class DBManager
    {
        protected DBConfig _dbConfig;

        protected Dictionary<int, DBThread> _dbThreadByIndex;

        protected Dictionary<int/*ServerID * 100 + EDBType*/, short/*DBIndex*/> _dbIndexByMakeKey;
        protected HashSet<EDBType> _setDBType;

        protected ulong _serialAllocator;
        protected Dictionary<ulong, QueryBase> _multiQueryBySerial;

        protected Logger _logFunc;

        //서버 종료시 DB처리가 남아있는 DBThread 리스트
        protected List<DBThread> _listEndDBThread;

        protected TimeCounter _serviceStopTimer;
        protected TimeCounter _troubleDisplayTimer;
        protected List<DBBase.DBInfo> _listTroubleDB;

        protected Dictionary<EDBType, long> _waitQueueSizeByDBType;
        protected Dictionary<EDBType, long> _completeQueueSizeByDBType;
        protected Dictionary<ulong/*NameHashCode*/, QueryTimeInfo> _queryTimeInfoByNameHashCode;

        public DBManager(Logger writeErrorLog)
        {
            _dbConfig = new DBConfig();

            _dbThreadByIndex = new Dictionary<int, DBThread>();

            _dbIndexByMakeKey = new Dictionary<int, short>();
            _setDBType = new HashSet<EDBType>();

            _serialAllocator = 0;
            _multiQueryBySerial = new Dictionary<ulong, QueryBase>();

            _logFunc = writeErrorLog;

            _serviceStopTimer = new TimeCounter();
            _troubleDisplayTimer = new TimeCounter();
            _listTroubleDB = new List<DBBase.DBInfo>();

            _waitQueueSizeByDBType = new Dictionary<EDBType, long>();
            _completeQueueSizeByDBType = new Dictionary<EDBType, long>();
            _queryTimeInfoByNameHashCode = new Dictionary<ulong, QueryTimeInfo>();
        }
        ~DBManager()
        {
            _dbThreadByIndex.Clear();
            _dbIndexByMakeKey.Clear();
            _setDBType.Clear();
            _multiQueryBySerial.Clear();
            _listTroubleDB.Clear();
            _waitQueueSizeByDBType.Clear();
            _completeQueueSizeByDBType.Clear();
            _queryTimeInfoByNameHashCode.Clear();
        }

        public bool EndThread()
        {
            int idx = 0;
            while (idx < _listEndDBThread.Count)
            {
                if (_listEndDBThread[idx].EndThread(true))
                {
                    _listEndDBThread.RemoveAt(idx);
                }
                else
                {
                    ++idx;
                }
            }
            return (_listEndDBThread.Count == 0);
        }
        public void SetupDB(string dbConfig)
        {
            try
            {
                _dbConfig.LoadConfig(dbConfig);

                List<DBSimpleInfo> listDBSimpleInfo = new List<DBSimpleInfo>();
                foreach (var pair in _dbConfig._dbSimpleInfoByDBType)
                {
                    DBSimpleInfo DBSimpleInfo = pair.Value;
                    listDBSimpleInfo.Add(DBSimpleInfo);
                }
                SetDB(listDBSimpleInfo);
            }
            catch (Exception Error)
            {
                throw new Exception("[DBManager.SetupDB] " + Error.Message);
            }
        }
        public bool SetDB(List<DBSimpleInfo> listDBSimpleInfo)
        {
            foreach (var rDBSimpleInfo in listDBSimpleInfo)
            {
                if (rDBSimpleInfo._dbType >= EDBType.Max)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(rDBSimpleInfo._dbName) || string.IsNullOrEmpty(rDBSimpleInfo._dbIP))
                {
                    continue;
                }

                DBBase.DBInfo _openDBInfo;
                _openDBInfo._dbType = rDBSimpleInfo._dbType;
                _openDBInfo._dbName = rDBSimpleInfo._dbName;
                _openDBInfo._dbIP = rDBSimpleInfo._dbIP;
                _openDBInfo._slavedbIP = string.IsNullOrEmpty(rDBSimpleInfo._slaveDBIP) ? rDBSimpleInfo._dbIP : rDBSimpleInfo._slaveDBIP;
                _openDBInfo._dbPort = rDBSimpleInfo._dbPort;
                _openDBInfo._id = rDBSimpleInfo._dbID;
                _openDBInfo._pw = rDBSimpleInfo._dbPW;
                _openDBInfo._indexByRedisDB = rDBSimpleInfo._redisDBIndex;

                int DBIndexKey = rDBSimpleInfo._serverID * 100 + (int)rDBSimpleInfo._dbType;
                _dbIndexByMakeKey.Add(DBIndexKey, rDBSimpleInfo._dbIndex);

                _setDBType.Add(rDBSimpleInfo._dbType);

                for (int threadIndex = 0; threadIndex < rDBSimpleInfo._threadCount; ++threadIndex)
                {
                    int DBKey = _MakeDBKey(rDBSimpleInfo._dbType, rDBSimpleInfo._threadCount, (Byte)threadIndex);

                    if (!_dbThreadByIndex.ContainsKey(DBKey))
                    {
                        DBThread dbThread = new DBThread(rDBSimpleInfo._dbType, _logFunc);
                        dbThread.BegineThread();

                        _dbThreadByIndex.Add(DBKey, dbThread);
                        _listEndDBThread.Add(dbThread);

                        dbThread.GetDB().Open(_openDBInfo, _dbConfig._dbReconnectTime);
                        if (_openDBInfo._dbType != EDBType.Redis1 && _openDBInfo._dbType != EDBType.Redis2)
                        {
                            dbThread.ConnectCheck();
                        }
                    }
                    else
                    {
                        DBThread dbThread = _dbThreadByIndex[DBKey];
                        if (dbThread.GetDB().GetDBInfo()._dbIP != _openDBInfo._dbIP)
                        {
                            dbThread.GetDB().Open(_openDBInfo, _dbConfig._dbReconnectTime);
                            if (_openDBInfo._dbType != EDBType.Redis1 && _openDBInfo._dbType != EDBType.Redis2)
                            {
                                dbThread.ConnectCheck();
                            }
                        }
                    }
                }
            }
            return true;
        }
        public void OnLoop()
        {
            _listTroubleDB.Clear();
            _completeQueueSizeByDBType.Clear();

            foreach (var pair in _dbThreadByIndex)
            {
                DBThread dbThread = pair.Value;

                QueryBase runningQuery = dbThread.GetRunningQuery();
                if (runningQuery != null && runningQuery.GetRunningTime() >= 1000)
                {
                    _logFunc.Log(ELogLevel.Err, "Task(" + runningQuery.vGetName() + ") is long Time(" + runningQuery.GetRunningTime() + ") !!!");
                }

                dbThread.Complete();

                if (dbThread.IsDBTroubleState() == EDBState.Disconnected)
                {
                    _listTroubleDB.Add(dbThread.GetDB().GetDBInfo());
                }

                if (dbThread.GetDB().GetDBInfo()._dbType < EDBType.Max)
                {
                    _waitQueueSizeByDBType[dbThread.GetDB().GetDBInfo()._dbType] += dbThread.GetWaitQueueSize();
                    _completeQueueSizeByDBType[dbThread.GetDB().GetDBInfo()._dbType] += dbThread.GetCompleteQueueSize();
                }
            }

            if (_listTroubleDB.Count > 0)
            {
                if (!_serviceStopTimer.IsActive())
                {
                    _serviceStopTimer.Start((int)_dbConfig._dbTroubleWaitTime);
                }
                if (!_troubleDisplayTimer.IsActive())
                {
                    _troubleDisplayTimer.Start();
                }
            }
            else
            {
                if (!IsServiceStop())
                {
                    _serviceStopTimer.SetActive(false);
                    _troubleDisplayTimer.SetActive(false);
                }
            }
        }

        public void ForEach_DBType(Action<EDBType> func)
        {
            foreach (EDBType dbType in _setDBType)
            {
                func(dbType);
            }
        }
        public void ForEach_QueryTimeInfo(Action<QueryTimeInfo> func)
        {
            foreach (var pair in _queryTimeInfoByNameHashCode)
            {
                QueryTimeInfo totalQueryTimeInfo = pair.Value;
                totalQueryTimeInfo.Reset();
            }

            foreach (var pair in _dbThreadByIndex)
            {
                DBThread dbThread = pair.Value;

                dbThread.ForEach_QueryTimeInfo((QueryTimeInfo queryTimeInfo) =>
                {
                    QueryTimeInfo totalQueryTimeInfo = null;
                    if (_queryTimeInfoByNameHashCode.ContainsKey(queryTimeInfo._nameHashCode))
                    {
                        totalQueryTimeInfo = _queryTimeInfoByNameHashCode[queryTimeInfo._nameHashCode];
                    }
                    else
                    {
                        totalQueryTimeInfo = new QueryTimeInfo(queryTimeInfo._name, queryTimeInfo._nameHashCode);
                        _queryTimeInfoByNameHashCode.Add(totalQueryTimeInfo._nameHashCode, totalQueryTimeInfo);
                    }
                    totalQueryTimeInfo._callCount += queryTimeInfo._callCount;
                    totalQueryTimeInfo._msTotalTime += queryTimeInfo._msTotalTime;
                    totalQueryTimeInfo._msPeakTime = Math.Max(totalQueryTimeInfo._msPeakTime, queryTimeInfo._msPeakTime);
                });
            }

            foreach (var pair in _queryTimeInfoByNameHashCode)
            {
                QueryTimeInfo totalQueryTimeInfo = pair.Value;
                func(totalQueryTimeInfo);
            }
        }
        public void QueryTimeInfoReset()
        {

            foreach (var pair in _dbThreadByIndex)
            {
                DBThread dbThread = pair.Value;
                dbThread.QueryTImeInfoReset();
            }
        }

        public bool IsTroubleDisplay() { return _troubleDisplayTimer.IsFinished(); }
        public bool IsServiceStop() { return _serviceStopTimer.IsFinished(); }
        public List<DBBase.DBInfo> GetRoubleDBList()
        {
            return _listTroubleDB;
        }

        //=================================================================
        // 멀티 쿼리(각 GameDB로 부터 세부정보를 받아서 하나로 모은다)
        //=================================================================
        public void PushMultiQuery(QueryBase parentQuery, Action CompleteCallback = null)
        {
            _logFunc.Log(ELogLevel.Trace, parentQuery.vGetName() + " PushMultiQuery()");

            parentQuery._parentSerial = ++_serialAllocator;
            parentQuery.SetCompleteCallback(CompleteCallback, true);

            _multiQueryBySerial.Add(parentQuery._parentSerial, parentQuery);
        }
        public void OneCompleted(ulong parentSerial, QueryBase childQuery)
        {
            if (!_multiQueryBySerial.ContainsKey(parentSerial))
            {
                throw new Exception("[OneCompleted] " + parentSerial.ToString() + " Not Found!");
            }

            QueryBase parentQuery = _multiQueryBySerial[parentSerial];

            parentQuery._remainedCount--;

            if (!parentQuery.IsSuccess())
            {
                parentQuery._strResult += childQuery._strResult;
            }
            if (parentQuery._remainedCount == 0)
            {
                parentQuery.Complete();
                _multiQueryBySerial.Remove(parentSerial);
            }
        }

        public long GetWaitQueueSize(EDBType type)
        {
            if (type < EDBType.Max)
            {
                return _waitQueueSizeByDBType[type];
            }
            return 0;
        }
        public long GetCompleteQueueSize(EDBType type)
        {
            if (type < EDBType.Max)
            {
                return _completeQueueSizeByDBType[type];
            }
            return 0;
        }

        public string GetDBName(EDBType type)
        {
            return type.ToString();
        }

        protected short _GetDBIndex(EDBType type, int serverID)
        {
            int DBIndexKey = serverID * 100 + (byte)type;

            if (!_dbIndexByMakeKey.ContainsKey(DBIndexKey))
            {
                return _dbIndexByMakeKey[DBIndexKey];
            }

            return 0;
        }
        protected int _MakeDBKey(EDBType type, short dbIndex, byte threadIndex)
        {
            int DBKey = (int)(type) * 1000000 + dbIndex * 1000 + threadIndex;
            return DBKey;
        }
        protected void _PushQuery(int DBKey, QueryBase query)
        {
            _logFunc.Log(ELogLevel.Trace, "[DB] " + query.vGetName() + " PushQuery!");

            if (!_dbThreadByIndex.ContainsKey(DBKey))
            {
                string queryName = query.vGetName();
                throw new Exception("[PushQuery] " + DBKey.ToString() + " Not Found! " + queryName);
            }

            if (query._parentSerial > 0)
            {
                if (_multiQueryBySerial.ContainsKey(query._parentSerial))
                {
                    QueryBase parentQuery = _multiQueryBySerial[query._parentSerial];
                    parentQuery._remainedCount++;
                }
            }

            DBThread dbThread = _dbThreadByIndex[DBKey];
            dbThread.PushQuery(query);
        }
        protected DBConfig _GetDBConfig() { return _dbConfig; }
    }
}