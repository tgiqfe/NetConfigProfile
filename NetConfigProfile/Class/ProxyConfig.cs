using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetConfigProfile
{
    public class ProxyConfig
    {
        public bool EnableAutoDetect { get; set; }
        public bool UseAutoConfScript { get; set; }
        public string ProxyPacURL { get; set; }
        public bool EnableProxyServer { get; set; }
        public string ProxyServer { get; set; }
        public int ProxyPort { get; set; }
        public bool ExcludeLocalServer { get; set; }
        public string ExcludeURL { get; set; }
    }
}
