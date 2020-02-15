using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Management;

namespace NetConfigProfile
{
    public class InterfaceConfig
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public AddressConfig[] NetworkAddresses { get; set; }
        public string[] DefaultGateway { get; set; }
        public string[] DNSServers { get; set; }

        private string _MACAddress = null;
        public string MACAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_MACAddress) && !string.IsNullOrEmpty(Name))
                {
                    ManagementObject netAdapter = new ManagementClass("Win32_NetworkAdapter").
                        GetInstances().
                        OfType<ManagementObject>().
                        FirstOrDefault(x => Name.Equals(x["NetConnectionID"] as string, StringComparison.OrdinalIgnoreCase));
                    if (netAdapter != null)
                    {
                        this._MACAddress = netAdapter["MACAddress"] as string;
                    }
                }
                return this._MACAddress;
            }
        }

        /// <summary>
        /// 登録されているIPアドレスの配列を取得
        /// </summary>
        /// <returns></returns>
        public string[] GetIPAddresses()
        {
            return NetworkAddresses.Select(x => x.IPAddress).ToArray();
        }

        /// <summary>
        /// 登録されているサブネットマスクの配列を取得
        /// </summary>
        /// <returns></returns>
        public string[] GetSubnetMasks()
        {
            return NetworkAddresses.Select(x => x.SubnetMask).ToArray();
        }

        /// <summary>
        /// 指定した名前のネットワークインタフェース情報を取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static InterfaceConfig Load(string name)
        {
            ManagementObject netAdapter = new ManagementClass("Win32_NetworkAdapter").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x => name.Equals(x["NetConnectionID"] as string, StringComparison.OrdinalIgnoreCase));
            if (netAdapter == null) { return null; }

            ManagementObject netConfig = new ManagementClass("Win32_NetworkAdapterConfiguration").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x => (x["SettingID"] as string).Equals(netAdapter["GUID"] as string));
            if (netConfig == null) { return null; }

            InterfaceConfig ic = new InterfaceConfig() { Name = name };

            //  ↓これでは想定通りに有効/無効が判断できない
            //    あとで修正すること
            ic.Enabled = (bool)netAdapter["NetEnabled"]; 
            
            //if (!(bool)netConfig["DHCPEnabled"])
            //{
                ic.NetworkAddresses = AddressConfig.FromStringArray(netConfig["IPAddress"] as string[], netConfig["IPSubnet"] as string[]);
                ic.DefaultGateway = netConfig["DefaultIPGateway"] as string[];
            //}
            ic.DNSServers = netConfig["DNSServerSearchOrder"] as string[];

            return ic;
        }

        /// <summary>
        /// IPアドレス/サブネットマスク/デフォルトゲートウェイ/DNSサーバアドレスを設定
        /// </summary>
        internal void ChangeNetworkAddresses()
        {
            ManagementObject netAdapter = new ManagementClass("Win32_NetworkAdapter").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x => Name.Equals(x["NetConnectionID"] as string, StringComparison.OrdinalIgnoreCase));
            if (netAdapter == null) { return; }

            ManagementObject netConfig = new ManagementClass("Win32_NetworkAdapterConfiguration").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x => (x["SettingID"] as string).Equals(netAdapter["GUID"] as string));
            if (netConfig == null) { return; }

            //  IPアドレス設定
            if (NetworkAddresses == null || NetworkAddresses.Length == 0)
            {
                netConfig.InvokeMethod("EnableDHCP", null);
            }
            else
            {
                netConfig.InvokeMethod("EnableStatic", new object[]
                {
                    GetIPAddresses(),
                    GetSubnetMasks(),
                });
            }

            //  デフォルトゲートウェイ設定
            if (DefaultGateway != null && DefaultGateway.Length > 0)
            {
                netConfig.InvokeMethod("SetGateways", new object[]
                {
                    DefaultGateway,
                });
            }

            //  DNSサーバアドレス設定
            if (DNSServers != null && DNSServers.Length > 0)
            {
                netConfig.InvokeMethod("SetDNSServerSearchOrder", new object[]
                {
                    DNSServers,
                });
            }
            else
            {
                netConfig.InvokeMethod("SetDNSServerSearchOrder", new object[] { });
            }
        }
    }
}
