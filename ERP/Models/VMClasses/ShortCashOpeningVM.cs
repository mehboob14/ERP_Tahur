using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class ShortCashOpeningVM
    {
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<double> Opening { get; set; }
    }
}