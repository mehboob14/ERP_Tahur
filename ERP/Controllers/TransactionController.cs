using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using ERP.Models;
using ERP.Models.VMClasses;
using System.Threading;
using System.Transactions;

namespace ERP.Controllers
{
    public class TransactionController : Controller
    {
        TransactionHandler Handler = new TransactionHandler();


        // GET: Transaction


        #region(---------------------------------------- Salesman Opening Stocks-----------------------------------------)

        public ActionResult SalesmanOpeningStock()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("6","001"))
            {
                ViewBag.CurrentDate = Handler.GetCurrentDate();
                ViewBag.ItemList = Handler.GetSaleItems();

                //using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                //{
                //    ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
                //}

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                //ViewBag.SalesmanDDL = new SelectList(Handler.GetEmpList().ToList(), "EmpCode", "EmpName");

                //ViewBag.RegionList = Handler.GetRegion();
                //ViewBag.GetEmpList = Handler.GetEmpList();
                

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
           
        }



        [HttpPost]
        public ActionResult GetSOSDetail(string GetSOSDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<SalesmanOpeningStockVM> SOSDetail = new List<SalesmanOpeningStockVM>();
            string UpdTrue = "";

            if (CommonDAL.UserRights("6","003"))
            {
                 SOSDetail = Handler.GetDinstinctSOSNo(GetSOSDate).ToList();
                 UpdTrue = "Update";
            }
            

            return Json(new { SOSDetail = SOSDetail,UpdTrue = UpdTrue }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetSingleSOSNo(string SOSNo)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<SalesmanOpeningStockVM> ItemList = Handler.GetSingleSOSNo(SOSNo);
            
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRegionEmp(string RegionCode)
        {
            var RegionEmp = Handler.GetRegionEmp(RegionCode);

            return Json(RegionEmp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ItemDiscountSOS(ItemDiscountVM[] ItemDiscountSOS)
        {
            Session["ItemDiscountSOS"] = ItemDiscountSOS;

            return Json(ItemDiscountSOS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalesmanOpeningStock(SalesmanOpeningStock Obj,string SOSDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditSalesmanOpeningStock(Obj,SOSDate);

            return RedirectToAction("SalesmanOpeningStock");
        }

        [HttpPost]
        public ActionResult DeleteSOS(string SOSNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.DeleteSOS(SOSNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region(--------------------------------------Daily Stock Receive----------------------------------------------)
        public ActionResult DailyStockReceive()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("5","001"))
            {
                
                ViewBag.ItemList = Handler.GetSaleItems();
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    //ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }
               
                //ViewBag.RegionList = Handler.GetRegion();
                ViewBag.CurrentDate = Handler.GetCurrentDate();

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            
        }

        [HttpPost]
        public ActionResult GetDSDDetail(string GetDSRDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<DailyStockReceiveVM> DSDDetail = new List<DailyStockReceiveVM>();
            string UpdTrue = "";

            if (CommonDAL.UserRights("5","003"))
            {
                DSDDetail = Handler.GetDinstinctDSRNoInfo(GetDSRDate).ToList();
                UpdTrue = "Update";
            }

            return Json(new { DSDDetail=DSDDetail,UpdTrue = UpdTrue }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSingleDSRNo(string DSRNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<DailyStockReceiveVM> ItemList = Handler.GetSingleDSRNo(DSRNo);
            ViewBag.ItemList = null;
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ItemDiscountDSR(ItemDiscountVM[] ItemDiscountDSR)
        {
            Session["ItemDiscountDSR"] = ItemDiscountDSR;

            return Json(ItemDiscountDSR, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DailyStockReceive(DailyStockReceive Obj,string DSRDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditDailyStockReceive(Obj,DSRDate);
            return RedirectToAction("DailyStockReceive");
        }
        #endregion


        #region(---------------------------------------------------Sale Item------------------------------------------------------)

        public ActionResult SaleItem()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("10","001"))
            {
                ViewBag.ItemList = Handler.GetSaleItems();

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
        }

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

        public ActionResult GetSingleItem(string ItemCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var data = Handler.GetSingleItem(ItemCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public ActionResult SaleItem(SaleItem Obj)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            //if (Session["ItemImg"] != null)
            //{
            //    HttpPostedFileBase ItemPic = (HttpPostedFileBase)Session["ItemImg"];
            //    if (ItemPic.FileName != "")
            //    {

            //        Obj.ItemPic = new byte[ItemPic.ContentLength];
            //        ItemPic.InputStream.Read(Obj.ItemPic, 0, ItemPic.ContentLength);
            //    }

            //}
            //else
            //{
            //    Obj.ItemPic = Obj.ItemPic;
            //}

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase ItemPic = Request.Files[0];
                if (ItemPic.FileName != "")
                {
                    Obj.ItemPic = new byte[ItemPic.ContentLength];
                    ItemPic.InputStream.Read(Obj.ItemPic, 0, ItemPic.ContentLength);
                }
                else
                {
                    Obj.ItemPic = Session["ItemPic"] as byte[];
                }

            }


            TempData["SuccessMsg"] = Handler.AddEditSaleItem(Obj);

            return RedirectToAction("SaleItem");
        }

        #endregion

        #region(-----------------------------------------------------Sale Invoice --------------------------------------------)

        public ActionResult SaleInvoice()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("7","001"))
            {
                //ViewBag.PartyDDL = new SelectList(Handler.GetSaleParties().ToList(), "PartyCode", "PartyName");

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                //ViewBag.SalesmanDDL = new SelectList(Handler.GetEmpList().ToList(), "EmpCode", "EmpName");

                ViewBag.ItemDDL = new SelectList(Handler.GetSaleItemsWithCode(), "ItemCode", "ItemDesc");

                ViewBag.CurrentDate = Handler.GetCurrentDate();

                //ViewBag.EmpList = Handler.GetEmpList();
                //ViewBag.RegionList = Handler.GetRegion();
                //ViewBag.PartyList = Handler.GetSaleParties();
                //ViewBag.ItemList = Handler.GetSaleItems();

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
           
        }


        public ActionResult GetDamagerInvoices(string DamageDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<SaleInvoiceMasterVM> DamageInvoice = new List<SaleInvoiceMasterVM>();
            if (CommonDAL.UserRights("7", "003"))
            {
                DamageInvoice = Handler.GetSaleInvoiceWithEmp(DamageDate);

                return Json(DamageInvoice, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(DamageInvoice, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMultiRegionEmp(string[] RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var GetRegionEmp = Handler.GetMultiRegionEmp(RegionCode);

            return Json(new { GetRegionEmp }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRegionParty(string RegionCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            //var Data = Handler.GetRegionParty(RegionCode);
            var GetRegionEmp = Handler.GetRegionEmp(RegionCode);

            return Json(GetRegionEmp , JsonRequestBehavior.AllowGet);
        }

        public ActionResult DDLPartyDetail(string RegionCode, string PartyCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var Party = Handler.DDLPartyDetail(RegionCode,PartyCode);
            return Json(Party,JsonRequestBehavior.AllowGet);
        }

        
        //public ActionResult GetRegionDateInvoiceData(string RegionCode,string InvoiceDate)
        //{
        //    if (Session["CompanyCode"] == null)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    List<InvoiceTableSrNo> InvoiceTable;

        //    DateTime Date = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", null);
          
        //    using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
        //    {
        //        InvoiceTable = DefinitionContext.InvoiceTableSrNoes.Where(x => x.RegionCode == RegionCode && x.InvoiceDate == Date).ToList();

        //        Session["InvoiceTable"] = InvoiceTable;
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
         
        //}

      
        //public ActionResult GetRegionDateManInvoice(string ManInvoiceNo)
        //{

        //    if (Session["CompanyCode"] == null)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    if (CommonDAL.UserRoleName() == "DEO")
        //    {
        //        if (Handler.CheckInvoiceDate(ManInvoiceNo))
        //        {
        //            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
        //            {
        //                var InvoiceTable = Session["InvoiceTable"] as List<InvoiceTableSrNo>;
                   
        //                var SrNo = InvoiceTable.Where(x => x.ManInvoiceNo.Trim() == ManInvoiceNo.Trim()).Select(x => x.SrNo).FirstOrDefault();
        //                SrNo++;
                   
        //                var ManInvoice = InvoiceTable.Where(x => x.SrNo == SrNo).Select(x => x.ManInvoiceNo).FirstOrDefault();
                   
        //                return Json(ManInvoice.Trim(), JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { Input = "NoData", ItemDetail = "NoData" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
        //        {
        //            var InvoiceTable = Session["InvoiceTable"] as List<InvoiceTableSrNo>;

        //            var SrNo = InvoiceTable.Where(x => x.ManInvoiceNo.Trim() == ManInvoiceNo.Trim()).Select(x => x.SrNo).FirstOrDefault();
        //            SrNo++;

        //            var ManInvoice = InvoiceTable.Where(x => x.SrNo == SrNo).Select(x => x.ManInvoiceNo).FirstOrDefault();

        //            return Json(ManInvoice.Trim(), JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

       
        //public ActionResult GetRegionDateManInvoicePre(string ManInvoiceNo)
        //{

        //    if (Session["CompanyCode"] == null)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    if (CommonDAL.UserRoleName() == "DEO")
        //    {
        //        if (Handler.CheckInvoiceDate(ManInvoiceNo))
        //        {
        //            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
        //            {
        //                var InvoiceTable = Session["InvoiceTable"] as List<InvoiceTableSrNo>;
                   
        //                var SrNo = InvoiceTable.Where(x => x.ManInvoiceNo.Trim() == ManInvoiceNo.Trim()).Select(x => x.SrNo).FirstOrDefault();
        //                SrNo--;
                   
        //                var ManInvoice = InvoiceTable.Where(x => x.SrNo == SrNo).Select(x => x.ManInvoiceNo).FirstOrDefault();
                   
        //                return Json(ManInvoice.Trim(), JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { Input = "NoData", ItemDetail = "NoData" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
        //        {
        //            var InvoiceTable = Session["InvoiceTable"] as List<InvoiceTableSrNo>;

        //            var SrNo = InvoiceTable.Where(x => x.ManInvoiceNo.Trim() == ManInvoiceNo.Trim()).Select(x => x.SrNo).FirstOrDefault();
        //            SrNo--;

        //            var ManInvoice = InvoiceTable.Where(x => x.SrNo == SrNo).Select(x => x.ManInvoiceNo).FirstOrDefault();

        //            return Json(ManInvoice.Trim(), JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

        public ActionResult CheckPartyDiscount(string RegionCode,string PartyCode,string ItemCode,string PreCurr)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var Data = Handler.GetSinglePartyDiscount(RegionCode, PartyCode, ItemCode.Trim());

                var Item = DefinitionContext.FindItemTax(ItemCode).FirstOrDefault();
                Session["txtPreCurr"] = PreCurr;

                ItemRateVM ItemRate = new ItemRateVM();

                ItemRate = DefinitionContext.SaleItems.Where(x => x.ItemCode == ItemCode).Select(x => new ItemRateVM
                {
                    Category = x.Category,
                    Consumer_Rate = x.Consumer_Rate,
                    RetTaxPer_Act = x.RetTaxPer_Act,
                    RetTaxPer_InAct = x.RetTaxPer_InAct,
                    DistTaxPer_Act = x.DistTaxPer_Act,
                    DistTaxPer_InAct = x.DistTaxPer_InAct,
                    FEDTaxPer = x.FEDTaxPer
                }).FirstOrDefault();

                //ItemRate.Category = DefinitionContext.SaleItems.Where(x => x.ItemCode == ItemCode).Select(x => x.Category).FirstOrDefault();
                //ItemRate.Consumer_Rate = DefinitionContext.SaleItems.Where(x => x.ItemCode == ItemCode).Select(x => x.Consumer_Rate).FirstOrDefault();

                if (PreCurr == "Curr")
                {
                   var NewObj = DefinitionContext.FindItemRate(RegionCode, ItemCode.Trim()).FirstOrDefault();
                    ItemRate.RegCorporateRate = NewObj.RegCorporateRate;
                    ItemRate.UnRegCorporateRate = NewObj.UnRegCorporateRate;
                    ItemRate.RegRetailerRate = NewObj.RegRetailerRate;
                    ItemRate.UnRegRetailerRate = NewObj.UnRegRetailerRate;
                    
                }
                else
                {
                   var NewObj = DefinitionContext.FindPreItemRate(RegionCode, ItemCode.Trim()).FirstOrDefault();
                    ItemRate.RegCorporateRate = NewObj.RegCorporateRate;
                    ItemRate.UnRegCorporateRate = NewObj.UnRegCorporateRate;
                    ItemRate.RegRetailerRate = NewObj.RegRetailerRate;
                    ItemRate.UnRegRetailerRate = NewObj.UnRegRetailerRate;
                }
                

                //var ItemRate = Handler.DDLItemRate(RegionCode, ItemCode);

                //var Item = Handler.GetSingleItemTax(ItemCode);

                return Json(new { Data, Item, ItemRate }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult GetSingleInvoice(int InvoiceNo)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var Input = Handler.GetSingleMasterInvoice(InvoiceNo);
            List<SaleInvoiceDetailVM> ItemDetail = Handler.GetSingleDetailInvoice(InvoiceNo);

            return Json(new { Input = Input, ItemDetail = ItemDetail }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSingleManInvoice(string ManInvoiceNo)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
           if(CommonDAL.UserRights("7", "003"))
           {
               if (CommonDAL.UserRoleName() == "DEO")
               {
                   if (Handler.CheckInvoiceDate(ManInvoiceNo))
                   {
                       var Input = Handler.GetSingleMasterManInvoice(ManInvoiceNo);
              
                       List<SaleInvoiceDetailVM> ItemDetail = Handler.GetSingleDetailManInvoice(ManInvoiceNo);
              
                       return Json(new { Input = Input, ItemDetail = ItemDetail }, JsonRequestBehavior.AllowGet);
                   }
                   else
                   {
                       return Json(new { Input = "NoData", ItemDetail = "NoData" }, JsonRequestBehavior.AllowGet);
                   }
               }
               else
               {
                   var Input = Handler.GetSingleMasterManInvoice(ManInvoiceNo);
              
                   List<SaleInvoiceDetailVM> ItemDetail = Handler.GetSingleDetailManInvoice(ManInvoiceNo);
              
                   return Json(new { Input = Input, ItemDetail = ItemDetail }, JsonRequestBehavior.AllowGet);
               }
            }
            else
            {
                return Json(new { Input = "NoData", ItemDetail = "NoData" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CheckLock()
        {
            var Lock = HttpContext.Application["Lock"];
            return Json(Lock,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaleInvoiceDetail(SaleInvoiceDetail[] ItemTable)
        {
           
                Session["SaleInvoiceDetail"] = ItemTable;

                return Json(JsonRequestBehavior.AllowGet);
           
        }
       
        [HttpPost]
        public ActionResult SaleInvoice(SaleInvoiceMaster Obj,string InvoiceDate)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var Msg = "";
            var SecMsg = "";
            var ThirdMsg = "";
            var FourthMsg = "";
            var FifthMsg = "";

             Msg = Handler.AddEditSaleInvoice(Obj, InvoiceDate);

            if (Msg == "DeadLock")
            {
                SecMsg  = Handler.AddEditSaleInvoiceAgain();

                if (SecMsg == "DeadLock Occur, Try Again . . . !")
                {
                    ThirdMsg = Handler.AddEditSaleInvoiceAgain();

                    if (ThirdMsg == "DeadLock Occur, Try Again . . . !")
                    {
                        FourthMsg = Handler.AddEditSaleInvoiceAgain();

                        if (FourthMsg == "DeadLock Occur, Try Again . . . !")
                        {
                            FifthMsg = Handler.AddEditSaleInvoiceAgain();

                            if (FifthMsg == "DeadLock Occur, Try Again . . . !")
                            {
                                TempData["DeadLockMsg"] = FifthMsg;
                            }
                            else
                            {
                                TempData["DeadLockMsg"] = FifthMsg;
                            }
                        }
                        else
                        {
                            TempData["DeadLockMsg"] = FourthMsg;
                        }
                    }
                    else
                    { 
                       TempData["SuccessMsg"] = ThirdMsg;
                    }
                }
                else
                {
                    TempData["SuccessMsg"] = SecMsg;
                }
            }
            else
            {
                TempData["SuccessMsg"] = Msg;
            }
           
            return RedirectToAction("SaleInvoice");
        }

        public ActionResult TryAgain()
        {
            using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                SaleInvoiceMaster MasterData = new SaleInvoiceMaster();
                List<SaleInvoiceDetail> DetailData = new List<SaleInvoiceDetail>();

                MasterData = Session["MasterData"] as SaleInvoiceMaster;
                DetailData = Session["DetailData"] as List<SaleInvoiceDetail>;

                DefinitionContext.SaleInvoiceMasters.Add(MasterData);
                DefinitionContext.SaleInvoiceDetails.AddRange(DetailData);
                DefinitionContext.SaveChanges();
                TempData["SuccessMsg"] = "Saved Successfully . . . !";

            }
           
            return RedirectToAction("SaleInvoice");
        }


        [HttpPost]
        public ActionResult CheckManInvoice(string ManInvoiceNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.CheckManInvoice(ManInvoiceNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteInvoice(int InvoiceNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.DeleteInvoice(InvoiceNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion

        public ActionResult SaleInvoiceResponsive()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefinitionContext.GetDistinctRegion().ToList();
                ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
            }

            ViewBag.ItemDDL = Handler.GetSaleItems();
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.UniqNo = DateTime.Now.ToString("ddMMyyyy-HHmmss");
            ViewBag.SaleInvoice = Handler.GetSaleInvoiceCurrentDate().ToList();

            return View();
        }

        [HttpPost]
        public ActionResult SaleInvoiceResponsive(SaleInvoiceMaster Obj, string InvoiceDate, string FormName)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var Msg = "";
           
            Msg = Handler.AddEditSaleInvoice(Obj, InvoiceDate);
           
            TempData["SuccessMsg"] = Msg;
            
            return RedirectToAction("SaleInvoiceResponsive");
        }



        #region (---------------------------------------------------------Recovery Against Sale------------------------------------------------------)

        public ActionResult RecoveryAgainstSale()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("8","001"))
            {
               
                //ViewBag.SaleInvoice = Handler.GetCreditSaleInvoices();
                //using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                //{
                //    ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
                //}

                //ViewBag.PartyDDL = new SelectList(Handler.GetDistinctMainParty().ToList(), "PartyCode", "PartyName");

                //using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                //{
                //    var List = DefinitionContext.GetDistinctRegion().ToList();
                //    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                //    var BList = DefinitionContext.Sp_FAS_ChartOfAccounts_Banks().ToList();
                //    ViewBag.BList = new SelectList(BList, "ChildCode", "AccountName");
                //}

                //ViewBag.SalesmanDDL = new SelectList(Handler.GetEmpList().ToList(), "EmpCode", "EmpName");

                //ViewBag.RegionList = Handler.GetRegion();
                //ViewBag.ItemList = Handler.GetSaleItems();
                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
          
        }

        [HttpPost]
        public ActionResult GetRecoveryDetailDateWise(string GetRecDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<RecoveryVM> RecDetail = new List<RecoveryVM>();
            string UpdTrue = "";

            if (CommonDAL.UserRights("8", "003"))
            {
                RecDetail = Handler.GetAllRecoveryList(GetRecDate).ToList();
                UpdTrue = "Update";
            }

            return Json( new { RecDetail = RecDetail,UpdTrue = UpdTrue}, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckRecoveryCheq(string CheqNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.CheckRecoveryCheqNo(CheqNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSingleRecovery(int RecoveryNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<RecoveryVM> List = Handler.GetSingleRecovery(RecoveryNo);

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRecoveryDetail(Recovery[] ItemTable)
        {
            Session["RecoveryDetail"] = ItemTable;

            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RecoveryAgainstSale(Recovery Obj,string RecoveryDate,string CheqDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditRecovery(Obj,RecoveryDate,CheqDate);

            return RedirectToAction("RecoveryAgainstSale");
        }

        [HttpPost]
        public ActionResult DeleteRecovery(int RecoveryNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.DeleteRecovery(RecoveryNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region(----------------------------------------------- PDC Post Dated Cheq -----------------------------------------)

        public ActionResult PDC()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefinitionContext.GetDistinctRegion().ToList();
                ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
            }

            return View();
        }

        public ActionResult GetRegionListDDL()
        {

            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                List<GetDistinctRegion_Result> RDDL = DefinitionContext.GetDistinctRegion().ToList();

                return Json(new { RDDL }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetRecoveryPDCData(string RegionCode,string CheqDate)
        {
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Dat = DateTime.ParseExact(CheqDate, "dd/MM/yyyy", null);
                Session["PDCFDate"] = Dat.ToString("dd/MM/yyyy");
                List<Sp_GetPDCData_Result> PDCList = DefinitionContext.Sp_GetPDCData(RegionCode, Dat).ToList();

                List<RecoveryVM> NewList = new List<RecoveryVM>();
                foreach (var item in PDCList)
                {
                    RecoveryVM Obj = new RecoveryVM();

                    //Obj.VoucherNo = item.VoucherNo;9m,km, 
                    Obj.CheqNo = (item.CheqNo == null) ? item.CheqNo : item.CheqNo.Trim();
                    Obj.CheqDate = item.CheqDate.ToString("dd/MM/yyyy");
                    Obj.RegionCode = item.RegionCode;
                    Obj.RecoveryDate = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.RecoveryDate).FirstOrDefault().ToString("dd/MM/yyyy");
                    Obj.Remarks = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.Remarks).FirstOrDefault();
                    //Obj.RecoveryNo = item.RecoveryNo;
                    //Obj.RecoveryDate = item.RecoveryDate.ToString("dd/MM/yyyy");
                    if (item.Count > 1)
                    {
                        Obj.PartyDesc = "Multiple Parties";
                        Obj.RecAmount = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).AsEnumerable().Sum(x => x.RecAmount);
                    }
                    else
                    {
                        Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                        Obj.RecAmount = item.RecAmount;
                    }
                    
                    Obj.AccountCode = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.AccountCode).FirstOrDefault();

                    if (Obj.AccountCode != null)
                    {
                        Obj.VoucherNo = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.VoucherNo).FirstOrDefault();
                        Obj.AccountName = DefinitionContext.Sp_Get_FAS_ChartOfAccounts_Banks(Obj.AccountCode).Select(x => x.AccountName).FirstOrDefault();
                        Obj.ClearOrBounce = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.ClearOrBounce).FirstOrDefault();
                        Obj.ClearanceDate = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.ClearanceDate).FirstOrDefault().ToString();
                    }

                    NewList.Add(Obj);
                }

                return Json(new { NewList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetPendingPDCData()
        {
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                
                List<Sp_GetPendingPDCData_Result> PDCList = DefinitionContext.Sp_GetPendingPDCData().ToList();

                List<RecoveryVM> NewList = new List<RecoveryVM>();
                foreach (var item in PDCList)
                {
                    RecoveryVM Obj = new RecoveryVM();

                    //Obj.VoucherNo = item.VoucherNo;9m,km, 
                    Obj.CheqNo = (item.CheqNo == null) ? item.CheqNo : item.CheqNo.Trim();
                    Obj.CheqDate = item.CheqDate.ToString("dd/MM/yyyy");
                    Obj.RegionCode = item.RegionCode;
                    Obj.RecoveryDate = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.RecoveryDate).FirstOrDefault().ToString("dd/MM/yyyy");
                    Obj.Remarks = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.Remarks).FirstOrDefault();
                    //Obj.RecoveryNo = item.RecoveryNo;
                    //Obj.RecoveryDate = item.RecoveryDate.ToString("dd/MM/yyyy");
                    if (item.Count > 1)
                    {
                        Obj.PartyDesc = "Multiple Parties";
                        Obj.RecAmount = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).AsEnumerable().Sum(x => x.RecAmount);
                    }
                    else
                    {
                        Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                        Obj.RecAmount = item.RecAmount;
                    }


                    Obj.AccountCode = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.AccountCode).FirstOrDefault();

                    if (Obj.AccountCode != null)
                    {
                        Obj.VoucherNo = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.VoucherNo).FirstOrDefault();
                        Obj.AccountName = DefinitionContext.Sp_Get_FAS_ChartOfAccounts_Banks(Obj.AccountCode).Select(x => x.AccountName).FirstOrDefault();
                        Obj.ClearOrBounce = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.ClearOrBounce).FirstOrDefault();
                        Obj.ClearanceDate = DefinitionContext.Recoveries.Where(x => x.CheqNo == Obj.CheqNo).Select(x => x.ClearanceDate).FirstOrDefault().ToString();
                    }

                    NewList.Add(Obj);
                }

                return Json(new { NewList }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetBankListDDL()
        {

            if (Session["UserCode"] == null)            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                List<DDL> BDDL = DefinitionContext.Sp_FAS_ChartOfAccounts_Banks().Select(x => new DDL { Code = x.ChildCode, Name = x.AccountName }).ToList();

                return Json(new { BDDL }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPDCDetail(Recovery ItemTable,string DeleteItem)
        {
            //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
            //{

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                using (FAS_BlazorEntities FContext = new FAS_BlazorEntities())
                {
                    if (DeleteItem == "Y")
                    {
                        List<Recovery> GetData = DefinitionContext.Recoveries.Where(x => x.VoucherNo == ItemTable.VoucherNo).ToList();
                        if (GetData.Count > 0)
                        {
                            foreach (var item in GetData)
                            {
                                item.VoucherNo = null;
                                item.AccountCode = null;
                                item.ClearOrBounce = null;
                                item.ClearanceDate = null;
                                DefinitionContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                DefinitionContext.SaveChanges();
                            }
                        }

                        var GetFData = FContext.BRV_Masters.Where(x => x.Voucher_Ref_No == ItemTable.VoucherNo).FirstOrDefault();
                        if (GetFData != null)
                        {
                            FContext.Entry(GetFData).State = System.Data.Entity.EntityState.Deleted;
                            FContext.SaveChanges();
                        }
                       
                    }
                    else
                    {

                        if (ItemTable.ClearOrBounce == "Clear" || ItemTable.ClearOrBounce == "Bounce")
                        {
                            List<Recovery> GetData = DefinitionContext.Recoveries.Where(x => x.CheqNo == ItemTable.CheqNo).ToList();

                            BRV_Masters BObj = new BRV_Masters();

                            string FindVoucher = FContext.BRV_Masters.Where(x => x.CreatedDate.Month == DateTime.Now.Month && x.Voucher_Ref_No.Length >= 16).OrderByDescending(x => x.BRV_No).Select(x => x.Voucher_Ref_No).FirstOrDefault();

                            if (FindVoucher == null)
                            {
                                BObj.Voucher_Ref_No = "BRV_PDC_" + DateTime.Now.ToString("MMyyyy") + "_" + 1;
                            }
                            else
                            {
                                var New = FindVoucher.Split('_');
                                int Code = Convert.ToInt32(New[3]);
                                Code += 1;
                                BObj.Voucher_Ref_No = "BRV_PDC_" + DateTime.Now.ToString("MMyyyy") + "_" + Code;
                            }
                           
                            BObj.BRV_Date = ItemTable.ClearanceDate ?? DateTime.Now;
                            BObj.MainDescription = "PDC"+ "--" + "CheqNo:" + ItemTable.CheqNo;
                            BObj.CreatedBy = CommonDAL.UserName();
                            BObj.CreatedDate = DateTime.Now;
                            BObj.UpdBy = CommonDAL.UserName();
                            BObj.UpdDate = DateTime.Now;

                            BRV_Details Obj1 = new BRV_Details();
                            Obj1.Voucher_Ref_No = BObj.Voucher_Ref_No;
                            Obj1.Detail_Acc_ChildCode = (int)ItemTable.AccountCode;
                            Obj1.Debit = (double)GetData.AsEnumerable().Sum(x=>x.RecAmount) - ((double)GetData.AsEnumerable().Sum(x => x.RecWHT) + (double)GetData.AsEnumerable().Sum(x => x.RecDiscount));
                            Obj1.Credit = 0;
                            Obj1.Description = "Bank Debit Amount";
                            BObj.BRV_Details.Add(Obj1);

                            BRV_Details Obj2 = new BRV_Details();
                            Obj2.Voucher_Ref_No = BObj.Voucher_Ref_No;
                            Obj2.Detail_Acc_ChildCode = 1300000;
                            Obj2.Debit = 0;
                            Obj2.Credit = (double)GetData.AsEnumerable().Sum(x => x.RecAmount);
                            Obj2.Description = "Party Credit Amount";
                            BObj.BRV_Details.Add(Obj2);

                            BRV_Details Obj3 = new BRV_Details();
                            Obj3.Voucher_Ref_No = BObj.Voucher_Ref_No;
                            Obj3.Detail_Acc_ChildCode = 1500000;
                            Obj3.Debit = (double)GetData.AsEnumerable().Sum(x => x.RecWHT);
                            Obj3.Credit = 0;
                            Obj3.Description = "WHT Amount";
                            BObj.BRV_Details.Add(Obj3);

                            BRV_Details Obj4 = new BRV_Details();
                            Obj4.Voucher_Ref_No = BObj.Voucher_Ref_No;
                            Obj4.Detail_Acc_ChildCode = 1700000;
                            Obj4.Debit = (double)GetData.AsEnumerable().Sum(x => x.RecDiscount);
                            Obj4.Credit = 0;
                            Obj4.Description = "Recovery Discount Amount";
                            BObj.BRV_Details.Add(Obj4);

                            BObj.BRV_TotalDebit = Obj1.Debit + Obj2.Debit + Obj3.Debit + Obj4.Debit;
                            BObj.BRV_TotalCredit = Obj1.Credit + Obj2.Credit + Obj3.Credit + Obj4.Credit;

                            foreach (var item in GetData)
                            {
                                item.AccountCode = ItemTable.AccountCode;
                                item.ClearOrBounce = ItemTable.ClearOrBounce;
                                item.ClearanceDate = ItemTable.ClearanceDate;

                                item.VoucherNo = BObj.Voucher_Ref_No;

                                DefinitionContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                DefinitionContext.SaveChanges();
                            }

                            
                            FContext.Entry(BObj).State = System.Data.Entity.EntityState.Added;
                            FContext.SaveChanges();
                            //ts.Complete();
                        }
                    }
                }
            }

            return Json(JsonRequestBehavior.AllowGet);
            

            //}
        }

        #endregion


        #region(------------------------------------------Party Discount--------------------------------------------)

        public ActionResult PartyDiscount()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
            }

            //ViewBag.RegionList = Handler.GetRegion();
            ViewBag.PartyList = Handler.GetSaleParties();
            ViewBag.GetPDList = Handler.GetPartyDiscount();

            return View();
        }

        public ActionResult PartyDiscountNew()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("9","001"))
            {
                ViewBag.CurrentDate = Handler.GetCurrentDate();
                //ViewBag.RegionList = Handler.GetRegion();
                //ViewBag.PartyList = Handler.GetSaleParties();
                ViewBag.ItemList = Handler.GetSaleItems();
                //ViewBag.PdList = Handler.GetDistinctPartyDiscountInfo();

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

        public ActionResult SinglePartyDDL(string RegionCode, string PartyCode)
        {
            SaleParty Party = new SaleParty();

           

           
                Party = Handler.DDLPartyDetail(RegionCode, PartyCode);
               
           

            return Json(Party, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDiscountedParty(string RegionCode,string PartyCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.CheckPartyDiscount(RegionCode,PartyCode);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetSinglePartyDiscount(string RegionCode,string PartyCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            string UpdTrue = "";
            List<SinglePartyDiscountNewVM> ItemList = new List<SinglePartyDiscountNewVM>();
            if (CommonDAL.UserRights("9", "003"))
            {
                ItemList = Handler.GetSinglePartyDiscountNew(RegionCode, PartyCode);
                UpdTrue = "Update";
                ViewBag.ItemList = null;
            }

               
            return Json( new { ItemList=ItemList,UpdTrue=UpdTrue }, JsonRequestBehavior.AllowGet);
        }

      

        [HttpPost]
        public JsonResult ItemDiscountDetail(ItemDiscountVM[] ItemTable)
        {
            Session["ItemTable"]  = ItemTable;
          
            return Json(ItemTable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PartyDiscountNew(Discount Obj)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddNewPartyDiscount(Obj);

            return RedirectToAction("PartyDiscountNew");
        }

        [HttpPost]
        public ActionResult GetRegionPartyDDL(string RegionCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<SaleParty> List = new List<SaleParty>();
            List = Handler.GetRegionParty(RegionCode).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetMulitRegionPartyDDL(string[] RegionCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<SaleParty> List = new List<SaleParty>();
            List = Handler.GetMultipleRegionParty(RegionCode).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRegionCreditPartyDDL(string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<SaleParty> List = new List<SaleParty>();
            List = Handler.GetRegionCreditParty(RegionCode).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRegionPartyFast(string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<Sp_GetRegionPartiesFast_Result> List = new List<Sp_GetRegionPartiesFast_Result>();
            List = Handler.GetRegionPartyFast(RegionCode).ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult PartyDiscount(PartyDiscount Obj)
        //{
        //    if (Session["CompanyCode"] == null)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    TempData["SuccessMsg"] = Handler.AddEditPartyDiscount(Obj);

        //    return RedirectToAction("PartyDiscount");
        //}

        [HttpPost]
        public ActionResult ItemDiscountedRate(string RegionCode,string STaxReg,string Category)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<ItemDiscountRateVM> ItemList = Handler.GetItemDiscountedRate(RegionCode, STaxReg, Category).ToList();
           
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region ---------------------------------------------------(Short Cash)-----------------------------------------------------------

        public ActionResult ShortCash()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefinitionContext.GetDistinctRegion().ToList();
                ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
            }

            ViewBag.CurrentDate = Handler.GetCurrentDate();
            ViewBag.CSList = Handler.GetDistinctShortCash().ToList();

            return View();
        }

        [HttpPost]
        public ActionResult GetSingleShortCash(string CSCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<ShortCashVM> ItemList = Handler.GetSingleShortCash(CSCode);
           
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShortCashDetail(ShortCash[] ItemTable)
        {
            Session["ShortCashDetail"] = ItemTable;

            return Json(ItemTable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShortCash(ShortCash Obj)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditShortCash(Obj);

            return RedirectToAction("ShortCash");
        }

        [HttpPost]
        public ActionResult DeleteShortCash(string CSCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.DeleteShortCash(CSCode);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region(--------------------------------------Daily Stock Receive Terminal----------------------------------------------)
        public ActionResult DailyStockReceiveTerminal()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("5", "001"))
            {
                ViewBag.ItemList = Handler.GetSaleItems();
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    //ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                //ViewBag.RegionList = Handler.GetRegion();
                ViewBag.CurrentDate = Handler.GetCurrentDate();

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
        }

        [HttpPost]
        public ActionResult GetDSTerminalDDetail(string GetDSRDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
             
            string UpdTrue = "";

            //if (CommonDAL.UserRights("5", "003"))
            //{
            List<DailyStockReceiveTerminalVM> DSDDetail = Handler.GetDinstinctDSRNoInfoTerminal(GetDSRDate);
                UpdTrue = "Update";
            //}

            return Json(new { DSDDetail = DSDDetail, UpdTrue = UpdTrue }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSingleDSRNoTerminal(string DSRDate,string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            DateTime Date = DateTime.ParseExact(DSRDate, "dd/MM/yyyy", null);

            List<DailyStockReceiveTerminalVM> ItemList = Handler.GetSingleDSRNoTerminal(Date, RegionCode);
            ViewBag.ItemList = null;
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ItemDiscountDSRTerminal(ItemDiscountVM[] ItemDiscountDSR)
        {
            Session["ItemDiscountDSR"] = ItemDiscountDSR;

            return Json(ItemDiscountDSR, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DailyStockReceiveTerminal(DailyStockReceiveTerminal Obj, string DSRDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditDailyStockReceiveTerminal(Obj, DSRDate);
            return RedirectToAction("DailyStockReceiveTerminal");
        }
        #endregion


        #region(---------------------------------------- Salesman Opening Stocks Terminal-----------------------------------------)

        public ActionResult SalesmanOpeningStockTerminal()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("6", "001"))
            {
                ViewBag.CurrentDate = Handler.GetCurrentDate();
                ViewBag.ItemList = Handler.GetSaleItems();

                //using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                //{
                //    ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
                //}

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                //ViewBag.SalesmanDDL = new SelectList(Handler.GetEmpList().ToList(), "EmpCode", "EmpName");

                //ViewBag.RegionList = Handler.GetRegion();
                //ViewBag.GetEmpList = Handler.GetEmpList();


                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }

        }



        [HttpPost]
        public ActionResult GetSOSDetailTerminal(string GetSOSDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
             
            string UpdTrue = "";

            //if (CommonDAL.UserRights("6", "003"))
            //{
                List<SalesmanOpeningStockTerminalVM> SOSDetail = Handler.GetDinstinctSOSNoTerminal(GetSOSDate).ToList();
                UpdTrue = "Update";
            //}


            return Json(new { SOSDetail = SOSDetail, UpdTrue = UpdTrue }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetSingleSOSNoTerminal(string SOSDate, string RegionCode,string EmpCode)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            DateTime Date = DateTime.ParseExact(SOSDate, "dd/MM/yyyy", null);

            List<SalesmanOpeningStockTerminalVM> ItemList = Handler.GetSingleSOSNoTerminal(Date, RegionCode,EmpCode);

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public JsonResult ItemDiscountSOSTerminal(ItemDiscountVM[] ItemDiscountSOS)
        {


            Session["ItemDiscountSOSTerminal"] = ItemDiscountSOS;

            return Json(ItemDiscountSOS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalesmanOpeningStockTerminal(SalesmanOpeningStockTerminal Obj, string SOSDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditSalesmanOpeningStockTerminal(Obj, SOSDate);

            return RedirectToAction("SalesmanOpeningStockTerminal");
        }

        [HttpPost]
        public ActionResult DeleteSOSTerminal(DateTime SOSDate, string RegionCode, string EmpCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.DeleteSOSTerminal(SOSDate, RegionCode, EmpCode);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region (----------------------------------- Short Cash Opening -------------------------------------)

        public ActionResult ShortCashOpening()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }


        [HttpGet]
        public JsonResult GetLazyShortCashOpening()
        {
            List<ShortCashOpeningVM> List = Handler.LazyShortCashOpening().ToList();

            return Json(List, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PMOpening(ShortCashOpening[] OpeningList)
        {
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            string Msg = Handler.AddEditShortCashOpening(OpeningList);

            if (Msg == "Saved Successfully . . . !")
            {
                TempData["SuccessMsg"] = Msg;
            }
            else if (Msg == "Updated Successfully . . . !")
            {
                TempData["UpdateMsg"] = Msg;
            }
            else
            {
                TempData["ErrorMsg"] = Msg;
            }

            return Json(JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region (--------------------------------------------------------- Short Cash Recovery ------------------------------------------------------)

        public ActionResult ShortCashRecovery()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetShortCashRecoveryDetail(string RecDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<ShortCashRecoveryVM> RecDetail = new List<ShortCashRecoveryVM>();
            string UpdTrue = "";

            if (CommonDAL.UserRights("8", "003"))
            {
                RecDetail = Handler.GetAllSCRecoveryList(RecDate).ToList();
                UpdTrue = "Update";
            }

            return Json(new { RecDetail = RecDetail, UpdTrue = UpdTrue }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckSCRecoveryCheq(string CheqNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.CheckSCRecoveryCheqNo(CheqNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSingleSCRecovery(int Id)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<ShortCashRecoveryVM> List = Handler.GetSingleSCRecovery(Id);

            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSCRecoveryDetail(ShortCashRecovery[] ItemTable)
        {
            Session["SCRecoveryDetail"] = ItemTable;

            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShortCashRecovery(ShortCashRecovery Obj, string RecDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["SuccessMsg"] = Handler.AddEditSCRecovery(Obj, RecDate);

            return RedirectToAction("ShortCashRecovery");
        }

        [HttpPost]
        public ActionResult DeleteSCRecovery(int Id)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            bool data = Handler.DeleteSCRecovery(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region(---------------------------------- 1 Year Active Parties ------------------------------------)

        public ActionResult ActivePartiesSalesman()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }


            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (CommonDAL.UserRoleName() == "Admin")
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }
                else
                {
                    string RegionCode = Session["RegionCode"].ToString();
                    if (RegionCode != null)
                    {
                        var List = DefinitionContext.GetDistinctRegion().Where(x=>x.RegionCode == RegionCode).ToList();
                        ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                    }
                    else
                    {
                        var List = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == "099").ToList();
                        ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                    }
                    
                }
               
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetActivePartiesSalesmanData(string RegionCode,string EmpCode)
        {
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                var APSList = DefinitionContext.Sp_GetDistinctActiveParties(RegionCode,EmpCode).ToList();

                List<ActivePartiesSalesmanVM> NewList = new List<ActivePartiesSalesmanVM>();

                foreach (var item in APSList)
                {
                    ActivePartiesSalesmanVM Obj = new ActivePartiesSalesmanVM();
                    Obj.Status = "Active";

                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.PartyName = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();

                    NewList.Add(Obj);
                }

                return Json(new { NewList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetInActivePartiesSalesmanData(string RegionCode,string EmpCode)
        {
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                var APSList = DefinitionContext.Sp_GetInActiveParteisSalesman(RegionCode).ToList();

                List<ActivePartiesSalesmanVM> NewList = new List<ActivePartiesSalesmanVM>();

                foreach (var item in APSList)
                {
                    ActivePartiesSalesmanVM Obj = new ActivePartiesSalesmanVM();

                    var Found = DefinitionContext.ActivePartiesSalesmen.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).FirstOrDefault();
                    Obj.Status = (Found == null) ? "In-Active" : "Active";
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.PartyName = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();

                    NewList.Add(Obj);
                }

                return Json(new { NewList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveActiveParties(ActivePartiesSalesmanVM[] ItemTable)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (ItemTable.Count() > 0)
                {
                    foreach (var item in ItemTable)
                    {
                        if (item.Status == "In-Active")
                        {
                            List<ActivePartiesSalesman> Find = DefinitionContext.ActivePartiesSalesmen.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode && x.EmpCode == item.EmpCode).ToList();
                            DefinitionContext.ActivePartiesSalesmen.RemoveRange(Find);
                            DefinitionContext.SaveChanges();
                        }
                        else
                        {
                            ActivePartiesSalesman Obj = new ActivePartiesSalesman();
                            Obj.RegionCode = item.RegionCode;
                            Obj.PartyCode = item.PartyCode;
                            Obj.EmpCode = item.EmpCode;
                            DefinitionContext.ActivePartiesSalesmen.Add(Obj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                }
                
            }

            return Json(JsonRequestBehavior.AllowGet);
            
        }


        [HttpPost]
        public ActionResult GetRegionPartyRate(string RegionCode, string PartyCode)
        {
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                var ItemList = DefinitionContext.SaleItems.ToList();

                List<RegionPartyRatesVM> NewList = new List<RegionPartyRatesVM>();

                foreach (var item in ItemList)
                {
                    RegionPartyRatesVM Obj = new RegionPartyRatesVM();

                    Obj.ItemCode = item.ItemCode.Trim();
                    Obj.ItemDesc = item.ItemDesc.Trim();

                    var NewObj = DefinitionContext.FindItemRate(RegionCode, Obj.ItemCode).FirstOrDefault();

                    if (NewObj != null)
                    {

                        var RegStatus = DefinitionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.STaxReg).FirstOrDefault();
                        var Category = DefinitionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.Category).FirstOrDefault();
                        double? Rate = 0;
                        if (RegStatus == "Y" && Category == "CORPORATE")
                        {
                            Rate = NewObj.RegCorporateRate;
                        }

                        else if (RegStatus == "N" && Category == "CORPORATE")
                        {
                            Rate = NewObj.UnRegCorporateRate;
                        }

                        else if (RegStatus == "Y" && Category == "Retailer")
                        {
                            Rate = NewObj.RegRetailerRate;
                        }

                        else if (RegStatus == "N" && Category == "Retailer")
                        {
                            Rate = NewObj.UnRegRetailerRate;
                        }

                        double? Discount = Handler.GetSinglePartyDiscount(RegionCode, PartyCode, Obj.ItemCode);
                        var Item = DefinitionContext.FindItemTax(Obj.ItemCode).FirstOrDefault();

                        if (RegStatus == "Y")
                        {

                            Obj.Discount = Discount / ((100 + Item.SaleGSTPer) / 100);
                            double? NewRate = Rate - Obj.Discount;
                            Rate = Rate - Discount;
                            Obj.FurtherTax = 0;
                            Obj.SaleTax = Rate / 100 * Item.SaleGSTPer;
                            Obj.Rate = NewRate + Obj.SaleTax + Obj.FurtherTax;

                        }
                        else
                        {

                            Obj.Discount = Discount / (((100 + Item.SaleGSTPer) + Item.SaleFurtherTaxPer) / 100);
                            double? NewRate = Rate - Obj.Discount;
                            Rate = Rate - Discount;
                            Obj.FurtherTax = Rate / 100 * Item.SaleFurtherTaxPer;
                            Obj.SaleTax = Rate / 100 * Item.SaleGSTPer;
                            Obj.Rate = NewRate + Obj.SaleTax + Obj.FurtherTax;
                        }


                        NewList.Add(Obj);
                    }
                }

                return Json(new { NewList }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
    }
}