using System;
using System.Collections.Generic;

namespace SmartBadge.Statistics
{
    public interface IMQTT
    {
        public IEnumerable<Packet> GetPackets(DateTime start, DateTime end);
    }
}
