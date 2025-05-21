using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Models;
using ERP.Models.VMClasses;

namespace ERP.Controllers
{
    public class HRController : Controller
    {
        // GET: HR
        public ActionResult HR_Dashboard()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                List<HR_EmployeeInfo> EList = HRContext.HR_EmployeeInfo.Where(x => x.DelFlag == "N").ToList();

                List<HR_EmployeeInfoVM> EList2 = new List<HR_EmployeeInfoVM>();

                foreach (var item in EList)
                {
                    HR_EmployeeInfoVM Obj = new HR_EmployeeInfoVM();

                    Obj.HR_DesignationInfo = item.HR_DesignationInfo;
                    Obj.SiteName = HRContext.HR_SiteInfo.Where(x => x.SiteCode == item.SiteCode).Select(x => x.SiteName).FirstOrDefault();
                    Obj.DepName = HRContext.HR_DepartmentInfo.Where(x => x.DepCode == item.DepCode).Select(x => x.DepDescription).FirstOrDefault();

                    
                    Obj.SiteCode = item.SiteCode;
                    Obj.DepCode = item.DepCode;
                    Obj.DesCode = item.DesCode;
                    Obj.EmpRefNo = item.EmpRefNo;
                    Obj.EmpName = item.EmpName;
                    Obj.EmpFatherName = item.EmpFatherName;
                    Obj.CNIC = item.CNIC;
                    Obj.ContactNo = item.ContactNo;
                    Obj.Address = item.Address;
                    Obj.Gender = item.Gender;
                    
                    Obj.DOB = item.DOB.ToString("dd/MM/yyyy");
                    Obj.DOJ = item.DOJ.ToString("dd/MM/yyyy");
                    Obj.DOR = item.DOR.ToString();
                    Obj.PlaceOfJoining = item.PlaceOfJoining;
                    Obj.Active = item.Active;
                    Obj.Status = item.Status;
                    Obj.InactiveDescription = item.InactiveDescription;
                    Obj.Qualification = item.Qualification;
                    Obj.FirstLetter = item.EmpName[0];

                    if (item.ProfilePicture != null)
                    {
                        string PP = Server.MapPath("~/HR_Content/ProfilePics/" + Obj.EmpRefNo + ".jpg");

                        if (System.IO.File.Exists(PP))
                        {
                            System.IO.File.Delete(PP);
                        }

                        System.IO.File.WriteAllBytes(PP, item.ProfilePicture);
                        Obj.ProfilePictureSource = "/HR_Content/ProfilePics/" + Obj.EmpRefNo + ".jpg";
                    }

                    EList2.Add(Obj);
                }

                ViewBag.EmpList = EList2;
                ViewBag.EmpListCount = EList2.Count();
                ViewBag.CountAll = HRContext.HR_EmployeeInfo.Where(x => x.DelFlag == "N").Count();

                return View();
            }
        }


        #region (----------------------------------------- HR Site --------------------------------------------)

        public ActionResult HR_SiteList()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                List<HR_SiteInfo> SiteList = HRContext.HR_SiteInfo.ToList();
                return View(SiteList);
            }
        }

        [HttpPost]
        public ActionResult HR_CreateEdit_SiteList(HR_SiteInfo Obj)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.SiteCode == 0)
                {
                    HRContext.Entry(Obj).State = System.Data.Entity.EntityState.Added;
                    HRContext.SaveChanges();
                }
                else
                {
                    var Found = HRContext.HR_SiteInfo.Where(x => x.SiteCode == Obj.SiteCode).FirstOrDefault();
                    Found.SiteName = Obj.SiteName;
                    HRContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                    HRContext.SaveChanges();
                }
                
            }
            return RedirectToAction("HR_SiteList");
        }

        [HttpPost]
        public ActionResult HR_Delete_SiteList(int SiteCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (SiteCode != 0)
                {
                    HRContext.HR_SiteInfo.Remove(HRContext.HR_SiteInfo.Where(x => x.SiteCode == SiteCode).FirstOrDefault());
                    HRContext.SaveChanges();
                }

            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region (----------------------------------------- HR Department --------------------------------------------)

        public ActionResult HR_DepartmentList()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                ViewBag.DepartmentList = HRContext.HR_DepartmentInfo.ToList();

                var List = HRContext.HR_SiteInfo.ToList();
                ViewBag.SiteDDL = new SelectList(List, "SiteCode", "SiteName");

                return View();
            }
        }

        [HttpPost]
        public ActionResult HR_CreateEdit_DepartmentList(HR_DepartmentInfo Obj)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.DepCode == 0)
                {
                    HRContext.Entry(Obj).State = System.Data.Entity.EntityState.Added;
                    HRContext.SaveChanges();
                }
                else
                {
                    var Found = HRContext.HR_DepartmentInfo.Where(x => x.DepCode == Obj.DepCode).FirstOrDefault();
                    Found.SiteCode = Obj.SiteCode;
                    Found.DepCode = Obj.DepCode;
                    Found.DepDescription = Obj.DepDescription;
                    Found.DepShortName = Obj.DepShortName;
                    HR_SiteInfo Site = HRContext.HR_SiteInfo.Where(x => x.SiteCode == Obj.SiteCode).FirstOrDefault();
                    Found.HR_SiteInfo = Site;
                    HRContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                    HRContext.SaveChanges();
                }
            }

            return RedirectToAction("HR_DepartmentList");
        }

        [HttpPost]
        public ActionResult HR_Delete_DepartmentList(int DepCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (DepCode != 0)
                {
                    HRContext.HR_DepartmentInfo.Remove(HRContext.HR_DepartmentInfo.Where(x => x.DepCode == DepCode).FirstOrDefault());
                    HRContext.SaveChanges();
                }

            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region (----------------------------------------- HR Sub Department --------------------------------------------)

        public ActionResult HR_SubDepartmentList()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                List<HR_SubDepartmentInfo> SDList = HRContext.HR_SubDepartmentInfo.ToList();

                foreach (var item in SDList)
                {
                    item.HR_DepartmentInfo = HRContext.HR_DepartmentInfo.Where(x => x.DepCode == item.DepCode).FirstOrDefault();
                }

                ViewBag.DepartmentSubList = SDList;

                var List = HRContext.HR_SiteInfo.ToList();
                ViewBag.SiteDDL = new SelectList(List, "SiteCode", "SiteName");

                return View();
            }
        }

        public ActionResult GetDepartmentDDL(int SiteCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                List<DDL> DepDDL = HRContext.HR_DepartmentInfo.Where(x => x.SiteCode == SiteCode).Select(x => new DDL { Code = x.DepCode, Name = x.DepDescription }).ToList();

                return Json(DepDDL, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult HR_CreateEdit_SubDepartmentList(HR_SubDepartmentInfo Obj)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.SubDepCode == 0)
                {
                    HRContext.Entry(Obj).State = System.Data.Entity.EntityState.Added;
                    HRContext.SaveChanges();
                }
                else
                {
                    var Found = HRContext.HR_SubDepartmentInfo.Where(x => x.SubDepCode == Obj.SubDepCode).FirstOrDefault();
                    Found.SiteCode = Obj.SiteCode;
                    Found.DepCode = Obj.DepCode;
                    Found.SubDepCode = Obj.SubDepCode;
                    Found.SubDepDescription = Obj.SubDepDescription;
                    Found.SubDepShortName = Obj.SubDepShortName;
                    HR_DepartmentInfo Dep = HRContext.HR_DepartmentInfo.Where(x => x.DepCode == Obj.DepCode).FirstOrDefault();
                    Found.HR_DepartmentInfo = Dep;
                    HRContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                    HRContext.SaveChanges();
                }
            }

            return RedirectToAction("HR_SubDepartmentList");
        }

        [HttpPost]
        public ActionResult HR_Delete_SubDepartmentList(int SubDepCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (SubDepCode != 0)
                {
                    HRContext.HR_SubDepartmentInfo.Remove(HRContext.HR_SubDepartmentInfo.Where(x => x.SubDepCode == SubDepCode).FirstOrDefault());
                    HRContext.SaveChanges();
                }

            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region (----------------------------------------- HR Designation --------------------------------------------)

        public ActionResult HR_DesignationList()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {

                List<HR_DesignationInfo> DList = HRContext.HR_DesignationInfo.ToList();
                List<HR_DesignationVM> HRList = new List<HR_DesignationVM>();
                foreach (var item in DList)
                {
                    HR_DesignationVM Obj = new HR_DesignationVM();
                    Obj.SiteCode = item.SiteCode;
                    Obj.SiteName = HRContext.HR_SiteInfo.Where(x => x.SiteCode == item.SiteCode).Select(x => x.SiteName).FirstOrDefault();
                    Obj.DepCode = item.DepCode;
                    Obj.DepDescription = HRContext.HR_DepartmentInfo.Where(x => x.DepCode == item.DepCode).Select(x => x.DepDescription).FirstOrDefault();
                    Obj.SubDepCode = item.SubDepCode;
                    Obj.SubDepDescription = HRContext.HR_SubDepartmentInfo.Where(x => x.SubDepCode == item.SubDepCode).Select(x => x.SubDepDescription).FirstOrDefault();
                    Obj.DesCode = item.DesCode;
                    Obj.DesDescription = item.DesDescription;
                    Obj.DesShortName = item.DesShortName;
                    HRList.Add(Obj);
                
                }

                ViewBag.DesignationList = HRList; 

                var List = HRContext.HR_SiteInfo.ToList();
                ViewBag.SiteDDL = new SelectList(List, "SiteCode", "SiteName");

                return View();
            }
        }

        public ActionResult GetSubDepartmentDDL(int DepCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                List<DDL> SubDepDDL = HRContext.HR_SubDepartmentInfo.Where(x => x.DepCode == DepCode).Select(x => new DDL { Code = x.SubDepCode, Name = x.SubDepDescription }).ToList();

               return Json(SubDepDDL, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult HR_CreateEdit_DesignationList(HR_DesignationInfo Obj,int? HSiteCode,int? HDepCode,int? HSubDepCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.DesCode == 0)
                {
                    HRContext.Entry(Obj).State = System.Data.Entity.EntityState.Added;
                    HRContext.SaveChanges();
                }
                else
                {
                    var Found = HRContext.HR_DesignationInfo.Where(x => x.DesCode == Obj.DesCode).FirstOrDefault();
                    Found.SiteCode = Convert.ToInt32(HSiteCode);
                    Found.DepCode = Convert.ToInt32(HDepCode);
                    Found.SubDepCode = Convert.ToInt32(HSubDepCode);
                    Found.DesDescription = Obj.DesDescription;
                    Found.DesShortName = Obj.DesShortName;
                    HR_SubDepartmentInfo SubDepartment = HRContext.HR_SubDepartmentInfo.Where(x => x.SubDepCode == Found.SubDepCode).FirstOrDefault();
                    Found.HR_SubDepartmentInfo = SubDepartment;
                    HRContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                    HRContext.SaveChanges();
                }
            }

            return RedirectToAction("HR_DesignationList");
        }

        [HttpPost]
        public ActionResult HR_Delete_DesignationList(int DesCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (DesCode != 0)
                {
                    HRContext.HR_DesignationInfo.Remove(HRContext.HR_DesignationInfo.Where(x => x.DesCode == DesCode).FirstOrDefault());
                    HRContext.SaveChanges();
                }

            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region (----------------------------------------- HR Employee --------------------------------------------)

        public ActionResult HR_EmployeeList()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {

                List<HR_EmployeeInfo> EList = HRContext.HR_EmployeeInfo.Where(x=>x.DelFlag == "N").ToList();

                List<HR_EmployeeInfoVM> EList2 = new List<HR_EmployeeInfoVM>();

                foreach (var item in EList)
                {
                    HR_EmployeeInfoVM Obj = new HR_EmployeeInfoVM();

                    Obj.HR_DesignationInfo = item.HR_DesignationInfo;
                    Obj.SiteName = HRContext.HR_SiteInfo.Where(x => x.SiteCode == item.SiteCode).Select(x => x.SiteName).FirstOrDefault();
                    Obj.DepName = HRContext.HR_DepartmentInfo.Where(x => x.DepCode == item.DepCode).Select(x => x.DepDescription).FirstOrDefault();
                    Obj.SubDepName = HRContext.HR_SubDepartmentInfo.Where(x => x.SubDepCode == item.SubDepCode).Select(x => x.SubDepDescription).FirstOrDefault();

                    Obj.EmpCode = item.EmpCode;
                    Obj.SiteCode = item.SiteCode;
                    Obj.DepCode = item.DepCode;
                    Obj.DesCode = item.DesCode;
                    Obj.EmpRefNo = item.EmpRefNo;
                    Obj.EmpName = item.EmpName;
                    Obj.EmpFatherName = item.EmpFatherName;
                    Obj.CNIC = item.CNIC;
                    Obj.ContactNo = item.ContactNo;
                    Obj.Address = item.Address;
                    Obj.Gender = item.Gender;
                    Obj.DOB = item.DOB.ToString();
                    Obj.DOJ = item.DOJ.ToString();
                    Obj.DOR = item.DOR.ToString();
                    Obj.PlaceOfJoining = item.PlaceOfJoining;
                    Obj.Active = item.Active;
                    Obj.Status = item.Status;
                    Obj.InactiveDescription = item.InactiveDescription;
                    Obj.Qualification = item.Qualification;
                    Obj.FirstLetter = item.EmpName[0];

                    if (item.ProfilePicture != null)
                    {
                        string PP = Server.MapPath("~/HR_Content/ProfilePics/" + Obj.EmpRefNo + ".jpg");

                        if (System.IO.File.Exists(PP))
                        {
                            System.IO.File.Delete(PP);
                        }

                        System.IO.File.WriteAllBytes(PP, item.ProfilePicture);
                        Obj.ProfilePictureSource = "/HR_Content/ProfilePics/" + Obj.EmpRefNo + ".jpg";
                    }

                    EList2.Add(Obj);
                }

                ViewBag.EmpList = EList2;

                var List = HRContext.HR_SiteInfo.ToList();
                ViewBag.SiteDDL = new SelectList(List, "SiteCode", "SiteName");

                return View();
            }
        }

        public ActionResult GetDesignationDDL(int SubDepCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                List<DDL> DepDDL = HRContext.HR_DesignationInfo.Where(x => x.SubDepCode == SubDepCode).Select(x => new DDL { Code = x.DesCode, Name = x.DesDescription }).ToList();

                return Json(DepDDL, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult GetSingleEmployee(int EmpCode)
        {

           

            HR_EmployeeInfoVM FoundData = new HR_EmployeeInfoVM();

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {

                HR_EmployeeInfo Found = HRContext.HR_EmployeeInfo.Where(x => x.EmpCode == EmpCode).FirstOrDefault();

               

                //FoundData.HR_DesignationInfo = Found.HR_DesignationInfo;
                FoundData.SiteName = HRContext.HR_SiteInfo.Where(x => x.SiteCode == Found.SiteCode).Select(x => x.SiteName).FirstOrDefault();
                FoundData.DepName = HRContext.HR_DepartmentInfo.Where(x => x.DepCode == Found.DepCode).Select(x => x.DepDescription).FirstOrDefault();
                FoundData.SubDepName = HRContext.HR_SubDepartmentInfo.Where(x => x.SubDepCode == Found.SubDepCode).Select(x => x.SubDepDescription).FirstOrDefault();
                FoundData.DesName = HRContext.HR_DesignationInfo.Where(x => x.DesCode == Found.DesCode).Select(x => x.DesDescription).FirstOrDefault();

                FoundData.EmpCode = Found.EmpCode;
                FoundData.SiteCode = Found.SiteCode;
                FoundData.DepCode = Found.DepCode;
                FoundData.SubDepCode = Found.SubDepCode;
                FoundData.DesCode = Found.DesCode;
                FoundData.EmpRefNo = Found.EmpRefNo;
                FoundData.EmpName = Found.EmpName;
                FoundData.EmpFatherName = Found.EmpFatherName;
                FoundData.CNIC = Found.CNIC;
                FoundData.ContactNo = Found.ContactNo;
                FoundData.Address = Found.Address;
                FoundData.Gender = Found.Gender;
                FoundData.DOB = Found.DOB.ToString("yyyy-MM-dd");
                FoundData.DOJ = Found.DOJ.ToString("yyyy-MM-dd");
                FoundData.DOR = Found.DOR.ToString();
                FoundData.PlaceOfJoining = Found.PlaceOfJoining;
                FoundData.Active = Found.Active;
                FoundData.Status = Found.Status;
                FoundData.InactiveDescription = Found.InactiveDescription;
                FoundData.Qualification = Found.Qualification;
                FoundData.FirstLetter = Found.EmpName[0];

                if (Found.ProfilePicture != null)
                {
                    string PP = Server.MapPath("~/HR_Content/ProfilePics/" + FoundData.EmpRefNo + ".jpg");

                    if (System.IO.File.Exists(PP))
                    {
                        System.IO.File.Delete(PP);
                    }

                    System.IO.File.WriteAllBytes(PP, Found.ProfilePicture);
                    FoundData.ProfilePictureSource = "/HR_Content/ProfilePics/" + FoundData.EmpRefNo + ".jpg";
                }

            }

            return Json(new { FoundData = FoundData }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult HR_CreateEdit_EmpList(HR_EmployeeInfo Obj, HttpPostedFileBase ProfilePicture, int? HSiteCode, int? HDepCode,int? HDesCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.EmpCode == 0)
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase ImageLogo = Request.Files[0];
                        if (ImageLogo.FileName != "")
                        {
                            Obj.ProfilePicture = new byte[ImageLogo.ContentLength];
                            ImageLogo.InputStream.Read(Obj.ProfilePicture, 0, ImageLogo.ContentLength);
                        }
                        //else
                        //{
                        //    byte[] LogoImg = System.Convert.FromBase64String(ModelImage);
                        //    Comp.Picture = LogoImg;
                        //}
                    }

                    string RefNo = HRContext.HR_DepartmentInfo.Where(x=>x.DepCode == Obj.DepCode).Select(x=>x.DepShortName).FirstOrDefault() + Obj.SiteCode + "-" + HRContext.HR_DesignationInfo.Where(x=>x.DesCode == Obj.DesCode).Select(x=>x.DesShortName).FirstOrDefault() + "-" + "1";
                    var Found = HRContext.HR_EmployeeInfo.Where(x => x.EmpRefNo == RefNo).Select(x => x.EmpRefNo).FirstOrDefault();
                    if (Found == null)
                    {
                        Obj.EmpRefNo = RefNo;
                    }
                    else
                    {
                        string[] Arr = Found.Split('-');
                        int No = Convert.ToInt32(Arr[2]) + 1;
                        Obj.EmpRefNo = Arr[0] + "-" + Arr[1] + "-" + No;
                    }

                    Obj.DelFlag = "N";
                    Obj.CreatedBy = CommonDAL.UserName();
                    Obj.CreatedDate = DateTime.Now;
                    Obj.UpdBy = CommonDAL.UserName();
                    Obj.UpdDate = DateTime.Now;
                    HRContext.Entry(Obj).State = System.Data.Entity.EntityState.Added;
                    HRContext.SaveChanges();
                }
                else
                {

                    var Found = HRContext.HR_EmployeeInfo.Where(x => x.EmpCode == Obj.EmpCode).FirstOrDefault();

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase ImageLogo = Request.Files[0];
                        if (ImageLogo.FileName != "")
                        {
                            Found.ProfilePicture = new byte[ImageLogo.ContentLength];
                            ImageLogo.InputStream.Read(Found.ProfilePicture, 0, ImageLogo.ContentLength);
                        }
                    }

                    Found.SiteCode = Convert.ToInt32(HSiteCode);
                    Found.DepCode = Convert.ToInt32(HDepCode);
                    Found.DesCode = Convert.ToInt32(HDesCode);
                    Found.EmpName = Obj.EmpName;
                    Found.EmpFatherName = Obj.EmpFatherName;
                    Found.CNIC = Obj.CNIC;
                    Found.ContactNo = Obj.ContactNo;
                    Found.Address = Obj.Address;
                    Found.Gender = Obj.Gender;
                    Found.DOB = Obj.DOB;
                    Found.DOJ = Obj.DOJ;
                    Found.DOR = Obj.DOR;
                    Found.PlaceOfJoining = Obj.PlaceOfJoining;
                    Found.Active = Obj.Active;
                    Found.Status = Obj.Status;
                    Found.InactiveDescription = Obj.InactiveDescription;
                    Found.Qualification = Obj.Qualification;
                    Found.UpdBy = CommonDAL.UserName();
                    Found.UpdDate = DateTime.Now;
                    
                    HR_DesignationInfo Designation = HRContext.HR_DesignationInfo.Where(x => x.DesCode == Found.DesCode).FirstOrDefault();
                    Found.HR_DesignationInfo = Designation;
                    HRContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                    HRContext.SaveChanges();
                }
            }

            return RedirectToAction("HR_EmployeeList");
        }

        [HttpPost]
        public ActionResult HR_Delete_EmpList(int EmpCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            using (AT_Tahur_SUITEEntities HRContext = new AT_Tahur_SUITEEntities())
            {
                if (EmpCode != 0)
                {
                    var Found = HRContext.HR_EmployeeInfo.Where(x => x.EmpCode == EmpCode).FirstOrDefault();
                    
                    Found.UpdBy = CommonDAL.UserName();
                    Found.UpdDate = DateTime.Now;
                    Found.DelFlag = "Y";
                    HR_DesignationInfo Designation = HRContext.HR_DesignationInfo.Where(x => x.DesCode == Found.DesCode).FirstOrDefault();
                    Found.HR_DesignationInfo = Designation;
                    HRContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                    HRContext.SaveChanges();
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}