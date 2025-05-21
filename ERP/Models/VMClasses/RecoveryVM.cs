using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class RecoveryVM
    {

        public string CompanyCode { get; set; }
        public int RecoveryNo { get; set; }
        public string RecoveryDate { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
        public string InvoiceNo { get; set; }
        public string ManInvoiceNo { get; set; }
        public string RepInvoiceNo { get; set; }
        public string CheqNo { get; set; }
        public string CheqDate { get; set; }
        public string ClearOrBounce { get; set; }
        public string ClearanceDate { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<int> AccountCode { get; set; }
        public string AccountName { get; set; }
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string PartyCode { get; set; }
        public string MainPartyCode { get; set; }
        public string PartyDesc { get; set; }
        public string PartyType { get; set; }
        public string STaxReg { get; set; } 
            
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<double> RecAmount { get; set; }
        public Nullable<double> RecWHT { get; set; }
        public Nullable<double> RecDiscount { get; set; }
        public string DelFlag { get; set; }
        public string PostFlag { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerm { get; set; }
        public string UpdUser { get; set; }
    }
}