using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class MainPartyVM
    {
        public string MainPartyCode { get; set; }
        public string MainPartyName { get; set; }

        public string NTN { get; set; }
        public string SalesTaxNumber { get; set; }
        public Nullable<decimal> Opening { get; set; }
    }
}