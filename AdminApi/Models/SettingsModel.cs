using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    public class AppSettings
    {
        public string JwtKey { get; set; }
        public int TokenExpire { get; set; }

        public string jwtAccountId { get; set; }
        public string jwtAccountName { get; set; }
        public string jwtFullName { get; set; }
        public string jwtEmail { get; set; }

        public int MerchantIdGiftCodeInsert { get; set; }
        public string MerchantKeyGiftCodeInsert { get; set; }
    }

    public class ConnectionStrings
    {
        public string dbTest { get; set; }
        public string adminConnection { get; set; }
        public string GiftCodeConnection { get; set; }
    }
     
}
