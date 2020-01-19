using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management;
using System.IO;
using NetConfigProfile.Serialize;
using System.Collections.ObjectModel;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.Set, "NetworkProfile")]
    public class SetNetworkProfile : PSCmdlet, IDynamicParameters
    {
        private List<NetworkProfile> _networkProfileList = null;
        private RuntimeDefinedParameterDictionary _dictionary;

        public object GetDynamicParameters()
        {
            if (_networkProfileList == null)
            {
                _networkProfileList = NetworkProfile.Load();
            }

            _dictionary = new RuntimeDefinedParameterDictionary();
            Collection<Attribute> attributes = new Collection<Attribute>()
            {
                new ParameterAttribute(){ Mandatory = true, Position = 0 },
                new ValidateSetAttribute(_networkProfileList.Select(x => x.Name).ToArray()),
            };
            RuntimeDefinedParameter rdParam = new RuntimeDefinedParameter("Name", typeof(string), attributes);
            _dictionary.Add("Name", rdParam);

            return _dictionary;
        }

        protected override void ProcessRecord()
        {
            if (!_dictionary.ContainsKey("Name")) { return; }

            string Name = _dictionary["Name"].Value as string;
            NetworkProfile networkProfile = _networkProfileList.FirstOrDefault(x =>
                x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            if (networkProfile != null)
            {
                foreach (InterfaceConfig ic in networkProfile.Interfaces)
                {
                    ManagementObject netAdapter = new ManagementClass("Win32_NetworkAdapter").
                        GetInstances().
                        OfType<ManagementObject>().
                        FirstOrDefault(x => ic.Name.Equals(x["NetConnectionID"] as string, StringComparison.OrdinalIgnoreCase));
                    if (netAdapter == null) { continue; }

                    ManagementObject netConfig = new ManagementClass("Win32_NetworkAdapterConfiguration").
                        GetInstances().
                        OfType<ManagementObject>().
                        FirstOrDefault(x => (x["SettingID"] as string).Equals(netAdapter["GUID"] as string));
                    if(netConfig == null) { continue; }

                    if (ic.NetworkAddresses == null || ic.NetworkAddresses.Length == 0)
                    {
                        //  DHCP自動取得
                        netConfig.InvokeMethod("EnableDHCP", null);
                    }
                    else
                    {
                        //  IPアドレス/サブネットマスク設定
                        netConfig.InvokeMethod("EnableStatic", new object[]
                        {
                            ic.GetIPAddresses(),
                            ic.GetSubnetMasks(),
                        });
                    }
                    if (!string.IsNullOrEmpty(ic.DefaultGateway))
                    {
                        //  デフォルトゲートウェイ設定
                        netConfig.InvokeMethod("SetGateways", new object[]
                        {
                            new string[1]{ ic.DefaultGateway },
                        });
                    }
                    if (ic.DNSServers != null && ic.DNSServers.Length > 0)
                    {
                        netConfig.InvokeMethod("SetDNSServerSearchOrder", new object[]
                        {
                            ic.DNSServers,
                        });
                    }
                }
            }
            _networkProfileList = null;
        }
    }
}
