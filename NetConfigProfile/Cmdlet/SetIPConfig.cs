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
    [Cmdlet(VerbsCommon.Set, "IPConfig")]
    public class SetIPConfig : PSCmdlet, IDynamicParameters
    {
        const string PARAM_Name = "Name";

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
            RuntimeDefinedParameter rdParam = new RuntimeDefinedParameter(PARAM_Name, typeof(string), attributes);
            _dictionary.Add(PARAM_Name, rdParam);

            return _dictionary;
        }

        protected override void ProcessRecord()
        {
            if (!_dictionary.ContainsKey(PARAM_Name)) { return; }

            string Name = _dictionary[PARAM_Name].Value as string;
            NetworkProfile networkProfile = _networkProfileList.FirstOrDefault(x =>
                x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            if (networkProfile != null)
            {
                foreach (InterfaceConfig ic in networkProfile.Interfaces)
                {
                    ic.ChangeNetworkAddresses();
                }
            }
            _networkProfileList = null;
        }
    }
}
