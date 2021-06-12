using System;
using KeyboardLedDriver.Common;
using Spectral;

namespace KeyboardLedDriver.Spectral
{
    public class SpectralLedController : ILedController
    {
        public bool CurrentAlertState { get; }
        public ColorScheme CurrentColorScheme { get; }
        
        public bool IsInitialized { get; private set; }

        private SpectralLedController _controller;

        public SpectralLedController()
        {
            _controller = new SpectralLedController();
        }
        
        public bool Initialize()
        {
            IsInitialized = _controller.Initialize();
            return IsInitialized;
        }

        public void SetColorScheme(ColorScheme scheme, bool showAlert)
        {
            throw new NotImplementedException();
        }

        public void ShutDown()
        {
            throw new NotImplementedException();
        }

        public void ToggleAlert(bool alertState)
        {
            throw new NotImplementedException();
        }
    }
}
