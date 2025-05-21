using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using ERP.Models;
using ERP.Models.VMClasses;
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
    public partial class CreditorAgeing : System.Web.UI.Page
    {
        ReportDocument rd = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {
            // Database Context Objects
            //  Objects of Data Model            

            AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

            string CompanyCode = CommonDAL.CompCode();
            var FromDate = Session["AgeFromDate"];
            string RegionCode = (string) Session["RegionCode"];
            string PartyCode = (string) Session["PartyCode"];
            string RegionAll = (string) Session["RegionAll"];
            string PartyAll = (string) Session["PartyAll"];
            var UserName = CommonDAL.UserName();
            

            if (RegionCode == null)
            {
                RegionCode = "";
            }

         
            try
            {

                DateTime SPDate = (DateTime)FromDate;

                List<SP_NewAgingReport_Result> Data;

                if (RegionAll == "RegionAll" &&  PartyAll == "PartyAll")
                {
                   Data = DefinitionContext.SP_NewAgingReport(SPDate).ToList();
                }
                else if (RegionAll != "RegionAll" && PartyAll == "PartyAll")
                {
                   Data = DefinitionContext.SP_NewAgingReport(SPDate).Where(x => x.RegionCode == RegionCode).ToList();
                }
                else if (RegionAll != "RegionAll" && PartyAll != "PartyAll")
                {
                   Data = DefinitionContext.SP_NewAgingReport(SPDate).Where(x => x.RegionCode == RegionCode && x.MainPartyCode == PartyCode).ToList();
                }
                else
                {
                    Data = DefinitionContext.SP_NewAgingReport(SPDate).ToList();
                }
                List<AgingReportVM> List = new List<AgingReportVM>();
               
                foreach (var item in Data)
                {
                    AgingReportVM NewObj = new AgingReportVM();

                    NewObj.RegionDescripion = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.MainPartyCode).Select(x => x.PartyCode).FirstOrDefault();                    // When 1-30 is equal to 0
                    //if (item.C1_30 == 0 && item.C31_60 == 0 && item.C61_90 == 0 && item.C91_120 == 0 && item.C121_150 == 0 && item.C151_180 == 0 && item.C181_210 == 0 && item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //{
                    //    item.C1_30        = item.Open_1_30;
                    //    item.C31_60       = item.Open_31_60;
                    //    item.C61_90       = item.Open_61_90;
                    //    item.C91_120      = item.Open_91_120;
                    //    item.C121_150     = item.Open_121_150;
                    //    item.C151_180     = item.Open_151_180;
                    //    item.C181_210     = item.Open_181_210;
                    //    item.C211_240     = item.Open_211_240;
                    //    item.C241_270     = item.Open_241_270;
                    //    item.C271_300     = item.Open_271_300;
                    //    item.C301_330     = item.Open_301_330;
                    //    item.C331_360 = item.Open_331_360;
                    //    item.MoreThan360  = item.Open_MoreThan360;
                    //}
                    //else
                    //{
                    //    // When 31-60 is equal to 0
                    //    if (item.C31_60 == 0 && item.C61_90 == 0 && item.C91_120 == 0 && item.C121_150 == 0 && item.C151_180 == 0 && item.C181_210 == 0 && item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //    {
                    //        item.C31_60   = item.Open_1_30;
                    //        item.C61_90   = item.Open_31_60;
                    //        item.C91_120  = item.Open_61_90;
                    //        item.C121_150 = item.Open_91_120;
                    //        item.C151_180 = item.Open_121_150;
                    //        item.C181_210 = item.Open_151_180;
                    //        item.C211_240 = item.Open_181_210;
                    //        item.C241_270 = item.Open_211_240;
                    //        item.C271_300 = item.Open_241_270;
                    //        item.C301_330 = item.Open_271_300;
                    //        item.C331_360 = item.Open_301_330;
                    //        item.MoreThan360 = item.Open_331_360 + item.Open_MoreThan360;
                    //    }
                    //    else
                    //    {
                    //        // When 61-90 is equal to 0
                    //        if (item.C61_90 == 0 && item.C91_120 == 0 && item.C121_150 == 0 && item.C151_180 == 0 && item.C181_210 == 0 && item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //        {
                    //            item.C61_90       = item.Open_1_30;
                    //            item.C91_120      = item.Open_31_60;
                    //            item.C121_150     = item.Open_61_90;
                    //            item.C151_180     = item.Open_91_120;
                    //            item.C181_210     = item.Open_121_150;
                    //            item.C211_240     = item.Open_151_180;
                    //            item.C241_270     = item.Open_181_210;
                    //            item.C271_300     = item.Open_211_240;
                    //            item.C301_330     = item.Open_241_270;
                    //            item.C331_360 = item.Open_271_300;
                    //            item.MoreThan360  = item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //        }
                    //        else
                    //        {
                    //            // When 91-120 is equal to 0
                    //            if (item.C91_120 == 0 && item.C121_150 == 0 && item.C151_180 == 0 && item.C181_210 == 0 && item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //            {
                    //                item.C91_120      = item.Open_1_30;
                    //                item.C121_150     = item.Open_31_60;
                    //                item.C151_180     = item.Open_61_90;
                    //                item.C181_210     = item.Open_91_120;
                    //                item.C211_240     = item.Open_121_150;
                    //                item.C241_270     = item.Open_151_180;
                    //                item.C271_300     = item.Open_181_210;
                    //                item.C301_330     = item.Open_211_240;
                    //                item.C331_360 = item.Open_241_270;
                    //                item.MoreThan360  = item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //            }
                    //            else
                    //            {
                    //                // When 121-150 is equal to 0
                    //                if (item.C121_150 == 0 && item.C151_180 == 0 && item.C181_210 == 0 && item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                {
                    //                    item.C121_150     = item.Open_1_30;
                    //                    item.C151_180     = item.Open_31_60;
                    //                    item.C181_210     = item.Open_61_90;
                    //                    item.C211_240     = item.Open_91_120;
                    //                    item.C241_270     = item.Open_121_150;
                    //                    item.C271_300     = item.Open_151_180;
                    //                    item.C301_330     = item.Open_181_210;
                    //                    item.C331_360 = item.Open_211_240;
                    //                    item.MoreThan360  = item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                }
                    //                else
                    //                {
                    //                    // When 151-180 is equal to 0
                    //                    if (item.C151_180 == 0 && item.C181_210 == 0 && item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                    {    
                    //                        item.C151_180     = item.Open_1_30;
                    //                        item.C181_210     = item.Open_31_60;
                    //                        item.C211_240     = item.Open_61_90;
                    //                        item.C241_270     = item.Open_91_120;
                    //                        item.C271_300     = item.Open_121_150;
                    //                        item.C301_330     = item.Open_151_180;
                    //                        item.C331_360 = item.Open_181_210;
                    //                        item.MoreThan360  = item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                    }
                    //                    else
                    //                    {
                    //                        // When 181-210 is equal to 0
                    //                        if (item.C181_210 == 0 && item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                        {
                    //                            item.C181_210     = item.Open_1_30;
                    //                            item.C211_240     = item.Open_31_60;
                    //                            item.C241_270     = item.Open_61_90;
                    //                            item.C271_300     = item.Open_91_120;
                    //                            item.C301_330     = item.Open_121_150;
                    //                            item.C331_360 = item.Open_151_180;
                    //                            item.MoreThan360  = item.Open_181_210 + item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                        }
                    //                        else
                    //                        {
                    //                            // When 211-240 is equal to 0
                    //                            if (item.C211_240 == 0 && item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                            {
                    //                                item.C211_240     = item.Open_1_30;
                    //                                item.C241_270     = item.Open_31_60;
                    //                                item.C271_300     = item.Open_61_90;
                    //                                item.C301_330     = item.Open_91_120;
                    //                                item.C331_360 = item.Open_121_150;
                    //                                item.MoreThan360  = item.Open_151_180 + item.Open_181_210 + item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                            }
                    //                            else
                    //                            {
                    //                                // When 241-270 is equal to 0
                    //                                if (item.C241_270 == 0 && item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                                {
                    //                                    item.C241_270     = item.Open_1_30;
                    //                                    item.C271_300     = item.Open_31_60;
                    //                                    item.C301_330     = item.Open_61_90;
                    //                                    item.C331_360 = item.Open_91_120;
                    //                                    item.MoreThan360  = item.Open_121_150 + item.Open_151_180 + item.Open_181_210 + item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                                }
                    //                                else
                    //                                {
                    //                                    // When 271-300 is equal to 0
                    //                                    if (item.C271_300 == 0 && item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                                    {
                    //                                        item.C271_300     = item.Open_1_30;
                    //                                        item.C301_330     = item.Open_31_60;
                    //                                        item.C331_360 = item.Open_61_90;
                    //                                        item.MoreThan360  = item.Open_91_120 + item.Open_121_150 + item.Open_151_180 + item.Open_181_210 + item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        // When 301-330 is equal to 0
                    //                                        if (item.C301_330 == 0 && item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                                        {
                    //                                            item.C301_330     = item.Open_1_30;
                    //                                            item.C331_360 = item.Open_31_60;
                    //                                            item.MoreThan360  = item.Open_61_90 + item.Open_91_120 + item.Open_121_150 + item.Open_151_180 + item.Open_181_210 + item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                                        }
                    //                                        else
                    //                                        {
                    //                                            // When 331-360 is equal to 0
                    //                                            if (item.C331_360 == 0 && item.MoreThan360 == 0)
                    //                                            {
                    //                                                item.C331_360 = item.Open_1_30;
                    //                                                item.MoreThan360  = item.Open_31_60 + item.Open_61_90 + item.Open_91_120 + item.Open_121_150 + item.Open_151_180 + item.Open_181_210 + item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                                            }
                    //                                            else
                    //                                            {
                    //                                                // When MoreThan360 is equal to 0
                    //                                                if (item.MoreThan360 == 0)
                    //                                                {
                    //                                                    item.MoreThan360 = item.Open_1_30 + item.Open_31_60 + item.Open_61_90 + item.Open_91_120 + item.Open_121_150 + item.Open_151_180 + item.Open_181_210 + item.Open_211_240 + item.Open_241_270 + item.Open_271_300 + item.Open_301_330 + item.Open_331_360 + item.Open_MoreThan360;
                    //                                                }
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}



                    //FIFO Base Aging Method

                    // For More than 360
                    if (item.MoreThan360 >= item.RecoveryAmount && item.MoreThan360 != 0)
                    {
                        NewObj.MoreThan360 = item.MoreThan360 - item.RecoveryAmount;
                        NewObj.RecoveryAmount = item.RecoveryAmount - item.RecoveryAmount;
                    }
                    else 
                    {
                        NewObj.RecoveryAmount = item.RecoveryAmount - item.MoreThan360;
                        NewObj.MoreThan360 = item.MoreThan360 - item.MoreThan360;
                        //NewObj.MoreThan360 = item.MoreThan360 ;
                    }

                    // For  331-360
                    if (item.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C331_360 = (item.C331_360 + NewObj.MoreThan360) - NewObj.RecoveryAmount;
                        item.MoreThan360 = 0;
                        item.C331_360 = NewObj.C331_360;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {

                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C331_360 = item.C331_360;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C331_360;
                            NewObj.C331_360 = item.C331_360 - item.C331_360;
                            //NewObj.C331_360 = item.C331_360 ;
                        }


                    }

                    // For 301-330
                    if (item.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0  && item.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {

                        NewObj.C301_330 = item.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C301_330 = NewObj.C301_330;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {

                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C301_330 = item.C301_330;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C301_330;
                            NewObj.C301_330 = item.C301_330 - item.C301_330;
                            //NewObj.C301_330 = item.C301_330;
                        }


                    }

                    // For 271-300
                    if (item.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C271_300 = item.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C271_300 = NewObj.C271_300;
                        item.C301_330 = 0;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {
                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C271_300 = item.C271_300;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C271_300;
                            NewObj.C271_300 = item.C271_300 - item.C271_300;
                            //NewObj.C271_300 = item.C271_300;
                        }


                    }

                    // For 241-270
                    if (item.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C241_270 = item.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C241_270 = NewObj.C241_270;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {
                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C241_270 = item.C241_270;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C241_270;
                            NewObj.C241_270 = item.C241_270 - item.C241_270;
                            //NewObj.C241_270 = item.C241_270;
                        }


                    }

                    // For 211-240
                    if (item.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C211_240 = item.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C211_240 = NewObj.C211_240;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {

                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C211_240 = item.C211_240;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C211_240;
                            NewObj.C211_240 = item.C211_240 - item.C211_240;
                            //NewObj.C211_240 = item.C211_240;
                        }


                    }

                    // For 181-210
                    if (item.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C181_210 = item.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C181_210 = NewObj.C181_210;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {
                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C181_210 = item.C181_210;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C181_210;
                            NewObj.C181_210 = item.C181_210 - item.C181_210;
                            //NewObj.C181_210 = item.C181_210;
                        }


                    }


                    // For 151-180
                    if (item.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C151_180 = item.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C151_180 = NewObj.C151_180;
                        item.C181_210 = 0;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C181_210 = 0;
                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {
                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C151_180 = item.C151_180;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C151_180;
                            NewObj.C151_180 = item.C151_180 - item.C151_180;
                            //NewObj.C151_180 = item.C151_180;
                        }


                    }


                    // For 121-150
                    if (item.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C121_150 = item.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C121_150 = NewObj.C121_150;
                        item.C151_180 = 0;
                        item.C181_210 = 0;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C151_180 = 0;
                        NewObj.C181_210 = 0;
                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {

                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C121_150 = item.C121_150;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C121_150;
                            NewObj.C121_150 = item.C121_150 - item.C121_150;
                            //NewObj.C121_150 = item.C121_150;
                        }


                    }


                    // For 91-120
                    if (item.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C91_120 = item.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C91_120 = NewObj.C91_120;
                        item.C121_150 = 0;
                        item.C151_180 = 0;
                        item.C181_210 = 0;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;
                        item.MoreThan360 = 0;

                        NewObj.C121_150 = 0;
                        NewObj.C151_180 = 0;
                        NewObj.C181_210 = 0;
                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;
                        NewObj.MoreThan360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {

                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C91_120 = item.C91_120;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C91_120;
                            NewObj.C91_120 = item.C91_120 - item.C91_120;
                            //NewObj.C91_120 = item.C91_120;
                        }


                    }


                    // For 61-90
                    if (item.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C61_90 = item.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C61_90 = NewObj.C61_90;
                        item.C91_120 = 0;
                        item.C121_150 = 0;
                        item.C151_180 = 0;
                        item.C181_210 = 0;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;

                        NewObj.C91_120 = 0;
                        NewObj.C121_150 = 0;
                        NewObj.C151_180 = 0;
                        NewObj.C181_210 = 0;
                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {
                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C61_90 = item.C61_90;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C61_90;
                            NewObj.C61_90 = item.C61_90 - item.C61_90;
                            //NewObj.C61_90 = item.C61_90;
                        }

                    }


                    // For 31-60
                    if (item.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C31_60 = item.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C31_60 = NewObj.C31_60;
                        item.C61_90 = 0;
                        item.C91_120 = 0;
                        item.C121_150 = 0;
                        item.C151_180 = 0;
                        item.C181_210 = 0;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;

                        NewObj.C61_90 = 0;
                        NewObj.C91_120 = 0;
                        NewObj.C121_150 = 0;
                        NewObj.C151_180 = 0;
                        NewObj.C181_210 = 0;
                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {
                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C31_60 = item.C31_60;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C31_60;
                            NewObj.C31_60 = item.C31_60 - item.C31_60;
                            //NewObj.C31_60 = item.C31_60;
                        }

                    }


                    // For 1-30
                    if (item.C1_30 + NewObj.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C1_30 + NewObj.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C1_30 = item.C1_30 + NewObj.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;
                        item.C1_30 = NewObj.C1_30;
                        item.C31_60 = 0;
                        item.C61_90 = 0;
                        item.C91_120 = 0;
                        item.C121_150 = 0;
                        item.C151_180 = 0;
                        item.C181_210 = 0;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;

                        NewObj.C31_60 = 0;
                        NewObj.C61_90 = 0;
                        NewObj.C91_120 = 0;
                        NewObj.C121_150 = 0;
                        NewObj.C151_180 = 0;
                        NewObj.C181_210 = 0;
                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {

                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C1_30 = item.C1_30;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C1_30;
                            NewObj.C1_30 = item.C1_30 - item.C1_30;
                            //NewObj.C1_30 = item.C1_30;
                        }


                    }


                    // For 0
                    if (item.C0 + NewObj.C1_30 + NewObj.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 >= NewObj.RecoveryAmount && NewObj.RecoveryAmount != 0 && item.C0 + NewObj.C1_30 + NewObj.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 != 0)
                    {
                        NewObj.C0 = item.C0 + NewObj.C1_30 + NewObj.C31_60 + NewObj.C61_90 + NewObj.C91_120 + NewObj.C121_150 + NewObj.C151_180 + NewObj.C181_210 + NewObj.C211_240 + NewObj.C241_270 + NewObj.C271_300 + NewObj.C301_330 + NewObj.C331_360 + NewObj.MoreThan360 - NewObj.RecoveryAmount;

                        item.C1_30 = 0;
                        item.C31_60 = 0;
                        item.C61_90 = 0;
                        item.C91_120 = 0;
                        item.C121_150 = 0;
                        item.C151_180 = 0;
                        item.C181_210 = 0;
                        item.C211_240 = 0;
                        item.C241_270 = 0;
                        item.C271_300 = 0;
                        item.C301_330 = 0;
                        item.C331_360 = 0;

                        NewObj.C1_30 = 0;
                        NewObj.C31_60 = 0;
                        NewObj.C61_90 = 0;
                        NewObj.C91_120 = 0;
                        NewObj.C121_150 = 0;
                        NewObj.C151_180 = 0;
                        NewObj.C181_210 = 0;
                        NewObj.C211_240 = 0;
                        NewObj.C241_270 = 0;
                        NewObj.C271_300 = 0;
                        NewObj.C301_330 = 0;
                        NewObj.C331_360 = 0;

                        NewObj.RecoveryAmount = NewObj.RecoveryAmount - NewObj.RecoveryAmount;
                    }
                    else
                    {
                        if (NewObj.RecoveryAmount == 0)
                        {
                            NewObj.C0 = item.C0;
                        }
                        else
                        {
                            NewObj.RecoveryAmount = NewObj.RecoveryAmount - item.C0;
                            NewObj.C0 = item.C0 - item.C0;
                            if (NewObj.RecoveryAmount != 0)
                            {
                                NewObj.C0 = NewObj.C0 - NewObj.RecoveryAmount;
                            }
                        }
                       
                        //NewObj.C0 = item.C0;
                        //if (item.C0 + item.C1_30 + item.C31_60 + item.C61_90 + item.C91_120 + item.C121_150 + item.C151_180 + item.C181_210 + item.C211_240 + item.C241_270 + item.C271_300 + item.C301_330 + item.C331_360 + item.MoreThan360 < NewObj.RecoveryAmount)
                        //{
                        //    NewObj.C0 = item.C0 + item.C1_30 + item.C31_60 + item.C61_90 + item.C91_120 + item.C121_150 + item.C151_180 + item.C181_210 + item.C211_240 + item.C241_270 + item.C271_300 + item.C301_330 + item.C331_360 + item.MoreThan360 - NewObj.RecoveryAmount;
                        //}
                       
                    }

                    NewObj.RegionCode = item.RegionCode;
                    NewObj.PartyCode = item.MainPartyCode;
                    NewObj.PartyName = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.MainPartyCode).Select(x => x.PartyName).FirstOrDefault();
                   
                    List.Add(NewObj);
                }

                rd.Load(Path.Combine(Server.MapPath("~/Reports/DetailReports/CreditorAgeing.rpt")));

                rd.SetDataSource(List);

                rd.SetParameterValue("CompanyCode", CompanyCode);
                rd.SetParameterValue("UserName", UserName);
                rd.SetParameterValue("FromDate", SPDate);


                //string strServer = ConfigurationManager.AppSettings["Srver"].ToString();
                //string strDatabase = ConfigurationManager.AppSettings["db"].ToString();
                //string strUserID = ConfigurationManager.AppSettings["Username"].ToString();
                //string strPwd = ConfigurationManager.AppSettings["pas"].ToString();
                //rd.DataSourceConnections[1].SetConnection(strServer, strDatabase, strUserID, strPwd);
                CrystalReportViewer1.ReportSource=rd;
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
            rd.Close();
            GC.Collect();
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