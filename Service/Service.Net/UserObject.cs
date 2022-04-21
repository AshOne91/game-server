using Service.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Service.Net
{
    public class UserObject : IDisposable
    {
        static private long s_userObjectCnt;
        SocketSession _session = null;
        protected ulong _objectID;
        protected int _lastCheckTick = 0;
        protected int _timeOverCount = 0;
        protected int _maxTimerOverCount = 5;
        protected int _timeOverInterval = 60 * 1000;


        public UserObject()
        {
            Interlocked.Increment(ref UserObject.s_userObjectCnt);
        }
        
        ~UserObject()
        {
            Interlocked.Decrement(ref UserObject.s_userObjectCnt);
        }

        #region IDisposable Members
        public bool disposed;
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            try
            {
                if (disposing)
                {

                }
            }
            catch
            {

            }
            finally
            {
                this.disposed = true;
            }

            this.disposed = true;
        }
        #endregion

        public void SetSocketSession(SocketSession session) { _session = session;}
        public SocketSession Session() { return _session; }

        public ulong GetObjectID() { return _objectID; }
        public static long GetUserObjCount() { return s_userObjectCnt; }
        public virtual void OnPacket(Packet packet)
        {
            _lastCheckTick = Environment.TickCount;
        }
        public virtual void OnAsyncTask(AsyncTaskObject task) { }
        public virtual void OnAccept(IPEndPoint ep) { }
        public virtual void OnConnect(IPEndPoint ep) { }
        public virtual void OnClose() { }
        public virtual void OnSendComplate() { }
        public virtual void OnFailedKeepAlive() { }
        public virtual void CheckKeepALive()
        {
            if (Environment.TickCount - _lastCheckTick > _timeOverInterval)
            {
                _timeOverCount++;
            }

            if (_timeOverCount >= _maxTimerOverCount)
            {
                OnFailedKeepAlive();
            }
            else
            {
                _timeOverCount = 0;
            }
        }
    }
}
