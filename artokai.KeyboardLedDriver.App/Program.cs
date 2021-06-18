using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Artokai.KeyboardLedDriver;
using KeyboardLedDriver.App.Configs;
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

        private static INetworkStatusProvider _vpnProvider;

        private static KeyColorMapping _vpnColorMapping = new KeyColorMapping()
        {
            DefaultColor = new ColorScheme(58, 178, 140), 
            ErrorColor = new ColorScheme(224, 0, 103), 
            Keys = new List<string>()
            {
                "V", "B", "N" 
            }
        };

        private static KeyColorMapping _buildColorMapping = new KeyColorMapping()
        {
            MonitoringTarget = "DataCollector_CI",
            DefaultColor = new ColorScheme(58, 178, 140),
            ErrorColor = new ColorScheme(224, 0, 103),
            Keys = new List<string>()
            {
                "F1", "F2", "F3", "F4"
            }
        };

        static void Main(string[] args)
        {
            var devOpsConfig = AppConfig.Instance.AzDevOpsConfig;

             AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            
            _controller = new GenericLedController();
            _controller.Initialize();

            _vpnProvider = new NetworkStatusProvider()
                {Interfaces = new List<string>() {"Pulse Secure"}, PollingInterval = 120};
            
            _provider = new AzureDevOpsProvider(devOpsConfig.DevOpsOrganization, 
                                                devOpsConfig.Project, 
                                                devOpsConfig.AccessToken);
            _vpnProvider.StatusChanged += _vpnProvider_StatusChanged;
            _provider.PollingInterval = devOpsConfig.Interval;
            _provider.StatusChanged += BuildStatusChanged;
            _provider.BuildNames.AddRange(devOpsConfig.Pipelines);

            try
            {
                _controller.SetColorForKeys(_buildColorMapping.DefaultColor, _buildColorMapping.Keys);
                
                _provider.StartMonitoring();
                _vpnProvider.StartMonitoring();
                if (_vpnProvider.IsUp)
                {
                    _controller.SetColorForKeys(_vpnColorMapping.DefaultColor, _vpnColorMapping.Keys);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }

            Console.Out.WriteLine("Monitoring - Press <Enter> to cancel.");
            Console.In.ReadLine();
            _provider.StopMonitoring();
            _vpnProvider.StopMonitoring();
            _controller.ShutDown();
        }

        private static void _vpnProvider_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            _controller.SetColorForKeys(e.IsErrorState ? _vpnColorMapping.ErrorColor : _vpnColorMapping.DefaultColor,
                _vpnColorMapping.Keys);
        }

        private static void BuildStatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (e.Source.Equals(_buildColorMapping.MonitoringTarget))
            {
                _controller.SetColorForKeys(
                    e.IsErrorState ? _buildColorMapping.ErrorColor : _buildColorMapping.DefaultColor,
                    _buildColorMapping.Keys);
            }
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
