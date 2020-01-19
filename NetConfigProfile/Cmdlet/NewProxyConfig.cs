using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace NetConfigProfile.Cmdlet
{
    [Cmdlet(VerbsCommon.New, "ProxyConfig")]
    public class NewProxyConfig : PSCmdlet
    {
        [Parameter]
        public bool EnableAutoDetect { get; set; } = true;
        [Parameter]
        public bool UseAutoConfScript { get; set; }
        [Parameter]
        public string ProxyPacURL { get; set; }
        [Parameter]
        public bool EnableProxyServer { get; set; }
        [Parameter]
        public string ProxyServer { get; set; }
        [Parameter]
        public int ProxyPort { get; set; }
        [Parameter]
        public bool ExcludeLocalServer { get; set; }
        [Parameter]
        public string ExcludeURL { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(new ProxyConfig()
            {
                EnableAutoDetect = EnableAutoDetect,
                UseAutoConfScript = UseAutoConfScript,
                ProxyPacURL = ProxyPacURL,
                EnableProxyServer = EnableProxyServer,
                ProxyServer = ProxyServer,
                ProxyPort = ProxyPort,
                ExcludeLocalServer = ExcludeLocalServer,
                ExcludeURL = ExcludeURL
            }, true);
        }
    }
}
