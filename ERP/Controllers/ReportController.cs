using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using ERP.Models;
using ERP.Models.VMClasses;

namespace ERP.Controllers
{
    public class ReportController : Controller
    {
        TransactionHandler Handler = new TransactionHandler();
        // GET: Report
        public ActionResult ReportIndex()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("11","001"))
            {
                //ViewBag.EmpList = Handler.GetEmpList();
                //ViewBag.PartyList = Handler.GetSaleParties();

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                ViewBag.ItemDDL = new SelectList(Handler.GetSaleItemsWithCode(), "ItemCode", "ItemDesc");

                //ViewBag.PartyDDL = new SelectList(Handler.GetSaleParties().ToList(), "PartyCode", "PartyName");
                //ViewBag.SalesmanDDL = new SelectList(Handler.GetEmpList().ToList(), "EmpCode", "EmpName");

                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
          
        }

        [HttpPost]
        public ActionResult ShowSaleInvoiceDetail( string FromDate, string ToDate, string EmpType, string PartyType,string EmpCode, string PartyCode, string PType,string PCategory, string PSTaxReg, string PTypeBoth,string PCategoryBoth,string PSTaxRegBoth, string[] RegionCode,string RegionType,string NorthRegion,string ItemType,string ItemCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["EmpCode"] = EmpCode;
            Session["PartyCode"] = PartyCode;
            Session["EmpType"] = EmpType;
            Session["PartyType"] = PartyType;
            Session["FromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["ToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            Session["OptRegionCode"] = RegionCode;
            Session["RegionType"] = RegionType;
            Session["PType"] = PType;
            Session["PCategory"] = PCategory;
            Session["PSTaxReg"] = PSTaxReg;
            Session["PTypeBoth"] = PTypeBoth;
            Session["PCategoryBoth"] = PCategoryBoth;
            Session["PSTaxRegBoth"] = PSTaxRegBoth;
            Session["NorthRegion"] = NorthRegion;
            Session["ItemType"] = ItemType;
            Session["ItemCode"] = ItemCode;


            return View();
        }

   
        public ActionResult PrintSaleSummary()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            var UserName = CommonDAL.UserName();

            DateTime FromDate = (DateTime)Session["FromDate"];
            //string FromDate = DateTime.ParseExact(FromDat, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture).ToString("dd'/'MM'/'yyyy");
            DateTime ToDate = (DateTime)Session["ToDate"];
            //ToDate = DateTime.ParseExact(ToDate, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture).ToString("dd'/'MM'/'yyyy");

           
            string DSRExcel = Request.QueryString["DSRExcel"];
            string PartyCode = (string)Session["PartyCode"];
            string EmpCode = (string)Session["EmpCode"];

            string EmpType = (string)Session["EmpType"];
            string PartyType = (string)Session["PartyType"];

            string[] RegionCode = (string[])Session["OptRegionCode"];
            string RegionType = (string)Session["RegionType"];

            string PType = (string)Session["PType"];
            string PCategory = (string)Session["PCategory"];
            string PSTaxReg = (string)Session["PSTaxReg"];
            string PTypeBoth = (string)Session["PTypeBoth"];
            string PCategoryBoth = (string)Session["PCategoryBoth"];
            string PSTaxRegBoth = (string)Session["PSTaxRegBoth"];
            string NorthRegion = (string)Session["NorthRegion"];
            string ItemType = (string)Session["ItemType"];
            string ItemCode = (string)Session["ItemCode"];



            string EmpName = "";
            string RegionDescription = "";

            if (EmpType == null)
            {
                foreach (var item in RegionCode)
                {
                    EmpName = DefinitionContext.CompanyEmps.Where(x => x.RegionCode == item && x.EmpCode == EmpCode).Select(x => x.EmpName).FirstOrDefault();
                }
            }
            else
            {
                EmpName = "All Salesman";
            }

            if (RegionType == null && NorthRegion == null)
            {
                foreach (var item in RegionCode)
                {
                    RegionDescription = RegionDescription + "  |  " + DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item).Select(x => x.RegionDescription).FirstOrDefault();
                }

            }
            else if (RegionType == null && NorthRegion != null)
            {
                RegionDescription = "North Regions";
            }
            else if (RegionType != null && NorthRegion == null)
            {
                RegionDescription = "All Regions";
            }

            ReportDocument rd = new ReportDocument();

            if (EmpCode == null)
            {
                EmpCode = "";
            }

            if (PartyCode == null)
            {
                PartyCode = "";
            }

            if (EmpType == null)
            {
                EmpType = "";
            }

            if (PartyType == null)
            {
                PartyType = "";
            }

            if (PTypeBoth == null)
            {
                PTypeBoth = "";
            }

            if (PCategoryBoth == null)
            {
                PCategoryBoth = "";
            }

            if (PSTaxRegBoth == null)
            {
                PSTaxRegBoth = "";
            }

            if (RegionType == null)
            {
                RegionType = "";
            }

            if (NorthRegion == null)
            {
                NorthRegion = "";
            }

            if (ItemType == null)
            {
                ItemType = "";
            }

            if (ItemCode == null)
            {
                ItemCode = "";
            }

            string R1 = "";
            string R2 = "";
            string R3 = "";
            string R4 = "";
            string R5 = "";
            string R6 = "";
            string R7 = "";
            string R8 = "";
            string R9 = "";
            string R10 = "";



            if (RegionCode != null)
            {

                foreach (var item in RegionCode)
                {
                    if (item == "001")
                    {
                        R1 = "001";
                    }
                    else if (item == "002")
                    {
                        R2 = "002";
                    }
                    else if (item == "003")
                    {
                        R3 = "003";
                    }
                    else if (item == "004")
                    {
                        R4 = "004";

                    }
                    else if (item == "005")
                    {
                        R5 = "005";
                    }
                    else if (item == "006")
                    {
                        R6 = "006";

                    }
                    else if (item == "007")
                    {
                        R7 = "007";
                    }
                    else if (item == "008")
                    {
                        R8 = "008";
                    }
                    else if (item == "009")
                    {
                        R9 = "009";
                    }
                    else if (item == "010")
                    {
                        R10 = "010";
                    }

                }
            }


            try
            {

                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/SaleInvoiceSummary.rpt")));


                rd.SetParameterValue("CompanyCode", CompanyCode/*, rd.Subreports[1].Name.ToString()*/);

                rd.SetParameterValue("UserName", UserName/*, rd.Subreports[1].Name.ToString()*/);

                rd.SetParameterValue("@RecFromDate", FromDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecToDate", ToDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecRegionType", RegionType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecRegionCode", RegionCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecEmpType", EmpType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecEmpCode", EmpCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecNorthRegion", NorthRegion/*, rd.Subreports[1].Name.ToString()*/);

               rd.SetParameterValue("@RecR1",R1);
               rd.SetParameterValue("@RecR2",R2);
               rd.SetParameterValue("@RecR3",R3);
               rd.SetParameterValue("@RecR4",R4);
               rd.SetParameterValue("@RecR5",R5);
               rd.SetParameterValue("@RecR6",R6);
               rd.SetParameterValue("@RecR7",R7);
               rd.SetParameterValue("@RecR8",R8);
               rd.SetParameterValue("@RecR9",R9);
               rd.SetParameterValue("@RecR10",R10);

                rd.SetParameterValue("@R1OPen", R1);
                rd.SetParameterValue("@R2OPen", R2);
                rd.SetParameterValue("@R3OPen", R3);
                rd.SetParameterValue("@R4OPen", R4);
                rd.SetParameterValue("@R5OPen", R5);
                rd.SetParameterValue("@R6OPen", R6);
                rd.SetParameterValue("@R7OPen", R7);
                rd.SetParameterValue("@R8OPen", R8);
                rd.SetParameterValue("@R9OPen", R9);
                rd.SetParameterValue("@R10Open", R10);

                rd.SetParameterValue("@SaleR1", R1);
                rd.SetParameterValue("@SaleR2", R2);
                rd.SetParameterValue("@SaleR3", R3);
                rd.SetParameterValue("@SaleR4", R4);
                rd.SetParameterValue("@SaleR5", R5);
                rd.SetParameterValue("@SaleR6", R6);
                rd.SetParameterValue("@SaleR7", R7);
                rd.SetParameterValue("@SaleR8", R8);
                rd.SetParameterValue("@SaleR9", R9);
                rd.SetParameterValue("@SaleR10",R10);


                rd.SetParameterValue("@SaleRegionType", RegionType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleRegionCode", RegionCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleEmpType", EmpType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleEmpCode", EmpCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleFrommDate", FromDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleTooDate", ToDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@NorthRegion", NorthRegion/*, rd.Subreports[1].Name.ToString()*/);


                rd.SetParameterValue("@RegionTypeForOpen", RegionType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RegionCodeForOpen", RegionCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@EmpTypeForOpen", EmpType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@EmpCodeForOpen", EmpCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@FromDateForOpen", FromDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@ToDateForOpen", ToDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@NorthRegionForOpen", NorthRegion/*, rd.Subreports[1].Name.ToString()*/);

                rd.SetParameterValue("RegionDescription", RegionDescription);
                rd.SetParameterValue("EmpName", EmpName);


                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();



               
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                rd.Close();
                rd.Dispose();
                GC.Collect();

                return File(stream, "SaleSummary.pdf");
            }
            catch
            {
                throw;
            }


            //return View();
        }

        public ActionResult PrintDSRDetail()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            var UserName = CommonDAL.UserName();

            DateTime FromDate = (DateTime)Session["FromDate"];
            //string FromDate = DateTime.ParseExact(FromDat, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture).ToString("dd'/'MM'/'yyyy");
            DateTime ToDate = (DateTime)Session["ToDate"];
            //ToDate = DateTime.ParseExact(ToDate, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture).ToString("dd'/'MM'/'yyyy");


            string DSRExcel = Request.QueryString["DSRExcel"];
            string PartyCode = (string)Session["PartyCode"];
            string EmpCode = (string)Session["EmpCode"];

            string EmpType = (string)Session["EmpType"];
            string PartyType = (string)Session["PartyType"];

            string[] OptRegionCode = (string[])Session["OptRegionCode"];
            string RegionType = (string)Session["RegionType"];

            string PType = (string)Session["PType"];
            string PCategory = (string)Session["PCategory"];
            string PSTaxReg = (string)Session["PSTaxReg"];
            string PTypeBoth = (string)Session["PTypeBoth"];
            string PCategoryBoth = (string)Session["PCategoryBoth"];
            string PSTaxRegBoth = (string)Session["PSTaxRegBoth"];
            string NorthRegion = (string)Session["NorthRegion"];
            string ItemType = (string)Session["ItemType"];
            string ItemCode = (string)Session["ItemCode"];

            ReportDocument rd = new ReportDocument();

            if (EmpCode == null)
            {
                EmpCode = "";
            }

            if (PartyCode == null)
            {
                PartyCode = "";
            }

            if (EmpType == null)
            {
                EmpType = "";
            }

            if (PartyType == null)
            {
                PartyType = "";
            }

            if (PTypeBoth == null)
            {
                PTypeBoth = "";
            }

            if (PCategoryBoth == null)
            {
                PCategoryBoth = "";
            }

            if (PSTaxRegBoth == null)
            {
                PSTaxRegBoth = "";
            }

            if (RegionType == null)
            {
                RegionType = "";
            }

            if (NorthRegion == null)
            {
                NorthRegion = "";
            }

            if (ItemType == null)
            {
                ItemType = "";
            }

            if (ItemCode == null)
            {
                ItemCode = "";
            }

            string R1 = "";
            string R2 = "";
            string R3 = "";
            string R4 = "";
            string R5 = "";
            string R6 = "";
            string R7 = "";
            string R8 = "";
            string R9 = "";
            string R10 = "";

            string RegionCode = "";

            if (OptRegionCode != null)
            {

                foreach (var item in OptRegionCode)
                {
                    if (item == "001")
                    {
                        R1 = "001";
                    }
                    else if (item == "002")
                    {
                        R2 = "002";
                    }
                    else if (item == "003")
                    {
                        R3 = "003";
                    }
                    else if (item == "004")
                    {
                        R4 = "004";

                    }
                    else if (item == "005")
                    {
                        R5 = "005";
                    }
                    else if (item == "006")
                    {
                        R6 = "006";

                    }
                    else if (item == "007")
                    {
                        R7 = "007";
                    }
                    else if (item == "008")
                    {
                        R8 = "008";
                    }
                    else if (item == "009")
                    {
                        R9 = "009";
                    }
                    else if (item == "010")
                    {
                        R10 = "010";
                    }

                    RegionCode = item;
                }
            }

            try
            {

                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/SaleInvoiceDetail.rpt")));

                rd.SetParameterValue("CompanyCode", CompanyCode);
                rd.SetParameterValue("EmpCode", EmpCode);
                rd.SetParameterValue("PartyCode", PartyCode);
                rd.SetParameterValue("EmpType", EmpType);
                rd.SetParameterValue("PartyType", PartyType);
                rd.SetParameterValue("UserName", UserName);
                rd.SetParameterValue("RegionCode", RegionCode);
                rd.SetParameterValue("RegionType", RegionType);
                rd.SetParameterValue("PType", PType);
                rd.SetParameterValue("PCategory", PCategory);
                rd.SetParameterValue("PSTaxReg", PSTaxReg);
                rd.SetParameterValue("PTypeBoth", PTypeBoth);
                rd.SetParameterValue("PCategoryBoth", PCategoryBoth);
                rd.SetParameterValue("PSTaxRegBoth", PSTaxRegBoth);
                rd.SetParameterValue("DNorthRegion", NorthRegion);
                rd.SetParameterValue("ItemType", ItemType);
                rd.SetParameterValue("ItemCode", ItemCode);

                rd.SetParameterValue("@FFromDate", FromDate);
                rd.SetParameterValue("@TToDate", ToDate);

                rd.SetParameterValue("R1", R1);
                rd.SetParameterValue("R2", R2);
                rd.SetParameterValue("R3", R3);
                rd.SetParameterValue("R4", R4);
                rd.SetParameterValue("R5", R5);
                rd.SetParameterValue("R6", R6);
                rd.SetParameterValue("R7", R7);
                rd.SetParameterValue("R8", R8);
                rd.SetParameterValue("R9", R9);
                rd.SetParameterValue("R10", R10);


                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "DSR Detail.pdf");
            }
            catch
            {
                throw;
            }
        }



        public ActionResult PartyLedger()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("12","001"))
            {

                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                }

                //ViewBag.PartyList = Handler.GetSaleParties();
                //ViewBag.PartyDDL = new SelectList(Handler.GetSaleParties().ToList(), "PartyCode", "PartyName");



                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
           
        }

        
        public ActionResult ShowPartyLedger(string FromDate, string ToDate, string PartyCode,string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
           
            Session["PartyCode"] = PartyCode;
            Session["RegionCode"] = RegionCode;
            Session["FromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["ToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            return View();
        }

        public ActionResult ExportPrint(string FromDate, string ToDate, string PartyCode, string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

          
            DateTime FromDat = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime ToDat = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            ReportDocument rd = new ReportDocument();

            rd.Dispose();
            rd.Close();
            GC.Collect();

            string CompanyCode = CommonDAL.CompCode();
            var UserName = CommonDAL.UserName();

            rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/PartyLedger.rpt")));
            rd.SetParameterValue("CompanyCode", CompanyCode);
            rd.SetParameterValue("UserName", UserName);
            rd.SetParameterValue("@PartyCode", PartyCode);
            rd.SetParameterValue("@RegionCode", RegionCode);
            rd.SetParameterValue("@FromDate", FromDat);
            rd.SetParameterValue("@ToDate", ToDat);
            rd.SetParameterValue("@OPartyCode", PartyCode);
            rd.SetParameterValue("@ORegionCode", RegionCode);
            rd.SetParameterValue("@OFromDate", FromDat);

            rd.SetParameterValue("@PLPartyCode", PartyCode);
            rd.SetParameterValue("@PLRegionCode", RegionCode);
            rd.SetParameterValue("@PLFromDate", FromDat);


            string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
            string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
            string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
            string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
            rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "SaleOrderReport.pdf");
            }
            catch
            {
                throw;
            }



            //return View();
        }


        public ActionResult DailyStockDetail()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("13", "001"))
            {
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

        [HttpPost]
        public ActionResult ShowDailyStockReport(string FromDate, string ToDate, string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }



            Session["RegionCode"] = RegionCode;
            Session["FromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["ToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            return View();
        }

        public ActionResult StockDetail()
        {
            ReportDocument rd = new ReportDocument();
            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            var FromDate = Session["FromDate"];

            var ToDate = Session["ToDate"];

            var UserName = CommonDAL.UserName();
            string RegionCode = (string)Session["RegionCode"];
            string RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
           

            try
            {

                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/DailyStockReport.rpt")));
                rd.SetParameterValue("CompanyCode", CompanyCode);
                rd.SetParameterValue("UserName", UserName);
                rd.SetParameterValue("RegionDesc", RegionDesc);

                rd.SetParameterValue("@RegionCode", RegionCode);
                rd.SetParameterValue("@ORegionCode", RegionCode);
                rd.SetParameterValue("@SRRegionCode", RegionCode);

                rd.SetParameterValue("@DFromDate", FromDate);
                rd.SetParameterValue("@DToDate", ToDate);

                rd.SetParameterValue("@OFromDate", FromDate);

                rd.SetParameterValue("@SRFromDate", FromDate);
                rd.SetParameterValue("@SRToDate", ToDate);

                rd.SetParameterValue("@OSFFromDate", FromDate);
                rd.SetParameterValue("@OSFRegionCode", RegionCode);
                //rd.SetParameterValue("@PPRFromDate", FromDate);


                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "DSR Detail.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult StockSummary()
        {
            ReportDocument rd = new ReportDocument();
            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            var FromDate = Session["FromDate"];

            var ToDate = Session["ToDate"];
            var UserName = CommonDAL.UserName();
            string RegionCode = (string)Session["RegionCode"];
            string RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
            

            try
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/DailyStockReportShort.rpt")));
                rd.SetParameterValue("RegionCode", RegionCode);
                rd.SetParameterValue("FromDate", FromDate);
                rd.SetParameterValue("ToDate", ToDate);
                rd.SetParameterValue("RegionDesc", RegionDesc);
                rd.SetParameterValue("UserName", UserName);

                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "Stock Summary.pdf");

            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }

        [HttpPost]
        public ActionResult ShowCreditorSummary(string FromDate, string ToDate, string RegionCode, string RegionType, string PartyCode, string PartyType)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["RegionCode"] = RegionCode;
            Session["RegionType"] = RegionType;
            Session["PartyCode"] = PartyCode;
            Session["PartyType"] = PartyType;
            Session["FromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["ToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            return View();
        }

        public ActionResult CreditorSummary()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }


            if (CommonDAL.UserRights("14", "001"))
            {
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


        [HttpPost]
        public ActionResult ShowSaleTaxRegister(string FromDate, string ToDate, string RegionCode,string PartyCode,string SelectRegion,string SelectParty, string STaxReg, string ExpTax, string PSTaxRegBoth, string PExpTaxBoth )
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["RegionCode"] = RegionCode;
            Session["ExpTax"] = ExpTax;

            Session["STaxReg"] = STaxReg;
            Session["PartyCode"] = PartyCode;

            Session["STRFromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["STRToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            Session["SelectRegion"] = SelectRegion;
            Session["SelectParty"] = SelectParty;

            Session["PSTaxRegBoth"] = PSTaxRegBoth;
            Session["PExpTaxBoth"] = PExpTaxBoth;

            return View();
        }

        public ActionResult SaleTaxRegister()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("15", "001"))
            {
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                    //ViewBag.PartyDDL = new SelectList(Handler.GetSaleParties().ToList(), "PartyCode", "PartyName");
                }
                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
        }


        public ActionResult CreditorAgeing()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (CommonDAL.UserRights("15", "001"))
            {
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var List = DefinitionContext.GetDistinctRegion().ToList();
                    ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                    //ViewBag.PartyDDL = new SelectList(Handler.GetSaleParties().ToList(), "PartyCode", "PartyName");
                }
                return View();
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
        }


        [HttpPost]
        public ActionResult ShowCreditorAgeing(string FromDate, string RegionCode, string PartyCode, string RegionAll, string PartyAll)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["RegionCode"] = RegionCode;
            Session["PartyCode"] = PartyCode;
            Session["AgeFromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            
            Session["RegionAll"] = RegionAll;
            Session["PartyAll"] = PartyAll;

            return View();
        }

        public ActionResult DailyIntakeReport()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }


        public ActionResult DailyProductionReport()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult DailyDispatchReport()
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

        public ActionResult DailyStockReport()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }


        public ActionResult RecoveryDetailReport()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefinitionContext.GetDistinctRegion().ToList();
                ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");
                //ViewBag.PartyDDL = new SelectList(Handler.GetSaleParties().ToList(), "PartyCode", "PartyName");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ShowRecoveryDetail(string FromDate, string ToDate, string RegionCode, string PartyCode, string SelectRegion, string SelectParty)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["RegionCode"] = RegionCode;
            Session["PartyCode"] = PartyCode;
            Session["RecFromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["RecToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            Session["SelectRegion"] = SelectRegion;
            Session["SelectParty"] = SelectParty;

            return View();
        }


        public ActionResult ShortCashReport()
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


        [HttpPost]
        public ActionResult ShowShortCash(string FromDate, string ToDate, string RegionCode, string EmpCode, string SelectRegion, string SelectEmp)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["RegionCode"] = RegionCode;
            Session["EmpCode"] = EmpCode;
            Session["FromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["ToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            Session["SelectRegion"] = SelectRegion;
            Session["SelectEmp"] = SelectEmp;

            return View();
        }

        public ActionResult ExportShortCashPrint(string FromDate, string ToDate, string RegionCode, string EmpCode, string SelectRegion, string SelectEmp)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }


            DateTime FromDat = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime ToDat = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            ReportDocument rd = new ReportDocument();

            rd.Dispose();
            rd.Close();
            GC.Collect();

            string CompanyCode = CommonDAL.CompCode();
            var UserName = CommonDAL.UserName();

            rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/ShortCashReport.rpt")));
            rd.SetParameterValue("UserName", UserName);
            rd.SetParameterValue("@FromDate", FromDat);
            rd.SetParameterValue("@ToDate", ToDat);
            rd.SetParameterValue("@RegionCode", RegionCode);
            rd.SetParameterValue("@EmpCode", EmpCode);
            rd.SetParameterValue("@SelectRegion", SelectRegion);
            rd.SetParameterValue("@SelectEmp", SelectEmp);

            rd.SetParameterValue("@RecFromDate", FromDat);
            rd.SetParameterValue("@RecToDate", ToDat);
            rd.SetParameterValue("@RecRegionCode", RegionCode);
            rd.SetParameterValue("@RecEmpCode", EmpCode);
            rd.SetParameterValue("@RecSelectRegion", SelectRegion);
            rd.SetParameterValue("@RecSelectEmp", SelectEmp);

            rd.SetParameterValue("@CSFromDate", FromDat);
            rd.SetParameterValue("@CSToDate", ToDat);
            rd.SetParameterValue("@CSRegionCode", RegionCode);
            rd.SetParameterValue("@CSEmpCode", EmpCode);
            rd.SetParameterValue("@CSSelectRegion", SelectRegion);
            rd.SetParameterValue("@CSSelectEmp", SelectEmp);

            rd.SetParameterValue("@OPFromDate", FromDat);
            
            rd.SetParameterValue("@OPRegionCode", RegionCode);
            rd.SetParameterValue("@OPEmpCode", EmpCode);
            rd.SetParameterValue("@OPSelectRegion", SelectRegion);
            rd.SetParameterValue("@OPSelectEmp", SelectEmp);
                                   
            rd.SetParameterValue("@OPRecFromDate", FromDat);
            
            rd.SetParameterValue("@OPRecRegionCode", RegionCode);
            rd.SetParameterValue("@OPRecEmpCode", EmpCode);
            rd.SetParameterValue("@OPRecSelectRegion", SelectRegion);
            rd.SetParameterValue("@OPRecSelectEmp", SelectEmp);
                                   
            rd.SetParameterValue("@OPCSFromDate", FromDat);
           
            rd.SetParameterValue("@OPCSRegionCode", RegionCode);
            rd.SetParameterValue("@OPCSEmpCode", EmpCode);
            rd.SetParameterValue("@OPCSSelectRegion", SelectRegion);
            rd.SetParameterValue("@OPCSSelectEmp", SelectEmp);

            string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
            string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
            string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
            string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
            rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "ShortCashReport.pdf");
            }
            catch
            {
                throw;
            }



            //return View();
        }
        public ActionResult DEOPerformance()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetDEOPerformance(string DEODate)
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                string fDate = DEODate + " 01:00:00";
                string tDate = DEODate + " 23:59:00";

                DateTime FromDate = DateTime.ParseExact(fDate , "dd/MM/yyyy HH:mm:ss", null);
                DateTime ToDate = DateTime.ParseExact(tDate , "dd/MM/yyyy HH:mm:ss", null);

                var GetDEODetail = DefinitionContext.GetDEOPerfomance1(FromDate, ToDate).ToList();

                return Json(GetDEODetail, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public ActionResult GetDEOInvoiceDetail(string User,string Date)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                if (Session["CompanyCode"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }


                string fDate = Date + " 01:00:00";
                string tDate = Date + " 23:59:00";

                DateTime FromDate = DateTime.ParseExact(fDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime ToDate = DateTime.ParseExact(tDate, "dd/MM/yyyy HH:mm:ss", null);

                List<SaleInvoiceMaster> GetInvoiceDetail = DefinitionContext.SaleInvoiceMasters.Where(x => x.UpdDate >= FromDate && x.UpdDate <= ToDate && x.UpdUser == User).OrderBy(x => x.UpdDate).ToList();

                List<SaleInvoiceMasterVM> InvoiceMas = new List<SaleInvoiceMasterVM>();

                foreach (var item in GetInvoiceDetail)
                {
                    SaleInvoiceMasterVM Obj = new SaleInvoiceMasterVM();

                    Obj.InvoiceNo = item.InvoiceNo;
                    Obj.InvoiceDate = item.InvoiceDate.ToString("dd/MM/yyyy");
                    Obj.ManInvoiceNo = item.ManInvoiceNo;
                    Obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    Obj.UpdDate = item.UpdDate.ToString("HH:mm:ss");

                    InvoiceMas.Add(Obj);
                }


                return Json(InvoiceMas, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult SaleTaxInvoice()
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


        [HttpPost]
        public ActionResult ShowSaleTaxInvoice(string ManInvoiceNo)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["ManInvoiceNo"] = ManInvoiceNo;

            return View();
        }


        public ActionResult ExportSaleTaxInvoice(string ManInvoiceNo)
        {
            ReportDocument rd = new ReportDocument();
            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            //var ManInvoiceNo = Session["ManInvoiceNo"];
           
            var UserName = CommonDAL.UserName();

            try
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports/Transaction/SaleTaxInvoice.rpt")));
                rd.SetParameterValue("ManInvoiceNo", ManInvoiceNo);

                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "Sale Tax Invoice.pdf");

            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }

        public ActionResult ExportMultiPrint(string FromDate, string ToDate, string PartyCode, string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }


            DateTime FromDat = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime ToDat = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            ReportDocument rd = new ReportDocument();

            rd.Dispose();
            rd.Close();
            GC.Collect();

            string CompanyCode = CommonDAL.CompCode();
            var UserName = CommonDAL.UserName();

            rd.Load(Path.Combine(Server.MapPath("~/Reports/Transaction/Multiple_STInvoices.rpt")));
            rd.SetParameterValue("RegionCode", RegionCode);
            rd.SetParameterValue("PartyCode", PartyCode);
            rd.SetParameterValue("FromDate", FromDat);
            rd.SetParameterValue("ToDate", ToDat);


            string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
            string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
            string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
            string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
            rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "SaleOrderReport.pdf");
            }
            catch
            {
                throw;
            }



            //return View();
        }


        public ActionResult ExportSaleTaxInvoiceA5(int InvoiceNo)
        {
            ReportDocument rd = new ReportDocument();
            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            //var ManInvoiceNo = Session["ManInvoiceNo"];

            var UserName = CommonDAL.UserName();
            //ReportDocument reportDocument = new ReportDocument();
            try
            {
                //PrintDialog printDialog = new PrintDialog();
                //if (printDialog.ShowDialog() == DialogResult.OK)
                //{

                rd.Load(Path.Combine(Server.MapPath("~/Reports/Transaction/SaleTaxInvoice_A5_2.rpt")));
                rd.SetParameterValue("InvoiceNo", InvoiceNo);
                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);

                //reportDocument.PrintOptions.PrinterName = printDialog.PrinterSettings.PrinterName;
                //reportDocument.PrintToPrinter(printDialog.PrinterSettings.Copies, printDialog.PrinterSettings.Collate, printDialog.PrinterSettings.FromPage, printDialog.PrinterSettings.ToPage);
                //}

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "SaleTaxInvoiceA5.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }

        public ActionResult PlantStockReport()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public ActionResult TerminalStockReport()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();

        }


        public ActionResult DemandWithDispatch()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public ActionResult SalesmanIncentive()
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

        public ActionResult SalesmanSummaryForEmployees()
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

        [HttpPost]
        public ActionResult ShowSaleSummary(string FromDate, string ToDate, string EmpType, string EmpCode, string[] RegionCode, string RegionType)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session["EmpCode"] = EmpCode;
            Session["EmpType"] = EmpType;
            Session["FromDate"] = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            Session["ToDate"] = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            Session["OptRegionCode"] = RegionCode;
            Session["RegionType"] = RegionType;

            return View();
        }


        public ActionResult PDC_Report()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public ActionResult ExportPDCRec(string VoucherRefNo)
        {
            ReportDocument rd = new ReportDocument();
           

            try
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports/FASReports/Rec_PDC_BRV.rpt")));
                rd.SetParameterValue("@VoucherRefNo", VoucherRefNo);
                rd.SetParameterValue("@RVoucherRefNo", VoucherRefNo);
                rd.SetParameterValue("@RVoucherRefNo", VoucherRefNo, rd.Subreports[0].Name.ToString());

                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "PDC.pdf");

            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }

        public ActionResult ShortCash_Report()
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


        public ActionResult ExportShortCashLedger(string FromDate,string ToDate, string RegionCode,string EmpCode)
        {
            ReportDocument rd = new ReportDocument();

            DateTime From = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime To = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            try
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/SalesmanShortCash.rpt")));
                rd.SetParameterValue("@SFromDate", From);
                rd.SetParameterValue("@DFromDate", From);
                rd.SetParameterValue("@DToDate", To);
                rd.SetParameterValue("@DRegionCode", RegionCode);
                rd.SetParameterValue("@DEmpCode", EmpCode);

                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "PDC.pdf");

            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }

        public ActionResult ExportShortCashSummary(string FromDate, string ToDate)
        {
            ReportDocument rd = new ReportDocument();

            DateTime From = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime To = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            try
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/SalesmanShortCashSummary.rpt")));
                rd.SetParameterValue("@SFromDate", From);
                rd.SetParameterValue("@DFromDate", From);
                rd.SetParameterValue("@DToDate", To);
                

                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "PDC.pdf");

            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }

        public ActionResult DEO_vs_Salesman()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public ActionResult ExportDEOSalesmanAnalysis(string FDate, string TDate)
        {
            ReportDocument rd = new ReportDocument();

            string ff = FDate + " 01:00:00";
            string tt = TDate + " 23:59:00";

            DateTime FromDate = DateTime.ParseExact(ff, "dd/MM/yyyy HH:mm:ss", null);
            DateTime ToDate = DateTime.ParseExact(tt, "dd/MM/yyyy HH:mm:ss", null);

            try
            {
                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/DEO_Salesman_EntryDetail.rpt")));
                rd.SetParameterValue("@FromDate", FromDate);
                rd.SetParameterValue("@ToDate", ToDate);


                string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "DEO_vs_Salesman.pdf");

            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }

    }
}