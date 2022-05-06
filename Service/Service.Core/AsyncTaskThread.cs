using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Service.Core
{
    class AsyncTaskThread
    {
        protected FlipQueue<AsyncTaskObject> _asyncTaskQueue;
        private AsyncTaskProcedure _procedure;
        private EventWaitHandle _eventWait;
        private Thread _thread;
        private bool _running = false;
        private long _lastExecuteTick = 0;
        public bool Create(AsyncTaskProcedure proc)
        {
            _procedure = proc;

            _asyncTaskQueue = new FlipQueue<AsyncTaskObject>();
            _lastExecuteTick = Environment.TickCount;
            _thread = new Thread(Run);
            _thread.Start();

            return true;
        }

        public void Destroy()
        {
            _running = false;

            _eventWait.Set();
            _eventWait.Set();
            _eventWait.Set();
        }

        public bool EnqueueAsyncTask(AsyncTaskObject task, int taskQueueLimitCount = 10000)
        {
            if (!_running)
            {
                return false;
            }

            lock(_asyncTaskQueue)
            {
                int cnt = _asyncTaskQueue.Count();
                if (cnt > taskQueueLimitCount)
                {
                    _eventWait.Set();
                    return false;
                }
                _asyncTaskQueue.Enqueue(task);
                _eventWait.Set();
                return true;
            }
        }

        private void Run()
        {
            _running = true;
            _eventWait = new EventWaitHandle(false, EventResetMode.AutoReset);

            while (_running)
            {
                if (_eventWait.WaitOne())
                {
                    Queue<AsyncTaskObject> queue = _asyncTaskQueue.GetQueue();
                    foreach(AsyncTaskObject task in queue)
                    {
                        _procedure.Execute(task);
                        _lastExecuteTick = Environment.TickCount;
                    }
                    queue.Clear();
                }
            }
        }
    }
}
