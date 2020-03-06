using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    public class dataLogin
    {
        public string userName { get; set; }
        public string passWord { get; set; }
    }

    public class AdminModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public string FullName { get; set; }
        public int isAdmin { get; set; }
        public string Avatar { get; set; }
        public int isAdminOrigin { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastTimeLogin { get; set; }

        public Token token { get; set; }
    }
     
    public class Token
    {
        public string tokenKey { get; set; }
        public int timeExpried { get; set; }
    }
}
