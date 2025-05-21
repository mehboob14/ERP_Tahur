using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class SaleInvoiceDetailVM
    {

        public string CompanyCode { get; set; }
        public int InvoiceNo { get; set; }
        public string ManInvoiceNo { get; set; }
        public string RepInvoiceNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<double> SaleQty { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<double> Damage { get; set; }
        public Nullable<double> Replace { get; set; }
        public Nullable<double> FOC { get; set; }
        public Nullable<double> Sampling { get; set; }
        public Nullable<double> Scheme { get; set; }
        public Nullable<double> Retrn { get; set; }
        public Nullable<double> WHT { get; set; }
        public Nullable<double> DiscPer { get; set; }
        public Nullable<double> DiscAmt { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<double> FurtherTaxPer { get; set; }
        public Nullable<double> FurtherTaxAmt { get; set; }
        public Nullable<double> IncTaxAmt { get; set; }
        public Nullable<double> ExcTaxAmt { get; set; }
        public Nullable<double> SaleTaxPer { get; set; }
        public Nullable<double> SaleTaxAmt { get; set; }
        public Nullable<double> IncomeTaxPer { get; set; }
        public Nullable<double> IncomeTaxAmt { get; set; }
        public Nullable<double> ConsumerRate { get; set; }

        public Nullable<double> FEDTaxAmt { get; set; }
        public Nullable<double> FEDTaxPer { get; set; }
    }
}