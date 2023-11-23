using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FramwWork
{
    public class TimerManager:AppSubSystem<TimerManager>
    {
        private float _secondPerFrame = 0.0f;
        private float _durationTime  = 0.0f;
        private float _lastTick = 0.0f;
        private int _framePerSecond = 0;
        private int _framePerSecondElapsed = 0;
        private float _framePerSecondCheck = 0;

        public float SecondPerFrame
        {
            get { return _secondPerFrame; }
        }
        public float DurationTime
        {
            get { return _durationTime; }
        }
        public float FramePerSecond
        {
            get { return _framePerSecond; }
        }

        public void Reset()
        {
            _secondPerFrame = 0.0f;
            _durationTime = 0.0f;
            _framePerSecond = 0;
            _framePerSecondElapsed = 0;
            _framePerSecondCheck = 0.0f;
        }
        public sealed override void DoUpdate()
        {
            Int64 tick = Environment.TickCount64;
            float currentTick = tick / 1000.0f;
            _secondPerFrame = currentTick - _lastTick;
            _durationTime += _secondPerFrame;
            if ((currentTick - _framePerSecondCheck) >= 1.0f)
            {
                _framePerSecond = _framePerSecondElapsed;
                _framePerSecondElapsed = 0;
                _framePerSecondCheck = currentTick;
            }
            ++_framePerSecondElapsed;
            _lastTick = currentTick;
        }
        public sealed override void OnEnable()
        {

        }
        public sealed override void OnDisable()
        {

        }
        public sealed override void OnInit()
        {
            _lastTick = Environment.TickCount64 / 1000.0f;
            _framePerSecondCheck = _lastTick;
            _framePerSecond = 0;
            _framePerSecondElapsed = 0;
            _secondPerFrame = 0.0f;
        }

        public sealed override void OnRelease()
        {

        }
    }
}
