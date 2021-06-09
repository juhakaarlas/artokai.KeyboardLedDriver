namespace KeyboardLedDriver.Common
{
    public interface ILedController
    {
        bool CurrentAlertState { get; }
        ColorScheme CurrentColorScheme { get; }
        bool IsInitialized { get; }

        bool Initialize();
        void SetColorScheme(ColorScheme scheme, bool showAlert);
        void ShutDown();
        void ToggleAlert(bool alertState);
    }
}