
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace YmtSystem.CrossCutting.Utility
{
    public class CommonConfiguration
    {
        public static readonly string MachineIP = string.Join(" / ", Dns.GetHostAddresses(Dns.GetHostName())
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork).Select(add => add.ToString()).ToArray());

        public static readonly string MachineName = Environment.MachineName;

        public static YmatouConfig GetConfig()
        {
            var config = LocalConfigService.GetConfig(new YmatouConfig());           
            return config;
        }
    }
}
