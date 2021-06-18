namespace KeyboardLedDriver.Common
{
    public class ColorScheme
    {
        public static readonly ColorScheme Default = new ColorScheme() { R = 0, G = 167, B = 224 };

        public static readonly ColorScheme Vpn = new ColorScheme() { R = 51, G = 176, B = 59 };

        public ColorScheme()
        {
            
        }

        public ColorScheme(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }
    }
}
