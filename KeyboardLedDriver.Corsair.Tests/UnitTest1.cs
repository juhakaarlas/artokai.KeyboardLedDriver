using KeyboardLedDriver.Common;
using KeyboarLedDriver.Corsair;
using Xunit;

namespace KeyboardLedDriver.Corsair.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TrySetColorScheme()
        {
            using (var ctrl = new CorsairLedController())
            {
                
                Assert.True(ctrl.Initialize());

                ctrl.SetColorScheme(new ColorScheme { R = 255, G = 255, B = 255 }, false); 
            }
        }

        [Fact]
        public void TrySetErrorColorScheme()
        {
            using (var ctrl = new CorsairLedController())
            {
                Assert.True(ctrl.Initialize());

                ctrl.SetColorScheme(new ColorScheme { R = 255, G = 255, B = 255 }, true);
                Assert.True(true);
            }
        }
    }
}
