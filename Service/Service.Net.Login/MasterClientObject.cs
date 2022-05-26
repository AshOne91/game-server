using GameBase.Common;
using GameBase.Controller;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Service.Net.Login
{
    public sealed class MasterClientObject : UserObject
    {
        private PacketAbuseChecker _packetAbuser;
        private List<string> _userLog = new List<string>();
        private int _serverId = -1;
        private GameBaseController _baseController = null;

        public MasterClientObject()
        {
            _objectID = (int)ObjectType.Master;
            _packetAbuser = new PacketAbuseChecker();
            _baseController = new GameBaseController(this);
        }
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public override void Dispose(bool disposing)
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

        public override void OnAccept(IPEndPoint ep)
        {

        }
        public override void OnConnect(IPEndPoint ep)
        {

        }
        public override void OnClose()
        {

        }
        public override void OnSendComplete()
        {
        }
        public void Disconnect(bool force = true)
        {
            if (force)
            {
                Log(ELogLevel.Err, "Force Disconnect!");
            }
            GetSession().Disconnect();
        }
        public void SendPacket(GPacket packet, bool canDrop = false)
        {
            _session.SendPacket(packet.Serialize(), canDrop);
            Log((ELogLevel.Trace), packet.GetLog());
        }

        public void Log(ELogLevel level, string format, params object[] args)
        {
            if (level < Logger.Default.GetLogLevel())
            {
                return;
            }

            string log = "";
            if (args.Length > 0)
            {
                log = string.Format(format, args);
            }
            else
            {
                log = format;
            }

            Logger.Default.Log(level, log);
            UserLog(log);
        }

        [Conditional("UserLog")]
        void UserLog(string log)
        {
            _userLog.Add(log);
            if (_userLog.Count > 200)
            {
                _userLog.RemoveAt(0);
            }
        }
        public override void OnPacket(Packet packet)
        {
            Log(ELogLevel.Trace, "OnPacket {0}", (packet.GetId()).ToString());
            _baseController._protocol.OnPacket(this, packet.GetId(), packet);
        }




    }
}
