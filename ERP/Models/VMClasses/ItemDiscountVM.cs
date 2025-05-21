using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class ItemDiscountVM
    {
        public string ItemCode { get; set; }

        public string ItemDesc { get; set; }

        public float DiscountAmt { get; set; }

        public string ItemComment { get; set; }

    }
}