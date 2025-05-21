using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ERP.Models;
using ERP.Models.VMClasses;

namespace ERP.Controllers
{
    public class CompanyController : Controller
    {
            CommonDAL common = new CommonDAL();
        TransactionHandler Handler = new TransactionHandler();
        string CompanyCode = CommonDAL.CompCode();


        #region (------------------------------CompanySetup-----------------------------------------)

        // GET: Company
        public ActionResult CompanySetup()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Session["CompanyCode"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                if (CommonDAL.UserRights("1", "001"))
                {



                    var DelImage = Server.MapPath("~/Content/plugins/images/At-Tahur-Logo.jpg");
                    if (System.IO.File.Exists(DelImage))
                    {
                        System.IO.File.Delete(DelImage);
                    }
                    CompanySetup CompSetup = DefinitionContext.CompanySetups.Where(x => x.CompCode == CompanyCode).FirstOrDefault();

                    if (CompSetup.Picture == null)
                    {
                        var Image = Server.MapPath("~/Content/plugins/images/At-Tahur-Logo.jpg");
                        byte[] bytes = Encoding.ASCII.GetBytes(Image);
                        CompSetup.Picture = bytes;
                    }
                    else
                    {
                        string ImageLogo = Server.MapPath("~/Content/plugins/images/At-Tahur-Logo.jpg");
                        System.IO.File.WriteAllBytes(ImageLogo, CompSetup.Picture);
                        ViewBag.LogoImg = ImageLogo;
                    }

                    return View(CompSetup);
                }
                else
                {
                    return RedirectToAction("PageNotFound", "Home");
                }
                
            }
        }

        [HttpPost]
        public ActionResult CompanySetup(CompanySetup Comp, HttpPostedFileBase Image, string ModelImage)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Comp != null)
                {
                    DefinitionContext.CompanySetups.Remove(DefinitionContext.CompanySetups.Where(x => x.CompCode == CompanyCode).FirstOrDefault());
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase ImageLogo = Request.Files[0];
                        if (ImageLogo.FileName != "")
                        {
                            Comp.Picture = new byte[ImageLogo.ContentLength];
                            ImageLogo.InputStream.Read(Comp.Picture, 0, ImageLogo.ContentLength);
                        }
                        else
                        {
                            byte[] LogoImg = System.Convert.FromBase64String(ModelImage);
                            Comp.Picture = LogoImg;
                        }
                    }

                    Comp.CompCode = "00001";
                    Comp.UpdDate = DateTime.Now;
                    Comp.UpdTerm = CommonDAL.UserName();
                    Comp.UpdUser = CommonDAL.UserName();
                    DefinitionContext.CompanySetups.Add(Comp);
                    DefinitionContext.SaveChanges();
                    return RedirectToAction("CompanySetup");
                }
                else
                {
                    TempData["TestAccessError"] = "You can not do this opertation. Contact to administrator";
                    return RedirectToAction("Error");
                }
            }
        }

        #endregion
       
        #region (-------------------------------RegionCode------------------------------------------)

        public ActionResult GetSingleRegion(string RegionCode)
        {
            List<RegionSetupVM> List = Handler.GetSingleRegion(RegionCode);

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegionDetail()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public JsonResult RegionItemRate(RegionSetup[] RegionItemRate)
        {
            Session["RegionItemRate"] = RegionItemRate;

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegionSetup()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("2","001"))
            {
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    ViewBag.RegionDetail = DefinitionContext.GetDistinctRegion().ToList();
                }

                ViewBag.ItemDDL = new SelectList(Handler.GetSaleItems(), "ItemCode", "ItemDesc");
                ViewBag.ItemList = Handler.GetSaleItems();
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
         
        }

       

        [HttpPost]
        public ActionResult RegionSetup(RegionSetup Obj)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditRegion(Obj);

            return RedirectToAction("RegionSetup");
        }

        public ActionResult DeleteRegion(string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var Confirm = Handler.DeleteRegion(RegionCode);
            return Json(Confirm,JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region (-------------------------------Previous RegionCode------------------------------------------)

        public ActionResult GetSinglePreRegion(string RegionCode)
        {
            List<RegionSetupVM> List = Handler.GetSinglePreRegion(RegionCode);

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreRegionDetail()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public JsonResult PreRegionItemRate(PreRegionSetup[] PreRegionItemRate)
        {
            Session["PreRegionItemRate"] = PreRegionItemRate;

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreRegionSetup()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("2", "001"))
            {
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    ViewBag.RegionDetail = DefinitionContext.GetDistinctPreRegion().ToList();
                }

                ViewBag.ItemDDL = new SelectList(Handler.GetSaleItems(), "ItemCode", "ItemDesc");
                ViewBag.ItemList = Handler.GetSaleItems();
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }

        }



        [HttpPost]
        public ActionResult PreRegionSetup(PreRegionSetup Obj)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditPreRegion(Obj);

            return RedirectToAction("PreRegionSetup");
        }

        public ActionResult DeletePreRegion(string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var Confirm = Handler.DeletePreRegion(RegionCode);
            return Json(Confirm, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region (----------------------------------Party Setup------------------------------------)
        public ActionResult PartySetup()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("3","001"))
            {
                //ViewBag.GetPartyList = Handler.GetSaleParties2();


                ViewBag.CurrentDate = Handler.GetCurrentDate();
               

                //ViewBag.PartyDDL = new SelectList(Handler.GetSaleParties().ToList(), "PartyCode", "PartyName");

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }

          
        }

        public ActionResult UpdParty(string RegionCode,string PartyCode)
        {
            SalePartyVM Party = new SalePartyVM();
            string NotFound = "";

            if (CommonDAL.UserRights("3", "003"))
            {
                Party = Handler.UpdParty(RegionCode, PartyCode);
                Session["UpdRegionCode"] = RegionCode;
                if (Party.PartyCode == null)
                {
                    NotFound = "NotFound";
                }
            }
            else
            {
                Party.PartyCode = null;
            }

            return Json(new { Party = Party,NotFound = NotFound },JsonRequestBehavior.AllowGet);
        }


        public ActionResult SinglePartyDDL(string RegionCode, string PartyCode)
        {
            SaleParty Party = Handler.DDLPartyDetail(RegionCode, PartyCode);

            return Json(Party, JsonRequestBehavior.AllowGet);
        }


         
        [HttpPost]
        public ActionResult PartySetup(SaleParty Obj,string radio,string CashCredit,string Taxable,string ManualMPartyCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditPartySetup(Obj,radio,CashCredit,Taxable);
            

            return RedirectToAction("PartySetup");
        }

        [HttpPost]
        public ActionResult DeleteParty(string RegionCode,string PartyCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool Confirm = Handler.DeleteParty(RegionCode,PartyCode);
            return Json(Confirm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MergeParty(string RegionCode, string PartyCode, string MainPartyCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool Confirm = Handler.MergeParty(RegionCode, PartyCode, MainPartyCode);
            return Json(Confirm, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region (-------------------------------CompanyEmployee--------------------------------)

        public ActionResult CompanyEmployee()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("4","001"))
            {
                ViewBag.CompanyEmpDetail = Handler.GetCompanyEmployee();

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    ViewBag.RegionDetail = DefinitionContext.GetDistinctRegion().ToList();

                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                
                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
           
        }


    [HttpPost]
        public ActionResult GetItemImg()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase ItemPic = Request.Files[0];

                Session["ItemImg"] = ItemPic;
            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CompanyEmployee(CompanyEmp Obj)
        {
            TempData["SuccessMsg"] = Handler.AddEditCompanyEmployee(Obj);

            return RedirectToAction("CompanyEmployee");
        }

        public ActionResult DeleteEmployee(string EmpCode)
        {
            bool Success = Handler.DeleteEmployee(EmpCode);

            return Json(Success, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region (-------------------------------UnitSetup------------------Not Currently Used---------------------)
        public ActionResult UnitDetail()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public ActionResult UnitSetup()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        #endregion

        #region(---------------------------------------AreaSetup------------------Not Currently Used-----------------)
        public ActionResult AreaSetup()
        {
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            return View();
        }

        #endregion
    }
}