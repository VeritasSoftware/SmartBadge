using Moq;
using SmartBadge.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartBadgeTests
{
    public class SmartBadgeTests
    {
        [Fact]
        public void SmartBadgeCountByArea()
        {
            var mockMQTT = new Mock<IMQTT>();

            mockMQTT.Setup(x => x.GetPackets(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<Packet>
                    {
                        new Packet
                        {
                            GatewayMacAddress ="1",
                            Details = new List<Detail>
                            {
                                //Duplicate
                                new Detail
                                {
                                    BeaconMacAddress = "100",
                                    StrengthOfSignal = 5                                    
                                },
                                new Detail
                                {
                                    BeaconMacAddress = "200",
                                    StrengthOfSignal = 5
                                }
                            }
                        },
                        new Packet
                        {
                            GatewayMacAddress ="2",
                            Details = new List<Detail>
                            {
                                //Duplicate
                                new Detail
                                {
                                    BeaconMacAddress = "100",
                                    StrengthOfSignal = 6
                                },
                                new Detail
                                {
                                    BeaconMacAddress = "300",
                                    StrengthOfSignal = 8
                                }
                            }
                        }
                    });

            var statistics = new SmartBadgeStatistics(mockMQTT.Object);

            var smartBadgeCountInArea = statistics.CountSmartBadgesByArea(DateTime.Now);

            Assert.True(smartBadgeCountInArea.Count() == 2);
            var area2 = smartBadgeCountInArea.First();
            Assert.True(area2.Area == "2" && area2.SmartBadgeCount == 2);
            var area1 = smartBadgeCountInArea.Last();
            Assert.True(area1.Area == "1" && area1.SmartBadgeCount == 1);
        }
    }
}
