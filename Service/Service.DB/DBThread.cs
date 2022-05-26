using Service.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public enum EDBState
    {
        None,
        Running,
        Disconnected,
        Max
    }
    public class DBThread : CThread
    {
        private DBBase _db;

        private ConcurrentQueue<QueryBase> _queueWait;
        private ConcurrentQueue<QueryBase> _queueComplete;

        object _lock = new object();
        private QueryBase _runningQuery;

        EDBState _isDBTroubleState;
        private long _totalPushCount;
        private long _totalCompleteCount;

        private Dictionary<ulong /*nameHashCode*/, QueryTimeInfo> _QueryTimeInfoByNameHashCode;

        public DBThread(EDBType dbType, Logger logFunc) : base("DBThread", logFunc)
        {
            _queueWait = new ConcurrentQueue<QueryBase>();
            _queueComplete = new ConcurrentQueue<QueryBase>();
            _QueryTimeInfoByNameHashCode = new Dictionary<ulong, QueryTimeInfo>();

            _runningQuery = null;
            _isDBTroubleState = EDBState.None;
            _totalPushCount = 0;
            _totalCompleteCount = 0;
            switch (dbType)
            {
                case EDBType.Redis1:
                case EDBType.Redis2:
                    {
                        _db = new RedisDB(logFunc);
                    }
                    break;
                default:
                    {
                        _db = new AdoDB(logFunc);
                    }
                    break;
            }

        }
        ~DBThread()
        {
            _QueryTimeInfoByNameHashCode.Clear();
            _queueWait.Clear();
            _queueComplete.Clear();
        }

        public void Complete()
        {
            int popSize = 0;
            int prevQueueSize = _queueComplete.Count;
            while (popSize < prevQueueSize)
            {
                QueryBase query = null;
                if (!_queueComplete.TryDequeue(out query))
                {
                    break;
                }
                popSize++;

                query.Complete();

                if (!query.IsSuccess())
                {
                    _logFunc.Log(ELogLevel.Err, "[DB:Complete Err] :" + query._strResult);
                }

                _QueryTimeInsert(query);
            }
        }
        public void PushQuery(QueryBase query)
        {
            if (GetThreadState() != ThreadState.RUN)
            {
                return;
            }
            _queueWait.Enqueue(query);
            ++_totalPushCount;
        }

        public void ForEach_QueryTimeInfo(Action<QueryTimeInfo> func)
        {
            foreach (var pair in _QueryTimeInfoByNameHashCode)
            {
                QueryTimeInfo queryTimeInfo = pair.Value;
                func(queryTimeInfo);
            }
        }
        public void QueryTImeInfoReset()
        {
            _QueryTimeInfoByNameHashCode.Clear();
        }

        public bool ConnectCheck()
        {
            AdoDB db = _db as AdoDB;
            bool rtn = false;
            try
            {
                QueryBuilder Query = new QueryBuilder("select 1", true);
                db.Execute(Query, true);

                if (db.RecordNotEOF())
                {
                    rtn = true;
                }
                db.RecordEnd();
            }
            catch (Exception error)
            {
                return false;
            }
            return rtn;
        }

        public DBBase GetDB() { return _db; }

        public void SetRunningQuery(QueryBase query) { lock (_lock) { _runningQuery = query; } }
        public QueryBase GetRunningQuery() { lock (_lock) { return _runningQuery; } }


        public EDBState IsDBTroubleState() { return _isDBTroubleState; }
        public long GetWaitQueueSize() { return _queueWait.Count; }
        public long GetCompleteQueueSize() { return _queueComplete.Count; }
        public long GetTotalPushCount() { return _totalPushCount; }
        public long GetTotalCompleteCount() { return _totalCompleteCount; }

        protected override void _Run()
        {
            _db.OnLoop();

            if (_db.IsOpen())
            {
                _isDBTroubleState = EDBState.Running;
            }
            else
            {
                if (_isDBTroubleState != EDBState.None)
                {
                    _isDBTroubleState = EDBState.Disconnected;
                }
                return;
            }

            int popSize = 0;
            int prevQueueSize = _queueWait.Count;
            while (popSize < prevQueueSize)
            {
                QueryBase query = null;
                if (!_queueWait.TryDequeue(out query))
                {
                    break;
                }
                popSize++;

                //Logger(ELogType::DB, ELogLevel::None, L"[DB] " + pQuery->vGetName() + L" Run Start...");
                _logFunc.Log(ELogLevel.Trace, "[DB] " + query.vGetName() + " Run Start...");

                SetRunningQuery(query);
                query.Run(_db);
                SetRunningQuery(null);

                if (_db.IsOpen())
                {
                    _queueComplete.Enqueue(query);
                    _totalCompleteCount++;
                }
                else
                {
                    query.ResultReset();
                    _isDBTroubleState = EDBState.Disconnected;
                    if (_db.IsRedisDB())
                    {
                        _queueComplete.Enqueue(query);
                    }
                    else
                    {
                        _queueWait.Enqueue(query);
                    }
                    break;
                }
            }
        }
        protected override void _End()
        {
            //남은 쿼리를 모두 처리
            try
            {
                _Run();
                Complete();
            }
            catch (Exception e)
            {
                //m_LogFunc(ELogType::DB, ELogLevel::Critical, L"[CDBThread::EndThread] " + CStringFunc::ConvertMulti2Wide(Error.what()));
                _logFunc.Log(ELogLevel.Err, "DBThread::EndThread] " + e.Message);
            }
        }
        private void _QueryTimeInsert(QueryBase query)
        {
            QueryTimeInfo queryTimeInfo = null;
            if (!_QueryTimeInfoByNameHashCode.ContainsKey(query.GetNameHashCode()))
            {
                queryTimeInfo = new QueryTimeInfo(query.vGetName(), query.GetNameHashCode());
                _QueryTimeInfoByNameHashCode.Add(query.GetNameHashCode(), queryTimeInfo);
            }
            else
            {
                queryTimeInfo = _QueryTimeInfoByNameHashCode[query.GetNameHashCode()];
            }
            queryTimeInfo._callCount++;
            queryTimeInfo._msTotalTime += query.GetRunCompletedTime();
            queryTimeInfo._msPeakTime = Math.Max(queryTimeInfo._msPeakTime, query.GetRunCompletedTime());

            if (query.GetRunCompletedTime() >= 500)
            {
                _logFunc.Log(ELogLevel.Err, query.vGetName() + " is long Time(" + (query.GetRunCompletedTime() / 1000.0).ToString() + ") !!!");
            }
        }

    }
}
