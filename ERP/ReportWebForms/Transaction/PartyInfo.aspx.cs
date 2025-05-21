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

namespace ERP.ReportWebForms.Transaction
{
    public partial class PartyInfo : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {
            // Database Context Objects
            //  Objects of Data Model            

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();


            string CompanyCode = CommonDAL.CompCode();
            var CompanyName = (from a in DefinitionContext.CompanySetups where (a.CompCode == CompanyCode) select a.CompName).FirstOrDefault();
            var CompanyAddress = (from a in DefinitionContext.CompanySetups where (a.CompCode == CompanyCode) select a.Address1).FirstOrDefault();
            var UserName = CommonDAL.UserName();
            string RegionCode = Request.QueryString["RegionCode"];
            string PartyCode = Request.QueryString["PartyCode"];
            try
            {
                bool isValid = true;

                if (isValid) // If Report Name provided then do other operation
                {
                   

                  


                    rd.Load(Path.Combine(Server.MapPath("~/Reports/Transaction/PartyInfo.rpt")));
                    
                    rd.SetParameterValue("CompanyCode", CompanyCode);
                    rd.SetParameterValue("RegionCode", RegionCode);
                    rd.SetParameterValue("PartyCode", PartyCode);
                    string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                    string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                    string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                    string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                    rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

                    CrystalReportViewer1.ReportSource = rd;
                    CrystalReportViewer1.Zoom(150);
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
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
            rd.Dispose();
            rd.Close();
            GC.Collect();

            CloseReports(rd);
            rd.Dispose();
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