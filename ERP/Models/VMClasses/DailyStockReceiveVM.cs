using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class DailyStockReceiveVM
    {
        public string CompCode { get; set; }
        public string RegionCode { get; set; }
        public string RegionDescription { get; set; }
        public string DSRNo { get; set; }
        public string DSRDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public double Quantity { get; set; }
        public string ItemComment { get; set; }
        public string MainComment { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdUser { get; set; }
        public string UpdTerm { get; set; }
    }
}