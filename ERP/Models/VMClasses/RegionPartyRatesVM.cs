using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class RegionPartyRatesVM
    {
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<double> SaleTax { get; set; }
        public Nullable<double> FurtherTax { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Rate { get; set; }
    }
}