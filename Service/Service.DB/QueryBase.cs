using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public abstract class QueryBase
    {
        public ulong _nameHashCode;
        public ulong _parentSerial;
        public short _remainedCount;
        public string _strResult;

        protected double _msRunStartTIme;
        protected double _msRunCompletedTime;
        Action _completeCallback;
        Action _multiCompleteCallback;

        public QueryBase()
        {
            _nameHashCode = 0;
            _parentSerial = 0;
            _remainedCount = 0;
            _strResult = "Success";

            _msRunStartTIme = 0;
            _msRunCompletedTime = 0;
            _completeCallback = null;
            _multiCompleteCallback = null;
        }
        ~QueryBase() { }

        public void Run(DBBase db)
        {
            _msRunStartTIme = Environment.TickCount;

            if (db.IsOpen())
            {
                switch (db.GetDBInfo()._dbType)
                {
                    case EDBType.Redis1:
                    case EDBType.Redis2:
                        {
                            vRunRedis((RedisDB)db);
                        }
                        break;
                    default:
                        {
                            vRun((AdoDB)db);
                        }
                        break;
                }
            }

            _msRunCompletedTime = Environment.TickCount - _msRunStartTIme;
        }
        public void Complete()
        {
            if (IsValidCheck())
            {
                vComplete();

                if (_multiCompleteCallback != null)
                {
                    _multiCompleteCallback();
                }
                else if (_completeCallback != null)
                {
                    _completeCallback();
                }
            }
        }

        public abstract string vGetName();
        public virtual bool IsValidCheck() { return true; }

        public ulong GetNameHashCode()
        {
            /*if (m_NameHashCode == 0)
            {
                m_NameHashCode = MHash::Str(CStringFunc::ConvertWide2Multi(vGetName()));
            }*/
            _nameHashCode = Hash.GenerateHash64(vGetName());
            return _nameHashCode;
        }
        public bool IsSuccess() { return _strResult == "Success"; }
        public void ResultReset() { _strResult = "Success"; }

        public void SetCompleteCallback(Action completeCallback, bool isMultiQuery = false)
        {
            if (isMultiQuery)
            {
                _multiCompleteCallback = completeCallback;
            }
            else
            {
                _completeCallback = completeCallback;
            }
        }

        public double GetRunningTime()
        {
            if (_msRunStartTIme > 0)
            {
                return Environment.TickCount - _msRunStartTIme;
            }
            else
            {
                return 0;
            }
        }
        public double GetRunCompletedTime() { return _msRunCompletedTime; }

        public virtual void vRun(AdoDB adbDB) { }
        public virtual void vRunRedis(RedisDB redisDB) { }
        public virtual void vComplete() { }
    }
}
