using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.ReportWebForms.DetailWebForms
{
    public partial class SaleInvoiceDetail : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {
            // Database Context Objects
            //  Objects of Data Model            

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();


            string CompanyCode = CommonDAL.CompCode();

            DateTime FromDate = (DateTime)Session["FromDate"];
            //string FromDate = DateTime.ParseExact(FromDat, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture).ToString("dd'/'MM'/'yyyy");
            DateTime ToDate = (DateTime)Session["ToDate"];
            //ToDate = DateTime.ParseExact(ToDate, "MM'/'dd'/'yyyy", CultureInfo.InvariantCulture).ToString("dd'/'MM'/'yyyy");

            var UserName = CommonDAL.UserName();
            string DSRExcel = Request.QueryString["DSRExcel"];
            string PartyCode = (string)Session["PartyCode"];
            string EmpCode = (string) Session["EmpCode"];
            
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




            string EmpName = "";
            string RegionDescription = "";

            if (EmpType == null || EmpType == "")
            {
                EmpName = DefinitionContext.CompanyEmps.Where(x => x.RegionCode == RegionCode && x.EmpCode == EmpCode).Select(x => x.EmpName).FirstOrDefault();
            }
            else
            {
                EmpName = "All Salesman";
            }

            if (RegionType == null && NorthRegion == null)
            {
                foreach (var item in OptRegionCode)
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

            try
            {

                if (DSRExcel != "null") 
                {

                    rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/SaleInvoiceDetail1.rpt")));


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
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

                    CrystalReportViewer1.ReportSource = rd;
                }

                if (DSRExcel == "null")
                {

                    rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/MainDSR.rpt")));
                    rd.SetParameterValue("CompanyCode", CompanyCode);
                    rd.SetParameterValue("FromDate", FromDate);
                    rd.SetParameterValue("ToDate", ToDate);
                    rd.SetParameterValue("UserName", UserName);

                  


                    rd.SetParameterValue("CompanyCode", CompanyCode, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("EmpCode", EmpCode, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PartyCode", PartyCode, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("EmpType", EmpType, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PartyType", PartyType, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("UserName", UserName, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("RegionCode", RegionCode, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("RegionType", RegionType, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PType", PType, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PCategory", PCategory, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PSTaxReg", PSTaxReg, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PTypeBoth", PTypeBoth, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PCategoryBoth", PCategoryBoth, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("PSTaxRegBoth", PSTaxRegBoth, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("DNorthRegion", NorthRegion, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("ItemType", ItemType, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("ItemCode", ItemCode, rd.Subreports[0].Name.ToString());

                    rd.SetParameterValue("@FFromDate", FromDate, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("@TToDate", ToDate, rd.Subreports[0].Name.ToString());

                    rd.SetParameterValue("R1", R1, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R2", R2, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R3", R3, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R4", R4, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R5", R5, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R6", R6, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R7", R7, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R8", R8, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R9", R9, rd.Subreports[0].Name.ToString());
                    rd.SetParameterValue("R10", R10, rd.Subreports[0].Name.ToString());




                    rd.SetParameterValue("CompanyCode", CompanyCode, rd.Subreports[1].Name.ToString());
                  
                    rd.SetParameterValue("UserName", UserName, rd.Subreports[1].Name.ToString());
                    
                    rd.SetParameterValue("@RecFromDate", FromDate, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecToDate", ToDate, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecRegionType", RegionType, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecRegionCode", RegionCode, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecEmpType", EmpType, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecEmpCode", EmpCode, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecNorthRegion", NorthRegion, rd.Subreports[1].Name.ToString());




                    rd.SetParameterValue("@SaleRegionType", RegionType, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleRegionCode", RegionCode, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleEmpType", EmpType, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleEmpCode", EmpCode, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleFrommDate", FromDate, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleTooDate", ToDate, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@NorthRegion", NorthRegion, rd.Subreports[1].Name.ToString());


                    rd.SetParameterValue("@RegionTypeForOpen", RegionType, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RegionCodeForOpen", RegionCode, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@EmpTypeForOpen", EmpType, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@EmpCodeForOpen", EmpCode, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@FromDateForOpen", FromDate, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@ToDateForOpen", ToDate, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@NorthRegionForOpen", NorthRegion, rd.Subreports[1].Name.ToString());

                    rd.SetParameterValue("RegionDescription", RegionDescription, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("EmpName", EmpName, rd.Subreports[1].Name.ToString());

                    rd.SetParameterValue("@RecR1", R1, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR2", R2, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR3", R3, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR4", R4, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR5", R5, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR6", R6, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR7", R7, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR8", R8, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR9", R9, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@RecR10", R10, rd.Subreports[1].Name.ToString());

                    rd.SetParameterValue("@R1OPen", R1, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R2OPen", R2, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R3OPen", R3, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R4OPen", R4, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R5OPen", R5, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R6OPen", R6, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R7OPen", R7, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R8OPen", R8, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R9OPen", R9, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@R10Open", R10, rd.Subreports[1].Name.ToString());

                    rd.SetParameterValue("@SaleR1", R1, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR2", R2, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR3", R3, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR4", R4, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR5", R5, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR6", R6, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR7", R7, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR8", R8, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR9", R9, rd.Subreports[1].Name.ToString());
                    rd.SetParameterValue("@SaleR10", R10, rd.Subreports[1].Name.ToString());



                    string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                    string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                    string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                    string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                    rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

                    CrystalReportViewer1.ReportSource = rd;
                    CrystalReportViewer1.Zoom(150);
                }
             
            }
            catch (Exception ex)
            {
                throw ex;
                //Response.Write("<H2>" + ex.ToString() + "</H2>");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void Page_Unload(object sender, EventArgs e)
        {

           

            CloseReports(rd);
            rd.Dispose();
            rd.Close();
            GC.Collect();
            CrystalReportViewer1.Dispose();
            CrystalReportViewer1 = null;

        }

        private void CloseReports(ReportDocument reportDocument)
        {
            Sections sections = reportDocument.ReportDefinition.Sections;
            foreach (Section section in sections)
            {
                ReportObjects reportObjects = section.ReportObjects;
                foreach (ReportObject reportObject in reportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subreportObject = (SubreportObject)reportObject;
                        ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        subReportDocument.Close();
                    }
                }
            }
            reportDocument.Close();
        }
    }
}