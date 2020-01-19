using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NetConfigProfile.Serialize;

namespace NetConfigProfile
{
    public class NetworkProfile
    {
        public string Name { get; set; }
        public InterfaceConfig[] Interfaces { get; set; }
        public ProxyConfig Proxy { get; set; }

        /// <summary>
        /// NetworkProfileリストを取得
        /// </summary>
        /// <param name="profilePath"></param>
        /// <returns></returns>
        public static List<NetworkProfile> Load()
        {
            List<NetworkProfile> networkProfileList = new List<NetworkProfile>();
            string[] extensions = new string[] { ".json", ".xml", ".yaml", ".yml" };

            Action<string> addNetworkProfileList = (fileName) =>
            {
                if (extensions.Any(x => Path.GetExtension(fileName).Equals(x, StringComparison.OrdinalIgnoreCase)))
                {
                    networkProfileList.Add(DataSerializer.Deserialize<NetworkProfile>(fileName));
                }
            };

            foreach (string tempFile in Directory.GetFiles(Item.WorkDirectory))
            {
                if (Directory.Exists(tempFile))
                {
                    foreach (string subTempFile in Directory.GetFiles(tempFile))
                    {
                        if (extensions.Any(x => Path.GetExtension(subTempFile).Equals(x, StringComparison.OrdinalIgnoreCase)))
                        {
                            networkProfileList.Add(DataSerializer.Deserialize<NetworkProfile>(subTempFile));
                        }
                    }
                }
                else
                {
                    if (extensions.Any(x => Path.GetExtension(tempFile).Equals(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        networkProfileList.Add(DataSerializer.Deserialize<NetworkProfile>(tempFile));
                    }
                }
            }
            return networkProfileList;
        }
    }
}
