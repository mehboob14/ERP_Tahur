using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class DailyIntakeVM
    {
        public string DICode { get; set; }
        public System.DateTime DIDate { get; set; }
        public string FarmCode { get; set; }
        public string FarmName { get; set; }
        public Nullable<double> RawMilkQty { get; set; }
        public string UpdTerm { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdUser { get; set; }
    }
}