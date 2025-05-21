using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class ShortCashVM
    {
        public string CSCode { get; set; }
        public Nullable<System.DateTime> CSDate { get; set; }
        public string RegionCode { get; set; }
        public string RegionDescription { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<double> CashSubmited { get; set; }
        public Nullable<double> CashPaidBack { get; set; }
        public Nullable<double> CheqPaidBack { get; set; }
        public Nullable<double> BouncedCheque { get; set; }
        public Nullable<double> Adjustment { get; set; }
    }
}