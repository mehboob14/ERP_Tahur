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
    public partial class ShortCashReport : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {
            // Database Context Objects
            //  Objects of Data Model            

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string FromDate = Session["FromDate"].ToString();
            string ToDate = Session["ToDate"].ToString();
            string RegionCode = Session["RegionCode"].ToString();
            string EmpCode = Session["EmpCode"].ToString();
            string SelectRegion = Session["SelectRegion"].ToString();
            string SelectEmp = Session["SelectEmp"].ToString();
            string UserName = CommonDAL.UserName();

            try
            {

                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/ShortCashReport.rpt")));
                rd.SetParameterValue("UserName", UserName);
                rd.SetParameterValue("@FromDate", FromDate);
                rd.SetParameterValue("@ToDate", ToDate);
                rd.SetParameterValue("@RegionCode", RegionCode);
                rd.SetParameterValue("@EmpCode", EmpCode);
                rd.SetParameterValue("@SelectRegion", SelectRegion);
                rd.SetParameterValue("@SelectEmp", SelectEmp);

                rd.SetParameterValue("@RecFromDate", FromDate);
                rd.SetParameterValue("@RecToDate", ToDate);
                rd.SetParameterValue("@RecRegionCode", RegionCode);
                rd.SetParameterValue("@RecEmpCode", EmpCode);
                rd.SetParameterValue("@RecSelectRegion", SelectRegion);
                rd.SetParameterValue("@RecSelectEmp", SelectEmp);

                rd.SetParameterValue("@CSFromDate", FromDate);
                rd.SetParameterValue("@CSToDate", ToDate);
                rd.SetParameterValue("@CSRegionCode", RegionCode);
                rd.SetParameterValue("@CSEmpCode", EmpCode);
                rd.SetParameterValue("@CSSelectRegion", SelectRegion);
                rd.SetParameterValue("@CSSelectEmp", SelectEmp);


                rd.SetParameterValue("@OPFromDate", FromDate);
               
                rd.SetParameterValue("@OPRegionCode", RegionCode);
                rd.SetParameterValue("@OPEmpCode", EmpCode);
                rd.SetParameterValue("@OPSelectRegion", SelectRegion);
                rd.SetParameterValue("@OPSelectEmp", SelectEmp);
                                       
                rd.SetParameterValue("@OPRecFromDate", FromDate);
               
                rd.SetParameterValue("@OPRecRegionCode", RegionCode);
                rd.SetParameterValue("@OPRecEmpCode", EmpCode);
                rd.SetParameterValue("@OPRecSelectRegion", SelectRegion);
                rd.SetParameterValue("@OPRecSelectEmp", SelectEmp);
                                       
                rd.SetParameterValue("@OPCSFromDate", FromDate);
                
                rd.SetParameterValue("@OPCSRegionCode", RegionCode);
                rd.SetParameterValue("@OPCSEmpCode", EmpCode);
                rd.SetParameterValue("@OPCSSelectRegion", SelectRegion);
                rd.SetParameterValue("@OPCSSelectEmp", SelectEmp);


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