using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class AgingReportVM
    {
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string RegionCode { get; set; }
        public string RegionDescripion { get; set; }
        public System.Double RecoveryAmount { get; set; }
        public System.Double TotalCredit { get; set; }
        public double C0 { get; set; }
        public double C1_30 { get; set; }
        public double C31_60 { get; set; }
        public double C61_90 { get; set; }
        public double C91_120 { get; set; }
        public double C121_150 { get; set; }
        public double C151_180 { get; set; }
        public double C181_210 { get; set; }
        public double C211_240 { get; set; }
        public double C241_270 { get; set; }
        public double C271_300 { get; set; }
        public double C301_330 { get; set; }
        public double C331_360 { get; set; }
        public double MoreThan360 { get; set; }
        public double Open_1_30 { get; set; }
        public double Open_31_60 { get; set; }
        public double Open_61_90 { get; set; }
        public double Open_91_120 { get; set; }
        public double Open_121_150 { get; set; }
        public double Open_151_180 { get; set; }
        public double Open_181_210 { get; set; }
        public double Open_211_240 { get; set; }
        public double Open_241_270 { get; set; }
        public double Open_271_300 { get; set; }
        public double Open_301_330 { get; set; }
        public double Open_331_360 { get; set; }
        public double Open_MoreThan360 { get; set; }
    }
}