using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class SalePartyVM
    {
        public string CompCode { get; set; }
        public string RegionCode { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string PartyRegisterName { get; set; }
        public string MainPartyDescription { get; set; }
        public string RegionDescription { get; set; }
        public string PartyAddress { get; set; }
        public string PartyType { get; set; }
        public string NTN { get; set; }

        public string CNIC { get; set; }
        public string SalesTaxNumber { get; set; }
        public string Category { get; set; }
        public Nullable<decimal> Opening { get; set; }
        public string ContactNumber { get; set; }
        public string MainPartyCode { get; set; }
        public string STaxReg { get; set; }
        public string Taxable { get; set; }
        public string ThirdSch_Category { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdUser { get; set; }
        public string UpdTerm { get; set; }
    }
}