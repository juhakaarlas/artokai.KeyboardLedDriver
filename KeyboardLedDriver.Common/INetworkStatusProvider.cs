using System.Collections.Generic;
using System.Dynamic;

namespace KeyboardLedDriver.Common
{
    public interface INetworkStatusProvider : IStatusProvider
    {
        List<string> Interfaces { get; set; }
        
        bool IsUp { get; }
    }
}