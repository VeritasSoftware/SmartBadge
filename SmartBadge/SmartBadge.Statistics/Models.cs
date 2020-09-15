using System;
using System.Collections.Generic;

namespace SmartBadge.Statistics
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

    public class Statistic
    {
        public string Area { get; set; }
        public int SmartBadgeCount { get; set; }
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
}
