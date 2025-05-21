using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class DailyDispatchVM
    {
        public string DispatchCode { get; set; }
        public Nullable<System.DateTime> DispatchDate { get; set; }
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string VehicleCode { get; set; }
        public string VehicleRegNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<double> DispatchQty { get; set; }



    }
}