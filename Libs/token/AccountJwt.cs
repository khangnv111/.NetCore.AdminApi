using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Libs.token
{
    public class AccountJwt
    {
        private IHttpContextAccessor _httpContextAccessor; 

        public AccountJwt(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpContext Current => _httpContextAccessor.HttpContext;

        public long AccountID
        {
            get
            {
                long accountId = 0;
                if (Current.User.Identity.IsAuthenticated)
                { 
                    string val = Current.User.FindFirst("AccountId").Value;
                    accountId = Int64.Parse(val);
                }  
                return accountId;
            }
        }

        public string AccountName
        {
            get
            {
                string val = "";
                if (Current.User.Identity.IsAuthenticated)
                {
                    val = Current.User.FindFirst("UserName").Value;
                }
                return val;
            }
        }
        public string FullName
        {
            get
            {
                string val = "";
                if (Current.User.Identity.IsAuthenticated)
                {
                    val = Current.User.FindFirst("FullName").Value;
                }
                return val;
            }
        }

        public string Email
        {
            get
            {
                string val = "";
                if (Current.User.Identity.IsAuthenticated)
                {
                    val = Current.User.FindFirst("Email").Value;
                }
                return val;
            }
        }

        public bool IsAuthenLogin()
        {
            if (Current.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }
    }
}
