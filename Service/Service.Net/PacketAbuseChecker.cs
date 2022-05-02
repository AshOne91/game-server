using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public sealed class PacketAbuseChecker
    {
        public void AddProtocol(ushort cmd, int minTick)
        {
            _AbuseMinTickMap.Add(cmd, minTick);
            _AbuseTickMap.Add(cmd, 0);

        }

        public bool Check(ushort cmd)
        {
            int tick = 0;
            bool found = _AbuseMinTickMap.TryGetValue(cmd, out tick);
            if (found)
            {
                int lastTick = 0;
                bool foundLast = _AbuseTickMap.TryGetValue(cmd, out lastTick);
                if (foundLast)
                {
                    int curTick = Environment.TickCount;
                    int curGap = curTick - lastTick;
                    if (curGap > tick)
                    {
                        _AbuseTickMap.TryAdd(cmd, curTick);
                        return true;
                    }
                    return false;
                }

            }
            return false;
        }


        private Dictionary<ushort, int> _AbuseMinTickMap = new Dictionary<ushort, int>();
        private Dictionary<ushort, int> _AbuseTickMap = new Dictionary<ushort, int>();
    }
}
