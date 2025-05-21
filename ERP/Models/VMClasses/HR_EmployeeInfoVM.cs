using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models.VMClasses
{
    public class HR_EmployeeInfoVM
    {
        public int SiteCode { get; set; }
        public string SiteName { get; set; }
        public int DepCode { get; set; }
        public string DepName { get; set; }
        public int SubDepCode { get; set; }
        public string SubDepName { get; set; }
        public int DesCode { get; set; }
        public string DesName { get; set; }
        public int EmpCode { get; set; }
        public string EmpRefNo { get; set; }
        public string EmpName { get; set; }
        public char FirstLetter { get; set; }
        public string EmpFatherName { get; set; }
        public string CNIC { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string DOJ { get; set; }
        public string DOR { get; set; }
        public string PlaceOfJoining { get; set; }
        public string Active { get; set; }
        public string Status { get; set; }
        public string InactiveDescription { get; set; }
        public string Qualification { get; set; }
        public string ProfilePictureSource { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdBy { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public string DelFlag { get; set; }

        public virtual HR_DesignationInfo HR_DesignationInfo { get; set; }
    }
}