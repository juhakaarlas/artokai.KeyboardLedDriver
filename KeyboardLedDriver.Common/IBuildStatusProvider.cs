using System.Collections.Generic;

namespace KeyboardLedDriver.Common
{
    /// <summary>
    /// Specific interface for build status monitoring.
    /// </summary>
    public interface IBuildStatusProvider : IStatusProvider
    {
        /// <summary>
        /// List of build definition names to monitor
        /// </summary>
        List<string> BuildNames { get; }

        /// <summary>
        /// Build status polling interval in seconds
        /// </summary>
        public int PollingInterval { get; set; }
    }
}