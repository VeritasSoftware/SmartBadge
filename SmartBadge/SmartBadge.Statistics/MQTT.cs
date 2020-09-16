using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBadge.Statistics
{
    public interface IMQTT
    {
        public IEnumerable<Packet> GetPackets(DateTime start, DateTime end);
    }

    public class MQTT : IMQTT
    {
        private readonly IEnumerable<Packet> _packets;

        public MQTT(IEnumerable<Packet> packets)
        {
            _packets = packets;
        }

        public virtual IEnumerable<Packet> GetPackets(DateTime start, DateTime end)
        {
            return _packets.Where(p => p.CreatedOn >= start && p.CreatedOn < end);
        }
    }
}
