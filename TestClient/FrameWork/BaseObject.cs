using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient.FrameWork
{
    public abstract class BaseObject : IDisposable, IEventInterface
    {
        private BaseObject _parent = null;
        public BaseObject Parent
        {
            get => _parent;
            set => _parent = value;
        }
        private static UInt64 alloc_index = 0;
        private UInt64 _index = 0;
        public UInt64 Index
        {
            get { return _index; }
            private set { _index = value; }
        }


        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {

            }

            _disposed = true;
        }
        public BaseObject()
        {
            Index = ++alloc_index;
        }
        ~BaseObject()
        {
            Dispose(false);
        }

        public virtual void Enable()
        {

        }
        public virtual void Disable()
        {

        }

        public virtual void Init()
        {
            EntityManager.Instance.RegisterEntity(this);
        }
        public virtual void Release()
        {
            EntityManager.Instance.RemoveEntity(this);
        }
        public virtual bool OnMessage(Message message)
        {
            return false;
        }
    }
}
