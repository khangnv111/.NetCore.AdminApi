using AdminApi.Models;
using Libs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdminApi.Services
{
    public class Jwt
    {
        private readonly AppSettings appSetting; 
        public Jwt(IOptions<AppSettings> myConfiguration)
        {
            appSetting = myConfiguration.Value; 
        }

        public string GenerateToken(AdminModel user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSetting.JwtKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(appSetting.jwtAccountId, user.UserId.ToString()),
                        new Claim(appSetting.jwtAccountName, user.UserName),
                        new Claim(appSetting.jwtFullName, user.FullName),
                        new Claim(appSetting.jwtEmail, user.Email.ToString()),
                        //new Claim("isAdmin", user.isAdmin.ToString()),
                        //new Claim("Avatar", user.Avatar)
                        //new Claim("isAdminOrigin", user.isAdminOrigin.ToString())
                    }),
                    Expires = DateTime.Now.AddHours(appSetting.TokenExpire),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                NLogLogger.Error(ex.ToString());
                return null;
            }
        }
    }
}
