using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class HR_DesignationVM
    {
        public int SiteCode { get; set; }
        public string SiteName { get; set; }
        public int DepCode { get; set; }
        public string DepDescription { get; set; }
        public int SubDepCode { get; set; }
        public string SubDepDescription { get; set; }
        public int DesCode { get; set; }
        public string DesDescription { get; set; }
        public string DesShortName { get; set; }
    }
}