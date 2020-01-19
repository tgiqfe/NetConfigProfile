using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.New, "InterfaceConfig")]
    public class NewInterfaceConfig : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }
        [Parameter]
        public bool Enabled { get; set; } = true;
        [Parameter]
        public AddressConfig[] NetworkAddress { get; set; }
        [Parameter]
        public string DefaultGateway { get; set; }
        [Parameter]
        public string[] DNSServer { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(new InterfaceConfig()
            {
                Name = Name,
                Enabled = Enabled,
                NetworkAddresses = NetworkAddress,
                DefaultGateway = DefaultGateway,
                DNSServers = DNSServer
            }, true);
        }
    }
}
