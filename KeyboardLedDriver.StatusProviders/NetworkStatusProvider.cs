using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using KeyboardLedDriver.Common;

namespace KeyboardLedDriver.StatusProviders
{
    public class NetworkStatusProvider : PollingProviderBase, INetworkStatusProvider
    {
        private CancellationTokenSource _cts;

        /// <inheritdoc/>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// The list of network interface names to monitor
        /// </summary>
        public List<string> Interfaces { get; set; }


        public bool IsUp { get; private set; }

        public bool IsMonitoring { get; private set; }

        public NetworkStatusProvider()
        {
            Interfaces = new List<string>();
        }
        
        public bool StartMonitoring()
        {
            if (IsMonitoring) return true;
            _cts = new CancellationTokenSource();

            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

            Run().ConfigureAwait(false);

            return IsMonitoring = true;
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            OnTick();
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            IsUp = e.IsAvailable;
        }

        private async Task<Task> Run()
        {
            if (PollingInterval <= 0) return Task.CompletedTask;

            while (!_cts.IsCancellationRequested)
            {
                OnTick();
                await Task.Delay(TimeSpan.FromSeconds(PollingInterval), _cts.Token);
            }

            return Task.CompletedTask;
        }

        private void OnTick()
        {
            bool currentState = IsUp;

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                IsUp = false;
                return;
            }

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (networkInterfaces.Length == 0)
            {
                IsUp = false;
                return;
            }

            bool areAllUp = true;

            foreach (var nic in Interfaces)
            {
                var nif = networkInterfaces.FirstOrDefault(n => n.Name.Equals(nic));
                areAllUp &= nif?.OperationalStatus == OperationalStatus.Up;
            }

            IsUp = areAllUp;

            if (currentState != IsUp)
            {
                StatusChanged?.Invoke(this,new StatusChangedEventArgs(){ IsErrorState = !IsUp });
            }
        }

        public bool StopMonitoring()
        {
            NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAvailabilityChanged;
            _cts.Cancel();

            return IsMonitoring = false;
        }
    }
}