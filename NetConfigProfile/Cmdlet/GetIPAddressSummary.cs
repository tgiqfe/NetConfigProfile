﻿using System;
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

            foreach (ManagementObject mo in new ManagementClass("Win32_NetworkAdapter").
                GetInstances().
                OfType<ManagementObject>().
                Where(x => x["NetConnectionID"] != null))
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
        };

        /// <summary>
        /// 既知の名前。含まれている場合、IPアドレス情報は収集しない
        /// </summary>
        private string[] WellKnownInvalidNames = new string[]
        {
            "VMware Virtual Ethernet Adapter",
            "Bluetooth Device",
        };

        /// <summary>
        /// Win32_NetworkAdapterのName値から判断して、IPAddressSummaryを取得するかどうかを判断
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        private bool IsPhysicalInterface(ManagementObject mo)
        {
            if (WellKnownInvalidNames.Any(x => mo["Name"].ToString().StartsWith(x, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            if (WellKnownValidNames.Any(x => mo["Name"].ToString().StartsWith(x, StringComparison.OrdinalIgnoreCase)))
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

*/