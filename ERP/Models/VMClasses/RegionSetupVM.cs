using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class RegionSetupVM
    {
        public string RegionCode { get; set; }
        public string RegionDescription { get; set; }
        public string NameAbbreviate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<double> RegCorporateRate { get; set; }
        public Nullable<double> UnRegCorporateRate { get; set; }
        public Nullable<double> RegRetailerRate { get; set; }
        public Nullable<double> UnRegRetailerRate { get; set; }
        public string UpdDate { get; set; }
      
    }
}