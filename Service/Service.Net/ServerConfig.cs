using System;

namespace Service.Net
{
    public class ServerConfiguration
    {
        public bool noDelay = true;
        public bool lingerState = false;
        public bool keepAlive = true;
        public int keepAliveInterval = 5000;
        public bool reuseAddress = true;

        public int pendingConnectionBacklog = 200;
        public int connectTimeout = 15000;
        public int receiveBufferSize = 65536;
        public int sendBufferSize = 65536;
        public int receiveTimeout = 0;
        public int sendTimeout = 0;

        public bool allowNatTraversal = false;
        public bool useSessionEventQueue = false;

        public int sendQueueSize = 100;
        public int packetPoolCount = 1000;
        public int evtPoolCount = 2000;
        public int evtQueueSize = 10000000;
        public int dbThreadCount = 6;

        public ServerConfiguration() {}
    }
}
