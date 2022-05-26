using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Service.Core
{
    public abstract class CThread
    {
        public enum ThreadState
        {
            RUN,
            STOP,
            END
        }

        private Thread _thread;
        private ThreadState _threadState;
        private EventWaitHandle _endCheck;

        private short _sleepPass;
        private byte _sleepMilliseconds;
        private FramePerSecond _frameRate;
        private ulong _affinityMask;

        protected string _threadName;
        protected Logger _logFunc;

        public CThread(string threadName, Logger logFunc)
        {
            _threadState = ThreadState.STOP;
            _sleepPass = 100;
            _sleepMilliseconds = 1;
            _frameRate = new FramePerSecond();

            _affinityMask = 0;
            for (int core = 1; core < Environment.ProcessorCount; ++core)
            {
                _affinityMask |= (ulong)(1) << core;
            }
            _threadName = threadName;
            _logFunc = logFunc;

            _thread = new Thread(this.__THREAD_ENTRY_POINT);
            _endCheck = new EventWaitHandle(false, EventResetMode.ManualReset);
        }
        ~CThread()
        {
            _Join();
        }

        public void BegineThread()
        {
            _thread.Start();
        }
        public bool EndThread(bool IsLog)
        {
            switch (_threadState)
            {
                case ThreadState.RUN:
                    {
                        if (IsLog)
                        {
                            _logFunc.Log(ELogLevel.Trace, _threadName + " Stop ...");
                        }
                        _threadState = ThreadState.STOP;
                    }
                    break;
                case ThreadState.STOP:
                    {
                        if (_endCheck.WaitOne(1000))
                        {
                            if (IsLog)
                            {
                                _logFunc.Log(ELogLevel.Trace, _threadName + " Stop Completed!");
                            }
                            _threadState = ThreadState.END;
                            return true;
                        }
                    }
                    break;
                case ThreadState.END:
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        public ThreadState GetThreadState() { return _threadState; }

        public short GetSleepPass() { return _sleepPass; }
        public void SetSleepPass(short sleepPass) { _sleepPass = sleepPass; }
        public void SetSleepMilliseconds(byte sleepMilliseconds) { _sleepMilliseconds = sleepMilliseconds; }

        public int GetFramePerSecond()
        {
            return _frameRate.GetFramePerSecond();
        }
        public void SetPriority(int nPriority)
        {
            _thread.Priority = (ThreadPriority)nPriority;
        }

        protected virtual bool _Start() { return true; }
        protected abstract void _Run();
        protected virtual void _End() { }

        private void __THREAD_ENTRY_POINT()
        {
            try
            {
                if (_Start())
                {
                    _threadState = ThreadState.RUN;

                    int sleepCnt = 0;
                    while (GetThreadState() == ThreadState.RUN)
                    {
                        try
                        {
                            _frameRate.CalcFramePerSecond();
                            _Run();
                        }
                        catch (Exception ex)
                        {
                            _logFunc.Log(ELogLevel.Err, "[Thread::_Run] " + ex.Message);
                        }

                        if (GetSleepPass() > 0 && ++sleepCnt >= GetSleepPass())
                        {
                            Thread.Sleep(_sleepMilliseconds);
                            sleepCnt = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //로그
                _logFunc.Log(ELogLevel.Err, "[Thread::Exit] " + ex.Message);
            }
            finally
            {
                _End();
                _endCheck.Set();
            }
        }
        private void _Join()
        {
            _thread.Join();
        }
    }
}
