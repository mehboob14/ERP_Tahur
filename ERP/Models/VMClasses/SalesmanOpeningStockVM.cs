using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class SalesmanOpeningStockVM
    {
        public string CompCode { get; set; }
        public string SOSNo { get; set; }
        public string SOSDate { get; set; }
        public string RegionCode { get; set; }
        public string RegionDescription { get; set; }
        public string VehicleCode { get; set; }
        public string ItemComment { get; set; }
        public string MainComment { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public double Quantity { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdUser { get; set; }
        public string UpdTerm { get; set; }
    }
}