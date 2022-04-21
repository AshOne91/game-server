using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public sealed class SocketSession : IDisposable
    {
        private readonly ServerApp _serverApp;
        private readonly ServerConfig _config;
        private readonly SessionManager _sessionManager;

        private ulong _uid;

        public SocketSession(ulong uid, ServerApp serverApp, ServerConfig config, SessionManager manager)
        {
            _uid = uid;
            _serverApp = serverApp;
            _config = config;
            _sessionManager = manager;
        }

        SocketSession() { }
        ~SocketSession()
        {
            this.Dispose(false);
        }
        public ulong GetUid() { return _uid; }
        public void SetUserObject(UserObject userObj) { _userObject = uwerObj; }
    }
}
