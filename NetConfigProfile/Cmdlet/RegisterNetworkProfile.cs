using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using NetConfigProfile.Serialize;
using System.IO;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsLifecycle.Register, "NetworkProfile")]
    public class RegisterNetworkProfile : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }
        [Parameter(Position = 1)]
        public InterfaceConfig[] Interfaces { get; set; }
        [Parameter(Position = 2)]
        public ProxyConfig Proxy { get; set; }
        [Parameter]
        public string ProfileDir { get; set; }

        private string _currentDirectory = null;

        protected override void BeginProcessing()
        {
            //  カレントディレクトリカレントディレクトリの一時変更
            _currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = this.SessionState.Path.CurrentFileSystemLocation.Path;
        }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(ProfileDir))
            {
                ProfileDir = Item.WorkDirectory;
            }
            if (!Directory.Exists(ProfileDir))
            {
                Directory.CreateDirectory(ProfileDir);
            }

            DataSerializer.Serialize<NetworkProfile>(
                new NetworkProfile()
                {
                    Name = Name,
                    Interfaces = Interfaces,
                    Proxy = Proxy,
                }, Path.Combine(ProfileDir, Name + ".json"));
        }

        protected override void EndProcessing()
        {
            //  カレントディレクトリを戻す
            Environment.CurrentDirectory = _currentDirectory;
        }
    }
}
