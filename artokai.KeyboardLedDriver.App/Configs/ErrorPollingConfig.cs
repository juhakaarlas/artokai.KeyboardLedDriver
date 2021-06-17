namespace KeyboardLedDriver.App.Configs
{
    public class ErrorPollingConfig
    {
        public bool Enabled { get; set; } = false;
        public int Interval { get; set; } = 15;
        public string Url { get; set; } = "";
    }
}
