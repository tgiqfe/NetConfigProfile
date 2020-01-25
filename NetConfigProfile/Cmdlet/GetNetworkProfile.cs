using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.IO;
using NetConfigProfile.Serialize;
using System.Collections.ObjectModel;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.Get, "NetworkProfile")]
    public class GetNetworkProfile : PSCmdlet, IDynamicParameters
    {
        const string PARAM_Name = "Name";

        [Parameter]
        public SwitchParameter Json { get; set; }

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
                new ParameterAttribute(){ Position = 0 },
                new ValidateSetAttribute(_networkProfileList.Select(x => x.Name).ToArray()),
            };
            RuntimeDefinedParameter rdParam = new RuntimeDefinedParameter(PARAM_Name, typeof(string), attributes);
            _dictionary.Add(PARAM_Name, rdParam);

            return _dictionary;
        }

        protected override void ProcessRecord()
        {
            if (_dictionary.ContainsKey(PARAM_Name))
            {
                string Name = _dictionary[PARAM_Name].Value as string;
                NetworkProfile profile = _networkProfileList.First(x => x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
                if (Json)
                {
                    WriteObject(DataSerializer.Serialize<NetworkProfile>(profile, DataType.Json), true);
                }
                else
                {
                    WriteObject(profile, true);
                }
            }
            else
            {
                if (Json)
                {
                    WriteObject(DataSerializer.Serialize<List<NetworkProfile>>(_networkProfileList, DataType.Json), true);
                }
                else
                {
                    WriteObject(_networkProfileList, true);
                }
            }

            _networkProfileList = null;
        }
    }
}
