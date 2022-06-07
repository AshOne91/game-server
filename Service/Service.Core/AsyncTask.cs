using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Service.Core
{
    public class AsyncTask
    {
        private List<AsyncTaskThread> _threadArray;
        private List<AsyncTaskProcedure> _procArray;
        public bool Create(int threadCount, int maxQueueCount = 100)
        {
            _threadArray = new List<AsyncTaskThread>();
            _procArray = new List<AsyncTaskProcedure>();
            _CreateAsyncTaskThread(threadCount);
            return true;
        }
        public void Destroy()
        {
            for (int i = 0; i < _threadArray.Count; ++i)
            {
                _threadArray[i].Destroy();
            }
        }

        public bool Execute(ulong key, AsyncTaskObject task, int waitSleepTime = 16)
        {
            ulong threadCount = GetThreadCount();
            if (GetThreadCount() < 1)
            {
                throw new Exception("No Task Thread!");
            }
            ulong idx = key % threadCount;

            AsyncTaskThread taskThread = _threadArray[(int)idx];
            bool result = taskThread.EnqueueAsyncTask(task);

            if (result)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    result = taskThread.EnqueueAsyncTask(task);
                    if (result)
                        return true;
                    else
                    {
                        Thread.Sleep(waitSleepTime);
                    }
                }
            }
            return false;
        }

        public ulong GetThreadCount() { return (ulong)_threadArray.Count; }

        public virtual bool _CreateAsyncTaskThread(int threadCount)
        {
            for ( int i = 0; i < threadCount; ++i)
            {
                AsyncTaskThread taskThread = new AsyncTaskThread();
                AsyncTaskProcedure proc = _CreateAsyncTaskProcedure();

                bool result = taskThread.Create(proc);
                if (!result)
                {
                    return false;
                }
                _threadArray.Add(taskThread);
            }

            return true;
        }
        public virtual AsyncTaskProcedure _CreateAsyncTaskProcedure()
        {
            return null;
        }
    }
}
