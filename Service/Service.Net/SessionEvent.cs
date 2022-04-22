using Service.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Service.Net
{
    public sealed class SessionEvent : IDisposable
    {
        public enum EvtType
        {
            RecvPacket,
            SendComplete,
            Accept,
            Connect,
            ConnectFailed,
            LocalDisconnected,
            RemoteDisconnected,
            SocketError,
            Timer,
            UserEvent,
            AsyncTask
        }

        public EvtType evtType;
        public SocketSession session;
        public Packet packet = null;
        public AsyncTaskObject task = null;
        public IPEndPoint localEP;
        public IPEndPoint remoteEP;
        public int transBytes = 0;
        public String msg;

        public void Clear()
        {
            evtType = 0;
            session = null;
            packet = null;
            transBytes = 0;
            session = null;
            msg = "";
        }

        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

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
    }
}
