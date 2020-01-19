using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NetConfigProfile
{
    class Function
    {
        /// <summary>
        /// IPアドレスのフォーマット確認
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool CheckFormatIPAddress(string ipAddress)
        {
            if (Regex.IsMatch(ipAddress, @"^((\d\d?|1\d\d|2[0-4]\d|25[0-5])\.){3}(\d\d?|1\d\d|2[0-4]\d|25[0-5])(\/\d+)?$"))
            {
                return true;
            }
            else
            {
                throw new IPAddressFormatException();
            }
        }
    }
}
