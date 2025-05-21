<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreditorAgeing.aspx.cs" Inherits="ERP.ReportWebForms.DetailWebForms.CreditorAgeing" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script src="../../aspnet_client/system_web/4_0_30319/crystalreportviewers13/js/crviewer/crv.js" type="text/javascript" > </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
               <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px" ToolbarImagesFolderUrl="" ToolPanelWidth="200px" Width="350px" />

                
            
               <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                   <Report FileName="Reports\DetailReports\CreditorAgeing.rpt">
                   </Report>
               </CR:CrystalReportSource>

                
            
        </div>
    </form>
</body>
</html>
