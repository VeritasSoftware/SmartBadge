using System;
using System.Collections.Generic;

namespace SmartBadgeTests
{
    public interface IMQTT
    {
        public IEnumerable<Packet> GetPackets(DateTime start, DateTime end);
    }
}
