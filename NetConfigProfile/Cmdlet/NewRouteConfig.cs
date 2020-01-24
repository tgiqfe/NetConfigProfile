using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Net;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.New, "RouteConfig")]
    public class NewRouteConfig : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string NetworkAddress { get; set; }
        [Parameter]
        public string SubnetMask { get; set; }
        [Parameter]
        public int? PrefixLength { get; set; }
        [Parameter(Mandatory = true)]
        public string DefaultGateway { get; set; }

        protected override void BeginProcessing()
        {
            Function.CheckFormatIPAddress(NetworkAddress);
        }

        protected override void ProcessRecord()
        {
            if (!string.IsNullOrEmpty(SubnetMask))
            {
                WriteObject(new RouteConfig()
                {
                    NetworkAddress = new AddressConfig(NetworkAddress, SubnetMask),
                    DefaultGateway = DefaultGateway,
                }, true);
            }
            else if (string.IsNullOrEmpty(SubnetMask) && PrefixLength != null)
            {
                WriteObject(new RouteConfig()
                {
                    NetworkAddress = new AddressConfig(NetworkAddress, (int)PrefixLength),
                    DefaultGateway = DefaultGateway,
                }, true);
            }
            else if (NetworkAddress.Contains("/"))
            {
                WriteObject(new RouteConfig()
                {
                    NetworkAddress = new AddressConfig(NetworkAddress),
                    DefaultGateway = DefaultGateway,
                }, true);
            }
        }
    }
}
