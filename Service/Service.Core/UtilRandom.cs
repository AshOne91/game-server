using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public static class UtilRandom
    {
        private static Random _rand = new Random();
        public static Int32 GetRandomValue(Int32 min, Int32 max)
        {
            if (min > max)
            {
                return 0;
            }

            return _rand.Next(min, max);
        }

        public static Int64 GetRandomValue64(Int64 min, Int64 max)
        {
            byte[] buf = new byte[8];
            _rand.NextBytes(buf);
            Int64 longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}
