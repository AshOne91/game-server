using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public class PriorityLock
    {
        public PriorityLock(int priority)
        {
            _priority = priority;
#if DEBUG
            if (priority < 0)
                throw new Exception("Priority must be greater then -1 ");

            bool found = PriortyList.Contains(priority);
            if (found)
            {
                throw new Exception("Duplicated priority number");
            }
#endif
        }
        public void Enter(string alias)
        {
#if DEBUG
            if (LastPriorty > 0 && LastPriorty < _priority)
            {
                string priorityLog = "";
                foreach (int p in PriortyStack.ToArray())
                {
                    priorityLog += p.ToString() + "\r\n";
                }

                string aliasLog = alias + "\r\n";
                foreach (string a in AliasStack.ToArray())
                {
                    aliasLog += a + "\r\n";
                }
                string lastLog = "Lock sequence error!\r\n" + priorityLog + aliasLog;
                Logger.WriteFileLog("Exception Message : " + lastLog, "/exception");

                throw new Exception(lastLog);
            }

            LastPriorty = _priority;
            PriortyStack.Push(LastPriorty);
            AliasStack.Push(alias);
#endif
            System.Threading.Monitor.Enter(_lockObject);

        }
        public void Exit()
        {
#if DEBUG
            PriortyStack.Pop();
            AliasStack.Pop();

            if (PriortyStack.Count > 0)
                LastPriorty = PriortyStack.Peek();
            else
                LastPriorty = -1;

#endif
            System.Threading.Monitor.Exit(_lockObject);
        }

#if DEBUG
        public static int LastPriorty
        {
            get
            {
                return _lastPriorty;
            }
            set
            {
                _lastPriorty = value;
            }
        }
        public static List<int> PriortyList
        {
            get
            {
                if (_priortyList == null)
                    _priortyList = new List<int>();
                return _priortyList;
            }
        }
        public static Stack<int> PriortyStack
        {
            get
            {
                if (_priortyStack == null)
                    _priortyStack = new Stack<int>();
                return _priortyStack;
            }
        }
        public static Stack<string> AliasStack
        {
            get
            {
                if (_aliasStack == null)
                    _aliasStack = new Stack<string>();
                return _aliasStack;
            }
        }

        [ThreadStatic]
        private static int _lastPriorty = -1;
        [ThreadStatic]
        private static List<int> _priortyList = new List<int>();
        [ThreadStatic]
        private static Stack<int> _priortyStack = new Stack<int>();
        [ThreadStatic]
        private static Stack<string> _aliasStack = new Stack<string>();

#endif
        private int _priority;
        private object _lockObject = new object();

        // try { Enter() ... } finally { Exit() } 형태의 코드를 단순하게 만들고 싶을 때
        // using(NwPriorityLock.ScopeLock(_MyLock, "MyAlias")) { ... } 처럼 사용
        public static ScopeLockGuard ScopeLock(PriorityLock InLock, string alias)
        {
            return new ScopeLockGuard(InLock, alias);
        }

        public struct ScopeLockGuard : IDisposable
        {
            public ScopeLockGuard(PriorityLock InTarget, string alias)
            {
                _TargetLock = InTarget;
                _TargetLock.Enter(alias);
            }

            public void Dispose()
            {
                _TargetLock.Exit();
                _TargetLock = null;
            }

            private PriorityLock _TargetLock;
        }
    }
}
