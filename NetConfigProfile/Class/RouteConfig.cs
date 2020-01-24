using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetConfigProfile
{
    public class RouteConfig
    {
        public AddressConfig NetworkAddress { get; set; }
        public string DefaultGateway { get; set; }
    }
}
