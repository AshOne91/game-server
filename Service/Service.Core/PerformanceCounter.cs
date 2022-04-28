using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class PerfData
    {
        public string _Key = "";
        public int _BeginTick = 0;
        public int _WarnTick = 200;
        public Int64 _CallCount = 0;
        public Int64 _AvgTick = 0;
        public Int64 _TotalTick = 0;
        public int _WarnCount = 0;
    }
    public class PerformanceCounter
    {
        public static void Start(string key, int warnTick)
        {
            PerfData perf;
            if (_PerfMap.TryGetValue(key, out perf) == false)
            {
                perf = new PerfData();
                perf._Key = key;
            }


            perf._WarnTick = warnTick;
            perf._BeginTick = Environment.TickCount;

            _PerfMap.TryAdd(key, perf);
        }

        public static void End(string key)
        {
            PerfData perf;
            if (_PerfMap.TryGetValue(key, out perf) == false)
            {
                throw new Exception(string.Format("Start[{0}] was not called!", key));
            }

            int delta = Environment.TickCount - perf._BeginTick;
            if (delta > perf._WarnTick)
            {
                if (_WarningEvent != null)
                {
                    _WarningEvent(delta);
                }
            }

            perf._CallCount += 1;
            perf._TotalTick += delta;
            perf._AvgTick = perf._TotalTick / perf._CallCount;
        }

        public static void Print()
        {
            if (Logger.Default != null)
            {
                Logger.Default.Log(ELogLevel.Always, "====================================================");
                foreach (KeyValuePair<String, PerfData> kv in _PerfMap)
                {
                    string log = string.Format("Key:{0} CallCount:{1} TotalTick:{2} AvgTick:{3} WarnTick:{4} WarnCount:{5}",
                                                kv.Key, kv.Value._CallCount, kv.Value._TotalTick, kv.Value._AvgTick, kv.Value._WarnTick, kv.Value._WarnCount);
                    Logger.Default.Log(ELogLevel.Always, log);
                }
                Logger.Default.Log(ELogLevel.Always, "====================================================");
            }
        }

        public delegate void OnWarning(int tick);
        public static event OnWarning _WarningEvent = null;
        private static Dictionary<String, PerfData> _PerfMap = new Dictionary<String, PerfData>();
    }
}
