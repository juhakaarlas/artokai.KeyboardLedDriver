using System;
using Artokai.KeyboardLedDriver;
using KeyboardLedDriver.Common;
using KeyboardLedDriver.Generic;
using KeyboardLedDriver.StatusProviders;

namespace KeyboardLedDriver.App
{
    class Program
    {
        private static Worker worker;

        private static ILedController _controller;

        static void Main(string[] args)
        {    
            //AppConfig.Configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", true, false)
            //    .Build();

            //AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            //NetworkChange.NetworkAddressChanged += NetworkAddressChanged;

            //using (var controller = new CorsairLedController())
            //{
            //    worker = new Worker(controller);
            //    worker.Run(); 
            //}
            _controller = new GenericLedController();
            _controller.Initialize();
            
            var provider = new AzureDevOpsProvider("MyOrg", "MyProject", "MyPAT");
            provider.StatusChanged += Provider_StatusChanged;
            provider.BuildNames.Add("MyBuildName");

            provider.StartMonitoring();
            Console.Out.WriteLine("Monitoring - Press <Enter> to cancel.");
            Console.In.ReadLine();
            provider.StopMonitoring();
            _controller.ShutDown();
        }

        private static void Provider_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            _controller.SetColorScheme(null, e.IsErrorState);
        }

        private static void NetworkAddressChanged(object sender, EventArgs e)
        {
            worker?.QueueNetworkCheck();
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            worker?.Stop();
        }
    }
}
