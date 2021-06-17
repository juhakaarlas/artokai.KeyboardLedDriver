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
            //Arrange
            var testee = new NetworkStatusProvider();
            //Change the name of the interface to something on your own computer
            testee.Interfaces.Add("Pulse Secure");
            testee.PollingInterval = 5;
            var receivedEvent = Assert.Raises<StatusChangedEventArgs>(handler => testee.StatusChanged += handler,
                handler => testee.StatusChanged -= handler, () => testee.StartMonitoring());

            //Act
            bool up = testee.IsUp;
            testee.StopMonitoring();

            //Assert
            Assert.True(up);
            Assert.False(receivedEvent.Arguments.IsErrorState);
        }
    }
}
