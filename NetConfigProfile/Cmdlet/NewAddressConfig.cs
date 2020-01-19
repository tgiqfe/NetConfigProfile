using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Net;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.New, "AddressConfig")]
    public class NewAddressConfig : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string IPAddress { get; set; }
        [Parameter(Position = 1)]
        public string SubnetMask { get; set; }
        [Parameter]
        public int? PrefixLength { get; set; }

        protected override void BeginProcessing()
        {
            Function.CheckFormatIPAddress(IPAddress);
        }

        protected override void ProcessRecord()
        {
            if (!string.IsNullOrEmpty(SubnetMask))
            {
                WriteObject(new AddressConfig(IPAddress, SubnetMask), true);
            }
            else if (string.IsNullOrEmpty(SubnetMask) && PrefixLength != null)
            {
                WriteObject(new AddressConfig(IPAddress, (int)PrefixLength), true);
            }
            else if (IPAddress.Contains("/"))
            {
                WriteObject(new AddressConfig(IPAddress), true);
            }
        }
    }
}
