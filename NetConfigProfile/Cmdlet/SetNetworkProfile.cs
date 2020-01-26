using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using NetConfigProfile.Serialize;
using System.IO;
using System.Collections.ObjectModel;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.Set, "NetworkProfile")]
    public class SetNetworkProfile : PSCmdlet, IDynamicParameters
    {
        const string PARAM_Name = "Name";

        [Parameter(Position = 1)]
        public InterfaceConfig[] Interfaces { get; set; }
        [Parameter(Position = 2)]
        public ProxyConfig Proxy { get; set; }
        [Parameter(Position = 3)]
        public RouteConfig[] StaticRoutes { get; set; }

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
                new ParameterAttribute(){ Position = 0, Mandatory = true },
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
                if (!string.IsNullOrEmpty(Name))
                {
                    NetworkProfile profile = _networkProfileList.First(x => x.Name == Name);
                    if (Interfaces != null) { profile.Interfaces = Interfaces; }
                    if (Proxy != null) { profile.Proxy = Proxy; }
                    if (StaticRoutes != null) { profile.StaticRoutes = StaticRoutes; }
                    DataSerializer.Serialize<NetworkProfile>(
                        profile, Path.Combine(Item.WorkDirectory, Name + ".json"));
                }
            }
        }

        protected override void EndProcessing()
        {
            _networkProfileList = null;
        }
    }
}
