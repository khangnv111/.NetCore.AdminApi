using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.DataAccess;
using AdminApi.Models;
using Libs;
using Libs.token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly AdminAccess _adminAccess;
        public readonly AccountJwt _account;

        public AdminController(AdminAccess adminAccess, AccountJwt account)
        {
            _adminAccess = adminAccess;
            _account = account;
        }

        [Authorize]
        [HttpGet("get-all")]
        public ActionResult<Response> getList(int Id = 0, string text = "", int status = -1)
        {
            try
            {
                var list = _adminAccess.SP_User_GetListAndInfo(Id, text, status);

                return new Response(list.Count, "ok", list);
            }
            catch (Exception e)
            {
                NLogLogger.Error(string.Format("CheckLogin: {0}", e.ToString()));
                return new Response(e.ToString());
            }
        }

        [Authorize]
        [HttpGet("get-info")]
        public ActionResult<Response> getInfo()
        {
            try
            {
                var list = _adminAccess.SP_User_GetListAndInfo(Convert.ToInt32(_account.AccountID), "", -1);
                var info = new AdminModel();
                if (list.Count > 0)
                    info = list[0];

                return new Response(info);
            }
            catch (Exception e)
            {
                NLogLogger.Error(string.Format("getInfo: {0}", e.ToString()));
                return new Response(e.ToString());
            }
        }
    }
}