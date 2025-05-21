using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class ActivePartiesSalesmanVM
    {
        public int Id { get; set; }
        public string RegionCode { get; set; }
        public string RegionDescription { get; set; }
        public string PartyCode { get; set; }
        public string MainPartyCode { get; set; }
        public string PartyName { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Status { get; set; }
    }
}