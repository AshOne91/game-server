using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public class QueryTimeInfo
    {
        public string _name;
        public ulong _nameHashCode;

        public long _callCount;
        public double _msTotalTime;
        public double _msPeakTime;

        public QueryTimeInfo(string szName, ulong nameHashCode)
        {
            _name = szName;
            _nameHashCode = nameHashCode;
            Reset();
        }
        public void Reset()
        {
            _callCount = 0;
            _msTotalTime = _msPeakTime = 0;
        }
        public double GetAverageTime()
        {
            if (_callCount > 0)
            {
                return _msTotalTime / _callCount;
            }
            return 0;
        }
    }
}
