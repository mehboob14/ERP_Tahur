using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class PartyDiscountNewVM
    {
        public string PdCode { get; set; }

        public DateTime PdDate { get; set; }

        public string RegionCode { get; set; }

        public string RegionDesc { get; set; }

        public string PartyCode { get; set; }

        public string PartyDesc { get; set; }
    }
}