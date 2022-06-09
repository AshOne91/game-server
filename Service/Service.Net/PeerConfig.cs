using System;

namespace Service.Net
{
    public class PeerConfig
    {
        public bool NoDelay = true;
        public bool LingerState = false;
        public bool KeepAlive = true;
        public int KeepAliveInterval = 5000;
        public bool ReuseAddress = true;

        public int PendingConnectionBacklog = 200;
        public int ConnectTimeout = 15000;
        public int ReceiveBufferSize = 65536;
        public int SendBufferSize = 65536;
        public int ReceiveTimeout = 0;
        public int SendTimeout = 0;

        public bool AllowNatTraversal = false;
        //임시로 true해놓음 출시시 false 되도록 확인
        public bool UseSessionEventQueue = true;

        public int SendQueueSize = 100;
        public int PacketPoolCount = 1000;
        public int EvtPoolCount = 2000;
        public int EvtQueueSize = 10000000;
        public int DBThreadCount = 6;

        public PeerConfig() {}
    }
}
