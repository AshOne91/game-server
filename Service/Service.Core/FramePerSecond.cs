using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class FramePerSecond
    {
        private int _framePerSecond;
        private int _frameCount;
        private int _lastTime;

        public FramePerSecond()
        {
            _framePerSecond = 0;
            _frameCount = 0;
            _lastTime = Environment.TickCount;
        }
        ~FramePerSecond() { }

        public void CalcFramePerSecond()
        {
            _frameCount++;

            int curTime = Environment.TickCount;
            int gapTime = curTime - _lastTime;
            if (gapTime > 1000)
            {
                _framePerSecond = _frameCount * 1000 / gapTime;
                _lastTime = curTime;
                _frameCount = 0;
            }

        }
        public int GetFramePerSecond() { return _framePerSecond; }
    }
}
