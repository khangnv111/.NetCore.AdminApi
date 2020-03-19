using AdminApi.Models;
using Libs;
using Libs.token;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.DataAccess
{
    public class GiftCodeAccess
    {
        private DBHelper db = null;
        private readonly ConnectionStrings _myConnect;
        private readonly AccountJwt _accountJwt;
        private readonly AppSettings _appSetting;

        public GiftCodeAccess(IOptions<ConnectionStrings> myConnect, AccountJwt accountJwt, IOptions<AppSettings> appSetting)
        {
            _myConnect = myConnect.Value;
            _accountJwt = accountJwt;
            _appSetting = appSetting.Value;
            db = new DBHelper(_myConnect.GiftCodeConnection);
        }

        public List<GiftCodeModel> SP_GiftCode_GetList(string GiftcodeName, int GiftcodeValue, int Status, string FromDate, string ToDate)
        {
            var lst = new List<GiftCodeModel>();
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_GiftCodeName", GiftcodeName),
                    new SqlParameter("@_GiftCodeValue", GiftcodeValue),
                    new SqlParameter("@_Status", Status),
                    new SqlParameter("@_FromDate", FromDate),
                    new SqlParameter("@_ToDate", ToDate),
                };

                lst = db.GetListSP<GiftCodeModel>("SP_GiftCode_GetList", pars);
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
            }
            return lst;
        }

        public List<GiftCodeData> SP_CMS_EventGiftcodeDetail(int GiftcodeID, int IsUsed, int Page, int Size, ref long TotalRow)
        {
            var lst = new List<GiftCodeData>();
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_TotalRow", SqlDbType.BigInt) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_GiftcodeID", GiftcodeID),
                    new SqlParameter("@_IsUsed", IsUsed),
                    new SqlParameter("@_Page", Page),
                    new SqlParameter("@_Size", Size),
                };
                lst = db.GetListSP<GiftCodeData>("SP_CMS_EventGiftcodeDetail", pars);
                TotalRow = Convert.ToInt64(pars[0].Value.ToString());
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
            }
            return lst;
        }

        public List<EventGiftCode> SP_CMS_Event_GetList(string EventName, int EventValue, string Staff, string Fromdate, string Todate, int TypeSelect = 0)
        {
            var lst = new List<EventGiftCode>();
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_TypeSelect", TypeSelect),
                    new SqlParameter("@_EventName", EventName),
                    new SqlParameter("@_EventValue", EventValue),
                    new SqlParameter("@_Staff", Staff),
                    new SqlParameter("@_Fromdate", Fromdate),
                    new SqlParameter("@_Todate", Todate)
                };
                lst = db.GetListSP<EventGiftCode>("SP_CMS_Event_GetList", pars);
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
            }
            return lst;
        }

        public int SP_GifCode_Event_Insert(string GiftCodeName, long GiftCodeValue, int EventID, int Quantity, int SourceID, string ClientIP, int NumberInput, ref long Balance, ref DateTime EndDate)
        {
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_Balance", SqlDbType.BigInt) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_EndDate", SqlDbType.DateTime) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_GifCodeName", GiftCodeName),
                    new SqlParameter("@_GifCodeValue", GiftCodeValue),
                    new SqlParameter("@_EventID", EventID),
                    //new SqlParameter("@_EventName", DBNull.Value),
                    new SqlParameter("@_Quantity", Convert.ToInt64(Quantity)),
                    new SqlParameter("@_Staff", _accountJwt.AccountName),
                    new SqlParameter("@_SourceID", SourceID),
                    new SqlParameter("@_MerchantID", _appSetting.MerchantIdGiftCodeInsert),
                    new SqlParameter("@_MerchantKey", _appSetting.MerchantKeyGiftCodeInsert.ToString()),
                    new SqlParameter("@_ClientIP", ClientIP),
                    new SqlParameter("@_NumberInput", NumberInput),
                    //new SqlParameter("@_Note", DBNull.Value),
                };

                db.ExecuteNonQuerySP("SP_GifCode_Event_Insert", pars);

                NLogLogger.Info(string.Format("SP_GifCode_Event_Insert Output _Balance: {0} | _ResponseStatus: {1} | _EndDate :  {2}", pars[0].Value, pars[1].Value, pars[2].Value));

                Balance = Convert.ToInt64(pars[0].Value.ToString());
                EndDate = DateTime.Parse(pars[2].Value.ToString());

                return int.Parse(pars[1].Value.ToString());
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
                Balance = 0;
                EndDate = new DateTime();
                return -99;
            }
        }

        public int SP_GifCodeData_Transaction(int GiftCodeID, string GiftCode, long Value, int SourceID, int Type, DateTime EndTime, string ClientIP)
        {
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_GifCodeID", GiftCodeID),
                    new SqlParameter("@_GifCode", GiftCode),
                    new SqlParameter("@_Value", Value),
                    new SqlParameter("@_SourceID", SourceID),
                    new SqlParameter("@_Type", Type),
                    new SqlParameter("@_EndDate", EndTime),
                    new SqlParameter("@_MerchantID", _appSetting.MerchantIdGiftCodeInsert),
                    new SqlParameter("@_MerchantKey", _appSetting.MerchantKeyGiftCodeInsert.ToString()),
                    new SqlParameter("@_ClientIP", ClientIP),
                };

                db.ExecuteNonQuerySP("SP_GifCodeData_Transaction", pars);

                NLogLogger.Info(string.Format("SP_GifCodeData_Transaction Output _ResponseStatus: {0}", pars[0].Value));

                return int.Parse(pars[0].Value.ToString());
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
                return -99;
            }
        }
    }
}
