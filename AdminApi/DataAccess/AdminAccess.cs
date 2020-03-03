using AdminApi.Models;
using Libs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.DataAccess
{
    public class AdminAccess
    {
        private DBHelper db = null;
        private readonly ConnectionStrings _myConnect;

        public AdminAccess(IOptions<ConnectionStrings> myConnect)
        {
            _myConnect = myConnect.Value;
            db = new DBHelper(_myConnect.adminConnection);
        }

        public AdminModel Login(string UserName, string Password, ref int ResponseStatus)
        {
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_UserName", UserName),
                    new SqlParameter("@_Password", Password),
                };

                var data = db.GetInstanceSP<AdminModel>("SP_Users_CheckLogin", pars);
                ResponseStatus = Convert.ToInt32(pars[0].Value);
                NLogLogger.Info(string.Format("Login Out: {0} {1} ", ResponseStatus, data));
                return data;
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
                ResponseStatus = -99;
                return new AdminModel();
            }
        }

        //Lấy ds tài khoản hoặc thông tin chi tiết
        public List<AdminModel> SP_User_GetListAndInfo(int Id, string TextSearch, int Status)
        {
            var lst = new List<AdminModel>();
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_Id", Id),
                    new SqlParameter("@_TextSearch", TextSearch),
                    new SqlParameter("@_Status", Status),
                };

                lst = db.GetListSP<AdminModel>("SP_User_GetListAndInfo", pars);
            }
            catch (Exception e)
            {
                NLogLogger.Exception(e);
            }
            return lst;
        }
    }
}
