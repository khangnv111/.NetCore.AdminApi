using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    public class GiftCodeModel
    {
        public int GifCodeID { get; set; }
        public string GifCodeName { get; set; }
        public int GifCodeValue { get; set; }
        public int AgencyID { get; set; }
        public string AgencyName { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateTime { get; set; }
        public long AccountID { get; set; }
        public string UserName { get; set; }
        public int Quantity { get; set; }
    }

    public class GiftCodeData
    {
        public string GifCode { get; set; }
        public bool IsUsed { get; set; }
        public int Status { get; set; }
        public DateTime? UsedTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string EndTime { get; set; }
        public int Value { get; set; }
        public string EventCreateTime { get; set; }
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public long AccountID { get; set; }
    }

    public class EventGiftCode
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public int Status { get; set; }
    }
}
