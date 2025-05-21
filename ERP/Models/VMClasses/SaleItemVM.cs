using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class SaleItemVM
    {
        public string CompanyCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public Nullable<double> SaleRateReg { get; set; }
        public Nullable<double> SaleRateUnReg { get; set; }
        public Nullable<double> SaleGSTPer { get; set; }
        public Nullable<double> SaleFurtherTaxPer { get; set; }
        public Nullable<double> Consumer_Rate { get; set; }
        public Nullable<int> ProductionOpening { get; set; }
        public string UpdUser { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerm { get; set; }
        public string ItemPic { get; set; }
        public Nullable<double> stdWeight { get; set; }
        public string WeightType { get; set; }
        public string ActiveFlag { get; set; }
        public Nullable<double> PackSize { get; set; }

        public Nullable<double> RetTaxPer_Act { get; set; }
        public Nullable<double> RetTaxPer_InAct { get; set; }
        public Nullable<double> DistTaxPer_Act { get; set; }
        public Nullable<double> DistTaxPer_InAct { get; set; }

        public Nullable<double> FEDTaxPer { get; set; }
    }
}