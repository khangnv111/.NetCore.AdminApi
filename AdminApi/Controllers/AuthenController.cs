using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.DataAccess;
using AdminApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Libs.Security;
using Libs;
using AdminApi.Services;
using Microsoft.Extensions.Options;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly AdminAccess _adminAccess;
        private readonly Jwt _jwtAuthen;
        private readonly AppSettings _appSetting;

        public AuthenController(AdminAccess adminAccess, Jwt jwt, IOptions<AppSettings> appSetting)
        {
            _adminAccess = adminAccess;
            _jwtAuthen = jwt;
            _appSetting = appSetting.Value;
        }

        [HttpPost]
        public ActionResult<Response> Login(dataLogin data)
        {
            try
            {
                if (string.IsNullOrEmpty(data.userName))
                {
                    return new Response(-9, "Bạn chưa nhập tên đăng nhập");
                }

                if (string.IsNullOrEmpty(data.passWord))
                {
                    return new Response(-10, "Bạn chưa nhập mật khẩu");
                }

                var status = 0;
                data.userName = data.userName.ToLower(); 
                AdminModel _user = _adminAccess.Login(data.userName, Encrypt.Md5(data.passWord), ref status);

                if (status > 0)
                {
                    var tokenKey = _jwtAuthen.GenerateToken(_user);
                    _user.token = new Token
                    {
                        tokenKey = tokenKey,
                        timeExpried = _appSetting.TokenExpire
                    };

                    return new Response(status, "Đăng nhập thành công", _user);
                }
                if (status == -1)
                {
                    return new Response(-1, "Tài khoản của bạn chưa đăng ký trên hệ thống");
                }
                if (status == -2)
                {
                    return new Response(-2, "Mật khẩu không chính xác");
                }
                if (status == -3)
                {
                    return new Response(-3, "Tài khoản của bạn đã bị khóa");
                }

                return new Response("Đăng nhập thất bại");
            }
            catch(Exception e)
            {
                NLogLogger.Error(string.Format("CheckLogin: {0}", e.ToString()));
                return new Response(e.ToString());
            } 
        }
    }
}