using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.ReportWebForms.DetailWebForms
{
    public partial class SaleTaxRegister : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {
            // Database Context Objects
            //  Objects of Data Model            

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            DateTime FromDate = (DateTime)Session["STRFromDate"];

            DateTime ToDate = (DateTime)Session["STRToDate"];
            string UserName = CommonDAL.UserName();

            string ExpTax =(string) Session["ExpTax"];
            string STaxReg =(string) Session["STaxReg"];
            string PSTaxRegBoth = (string)Session["PSTaxRegBoth"];
            string PExpTaxBoth = (string)Session["PExpTaxBoth"]; 

            string RegionCode = (string)Session["RegionCode"];
            string PartyCode = (string)Session["PartyCode"];
            string SelectRegion = (string)Session["SelectRegion"];
            string SelectParty = (string)Session["SelectParty"];
            string FBR = Request.QueryString["FBR"];

          
            if (FBR != "ForFBR")
            {
                try
                {

                    rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/SaleTaxRegister.rpt")));
                    rd.SetParameterValue("CompanyCode", CompanyCode);
                    rd.SetParameterValue("UserName", UserName);
                    rd.SetParameterValue("RegionCode", RegionCode);
                    rd.SetParameterValue("PartyCode", PartyCode);
                    rd.SetParameterValue("@FromDate", FromDate);
                    rd.SetParameterValue("@ToDate", ToDate);
                    rd.SetParameterValue("SelectRegion", SelectRegion);
                    rd.SetParameterValue("SelectParty", SelectParty);

                    rd.SetParameterValue("ExpTax", ExpTax);
                    rd.SetParameterValue("STaxReg", STaxReg);
                    rd.SetParameterValue("PSTaxRegBoth", PSTaxRegBoth);
                    rd.SetParameterValue("PExpTaxBoth", PExpTaxBoth);

                    string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                    string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                    string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                    string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                    rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

                    CrystalReportViewer1.ReportSource = rd;
                    CrystalReportViewer1.Zoom(150);

                }
                catch (Exception ex)
                {
                    throw ex;
                    //Response.Write("<H2>" + ex.ToString() + "</H2>");
                }
            }
            else
            {
                try
                {
                    if (ExpTax == "Taxable")
                    {
                        ExpTax = "10";
                    }

                    rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/STRForFBR.rpt")));
                    rd.SetParameterValue("@FromDate", FromDate);
                    rd.SetParameterValue("@ToDate", ToDate);
                    rd.SetParameterValue("STaxReg", STaxReg);
                    rd.SetParameterValue("PSTaxRegBoth", PSTaxRegBoth);
                    rd.SetParameterValue("ExpTax", ExpTax);
                    rd.SetParameterValue("PExpTaxBoth", PExpTaxBoth);

                    rd.SetParameterValue("SelectRegion", SelectRegion);
                    rd.SetParameterValue("SelectParty", SelectParty);
                    rd.SetParameterValue("RegionCode", RegionCode);
                    rd.SetParameterValue("PartyCode", PartyCode);

                    string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                    string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                    string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                    string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                    rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

                    CrystalReportViewer1.ReportSource = rd;
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                    //Response.Write("<H2>" + ex.ToString() + "</H2>");
                }
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