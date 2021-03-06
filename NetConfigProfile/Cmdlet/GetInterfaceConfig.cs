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
    /// <summary>
    /// 指定したインタフェース設定を取得
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "InterfaceConfig")]
    public class GetInterfaceConfig : PSCmdlet, IDynamicParameters
    {
        const string PARAM_Name = "Name";

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
                new ParameterAttribute(){ Position = 0 },
                new ValidateSetAttribute(_interfaceNames),
            };
            RuntimeDefinedParameter rdParam = new RuntimeDefinedParameter(PARAM_Name, typeof(string), attributes);
            _dictionary.Add(PARAM_Name, rdParam);

            return _dictionary;
        }

        protected override void ProcessRecord()
        {
            string Name = _dictionary[PARAM_Name].Value as string;

            List<InterfaceConfig> icList = new List<InterfaceConfig>();
            if (string.IsNullOrEmpty(Name))
            {
                foreach (ManagementObject mo in new ManagementClass("Win32_NetworkAdapter").
                    GetInstances().
                    OfType<ManagementObject>().
                    Where(x => x["NetConnectionID"] != null))
                {
                    icList.Add(InterfaceConfig.Load(mo["NetConnectionID"] as string));
                }
            }
            else
            {
                icList.Add(InterfaceConfig.Load(Name));
            }

            WriteObject(icList, true);

            _interfaceNames = null;
        }
    }
}
