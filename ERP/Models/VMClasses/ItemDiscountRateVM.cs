using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class ItemDiscountRateVM
    {
        public string ItemCode { get; set; }
        
        public string ItemDesc { get; set; }

        public double DiscountAmt { get; set; }

        public double DiscountRate { get; set; }
    }
}