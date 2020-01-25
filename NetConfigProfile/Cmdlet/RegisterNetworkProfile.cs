using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using NetConfigProfile.Serialize;
using System.IO;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsLifecycle.Register, "NetworkProfile")]
    public class RegisterNetworkProfile : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }
        [Parameter(Position = 1)]
        public InterfaceConfig[] Interfaces { get; set; }
        [Parameter(Position = 2)]
        public ProxyConfig Proxy { get; set; }
        [Parameter(Position = 3)]
        public RouteConfig[] StaticRoutes { get; set; }

        protected override void ProcessRecord()
        {
            DataSerializer.Serialize<NetworkProfile>(
                new NetworkProfile()
                {
                    Name = Name,
                    Interfaces = Interfaces,
                    Proxy = Proxy,
                }, Path.Combine(Item.WorkDirectory, Name + ".json"));
        }
    }
}
