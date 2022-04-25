using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public class SessionEventPool : ObjectPool<SessionEvent>
    {
        public void Initialize(int initialCount = 1000)
        {
            for (int i = 0; i < initialCount; i++)
            {
                Add(CreatePoolObject());
            }
        }

        protected override SessionEvent CreatePoolObject()
        {
            return new SessionEvent();
        }

        public void Return(SessionEvent evt)
        {
            evt.Clear();
            Add(evt);
        }
    }
}
