using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBadge.Statistics
{
    public interface ISmartBadgeStatistics
    {
        IEnumerable<Statistic> CountSmartBadgesByArea(DateTime start);
    }

    /// <summary>
    /// Class to compute Smart Badge statistics
    /// </summary>
    public class SmartBadgeStatistics : ISmartBadgeStatistics
    {
        MQTT _feed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="feed">The IMQTT feed</param>
        public SmartBadgeStatistics(MQTT feed)
        {
            _feed = feed;
        }

        /// <summary>
        /// Count smart badges by area
        /// </summary>
        /// <param name="start">The start time</param>
        /// <returns><see cref="IEnumerable{Statistic}"/></returns>
        public IEnumerable<Statistic> CountSmartBadgesByArea(DateTime start)
        {
            var end = start.AddSeconds(2);

            var packetsInInterval = _feed.GetPackets(start, end);

            var flattened = packetsInInterval.SelectMany(x => x.Details, (packet, detail) => new Flattened { Packet = packet, Detail = detail });

            var filteredByMaxStrength = flattened.GroupBy(x => x.Detail.BeaconMacAddress)
                                                 .Select(x => new Filtered
                                                 {
                                                     BeaconMacAddress = x.Key,
                                                     Flattened = x.Aggregate((y1, y2) => (y1.Detail.BeaconMacAddress == y2.Detail.BeaconMacAddress) && (y1.Detail.StrengthOfSignal > y2.Detail.StrengthOfSignal) ? y1 : y2)
                                                 });

            var countInArea = filteredByMaxStrength.GroupBy(x => x.Flattened.Packet.GatewayMacAddress)
                                                   .Select(x => new Statistic { Area = x.Key, SmartBadgeCount = x.Count() });

            return countInArea;
        }
    }
}
