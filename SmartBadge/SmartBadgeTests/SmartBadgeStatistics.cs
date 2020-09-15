using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBadgeTests
{
    public class Packet
    {
        public string GatewayMacAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<Detail> Details { get; set; }
    }

    public class Detail
    {
        public string BeaconMacAddress { get; set; }
        public int StrengthOfSignal { get; set; }
    }

    public class Flattened
    {
        public Packet Packet { get; set; }
        public Detail Detail { get; set; }
    }

    public class Filtered
    {
        public Flattened Flattened { get; set; }
        public string BeaconMacAddress { get; set; }
    }

    public class Statistic
    {
        public string Area { get; set; }
        public int SmartBadgeCount { get; set; }
    }


    public interface IMQTT
    {
        public IEnumerable<Packet> GetPackets(DateTime start, DateTime end);
    }


    public class SmartBadgeStatistics
    {
        IMQTT _feed;

        public SmartBadgeStatistics(IMQTT feed)
        {
            _feed = feed;
        }

        public IEnumerable<Statistic> CountSmartBadgesByArea (DateTime start)
        {
            var end = start.AddSeconds(2);

            var packetsInInterval = _feed.GetPackets(start, end);

            var flattened = packetsInInterval.SelectMany(x => x.Details, (packet, detail) => new Flattened { Packet = packet, Detail = detail });

            var maxStrength = flattened.GroupBy(x => x.Detail.BeaconMacAddress).Select(x => new Filtered { BeaconMacAddress = x.Key, Flattened = x.Aggregate((y1, y2) => (y1.Detail.BeaconMacAddress == y2.Detail.BeaconMacAddress) && (y1.Detail.StrengthOfSignal > y2.Detail.StrengthOfSignal) ? y1 : y2) });

            var countInArea = maxStrength.GroupBy(x => x.Flattened.Packet.GatewayMacAddress).Select(x => new Statistic { Area = x.Key, SmartBadgeCount = x.Count() });

            return countInArea;
        }
    }
}
