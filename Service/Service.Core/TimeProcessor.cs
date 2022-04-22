using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    using TimerID = UInt64;
    using TimerType = UInt32;

    public interface TimeDispatcher
    {
        void OnTimer(TimerHandle timer);
    }

    public class TimerHandle
    {
        public virtual void Process(int curTick)
        {
            if (_lastUpdateTime == 0)
            {
                _lastUpdateTime = curTick;
            }
            if (_dispatcher != null && IsActive())
            {
                if (curTick >= (_lastUpdateTime + _Interval))
                {
                    _dispatcher.OnTimer(this);

                    if (_moreRequest)
                        Active();
                    else
                        InActive();
                }
            }
        }


        public TimerID _TimerId = 0;
        public TimerType _TimerType = 0;
        public UInt32 _Interval = 0;
        public object _ExtraData = null;

        public void Active()
        {
            _active = true;
            _lastUpdateTime = 0;
            _moreRequest = false;
        }
        public void InActive()
        {
            _active = false;
            _moreRequest = false;
        }
        public bool IsActive() { return _active; }
        public void MoreRequest() { _moreRequest = true; }
        public void SetDispatcher(TimeDispatcher dispatcher) { _dispatcher = dispatcher; }

        private bool _moreRequest = false;

        private TimeDispatcher _dispatcher = null;
        private int _lastUpdateTime = 0;
        private bool _active = false;
    }

    public class TimeProcessor
    {
        public TimerID AddTimer(TimerType timerType, TimeDispatcher dispatcher, UInt32 interval, object extraObject)
        {
            TimerHandle handle = new TimerHandle();
            handle._TimerId = AllocId();
            handle._TimerType = timerType;
            handle._Interval = interval;
            handle._ExtraData = extraObject;
            handle.Active();
            handle.SetDispatcher(dispatcher);

            _waitTimerHandlers.Add(handle);
            return handle._TimerId;

        }
        public bool RemoveTimer(TimerID timerID)
        {
            if (_timerHandlers.RemoveAll(r => r._TimerId == timerID) > 0)
                return true;

            return false;
        }

        public void ProcessTimer()
        {
            if (_waitTimerHandlers.Count > 0)
            {
                _timerHandlers.AddRange(_waitTimerHandlers);
                _waitTimerHandlers.Clear();
            }

            int idx = 0;
            while (idx < _timerHandlers.Count)
            {
                TimerHandle handle = _timerHandlers[idx];
                if (handle.IsActive())
                {
                    handle.Process(Time.TickCount());
                    ++idx;
                }
                else
                {
                    _timerHandlers.RemoveAt(idx);
                }
            }
        }
        public int GetTimerCount() { return _timerHandlers.Count; }

        TimerID AllocId() { return ++_idSeq; }

        private TimerID _idSeq = 0;
        private List<TimerHandle> _timerHandlers = new List<TimerHandle>();
        private List<TimerHandle> _waitTimerHandlers = new List<TimerHandle>();
    }
}
