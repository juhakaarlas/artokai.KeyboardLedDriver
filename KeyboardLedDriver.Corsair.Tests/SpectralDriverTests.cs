using KeyboardLedDriver.Common;
using KeyboardLedDriver.Generic;
using System;
using Xunit;

namespace KeyboardLedDriver.Tests
{
    public class SpectralFixture : IDisposable
    {
        public GenericLedController Controller { get; private set; }

        public SpectralFixture()
        {
            Controller = new GenericLedController();
            Controller.Initialize();
        }

        public void Dispose()
        {
            Controller.ShutDown();
        }
    }

    public class SpectralDriverTests :IClassFixture<SpectralFixture>
    {
        private SpectralFixture _fixture;

        public SpectralDriverTests(SpectralFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TrySetColorScheme()
        {
           _fixture.Controller.SetColorScheme(new ColorScheme { R = 255, G = 255, B = 255 }, false);
        }

        [Fact]
        public void TrySetErrorColorScheme()
        {
            _fixture.Controller.SetColorScheme(new ColorScheme { R = 255, G = 255, B = 255 }, true);
        }
    }
}