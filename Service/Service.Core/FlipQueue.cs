using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core
{
    public sealed class FlipQueue<T>
    {
        private object _lockObject;
        private Queue<T> _queue1;
        private Queue<T> _queue2;
        private Queue<T> _refInput;
        private Queue<T> _refOutput;

        public FlipQueue()
        {
            _queue1 = new Queue<T>();
            _queue2 = new Queue<T>();
            _refInput = _queue1;
            _refOutput = _queue2;
            _lockObject = new object();
        }

        public void Enqueue(T item)
        {
            lock (_lockObject)
            {
                _refInput.Enqueue(item);
            }
        }

        public Queue<T> GetQueue()
        {
            Flip();
            return _refOutput;
        }

        public int Count()
        {
            return _refInput.Count;
        }

        private void Flip()
        {
            lock(_lockObject)
            {
                Queue<T> tmp = _refInput;
                _refInput = _refOutput;
                _refOutput = tmp;
            }
        }
    }
}
