using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class MissCalInvoiceVM
    {
        public int InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ManInvoiceNo { get; set; }
        public string UpdUser { get; set; }
    }
}