using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    public class Response
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public dynamic Data { get; set; }

        public Response()
        {

        }

        public Response(int code, string desc)
        {
            Description = desc;
            Code = code;
        }
        public Response(int code, string desc, dynamic data)
        {
            Description = desc;
            Code = code;
            Data = data;
        }
        public Response(dynamic data)
        {
            Code = 1;
            Description = "Thành công";
            Data = data;
        }

        public Response(string desc)
        {
            Description = desc;
            Code = -99;
        }
    }
}
