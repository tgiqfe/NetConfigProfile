using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetConfigProfile
{
    internal class Item
    {
        public static string WorkDirectory =
            Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), "NetConfigProfile");
        

    }
}
