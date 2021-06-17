using System;
using System.Configuration;
using System.IO;
using Artokai.KeyboardLedDriver;
using KeyboardLedDriver.Common;
using KeyboardLedDriver.Spectral;
using KeyboardLedDriver.StatusProviders;
using Microsoft.Extensions.Configuration;

namespace KeyboardLedDriver.App
{
    class Program
    {
        private static ILedController _controller;

        private static IBuildStatusProvider _provider;

        static void Main(string[] args)
        {
            var devOpsConfig = AppConfig.Instance.AzDevOpsConfig;

             AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            
            _controller = new GenericLedController();
            _controller.Initialize();

            _provider = new AzureDevOpsProvider(devOpsConfig.DevOpsOrganization, devOpsConfig.Project, devOpsConfig.AccessToken);
            _provider.PollingInterval = devOpsConfig.Interval;
            _provider.StatusChanged += Provider_StatusChanged;
            _provider.BuildNames.AddRange(devOpsConfig.Pipelines);

            try
            {
                _controller.SetColorScheme(null, false);
                _provider.StartMonitoring();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }

            Console.Out.WriteLine("Monitoring - Press <Enter> to cancel.");
            Console.In.ReadLine();
            _provider.StopMonitoring();
            _controller.ShutDown();
        }

        private static void Provider_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            _controller.SetColorScheme(null, e.IsErrorState);
        }

        private static void NetworkAddressChanged(object sender, EventArgs e)
        {
            
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            _provider?.StopMonitoring();
            _controller?.ShutDown();
        }
    }
}
