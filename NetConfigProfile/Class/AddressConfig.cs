using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace NetConfigProfile
{
    public class AddressConfig
    {
        public string IPAddress { get; set; }
        public string SubnetMask { get; set; }

        /// <summary>
        /// コンストラクタ (値無し)
        /// </summary>
        public AddressConfig() { }
        public AddressConfig(string ipAddress, string subnetMask)
        {
            this.IPAddress = ipAddress;
            this.SubnetMask = subnetMask;
        }
        public AddressConfig(string ipAddress, int prefixLength)
        {
            this.IPAddress = ipAddress;
            this.SubnetMask = IntToSubnetMask(prefixLength);
        }
        public AddressConfig(string ipAddressAndPrefixLength)
        {
            if (ipAddressAndPrefixLength.Contains("/"))
            {
                this.IPAddress = ipAddressAndPrefixLength.Substring(0, ipAddressAndPrefixLength.IndexOf("/"));
                this.SubnetMask = int.TryParse(ipAddressAndPrefixLength.Substring(ipAddressAndPrefixLength.IndexOf("/") + 1), out int prefixLength) ?
                    IntToSubnetMask(prefixLength) : IntToSubnetMask(24);
            }
        }
        public AddressConfig(IPAddress ipAddress, IPAddress subnetMask)
        {
            this.IPAddress = ipAddress.ToString();
            this.SubnetMask = subnetMask.ToString();
        }
        public AddressConfig(IPAddress ipAddress, int prefixLength)
        {
            this.IPAddress = ipAddress.ToString();
            this.SubnetMask = IntToSubnetMask(prefixLength);
        }

        /// <summary>
        /// プレフィックスレングスからサブネットマスクに変換
        /// </summary>
        /// <param name="prefixLength"></param>
        /// <returns></returns>
        private string IntToSubnetMask(int prefixLength)
        {
            int length = prefixLength >= 0 || prefixLength <= 32 ? prefixLength : 24;
            string byteStr = new string('1', prefixLength) + new string('0', 32 - prefixLength);
            return string.Format("{0}.{1}.{2}.{3}",
                Convert.ToInt32(byteStr.Substring(0, 8), 2),
                Convert.ToInt32(byteStr.Substring(8, 8), 2),
                Convert.ToInt32(byteStr.Substring(16, 8), 2),
                Convert.ToInt32(byteStr.Substring(24, 8), 2));
        }

        /// <summary>
        /// サブネットマスクの文字列からプレフィックスレングスに変換
        /// </summary>
        /// <returns></returns>
        private int SubnetMaskToInt()
        {
            return SubnetMaskToInt(SubnetMask);
        }

        /// <summary>
        /// サブネットマスクの文字列からプレフィックスレングスに変換
        /// </summary>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        private int SubnetMaskToInt(string subnetMask)
        {
            string prefix =
                string.Join("", subnetMask.Split('.').Select(x => Convert.ToString(int.Parse(x), 2)).ToArray());
            return Regex.Matches(prefix, @"^1+")[0].Value.Length;
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", IPAddress, SubnetMaskToInt());
        }

        /// <summary>
        /// IPアドレスの文字列配列とサブネットマスクの文字列配列から、AddressConfigのListを取得
        /// </summary>
        /// <param name="ipAddresses"></param>
        /// <param name="subnetMasks"></param>
        /// <returns></returns>
        public static AddressConfig[] FromStringArray(string[] ipAddresses, string[] subnetMasks)
        {
            List<AddressConfig> addressConfigList = new List<AddressConfig>();
            if (ipAddresses != null && subnetMasks != null && ipAddresses.Length == subnetMasks.Length)
            {
                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    addressConfigList.Add(new AddressConfig(ipAddresses[i], subnetMasks[i]));
                }
            }
            return addressConfigList.Count == 0 ? null : addressConfigList.ToArray();
        }
    }
}
