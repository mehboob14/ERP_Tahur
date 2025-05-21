using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class CompanyEmpVM
    {
        public string CompCode { get; set; }
        public string RegionCode { get; set; }
        public string RegionDescription { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpRemarks { get; set; }
        public string EmpAddress { get; set; }
        public string EmpDOB { get; set; }
        public string EmpJoiningDate { get; set; }
        public string EmpLeavingDate { get; set; }
        public string EmpDesignation { get; set; }
        public string EmpDepartment { get; set; }
        public Nullable<double> EmpBasicSalary { get; set; }
        public Nullable<double> EmpEperiance { get; set; }
        public string EmpContactNo { get; set; }
        public string EmpStatus { get; set; }
        public string EmpQualification { get; set; }
        public Nullable<double> OpeningShortCash { get; set; }
        public string DelFlag { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string UpdTerm { get; set; }
        public string UpdUser { get; set; }
    }
}