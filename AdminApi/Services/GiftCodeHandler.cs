using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Services
{
    public class GiftCodeHandler
    {
        private static Random random = new Random();

        public static string GeneralGiftCode(string prefix, int length)
        {
            int _len = length - prefix.Length;
            string _strTmp = string.Empty;

            const string key = "123456789ABCDEFGHIJKLMNPQRSTUVXYZ";

            var str = new string(Enumerable.Repeat(key, _len).Select(s => s[random.Next(s.Length)]).ToArray());
            str = prefix + str;

            return str.ToUpper();
        }
    }
}
