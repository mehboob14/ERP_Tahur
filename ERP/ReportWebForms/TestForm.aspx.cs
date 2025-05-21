using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using ERP.Models;

namespace ERP.WebForms
{
    public partial class TestForm : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            // Database Context Objects
            //  Objects of Data Model            

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();


            string CompanyCode = CommonDAL.CompCode();
            var CompanyName = (from a in DefinitionContext.CompanySetups where (a.CompCode == CompanyCode) select a.CompName).FirstOrDefault();
            var CompanyAddress = (from a in DefinitionContext.CompanySetups where (a.CompCode == CompanyCode) select a.Address1).FirstOrDefault();
            var UserName = CommonDAL.UserName();
            try
            {
                bool isValid = true;
             
                if (isValid) // If Report Name provided then do other operation
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Reports/TestReport.rpt")));
                   
                    string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                    string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                    string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                    string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                    rd.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                  
                    CrystalReportViewer1.ReportSource = rd;
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
    }
}