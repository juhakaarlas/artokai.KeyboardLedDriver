using KeyboardLedDriver.Common;
using System;

namespace KeyboarLedDriver.Corsair
{
    public class CorsairLedController : ILedController
    {
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
    }
}
