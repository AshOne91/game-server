using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{

    public class Time
    {
        static public int TickCount()
        {
            return Environment.TickCount;
        }
    }

    public class TimeCounter
    {
        public TimeCounter()
        {
            _Active = false;
            _Duration = 0;
            _Time = 0;
        }

        public TimeCounter(int duration)
        {
            _Active = false;
            _Duration = duration;
            _Time = 0;
        }

        public bool Start(int duration)
        {
            if (duration >= 0)
            {
                _Duration = duration;
                _Time = Time.TickCount();
                _Active = true;

                return true;
            }
            return false;
        }

        public bool Start()
        {
            if (_Duration >= 0)
            {
                _Time = Time.TickCount();
                _Active = true;
                return true;
            }
            return false;
        }

        // Start()를 호출하기 전에는 IsFinished는 false
        // Start()가 호출된 이후에는 Duration을 넘어가면 true
        //                          Start 호출전 상태로 바꿈
        public bool IsFinished()
        {
            if (IsActive() == false)
                return false;

            if (_Duration <= ProcessTime())
            {
                _Active = false;
                return true;
            }

            return false;
        }

        /// 지난 시간
        public int ProcessTime()
        {
            return Time.TickCount() - _Time;
        }

        /// 남은 시간
        public int RemainTime()
        {
            if (IsActive() == false)
                return 0;

            int remain = _Duration - ProcessTime();
            return remain;
        }

        public bool IsActive() { return _Active; }

        // SetActive(False)는 IsFinished()를 항상 false로 만들어서 바인딩된 함수 실행 X
        public void SetActive(bool state) { _Active = state; }

        // ForceFinish()는 IsFinished()를 true만들어서, 바인딩된 함수 한번 실행 후 타이머 종료
        public void ForceFinish() { _Duration = 0; }

        private bool _Active;
        private int _Duration;
        private int _Time;
    }
}
