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
        private static ulong _allocUid = 0;
        private ulong _uid = 0;
        public ulong UId
        {
            get { return _uid; }
        }

        static private long s_userObjectCnt;

        protected SocketSession _session = null;
        protected ulong _objectID;

        protected ulong _accountDBKey;
        protected ulong _userDBKey;
        protected ulong _playerDBKey;

        protected short _gameDBIdx;
        protected short _logDBIdx;

        protected int _lastCheckTick = 0;
        protected int _timeOverCount = 0;
        protected int _maxTimerOverCount = 5;
        protected int _timeOverInterval = 60 * 1000;

        public UserObject()
        {
            _uid = ++_allocUid;
            Interlocked.Increment(ref UserObject.s_userObjectCnt);
        }
        
        ~UserObject()
        {
            Dispose(true);
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
            Interlocked.Decrement(ref UserObject.s_userObjectCnt);
        }
        #endregion

        public void SetSocketSession(SocketSession session) { _session = session;}
        public SocketSession GetSession() { return _session; }

        public ulong ObjectID { get { return _objectID; } set { _objectID = value; } }
        public ulong AccountDBKey { get { return _accountDBKey; } set { _accountDBKey = value; } }
        public ulong UserDBKey { get { return _userDBKey; } set { _userDBKey = value; } }
        public ulong PlayerDBKey { get { return _playerDBKey; } set { _playerDBKey = value; } }
        public short GameDBIdx { get { return _gameDBIdx; } set { _gameDBIdx = value; } }
        public short LogDBIdx { get { return _logDBIdx; } set { _logDBIdx = value; } }

        public static long GetUserObjCount() { return s_userObjectCnt; }
        public virtual void OnPacket(Packet packet)
        {
            _lastCheckTick = Environment.TickCount;
        }
        public virtual void OnAsyncTask(AsyncTaskObject task) { }
        public virtual void OnAccept(IPEndPoint ep) { }
        public virtual void OnConnect(IPEndPoint ep) { }
        public virtual void OnDisconnected() { }
        public virtual void OnClose() { }
        public virtual void OnConnectFailed() { }
        public virtual void OnSendComplete() { }
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

        public virtual void OnUpdate(float dt)
        {

        }
    }
}
