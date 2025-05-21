using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class SaleInvoiceMasterVM
    {

        public string CompanyCode { get; set; }
        public int InvoiceNo { get; set; }
        public string ManInvoiceNo { get; set; }
        public string RepInvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string PartyCode { get; set; }
        public string MainPartyCode { get; set; }
        public string PartyDesc { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string ExemptTax { get; set; }
        public string PartyStatus { get; set; }
        public string Category { get; set; }
        public string ThirdSch_Category { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> TotalAmt { get; set; }
        public Nullable<double> SaleTaxAmt { get; set; }
        public Nullable<double> FurtherTaxAmt { get; set; }
        public Nullable<double> NetTotal { get; set; }
        public string DelFlag { get; set; }
        public string PostFlag { get; set; }
        public string TransType { get; set; }
        public string UpdUser { get; set; }
        public string UpdDate { get; set; }
        public string UpdTerm { get; set; }
        public Nullable<double> IncTaxAmt { get; set; }
        public Nullable<double> ExcTaxAmt { get; set; }

    }
}