using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public enum EDBType
    {
        Global,
        Sharding,
        Game,
        Redis1,
        Redis2,
        Max
    }
    public class DBBase
    {
        public struct DBInfo
        {
            public EDBType _dbType;
            public string _dbName;
            public string _dbIP;
            public string _slavedbIP;
            public int _dbPort;
            public Dictionary<string, short> _indexByRedisDB;
            public string _id;
            public string _pw;

            public short GetRedisDBIndex(string key)
            {
                if (_indexByRedisDB.ContainsKey(key) == false)
                {
                    return -1;
                }
                return _indexByRedisDB[key];
            }
        }

        private DBInfo _dbInfo;
        private bool _isOpened;

        private double _maxReconnectTime;
        private TimeCounter _reconnectTimer;

        Logger _logFunc;

        public DBBase(Logger writeErrorLog)
        {
            _logFunc = writeErrorLog;
            _isOpened = false;
            _maxReconnectTime = 5;
            _reconnectTimer = new TimeCounter();
        }
        ~DBBase()
        {

        }

        public virtual bool IsOpen()
        {
            return _isOpened;
        }
        public virtual void Open(DBInfo rDBInfo, double reconnectTime)
        {
            SetDBInfo(rDBInfo);
            _isOpened = true;
            _maxReconnectTime = reconnectTime;
            _reconnectTimer.Start(0);
        }
        public virtual void Close()
        {
        }

        public virtual void OnLoop()
        {
            if (_isOpened && !IsOpen())
            {
                if (_reconnectTimer.IsFinished())
                {
                    try
                    {
                        _logFunc.Log(ELogLevel.Err, "[CDBBase::OnLoop] " + _dbInfo._dbName + " Reconnect Success!!");
                    }
                    catch (Exception ex)
                    {
                        //처음에는 자주 접속 시도하다 서서히 시간을 늘려간다.
                        double NextReconnectTime = Math.Min(_maxReconnectTime, _reconnectTimer.GetDuration() + 1);
                        _reconnectTimer.Start((int)NextReconnectTime);
                        throw ex;
                    }
                }
            }
        }

        public DBInfo GetDBInfo() { return _dbInfo; }
        public void SetDBInfo(DBInfo dbInfo) { _dbInfo = dbInfo; }
        public virtual bool IsRedisDB() { return false; }

        protected virtual void _ThrowErrorMsg(string szMsg)
        {
            throw new Exception(szMsg);
        }
    }
}
