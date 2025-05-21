using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class ShortCashRecoveryVM
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CheqNo { get; set; }
        public string CheqDate { get; set; }
        public string RecDate { get; set; }
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string PartyCode { get; set; }
        public string PartyDesc { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public Nullable<double> RecAmount { get; set; }

    }
}