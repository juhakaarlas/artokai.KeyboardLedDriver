using KeyboardLedDriver.Common;
using System;

namespace KeyboarLedDriver.Corsair
{
    public class CorsairLedController : ILedController, IDisposable
    {
        private bool disposedValue;

        public bool CurrentAlertState => throw new NotImplementedException();

        public ColorScheme CurrentColorScheme => throw new NotImplementedException();

        public bool IsInitialized => throw new NotImplementedException();

        public bool Initialize()
        {
            throw new NotImplementedException();
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
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
