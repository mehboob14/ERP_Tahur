using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class SinglePartyDiscountNewVM
    {
        public string CompCode { get; set; }
        public string PdCode { get; set; }
        public Nullable<System.DateTime> PdDate { get; set; }
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string PartyCode { get; set; }
        public string PartyDesc { get; set; }
        public string PartyType { get; set; }
        public string NTN { get; set; }
        public string CNIC { get; set; }
        public string SalesTaxNumber { get; set; }
        public string Category { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<double> ItemRate { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerm { get; set; }
        public string UpdUser { get; set; }
    }
}