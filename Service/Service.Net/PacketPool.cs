using Service.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public class PacketPool : ObjectPool<Packet>
    {

        public void Initialize(int initialCount = 1000)
        {
            for (int i = 0; i < initialCount; i++)
            {
                Add(CreatePoolObject());
            }
        }

        protected override Packet CreatePoolObject()
        {
            return new Packet();
        }

        public void Return(Packet packet)
        {
            packet.Clear();
            Add(packet);
        }
    }
}
