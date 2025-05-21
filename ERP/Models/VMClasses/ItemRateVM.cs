using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class ItemRateVM
    {
        public Nullable<double> DistTaxPer_Act { get; set; }
        public Nullable<double> DistTaxPer_InAct { get; set; }
        public Nullable<double> RetTaxPer_Act { get; set; }
        public Nullable<double> RetTaxPer_InAct { get; set; }
        public Nullable<double> RegCorporateRate { get; set; }
        public Nullable<double> UnRegCorporateRate { get; set; }
        public Nullable<double> RegRetailerRate { get; set; }
        public Nullable<double> UnRegRetailerRate { get; set; }
        public string Category { get; set; }
        public Nullable<double> Consumer_Rate { get; set; }
        public Nullable<double> FEDTaxPer { get; set; }
    }
}