using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management;
using System.Collections.ObjectModel;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.Get, "IPAddressSummary")]
    public class GetIPAddressSummary : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            List<InterfaceConfig> icList = new List<InterfaceConfig>();

            foreach (ManagementObject mo in new ManagementClass("Win32_NetworkAdapter").GetInstances())
            {
                if (IsPhysicalInterface(mo))
                {
                    icList.Add(InterfaceConfig.Load(mo["NetConnectionID"] as string));
                }
            }
            WriteObject(icList);
        }

        /// <summary>
        /// 既知の名前。含まれている場合、IPアドレス情報を収集
        /// </summary>
        private string[] WellKnownValidNames = new string[]
        {
            "Intel(R) Ethernet Connection",
            "Intel(R) Dual Band Wireless",
            "Realtek USB GbE Family Controller",
            "Killer Wireless-n/a/ac",
        };

        /// <summary>
        /// 既知の名前。含まれている場合、IPアドレス情報は収集しない
        /// </summary>
        private string[] WellKnownInvalidNames = new string[]
        {
            "VMware Virtual Ethernet Adapter",
            "VirtualBox Host-Only Ethernet Adapter",
            "Bluetooth Device",
            "TAP-Windows Adapter",
        };

        /// <summary>
        /// Win32_NetworkAdapterのName値から判断して、IPAddressSummaryを取得するかどうかを判断
        /// </summary>
        /// <param name="netAdapter"></param>
        /// <returns></returns>
        private bool IsPhysicalInterface(ManagementObject netAdapter)
        {
            if (!(bool)netAdapter["PhysicalAdapter"])
            {
                return false;
            }
            if (WellKnownInvalidNames.Any(x => netAdapter["Name"].ToString().StartsWith(x, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            if (WellKnownValidNames.Any(x => netAdapter["Name"].ToString().StartsWith(x, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }

    }
}

/*
[Win32_NetworkAdapterのNameのサンプル]

VMware Virtual Ethernet Adapter for VMnet1
VMware Virtual Ethernet Adapter for VMnet8
Intel(R) Ethernet Connection (2) I219-LM
Intel(R) Dual Band Wireless-AC 8260
Bluetooth Device (Personal Area Network)
Killer Wireless-n/a/ac 1435 Wireless Network Adapter
TAP-Windows Adapter V9
Realtek USB GbE Family Controller #2
VirtualBox Host-Only Ethernet Adapter

*/
