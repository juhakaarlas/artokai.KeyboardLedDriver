using System;
using System.IO;
using System.Net.NetworkInformation;
using KeyboarLedDriver.Corsair;
using Microsoft.Extensions.Configuration;

namespace Artokai.KeyboardLedDriver
{
    class Program
    {
        private static Worker worker;

        static void Main(string[] args)
        {    
            AppConfig.Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, false)
                .Build();

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            NetworkChange.NetworkAddressChanged += NetworkAddressChanged;

            using (var controller = new CorsairLedController())
            {
                worker = new Worker(controller);
                worker.Run(); 
            }
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
