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
    public partial class SalesmanSummary : System.Web.UI.Page
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
          
            string EmpCode = (string)Session["EmpCode"];

            string EmpType = (string)Session["EmpType"];
           

            string[] OptRegionCode = (string[])Session["OptRegionCode"];
            string RegionType = (string)Session["RegionType"];

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

            if (RegionType == null)
            {
                foreach (var item in OptRegionCode)
                {
                    RegionDescription = RegionDescription + "  |  " + DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item).Select(x => x.RegionDescription).FirstOrDefault();
                }

            }
            else
            {
                RegionDescription = "All Region";
            }
         

            if (EmpCode == null)
            {
                EmpCode = "";
            }

          

            if (EmpType == null)
            {
                EmpType = "";
            }

            

            if (RegionType == null)
            {
                RegionType = "";
            }

           

            try
            {

                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/SalesmanSummaryAll.rpt")));

                rd.SetParameterValue("RegionDescription", RegionDescription);
                rd.SetParameterValue("EmpName", EmpName);

                rd.SetParameterValue("@RecFromDate", FromDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecToDate", ToDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecRegionType", RegionType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecRegionCode", RegionCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecEmpType", EmpType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@RecEmpCode", EmpCode/*, rd.Subreports[1].Name.ToString()*/);

                rd.SetParameterValue("@RecR1", R1);
                rd.SetParameterValue("@RecR2", R2);
                rd.SetParameterValue("@RecR3", R3);
                rd.SetParameterValue("@RecR4", R4);
                rd.SetParameterValue("@RecR5", R5);
                rd.SetParameterValue("@RecR6", R6);
                rd.SetParameterValue("@RecR7", R7);
                rd.SetParameterValue("@RecR8", R8);
                rd.SetParameterValue("@RecR9", R9);
                rd.SetParameterValue("@RecR10", R10);

                rd.SetParameterValue("@SaleR1", R1);
                rd.SetParameterValue("@SaleR2", R2);
                rd.SetParameterValue("@SaleR3", R3);
                rd.SetParameterValue("@SaleR4", R4);
                rd.SetParameterValue("@SaleR5", R5);
                rd.SetParameterValue("@SaleR6", R6);
                rd.SetParameterValue("@SaleR7", R7);
                rd.SetParameterValue("@SaleR8", R8);
                rd.SetParameterValue("@SaleR9", R9);
                rd.SetParameterValue("@SaleR10", R10);

                rd.SetParameterValue("@SaleRegionType", RegionType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleRegionCode", RegionCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleEmpType", EmpType/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleEmpCode", EmpCode/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleFrommDate", FromDate/*, rd.Subreports[1].Name.ToString()*/);
                rd.SetParameterValue("@SaleTooDate", ToDate/*, rd.Subreports[1].Name.ToString()*/);

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