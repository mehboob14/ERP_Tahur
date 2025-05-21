using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class DailyProductionVM
    {
        public string DPCode { get; set; }
        public Nullable<System.DateTime> DPDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<double> Quantity { get; set; }
    }
}