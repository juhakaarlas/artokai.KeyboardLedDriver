using KeyboardLedDriver.StatusProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardLedDriver.Common;
using Xunit;

namespace KeyboardLedDriver.Tests
{
    public class NetworkProviderTests
    {
        [Fact]
        public void CanReadNetworkInterfaces()
        {
            var testee = new NetworkStatusProvider();
            testee.Interfaces.Add("USB-C Ethernet");
            testee.PollingInterval = 5;

            var receivedEvent = Assert.Raises<StatusChangedEventArgs>(handler => testee.StatusChanged += handler,
                handler => testee.StatusChanged -= handler, () => testee.StartMonitoring());

            bool up = testee.IsUp;
            testee.StopMonitoring();
            Assert.True(up);

            Assert.False(receivedEvent.Arguments.IsErrorState);
        }
    }
}
