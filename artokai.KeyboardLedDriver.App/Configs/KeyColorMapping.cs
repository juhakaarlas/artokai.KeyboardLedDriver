using System.Collections.Generic;
using KeyboardLedDriver.Common;

namespace KeyboardLedDriver.App.Configs
{
    public class KeyColorMapping
    {
        public string MonitoringTarget { get; set; }

        /// <summary>
        /// The list of key names to set when a state changes.
        /// E.g. F1, F2, F3, F4, PrintScreen, ScrollLock...
        /// </summary>
        public List<string> Keys { get; set; }

        /// <summary>
        /// The default key color. E.g. "non error" color.
        /// </summary>
        public ColorScheme DefaultColor { get; set; }

        /// <summary>
        /// The color to use for the given key in an error state.
        /// </summary>
        public ColorScheme ErrorColor { get; set; }
    }
}
