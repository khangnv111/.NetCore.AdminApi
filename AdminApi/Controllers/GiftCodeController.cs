using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.DataAccess;
using AdminApi.Models;
using AdminApi.Services;
using Libs;
using Libs.token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCodeController : ControllerBase
    {
        public readonly GiftCodeAccess _giftcodeAccess;
        public readonly AccountJwt _account;
        private IHttpContextAccessor _accessor;

        public GiftCodeController(GiftCodeAccess giftcodeAccess, AccountJwt account, IHttpContextAccessor accessor)
        {
            _giftcodeAccess = giftcodeAccess;
            _account = account;
            _accessor = accessor;
        }

        [Authorize]
        [HttpGet("get-list")]
        public ActionResult<Response> getList(string giftCodeName = "", int giftCodeValue = 0, int status = -1, string fromDate = "", string toDate = "")
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate))
                {
                    fromDate = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrEmpty(toDate))
                {
                    toDate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                var list = _giftcodeAccess.SP_GiftCode_GetList(giftCodeName, giftCodeValue, status, fromDate, toDate);

                return new Response(list.Count, "ok", list);
            }
            catch (Exception e)
            {
                NLogLogger.Error(string.Format("getList: {0}", e.ToString()));
                return new Response(e.ToString());
            }
        }

        [Authorize]
        [HttpGet("get-detail")]
        public ActionResult<Response> getDetailGiftCode(int GiftCodeId = 0, int IsUsed = -1, int Page = 1, int Size = 50)
        {
            try
            {
                long Total = 0;
                var list = _giftcodeAccess.SP_CMS_EventGiftcodeDetail(GiftCodeId, IsUsed, Page, Size, ref Total);

                return new Response(Convert.ToInt32(Total), "ok", list);
            }
            catch (Exception e)
            {
                NLogLogger.Error(string.Format("getList: {0}", e.ToString()));
                return new Response(e.ToString());
            }
        }

        [Authorize]
        [HttpGet("get-list-event")]
        public ActionResult<Response> getListEvent()
        {
            try
            {
                var list = _giftcodeAccess.SP_CMS_Event_GetList("", 0, "", "", "", 1);

                return new Response(list);
            }
            catch (Exception e)
            {
                NLogLogger.Error(string.Format("getListEvent: {0}", e.ToString()));
                return new Response(e.ToString());
            }
        }

        [Authorize]
        [HttpPost("insert")]
        public ActionResult<Response> InsertGiftCode(int EventID = 0, string GiftCodeName = "", long GiftcodeValue = 0, int Quantity = 0, int NumberInput = 0)
        {
            try
            {
                long Balance = 0;
                DateTime EndDate = new DateTime();
                var res = _giftcodeAccess.SP_GifCode_Event_Insert(GiftCodeName, GiftcodeValue, EventID, Quantity, 0, _accessor.HttpContext.Connection.RemoteIpAddress.ToString(), NumberInput, ref Balance, ref EndDate);
                if (res >= 0)
                {
                    string _preFix = "E" + EventID + res;

                    for (int i = 0; i < Quantity; i++)
                    {
                        string _giftCode = GiftCodeHandler.GeneralGiftCode(_preFix, 12);
                        var resData = _giftcodeAccess.SP_GifCodeData_Transaction(res, _giftCode, GiftcodeValue, 0, 1, EndDate, _accessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        if (resData == -200)
                        {
                            i--;
                        }
                        else if (resData < 0)
                        {
                            return new Response("Không thành công");
                        }
                    }

                    return new Response(res, "Thành công");
                }

                return new Response("Không thành công");
            }
            catch (Exception e)
            {
                NLogLogger.Error(string.Format("getListEvent: {0}", e.ToString()));
                return new Response(e.ToString());
            }
        }
    }
}