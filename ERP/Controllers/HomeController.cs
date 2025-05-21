using ERP.Models;
using ERP.Models.VMClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ERP.Controllers
{
    public class HomeController : Controller
    {
        TransactionHandler Handler = new TransactionHandler();
        CommonDAL Common = new CommonDAL();
        string CompanyCode = CommonDAL.CompCode();
        AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();
        // GET: Home
        public ActionResult Index()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            //ViewBag.CountSaleParty = DefinitionContext.Sp_PartyAnalysis().Count();
            ViewBag.CountSaleParty = DefinitionContext.SaleParties.Count();
            ViewBag.CountSaleItem = Handler.GetSaleItems().Count();
            ViewBag.CountSalesman = Handler.GetCompanyEmployee().Count();
            using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                ViewBag.CountDiscountParty = DefinitionContext.CountPartyDis(CompanyCode).Count();
                //double CountAll = DefinitionContext.Sp_PartyAnalysis().Count();
                //double InactiveCount = DefinitionContext.Sp_PartyAnalysis().Where(x => x.SaleAmount == 0 && x.NoofInvoices == 0).Count();
                //double ActiveCount = DefinitionContext.Sp_PartyAnalysis().Where(x => x.SaleAmount != 0 && x.NoofInvoices != 0).Count();
                //ViewBag.InactivePartiesCount = DefinitionContext.Sp_PartyAnalysis().Where(x => x.SaleAmount == 0 && x.NoofInvoices == 0).Count();
                //ViewBag.ActivePartiesCount = DefinitionContext.Sp_PartyAnalysis().Where(x => x.SaleAmount != 0 && x.NoofInvoices != 0).Count();

              
                //ViewBag.ActivePer = (ActiveCount/CountAll) * 100;
                //ViewBag.InactivePer = (InactiveCount/CountAll) * 100;

                //ViewBag.LastSaleParties = DefinitionContext.Sp_LastSaleParties().ToList();

            }

            

            return View();
        }


        
        public ActionResult PartyAnalysis()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                if (Session["CompanyCode"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                ViewBag.PartyAnalysis = DefinitionContext.Sp_PartyAnalysis().ToList();

                return View();
            }

        }


        public ActionResult SearchParty(string term)
        {
            List<string> List = new List<string>();

            string RegionCode = "001";

            List = DefinitionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyName.StartsWith(term)).Select(x => x.PartyCode + "  ==>  " + x.PartyName).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPicChartData(string FromDate,string ToDate)
        {
            if (CommonDAL.UserRights("16", "001"))
            {

                DateTime From;
                DateTime To;
                if (FromDate == "")
                {
                    From = DateTime.Now;
                }
                else
                {
                    From = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
                }

                if (ToDate == "")
                {
                    To = DateTime.Now;
                }
                else
                {
                    To = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
                }

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var MilkQty = DefinitionContext.MilkAllQty(From, To).Select(x => x.Qty).FirstOrDefault();
                    var YogurtQty = DefinitionContext.YougartAllQty(From, To).Select(x => x.Qty).FirstOrDefault();
                    var RaitaQty = DefinitionContext.RaitaAllQty(From, To).Select(x => x.Qty).FirstOrDefault();
                    var ButterQty = DefinitionContext.ButterAllQty(From, To).Select(x => x.Qty).FirstOrDefault();

                    if (MilkQty == null)
                    {
                        MilkQty = 1;
                    }

                    if (YogurtQty == null)
                    {
                        YogurtQty = 1;
                    }

                    if (RaitaQty == null)
                    {
                        RaitaQty = 1;
                    }

                    if (ButterQty == null)
                    {
                        ButterQty = 1;
                    }
                    return Json(new { MilkQty = MilkQty, YogurtQty = YogurtQty, ButterQty = ButterQty, RaitaQty = RaitaQty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { MilkQty = "", YogurtQty = "", ButterQty = "", RaitaQty = "" }, JsonRequestBehavior.AllowGet);
            }
            
               
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username,string Password)
        {
            Username = Username.ToUpper().Trim();
            Password = Password.ToUpper().Trim();

            if (Common.Authenticate(Username,Password))
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Invalid Username Or Password . . . !";
            }

            return View();
        }


        public ActionResult SaleParties()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.SaleParties = Handler.GetSaleParties();

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["UserName"] = null;
            Session["UserCode"] = null;
            Session["CompanyCode"] = null;
            return RedirectToAction("Login");

        }

        public ActionResult GetAllPartyDiscount()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.DiscountParties = Handler.GetAllPartyDiscountNew();
            return View();
        }

        public ActionResult GetAllItemDetail()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.ItemDescription = Handler.GetSaleItems();
            return View();

        }

        public ActionResult GetAllSalesmanDetail()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.SalesmanDetail = Handler.GetCompanyEmployee();

            return View();
        }

        [HttpPost]
        public ActionResult GetSTRForFBR(string FromDate,string ToDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            DateTime FDate = DateTime.Parse(FromDate);
            DateTime TDate = DateTime.Parse(ToDate);

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var STRForFBR = DefinitionContext.SaleTaxRegisterFBR(FDate, TDate).ToList();

                return Json(STRForFBR, JsonRequestBehavior.AllowGet);
            }
           
        }

        public ActionResult STRForFBR()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        //public ActionResult AgingReportData(string RegionAll, string PartyAll, string RegionCode, string PartyCode, string FromDate)
        //{

        //    DateTime SPDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);

        //    List<AgingReport_Result> Data;

        //    if (RegionAll == "RegionAll" && PartyAll == "PartyAll")
        //    {
        //        Data = DefinitionContext.AgingReport(SPDate).ToList();
        //    }
        //    else if (RegionAll != "RegionAll" && PartyAll == "PartyAll")
        //    {
        //        Data = DefinitionContext.AgingReport(SPDate).Where(x => x.RegionCode == RegionCode).ToList();
        //    }
        //    else if (RegionAll != "RegionAll" && PartyAll != "PartyAll")
        //    {
        //        Data = DefinitionContext.AgingReport(SPDate).Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).ToList();
        //    }
        //    else
        //    {
        //        Data = DefinitionContext.AgingReport(SPDate).ToList();
        //    }
        //    List<AgingReportVM> List = new List<AgingReportVM>();

        //    foreach (var item in Data)
        //    {
        //        AgingReportVM NewObj = new AgingReportVM();

        //        //FIFO Base Aging Method

        //        // For More than 360
        //        if (item.More_Than_360 >= item.RecoveryAmount)
        //        {
        //            NewObj.More_Than_360 = item.More_Than_360 - item.RecoveryAmount;
        //            NewObj.RecoveryAmount = item.RecoveryAmount - item.RecoveryAmount;
        //        }
        //        else
        //        {
        //            NewObj.RecoveryAmount = item.RecoveryAmount - item.More_Than_360;
        //            NewObj.More_Than_360 = item.More_Than_360 - item.More_Than_360;
        //        }

        //        // For  181-360
        //        if (item.C181_360 >= NewObj.RecoveryAmount)
        //        {
        //            NewObj.C181_360 = item.C181_360 - NewObj.RecoveryAmount;
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
        //        }
        //        else
        //        {
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C181_360;
        //            NewObj.C181_360 = item.C181_360 - item.C181_360;
        //        }

        //        // For 91-180
        //        if (item.C91_180 >= NewObj.RecoveryAmount)
        //        {
        //            NewObj.C91_180 = item.C91_180 - NewObj.RecoveryAmount;
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
        //        }
        //        else
        //        {
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C91_180;
        //            NewObj.C91_180 = item.C91_180 - item.C91_180;
        //        }

        //        // For 61-90
        //        if (item.C61_90 >= NewObj.RecoveryAmount)
        //        {
        //            NewObj.C61_90 = item.C61_90 - NewObj.RecoveryAmount;
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
        //        }
        //        else
        //        {
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C61_90;
        //            NewObj.C61_90 = item.C61_90 - item.C61_90;
        //        }

        //        // For 31-60
        //        if (item.C31_60 >= NewObj.RecoveryAmount)
        //        {
        //            NewObj.C31_60 = item.C31_60 - NewObj.RecoveryAmount;
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
        //        }
        //        else
        //        {
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C31_60;
        //            NewObj.C31_60 = item.C31_60 - item.C31_60;
        //        }

        //        // For 0-30
        //        if (item.C1_30 >= NewObj.RecoveryAmount)
        //        {
        //            NewObj.C1_30 = item.C1_30 - NewObj.RecoveryAmount;
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
        //        }
        //        else
        //        {
        //            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C1_30;
        //            NewObj.C1_30 = item.C1_30 - item.C1_30;
        //        }

        //        NewObj.RegionCode = item.RegionCode;
        //        NewObj.PartyCode = item.PartyCode;
        //        NewObj.RegionDescripion = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
        //        NewObj.PartyName = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
        //        NewObj.TotalCredit = item.TotalCredit;

        //        List.Add(NewObj);
              
        //    }

        //    ViewBag.AgingData = List;
        //    ViewBag.FromDate = FromDate;
        //    return View();
        //}


        public ActionResult PageNotFound()
        {
            return View();
        }


        public ActionResult GetAllMissCalInvoices()
        {
            try
            {
                var FoundMissCalInvoices = DefinitionContext.Sp_GetMissingCalculationInvoices().GroupBy(x => x.ManInvoiceNo).Select(group => group.FirstOrDefault()).ToList();

                List<MissCalInvoiceVM> FoundList = new List<MissCalInvoiceVM>();
                foreach (var item in FoundMissCalInvoices)
                {
                    MissCalInvoiceVM Obj = new MissCalInvoiceVM();
                    Obj.ManInvoiceNo = item.ManInvoiceNo;
                    Obj.InvoiceDate = item.InvoiceDate.ToString("dd/MM/yyyy");
                    Obj.UpdUser = item.UpdUser;
                    Obj.InvoiceNo = item.InvoiceNo;

                    FoundList.Add(Obj);
                }

                return Json(new { MissCalInc = FoundList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }


        public ActionResult GetAllMissCalInvoicesCount()
        {
            try
            {
                var Count = DefinitionContext.Sp_GetMissingCalculationInvoices().GroupBy(x => x.ManInvoiceNo).Count();

                return Json(new { CountInv = Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

}
