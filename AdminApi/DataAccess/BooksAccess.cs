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
    public class BooksAccess
    {
        private DBHelper db = null;
        private readonly ConnectionStrings _myConnect;

        public BooksAccess(IOptions<ConnectionStrings> myConnect)
        {
            _myConnect = myConnect.Value;
            db = new DBHelper(_myConnect.dbTest); 
        }

        public int CreateNewBook(string BookName, decimal Price, string Cate, string author)
        {
            try
            {
                var pars = new SqlParameter[] {
                    new SqlParameter("@_res", SqlDbType.Int) { Direction = ParameterDirection.Output },
                    new SqlParameter("@_BookName", BookName),
                    new SqlParameter("@_Price", Price),
                    new SqlParameter("@_category", Cate),
                    new SqlParameter("@_author", author),
                };

                db.ExecuteNonQuerySP("SP_CreateBooks", pars); 

                return Convert.ToInt32(pars[0].Value.ToString());
            }
            catch (Exception e)
            {
                return -99;
            } 
        }
    }
}
