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
    public partial class DailyStockShort : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {
            // Database Context Objects
            //  Objects of Data Model            

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            var FromDate = Session["FromDate"];

            var ToDate = Session["ToDate"];
            var UserName = CommonDAL.UserName();
            string RegionCode = (string)Session["RegionCode"];
            string RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
            string DS = Request.QueryString["DS"];
           
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