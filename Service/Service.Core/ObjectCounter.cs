using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Service.Core
{
    public class ObjectCounter : IDisposable
    {
        public ObjectCounter()
        {
            Default._ObjectCount = Interlocked.Increment(ref Default._ObjectCount);
        }
        ~ObjectCounter()
        {
            Dispose();
        }


        private long _ObjectCount = 0;
        public static ObjectCounter Default = new ObjectCounter();

        private bool disposed;

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (this.disposed)
                return;

            try
            {
                if(disposing)
                {

                }
            }
            catch
            {

            }
            finally
            {

            }

            Default._ObjectCount = Interlocked.Decrement(ref Default._ObjectCount);
            disposed = true;
        }
    }
}
