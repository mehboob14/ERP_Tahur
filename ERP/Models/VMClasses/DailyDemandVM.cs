using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class DailyDemandVM
    {
        public int DCode { get; set; }
        public string DDate { get; set; }
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<double> DemandQty { get; set; }
    }
}