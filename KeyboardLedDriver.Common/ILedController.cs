using System;
using System.Collections.Generic;

namespace KeyboardLedDriver.Common
{
    public interface ILedController
    {
        bool CurrentAlertState { get; }
        ColorScheme CurrentColorScheme { get; }
        bool IsInitialized { get; }

        bool Initialize();
        void SetColorScheme(ColorScheme scheme, bool showAlert);
        bool SetColorForKeys(ColorScheme color, List<string> keys);

        void ShutDown();
        void ToggleAlert(bool alertState);
    }
}