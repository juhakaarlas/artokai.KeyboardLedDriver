using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyboardLedDriver.Common
{
    public interface IStatusProvider
    {
        /// <summary>
        /// This event is raised if the status of the monitoring target was changed.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Starts the monitoring process.
        /// </summary>
        /// <returns><c>true</c> if the process was started successfully. Otherwise false.</returns>
        bool StartMonitoring();

        /// <summary>
        /// Stops the monitoring process.
        /// </summary>
        /// <returns><c>true</c> when the monitoring process has been successfully stopped. Otherwise false.</returns>
        bool StopMonitoring();

        /// <summary>
        /// Returns <c>true</c> if the monitoring is in progress.
        /// </summary>
        public bool IsMonitoring { get; }
    }

    public class StatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns <c>true</c> if the new state is an error state
        /// </summary>
        public bool IsErrorState { get; set; }
    }
}