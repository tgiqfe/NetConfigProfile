using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.New, "InterfaceConfig")]
    public class NewInterfaceConfig : PSCmdlet, IDynamicParameters
    {
        const string PARAM_Name = "Name";

        [Parameter]
        public bool Enabled { get; set; } = true;
        [Parameter]
        public AddressConfig[] NetworkAddress { get; set; }
        [Parameter]
        public string DefaultGateway { get; set; }
        [Parameter]
        public string[] DNSServer { get; set; }

        private string[] _interfaceNames = null;
        private RuntimeDefinedParameterDictionary _dictionary;

        public object GetDynamicParameters()
        {
            if (_interfaceNames == null)
            {
                _interfaceNames = new ManagementClass("Win32_NetworkAdapter").
                    GetInstances().
                    OfType<ManagementObject>().
                    Where(x => x["NetConnectionID"] != null).
                    Select(x => x["NetConnectionID"] as string).
                    ToArray();
            }

            _dictionary = new RuntimeDefinedParameterDictionary();
            Collection<Attribute> attributes = new Collection<Attribute>()
            {
                new ParameterAttribute(){ Position = 0, Mandatory = true },
                new ValidateSetAttribute(_interfaceNames),
            };
            RuntimeDefinedParameter rdParam = new RuntimeDefinedParameter(PARAM_Name, typeof(string), attributes);
            _dictionary.Add(PARAM_Name, rdParam);

            return _dictionary;
        }

        protected override void ProcessRecord()
        {
            string Name = _dictionary[PARAM_Name].Value as string;

            if (!string.IsNullOrEmpty(Name))
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
}
