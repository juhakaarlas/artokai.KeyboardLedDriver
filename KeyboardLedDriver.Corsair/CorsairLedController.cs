using Corsair.CUE.SDK;
using KeyboardLedDriver.Common;
using System;

namespace KeyboardLedDriver.Corsair
{
    public class CorsairLedController : ILedController, IDisposable
    {
        private readonly CorsairLedId[] ALERT_KEYS = 
            {
                CorsairLedId.CLK_Escape,
                CorsairLedId.CLK_F1,
                CorsairLedId.CLK_F2,
                CorsairLedId.CLK_F3,
                CorsairLedId.CLK_F4,
                CorsairLedId.CLK_F5,
                CorsairLedId.CLK_F6,
                CorsairLedId.CLK_F7,
                CorsairLedId.CLK_F8,
                CorsairLedId.CLK_F9,
                CorsairLedId.CLK_F10,
                CorsairLedId.CLK_F11,
                CorsairLedId.CLK_F12,
                CorsairLedId.CLK_PrintScreen,
                CorsairLedId.CLK_ScrollLock,
                CorsairLedId.CLK_PauseBreak
            };

        private bool disposedValue;

        public bool CurrentAlertState { get; private set; } = false;

        public ColorScheme CurrentColorScheme { get; private set; }

        public ColorScheme AlertColorScheme { get; set; }

        public bool IsInitialized { get; private set; }

        private int _deviceCount;

        public bool Initialize()
        {
            CUESDK.CorsairPerformProtocolHandshake();
            
            _deviceCount = CUESDK.CorsairGetDeviceCount();
            
            if (_deviceCount < 1)
            {
                ProcessError();
            }

            var deviceInfo = CUESDK.CorsairGetDeviceInfo(0);

            if (deviceInfo == null)
            {
                ProcessError();
            }
           
            IsInitialized = _deviceCount > 0;

            if (AlertColorScheme == null)
            {
                AlertColorScheme = new ColorScheme { R = 255, G = 10, B = 10 };
            }
                        
            return IsInitialized;
        }

        private void ProcessError()
        {
            var error = CUESDK.CorsairGetLastError();
            throw new Exception($"Corsair error {error}");
        }

        public void SetColorScheme(ColorScheme scheme, bool showAlert)
        {
            if (!IsInitialized) return;

            CorsairLedColor[] colorSet = 
                { 
                    new CorsairLedColor
                    {
                        ledId = CorsairLedId.CLK_Escape,
                        r = scheme.R,
                        g = scheme.G,
                        b = scheme.B
                    }
            };

            if (showAlert)
            {
                colorSet = new CorsairLedColor[ALERT_KEYS.Length];

                for(int i = 0; i < ALERT_KEYS.Length; i++)
                {
                    colorSet[i] = new CorsairLedColor 
                    { 
                        ledId = ALERT_KEYS[i], 
                        r = AlertColorScheme.R, 
                        g = AlertColorScheme.G,  
                        b = AlertColorScheme.B 
                    };
                }
            }

            CUESDK.CorsairSetLedsColorsBufferByDeviceIndex(0, colorSet.Length, colorSet);
            CUESDK.CorsairSetLedsColorsFlushBuffer();
        }

        public void ShutDown()
        {
            CUESDK.CorsairUnsubscribeFromEvents();
        }

        public void ToggleAlert(bool alertState)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CorsairLedController()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
