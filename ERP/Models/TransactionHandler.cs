using ERP.Models.VMClasses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web;


namespace ERP.Models
{
    public class TransactionHandler
    {
        string CompanyCode = CommonDAL.CompCode();
        public string GetCurrentDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy");
        }
        public List<CompanyEmp> GetEmpList()
        {
            using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.DelFlag == "N").OrderByDescending(x => x.EmpCode).ToList();
                List<CompanyEmp> CoList = new List<CompanyEmp>();
                foreach (var item in List)
                {
                    CompanyEmp Co = new CompanyEmp();
                    Co.EmpCode = item.EmpCode;
                    Co.EmpName = (item.EmpCode + " | " + item.EmpName);
                    CoList.Add(Co);
                }

                return CoList;
            }
           
        }

      
      

        public List<SaleParty> GetRegionParty(string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).ToList();
            }

        }


        public List<SaleParty> GetMultipleRegionParty(string[] RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && RegionCode.Contains(x.RegionCode)).ToList();
            }
              
        }

        public List<SaleParty> GetRegionCreditParty(string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).ToList();
            }

        }


        public List<Sp_GetRegionPartiesFast_Result> GetRegionPartyFast(string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.Sp_GetRegionPartiesFast(RegionCode).ToList();
            }

        }


        public double GetSinglePartyDiscount(string RegionCode, string PartyCode, string ItemCode)
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var MainPartyCode = DefintionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.MainPartyCode).FirstOrDefault();
                var Discount = DefintionContext.Discounts.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode && x.PartyCode == MainPartyCode && x.ItemCode == ItemCode.Trim()).Select(x => x.Amount).FirstOrDefault();
                if (Discount != null)
                {
                    return (double)Discount;
                }
                else
                {
                    return 0;
                }

            }

        }

        public SaleItem GetSingleItemTax (string ItemCode)
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                return DefintionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == ItemCode).FirstOrDefault();
            }

        }

        public SaleParty DDLPartyDetail(string RegionCode, string PartyCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).FirstOrDefault();
            }
        }

        public RegionSetup DDLItemRate(string RegionCode, string ItemCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.RegionSetups.Where(x => x.RegionCode == RegionCode && x.ItemCode == ItemCode).FirstOrDefault();
            }
        }

        #region(--------------------------Region Detail -------------------------------)


        public List<RegionSetupVM> GetSingleRegion(string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).ToList();
                List<RegionSetupVM> NewList = new List<RegionSetupVM>();

                foreach (var item in OldList)
                {
                    RegionSetupVM Obj = new RegionSetupVM();
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDescription = item.RegionDescription;
                    Obj.NameAbbreviate = item.NameAbbreviate;
                    Obj.Phone = item.Phone;
                    Obj.Address = item.Address;
                    Obj.City = item.City;
                    Obj.ItemCode = item.ItemCode;
                    
                    Obj.ItemDescription = DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == item.ItemCode).Select(x => x.ItemDesc).FirstOrDefault();
                    Obj.RegCorporateRate = item.RegCorporateRate;
                    Obj.UnRegCorporateRate = item.UnRegCorporateRate;
                    Obj.RegRetailerRate = item.RegRetailerRate;
                    Obj.UnRegRetailerRate = item.UnRegRetailerRate;
                    Obj.UpdDate = ((DateTime)item.UpdDate).ToString("dd/MM/yyyy");
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public List<RegionSetup> GetRegion()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.RegionSetups.ToList();
            }
        }
        public string AddEditRegion(RegionSetup Obj)
        {
            using(AT_Tahur_SUITEEntities DefinitionContext= new AT_Tahur_SUITEEntities())
            {
                string SuccessMsg = "";
                if (Obj.RegionCode == null)
                {
                    var RegionList = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode).ToList();
                    if (RegionList.Count == 0)
                    {
                        Obj.RegionCode = "001";
                    }
                    else
                    {
                        var Autogen = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode).Select(x => x.RegionCode).OrderByDescending(x => x).FirstOrDefault();
                        Autogen = (Convert.ToInt64(Autogen) + 1).ToString();
                        while (Autogen.Length != 3)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.RegionCode = Autogen;
                    }


                    var List = HttpContext.Current.Session["RegionItemRate"] as RegionSetup[];

                    foreach (var item in List)
                    {
                        RegionSetup NewObj = new RegionSetup();
                        NewObj.CompCode = CompanyCode;
                        NewObj.RegionCode = Obj.RegionCode ;
                        NewObj.RegionDescription = Obj.RegionDescription;
                        NewObj.NameAbbreviate = Obj.NameAbbreviate;
                        NewObj.Phone = Obj.Phone;
                        NewObj.Address = Obj.Address;
                        NewObj.City = Obj.City;
                        NewObj.DelFlag = "N";
                        NewObj.ItemCode = item.ItemCode.Trim();
                        NewObj.RegCorporateRate = item.RegCorporateRate;
                        NewObj.UnRegCorporateRate = item.UnRegCorporateRate;
                        NewObj.RegRetailerRate = item.RegRetailerRate;
                        NewObj.UnRegRetailerRate = item.UnRegRetailerRate;
                        NewObj.UpdDate = DateTime.Now;
                        NewObj.UpdTerm = Environment.MachineName;
                        NewObj.UpdUser = CommonDAL.UserName();
                        DefinitionContext.RegionSetups.Add(NewObj);
                        DefinitionContext.SaveChanges();
                    }
                    SuccessMsg = "Saved Successfully. . . !";

                }
                else
                {
                    DefinitionContext.RegionSetups.RemoveRange(DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).ToList());
                    var List = HttpContext.Current.Session["RegionItemRate"] as RegionSetup[];

                    foreach (var item in List)
                    {
                        RegionSetup NewObj = new RegionSetup();
                        NewObj.CompCode = CompanyCode;
                        NewObj.RegionCode = Obj.RegionCode;
                        NewObj.RegionDescription = Obj.RegionDescription;
                        NewObj.NameAbbreviate = Obj.NameAbbreviate;
                        NewObj.Phone = Obj.Phone;
                        NewObj.Address = Obj.Address;
                        NewObj.City = Obj.City;
                        NewObj.DelFlag = "N";
                        NewObj.ItemCode = item.ItemCode.Trim();
                        NewObj.RegCorporateRate = item.RegCorporateRate;
                        NewObj.UnRegCorporateRate = item.UnRegCorporateRate;
                        NewObj.RegRetailerRate = item.RegRetailerRate;
                        NewObj.UnRegRetailerRate = item.UnRegRetailerRate;
                        NewObj.UpdDate = DateTime.Now;
                        NewObj.UpdTerm = Environment.MachineName;
                        NewObj.UpdUser = CommonDAL.UserName();
                        DefinitionContext.RegionSetups.Add(NewObj);
                        DefinitionContext.SaveChanges();
                    }
                    SuccessMsg = "Updated Successfully . . . !";
                }

                return SuccessMsg;
            }
        }
        public bool DeleteRegion(string RegionCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var FindRegion = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).ToList();

                foreach (var item in FindRegion)
                {
                    item.UpdDate = DateTime.Now;
                    item.UpdTerm = Environment.MachineName;
                    item.UpdUser = CommonDAL.UserName();
                    item.DelFlag = "Y";
                    DefinitionContext.SaveChanges();
                }

              
                var FoundDeleteReg = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).Select(x => x.DelFlag).FirstOrDefault();
                if (FoundDeleteReg == "Y")
                {
                    SuccessReg = true;
                }

                return SuccessReg;
            }
        }

        #endregion



        #region(-------------------------- Previuos Region Detail -------------------------------)


        public List<RegionSetupVM> GetSinglePreRegion(string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.PreRegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).ToList();
                List<RegionSetupVM> NewList = new List<RegionSetupVM>();

                foreach (var item in OldList)
                {
                    RegionSetupVM Obj = new RegionSetupVM();
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDescription = item.RegionDescription;
                    Obj.NameAbbreviate = item.NameAbbreviate;
                    Obj.Phone = item.Phone;
                    Obj.Address = item.Address;
                    Obj.City = item.City;
                    Obj.ItemCode = item.ItemCode;

                    Obj.ItemDescription = DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == item.ItemCode).Select(x => x.ItemDesc).FirstOrDefault();
                    Obj.RegCorporateRate = item.RegCorporateRate;
                    Obj.UnRegCorporateRate = item.UnRegCorporateRate;
                    Obj.RegRetailerRate = item.RegRetailerRate;
                    Obj.UnRegRetailerRate = item.UnRegRetailerRate;
                    Obj.UpdDate = ((DateTime)item.UpdDate).ToString("dd/MM/yyyy");
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public List<PreRegionSetup> GetPreRegion()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                return DefinitionContext.PreRegionSetups.ToList();
            }
        }
        public string AddEditPreRegion(PreRegionSetup Obj)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                string SuccessMsg = "";
                if (Obj.RegionCode == null)
                {
                    var RegionList = DefinitionContext.PreRegionSetups.Where(x => x.CompCode == CompanyCode).ToList();
                    if (RegionList.Count == 0)
                    {
                        Obj.RegionCode = "001";
                    }
                    else
                    {
                        var Autogen = DefinitionContext.PreRegionSetups.Where(x => x.CompCode == CompanyCode).Select(x => x.RegionCode).OrderByDescending(x => x).FirstOrDefault();
                        Autogen = (Convert.ToInt64(Autogen) + 1).ToString();
                        while (Autogen.Length != 3)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.RegionCode = Autogen;
                    }


                    var List = HttpContext.Current.Session["PreRegionItemRate"] as PreRegionSetup[];

                    foreach (var item in List)
                    {
                        PreRegionSetup NewObj = new PreRegionSetup();
                        NewObj.CompCode = CompanyCode;
                        NewObj.RegionCode = Obj.RegionCode;
                        NewObj.RegionDescription = Obj.RegionDescription;
                        NewObj.NameAbbreviate = Obj.NameAbbreviate;
                        NewObj.Phone = Obj.Phone;
                        NewObj.Address = Obj.Address;
                        NewObj.City = Obj.City;
                        NewObj.DelFlag = "N";
                        NewObj.ItemCode = item.ItemCode.Trim();
                        NewObj.RegCorporateRate = item.RegCorporateRate;
                        NewObj.UnRegCorporateRate = item.UnRegCorporateRate;
                        NewObj.RegRetailerRate = item.RegRetailerRate;
                        NewObj.UnRegRetailerRate = item.UnRegRetailerRate;
                        NewObj.UpdDate = DateTime.Now;
                        NewObj.UpdTerm = Environment.MachineName;
                        NewObj.UpdUser = CommonDAL.UserName();
                        DefinitionContext.PreRegionSetups.Add(NewObj);
                        DefinitionContext.SaveChanges();
                    }
                    SuccessMsg = "Saved Successfully. . . !";

                }
                else
                {
                    DefinitionContext.PreRegionSetups.RemoveRange(DefinitionContext.PreRegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).ToList());
                    var List = HttpContext.Current.Session["PreRegionItemRate"] as PreRegionSetup[];

                    foreach (var item in List)
                    {
                        PreRegionSetup NewObj = new PreRegionSetup();
                        NewObj.CompCode = CompanyCode;
                        NewObj.RegionCode = Obj.RegionCode;
                        NewObj.RegionDescription = Obj.RegionDescription;
                        NewObj.NameAbbreviate = Obj.NameAbbreviate;
                        NewObj.Phone = Obj.Phone;
                        NewObj.Address = Obj.Address;
                        NewObj.City = Obj.City;
                        NewObj.DelFlag = "N";
                        NewObj.ItemCode = item.ItemCode.Trim();
                        NewObj.RegCorporateRate = item.RegCorporateRate;
                        NewObj.UnRegCorporateRate = item.UnRegCorporateRate;
                        NewObj.RegRetailerRate = item.RegRetailerRate;
                        NewObj.UnRegRetailerRate = item.UnRegRetailerRate;
                        NewObj.UpdDate = DateTime.Now;
                        NewObj.UpdTerm = Environment.MachineName;
                        NewObj.UpdUser = CommonDAL.UserName();
                        DefinitionContext.PreRegionSetups.Add(NewObj);
                        DefinitionContext.SaveChanges();
                    }
                    SuccessMsg = "Updated Successfully . . . !";
                }

                return SuccessMsg;
            }
        }
        public bool DeletePreRegion(string RegionCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var FindRegion = DefinitionContext.PreRegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).ToList();

                foreach (var item in FindRegion)
                {
                    item.UpdDate = DateTime.Now;
                    item.UpdTerm = Environment.MachineName;
                    item.UpdUser = CommonDAL.UserName();
                    item.DelFlag = "Y";
                    DefinitionContext.SaveChanges();
                }


                var FoundDeleteReg = DefinitionContext.PreRegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).Select(x => x.DelFlag).FirstOrDefault();
                if (FoundDeleteReg == "Y")
                {
                    SuccessReg = true;
                }

                return SuccessReg;
            }
        }

        #endregion


        #region(------------------------------Party Detail-----------------------------------)


        public SalePartyVM UpdParty(string RegionCode,string PartyCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                SaleParty List = DefinitionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).FirstOrDefault();

                SalePartyVM Obj = new SalePartyVM();
                if (List != null)
                {
                    Obj.PartyCode = List.PartyCode;
                    Obj.PartyName = List.PartyName;
                    Obj.MainPartyCode = List.MainPartyCode;
                    Obj.MainPartyDescription = DefinitionContext.SaleParties.Where(x => x.RegionCode == List.RegionCode && x.PartyCode == List.MainPartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Obj.PartyRegisterName = List.PartyRegisterName;
                    Obj.CompCode = List.CompCode;
                    Obj.Category = List.Category;
                    Obj.CNIC = List.CNIC;
                    Obj.ContactNumber = List.ContactNumber;
                    Obj.NTN = List.NTN;
                    Obj.Opening = List.Opening;
                    Obj.PartyAddress = List.PartyAddress;
                    Obj.PartyType = List.PartyType;
                    Obj.RegionCode = List.RegionCode;
                    Obj.SalesTaxNumber = List.SalesTaxNumber;
                    Obj.STaxReg = List.STaxReg;
                    Obj.Taxable = List.Taxable;
                    Obj.ThirdSch_Category = List.ThirdSch_Category;
                }
               

                return Obj;
            }
                
        }


        public List<SalePartyVM> GetSaleParties2()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                var GetSaleParty = DefinitionContext.GetSalePartiesFast().ToList();
                List<SalePartyVM> List = new List<SalePartyVM>();
                foreach (var item in GetSaleParty)
                {
                    SalePartyVM Party = new SalePartyVM();
                    Party.CompCode = item.CompCode;
                    Party.PartyCode = item.PartyCode;
                    Party.PartyName = (item.PartyName);
                    Party.PartyRegisterName = item.PartyRegisterName;
                    Party.RegionCode = item.RegionCode;
                    Party.RegionDescription = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Party.MainPartyCode = item.MainPartyCode;
                    Party.MainPartyDescription = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == item.MainPartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Party.PartyAddress = item.PartyAddress;
                    Party.ContactNumber = item.ContactNumber;
                    Party.CNIC = item.CNIC;
                    Party.NTN = item.NTN;
                    Party.SalesTaxNumber = item.SalesTaxNumber;
                    Party.STaxReg = item.STaxReg;
                    Party.Taxable = item.Taxable;
                    Party.PartyType = item.PartyType;
                    Party.Category = item.Category;
                    Party.Opening = item.Opening;
                    Party.UpdDate = item.UpdDate;
                    List.Add(Party);
                }

                return List;


            }
        }


        public List<SalePartyVM> GetSaleParties()
        {
            using ( AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities()) {

                var GetSaleParty = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.DelFlag == "N").OrderByDescending(x => x.PartyCode).ToList();
                List<SalePartyVM> List = new  List<SalePartyVM>();
                foreach (var item in GetSaleParty)
                {
                    SalePartyVM Party = new SalePartyVM();
                    Party.CompCode = item.CompCode;
                    Party.PartyCode = item.PartyCode;
                    Party.PartyName = (item.PartyCode + " | " + item.PartyName);
                    Party.PartyRegisterName = item.PartyRegisterName;
                    Party.RegionCode = item.RegionCode;
                    Party.RegionDescription = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Party.MainPartyCode = item.MainPartyCode;
                    Party.MainPartyDescription = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.MainPartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Party.PartyAddress = item.PartyAddress;
                    Party.ContactNumber = item.ContactNumber;
                    Party.CNIC = item.CNIC;
                    Party.NTN = item.NTN;
                    Party.SalesTaxNumber = item.SalesTaxNumber;
                    Party.STaxReg = item.STaxReg;
                    Party.Taxable = item.Taxable;
                    Party.PartyType = item.PartyType;
                    Party.Category = item.Category;
                    Party.Opening = item.Opening;
                    Party.UpdDate = item.UpdDate;
                    Party.ThirdSch_Category = item.ThirdSch_Category;
                    List.Add(Party);
                }

                return List;
                

            }
        }
        public string AddEditPartySetup(SaleParty Obj, string radio, string CashCredit, string Taxable)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                string SuccessMsg = "";
                if (Obj.PartyCode == null)
                {
                    var PartyList = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).ToList();
                    if (PartyList.Count == 0)
                    {
                        Obj.PartyCode = "00001";
                    }
                    else
                    {
                        var Autogen = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).Select(x => x.PartyCode).OrderByDescending(x => x).FirstOrDefault();
                        Autogen = (Convert.ToInt64(Autogen) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.PartyCode = Autogen;
                    }
                    if (Obj.MainPartyCode == "00000" || Obj.MainPartyCode == null || Obj.MainPartyCode == "")
                    {
                        Obj.MainPartyCode = Obj.PartyCode;
                    }
                   
                    Obj.CompCode = CompanyCode;
                    Obj.STaxReg = radio;
                    if (Taxable == null)
                    {
                        Obj.Taxable = "N";
                    }
                    else
                    {
                        Obj.Taxable = "Y";
                    }

                    Obj.PartyType = CashCredit;
                    Obj.PartyAddress = Obj.PartyAddress.ToUpper();
                    Obj.PartyName = Obj.PartyName.ToUpper();
                    Obj.DelFlag = "N";
                        
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdTerm = Environment.MachineName;
                    Obj.UpdUser = CommonDAL.UserName();
                    DefinitionContext.SaleParties.Add(Obj);
                    ActivePartiesSalesman AObj = new ActivePartiesSalesman();
                    AObj.RegionCode = Obj.RegionCode;
                    AObj.PartyCode = Obj.PartyCode;
                    AObj.MainPartyCode = Obj.MainPartyCode;
                    DefinitionContext.ActivePartiesSalesmen.Add(AObj);
                    DefinitionContext.SaveChanges();
                    HttpContext.Current.Session["RegionCode"] = Obj.RegionCode;
                    SuccessMsg = "(" + Obj.PartyCode + "   ===>  " + Obj.PartyName + ")" + "     Saved Successfully . . . !";
                }
                else
                {
                    DefinitionContext.SaleParties.Remove(DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode && x.PartyCode == Obj.PartyCode).FirstOrDefault());
                    var Find = DefinitionContext.ActivePartiesSalesmen.Where(x => x.RegionCode == Obj.RegionCode && x.PartyCode == Obj.PartyCode).FirstOrDefault();
                    if (Find != null)
                    {
                        DefinitionContext.ActivePartiesSalesmen.Remove(Find);
                    }
                    if (Obj.MainPartyCode == null)
                    {
                        Obj.MainPartyCode = Obj.PartyCode;
                    }

                    Obj.CompCode = CompanyCode;
                    Obj.STaxReg = radio;
                    if (Taxable == null)
                    {
                        Obj.Taxable = "N";
                    }
                    else
                    {
                        Obj.Taxable = "Y";
                    }

                    Obj.PartyType = CashCredit;
                    Obj.PartyAddress = Obj.PartyAddress.ToUpper();
                    Obj.PartyName = Obj.PartyName.ToUpper();
                    Obj.DelFlag = "N";
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdTerm = Environment.MachineName;
                    Obj.UpdUser = CommonDAL.UserName();
                    DefinitionContext.SaleParties.Add(Obj);
                    ActivePartiesSalesman AObj = new ActivePartiesSalesman();
                    AObj.RegionCode = Obj.RegionCode;
                    AObj.PartyCode = Obj.PartyCode;
                    AObj.MainPartyCode = Obj.MainPartyCode;
                    DefinitionContext.ActivePartiesSalesmen.Add(AObj);
                    DefinitionContext.SaveChanges();

                   
                    

                    HttpContext.Current.Session["RegionCode"] = Obj.RegionCode;
                    SuccessMsg = "(" + Obj.PartyCode + "   ===>  " + Obj.PartyName + ")" + "       Updated Successfully . . . !";
                }

                return SuccessMsg;

            }
        }

        public List<MainPartyVM> GetDistinctMainParty()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                List<MainPartyVM> List = new List<MainPartyVM>();
                var MainPartyDistinctCode = DefinitionContext.GetDinstinctParty(CompanyCode).ToList();
                foreach (var MainPartyCode in MainPartyDistinctCode)
                {
                    MainPartyVM MainParty = new MainPartyVM();
                    MainParty.MainPartyCode = MainPartyCode;
                    MainParty.MainPartyName = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == MainPartyCode).Select(x => x.PartyName).FirstOrDefault();
                    MainParty.SalesTaxNumber = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == MainPartyCode).Select(x => x.SalesTaxNumber).FirstOrDefault();
                    MainParty.NTN = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == MainPartyCode).Select(x => x.NTN).FirstOrDefault();
                    MainParty.Opening = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == MainPartyCode).Select(x => x.Opening).FirstOrDefault();
                    List.Add(MainParty);
                }

                return List;
            }
        }

        public bool DeleteParty(string RegionCode,string PartyCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                bool ConfirmMsg = false;
                //var FindParty = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == PartyCode).FirstOrDefault();
                DefinitionContext.SaleParties.Remove(DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode && x.PartyCode == PartyCode).FirstOrDefault());
                var Find = DefinitionContext.ActivePartiesSalesmen.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).FirstOrDefault();
                if (Find != null)
                {
                    DefinitionContext.ActivePartiesSalesmen.Remove(Find);
                }
                
                DefinitionContext.SaveChanges();
                //FindParty.UpdDate = DateTime.Now;
                //FindParty.UpdTerm = Environment.MachineName;
                //FindParty.UpdUser = CommonDAL.UserName();
                //FindParty.DelFlag = "Y";
                //DefinitionContext.SaveChanges();

                //var FoundDelFlag = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == PartyCode).Select(x => x.DelFlag).FirstOrDefault();
                //if (FoundDelFlag == "Y")
                //{
                ConfirmMsg = true;
                //}

                return ConfirmMsg;
            }
        }


        public bool MergeParty(string RegionCode, string PartyCode, string MainPartyCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    bool ConfirmMsg = false;
                    try
                    {
                        var PartySale = DefinitionContext.SaleInvoiceMasters.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).ToList();

                        var PartyRecovery = DefinitionContext.Recoveries.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).ToList();
                        foreach (var item in PartySale)
                        {
                            item.MainPartyCode = MainPartyCode;
                            DefinitionContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            DefinitionContext.SaveChanges();
                        }
                        foreach (var Rec in PartyRecovery)
                        {
                            Rec.MainPartyCode = MainPartyCode;
                            DefinitionContext.Entry(Rec).State = System.Data.Entity.EntityState.Modified;
                            DefinitionContext.SaveChanges();
                        }

                        //if (PartySale.Count != 0)
                        //{
                        //    ts.Complete();
                        //    //DefinitionContext.SaleInvoiceMasters.RemoveRange(PartySale);
                        //    //DefinitionContext.SaveChanges();
                        //    //DefinitionContext.SaleInvoiceMasters.AddRange(PartySale);
                        //    //DefinitionContext.SaveChanges();
                        //    DefinitionContext.SP_MergeSaleDetail(RegionCode, PartyCode);
                        //    DefinitionContext.SaveChanges();
                        //}

                        //if (PartyRecovery.Count != 0)
                        //{
                        //    DefinitionContext.Recoveries.RemoveRange(PartyRecovery);
                        //    DefinitionContext.SaveChanges();
                        //    DefinitionContext.Recoveries.AddRange(PartyRecovery);
                        //    DefinitionContext.SaveChanges();
                        //}
                       
                        ts.Complete();
                        ConfirmMsg = true;
                        return ConfirmMsg;
                    }
                    catch (Exception ex)
                    {
                        ts.Dispose();
                        throw ex;
                        
                        //return ConfirmMsg;
                    }

                }
            }
        }

        #endregion

        #region(------------------------------------Company Employee Details---------------------------------------)

        public List<CompanyEmpVM> GetCompanyEmployee()
        {
            List<CompanyEmpVM> List = new List<CompanyEmpVM>();
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
               var FoundEmp = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.DelFlag == "N").OrderByDescending(x => x.EmpCode).ToList();

                foreach (var item in FoundEmp)
                {
                    CompanyEmpVM Emp = new CompanyEmpVM();
                    Emp.EmpCode = item.EmpCode;
                    Emp.EmpName = item.EmpName;
                    Emp.RegionCode = item.RegionCode;
                    Emp.RegionDescription = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Emp.EmpAddress = item.EmpAddress;
                    Emp.EmpRemarks = item.EmpRemarks;
                    Emp.EmpQualification = item.EmpQualification;
                    Emp.EmpStatus = item.EmpStatus;
                    Emp.EmpContactNo = item.EmpContactNo;
                    Emp.EmpDepartment = item.EmpDepartment;
                    Emp.EmpDesignation = item.EmpDesignation;
                    Emp.EmpBasicSalary = item.EmpBasicSalary;
                    Emp.EmpEperiance = item.EmpEperiance;
                    Emp.EmpJoiningDate = item.EmpJoiningDate;
                    Emp.EmpLeavingDate = item.EmpLeavingDate;
                    Emp.EmpDOB = item.EmpDOB;
                    Emp.OpeningShortCash = item.OpeningShortCash;
                    List.Add(Emp);
                }

                return List;
            }
        }
        
        public string AddEditCompanyEmployee(CompanyEmp Obj)
        {
            string SuccessMsg = "";
            if (Obj.EmpCode == null)
            {
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                    var EmpList = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).ToList();
                    if (EmpList.Count == 0)
                    {
                        Obj.EmpCode = "00000001";
                    }
                    else
                    {
                        var Autogen = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).Select(x => x.EmpCode).OrderByDescending(x => x).FirstOrDefault();
                        Autogen = (Convert.ToInt64(Autogen) + 1).ToString();
                        while (Autogen.Length != 8)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.EmpCode = Autogen;
                    }

                    Obj.CompCode = CompanyCode;
                    Obj.DelFlag = "N";
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdTerm = Environment.MachineName;
                    Obj.UpdUser = CommonDAL.UserName();
                    DefinitionContext.CompanyEmps.Add(Obj);
                    DefinitionContext.SaveChanges();
                    SuccessMsg = "Saved Successfully . . . !";
                }
            }
            else
            {
                using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
                {
                   DefinitionContext.CompanyEmps.Remove(DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode && x.EmpCode == Obj.EmpCode).FirstOrDefault());

                    Obj.CompCode = CompanyCode;
                    Obj.DelFlag = "N";
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdTerm = Environment.MachineName;
                    Obj.UpdUser = CommonDAL.UserName();
                    DefinitionContext.CompanyEmps.Add(Obj);
                    DefinitionContext.SaveChanges();
                    SuccessMsg = "Updated Successfully . . . !";
                }
            }

            return SuccessMsg;
        }

        public bool DeleteEmployee(string EmpCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                bool ConfirmMsg = false;
                var FindEmployee = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.EmpCode == EmpCode).FirstOrDefault();

                FindEmployee.UpdDate = DateTime.Now;
                FindEmployee.UpdTerm = Environment.MachineName;
                FindEmployee.UpdUser = CommonDAL.UserName();
                FindEmployee.DelFlag = "Y";
                DefinitionContext.SaveChanges();

                var FoundDelFlag = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.EmpCode == EmpCode).Select(x => x.DelFlag).FirstOrDefault();
                if (FoundDelFlag == "Y")
                {
                    ConfirmMsg = true;
                }

                return ConfirmMsg;
            }
        }

        #endregion

        #region(-------------------------------------------Daily Stock Receive---------------------------------------------)

        public List<DailyStockReceiveVM> GetDinstinctDSRNoInfo(string GetDSRDate)
        {

            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(GetDSRDate, "dd/MM/yyyy", null);
                var List = DefintionContext.GetDinstinctDSRNo(CompanyCode).Where(x => x.DSRDate == Date).ToList();
                List<DailyStockReceiveVM> NewList = new List<DailyStockReceiveVM>();

                foreach (var item in List)
                {
                    DailyStockReceiveVM Obj = new DailyStockReceiveVM();
                    Obj.DSRNo = item.DSRNo;
                    Obj.DSRDate = item.DSRDate.ToString("dd/MM/yyyy");
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDescription = DefintionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.MainComment = DefintionContext.DailyStockReceives.Where(x => x.DSRNo == item.DSRNo).Select(x => x.MainComment).FirstOrDefault();
                    NewList.Add(Obj);
                }

                return NewList;
            }
        }


        public List<DailyStockReceiveVM> GetSingleDSRNo(string DSRNo)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.DailyStockReceives.Where(x => x.CompCode == CompanyCode && x.DSRNo == DSRNo).ToList();
                var ItemList = DefinitionContext.SaleItems.ToList();
                List<DailyStockReceiveVM> NewList = new List<DailyStockReceiveVM>();

                foreach (var item in ItemList)
                {
                    DailyStockReceiveVM Obj = new DailyStockReceiveVM();
                    Obj.DSRNo = DSRNo;
                    Obj.DSRDate = OldList.Select(x => x.DSRDate).FirstOrDefault().ToString("dd/MM/yyyy");
                    Obj.RegionCode = OldList.Select(x => x.RegionCode).FirstOrDefault();
                    Obj.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == Obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.MainComment = OldList.Select(x => x.MainComment).FirstOrDefault();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = item.ItemDesc;
                    Obj.ItemComment  = OldList.Where(x => x.ItemCode == item.ItemCode).Select(x => x.ItemComment).FirstOrDefault();

                    var Qty = OldList.Where(x => x.ItemCode == Obj.ItemCode).Select(x => x.Quantity).FirstOrDefault();

                    if (Qty != null)
                    {
                        Obj.Quantity = (double)Qty;
                    }
                    else
                    {
                        Obj.Quantity = 0;
                    }
                    
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public List<DailyStockReceiveVM> GetDailyStockReceives()
        {
            using(AT_Tahur_SUITEEntities DefinitionContext =new AT_Tahur_SUITEEntities())
            {
                List<DailyStockReceiveVM> List = new List<DailyStockReceiveVM>();
                var GetList = DefinitionContext.DailyStockReceives.Where(x => x.CompCode == CompanyCode).OrderByDescending(x => x.DSRNo).ToList();
                foreach (var item in GetList)
                {
                    DailyStockReceiveVM DSR = new DailyStockReceiveVM();
                  

                    List.Add(DSR);
                }

                return List;
            }
        }

        public string AddEditDailyStockReceive(DailyStockReceive Obj,string DSRDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.DSRNo == null)
                {
                    var FoundDailyStock = DefinitionContext.DailyStockReceives.Where(x => x.CompCode == CompanyCode).ToList();
                    if (FoundDailyStock.Count == 0)
                    {
                        Obj.DSRNo = "19/00001";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.DailyStockReceives.Where(x => x.CompCode == CompanyCode).Select(x => x.DSRNo).OrderByDescending(x => x).FirstOrDefault();
                        string date = DateTime.Now.ToString("yy");
                        string year = date + "/";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.DSRNo = year + Autogen;
                    }

                    var List = HttpContext.Current.Session["ItemDiscountDSR"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            DailyStockReceive NewObj = new DailyStockReceive();
                            NewObj.CompCode = CompanyCode;
                            NewObj.DSRNo = Obj.DSRNo;
                            NewObj.DSRDate = DateTime.ParseExact(DSRDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.ItemComment = item.ItemComment;
                            NewObj.MainComment = Obj.MainComment;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyStockReceives.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.DailyStockReceives.RemoveRange(DefinitionContext.DailyStockReceives.Where(x => x.CompCode == CompanyCode && x.DSRNo == Obj.DSRNo).ToList());

                    var List = HttpContext.Current.Session["ItemDiscountDSR"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            DailyStockReceive NewObj = new DailyStockReceive();
                            NewObj.CompCode = CompanyCode;
                            NewObj.DSRNo = Obj.DSRNo;
                            NewObj.DSRDate = DateTime.ParseExact(DSRDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.ItemComment = item.ItemComment;
                            NewObj.MainComment = Obj.MainComment;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyStockReceives.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully. . . !";
                }
            }

            return SuccessMsg;
        }


        #endregion

        #region(---------------------------------------------Salesman Opening Stock------------------------------------------------)

        public List<RegionEmpVM> GetRegionEmp(string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
               var OldList = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).OrderBy(x=>x.EmpName).ToList();
                List<RegionEmpVM> NewList = new List<RegionEmpVM>();
                foreach (var item in OldList)
                {
                    RegionEmpVM Obj = new RegionEmpVM();
                    Obj.EmpCode = item.EmpCode;
                    Obj.EmpName = item.EmpName;
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    NewList.Add(Obj);
                }

                return NewList;
            }
        }

        public List<CompanyEmp> GetMultiRegionEmp(string[] RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && RegionCode.Contains(x.RegionCode)).ToList();

                return OldList;
            }
        }


        public List<SalesmanOpeningStockVM> GetDinstinctSOSNo(string GetSOSDate)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(GetSOSDate, "dd/MM/yyyy", null);
                List<SalesmanOpeningStockVM> List = new List<SalesmanOpeningStockVM>();
                var GetList = DefinitionContext.GetDinstinctSOSNo(CompanyCode).Where(x => x.SOSDate == Date).ToList();
                foreach (var item in GetList)
                {
                    SalesmanOpeningStockVM SOS = new SalesmanOpeningStockVM();

                    SOS.SOSNo = item.SOSNo;
                    SOS.SOSDate = item.SOSDate.ToString("dd/MM/yyyy");
                    SOS.RegionCode = item.RegionCode;
                    SOS.RegionDescription = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    SOS.EmpCode = item.EmpCode;
                    SOS.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    List.Add(SOS);
                }

                return List;
            }
        }

        public List<SalesmanOpeningStockVM> GetSingleSOSNo(string SOSNo)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                List<SalesmanOpeningStockVM> List = new List<SalesmanOpeningStockVM>();
                var GetList = DefinitionContext.SalesmanOpeningStocks.Where(x => x.CompCode == CompanyCode && x.SOSNo == SOSNo).ToList();
                var ItemList = DefinitionContext.SaleItems.ToList();
                foreach (var item in ItemList)
                {
                    SalesmanOpeningStockVM SOS = new SalesmanOpeningStockVM();
                   
                    SOS.SOSNo = SOSNo;
                    SOS.SOSDate = GetList.Select(x => x.SOSDate).FirstOrDefault().ToString("dd/MM/yyyy");
                    SOS.RegionCode = GetList.Select(x => x.RegionCode).FirstOrDefault();
                    SOS.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == SOS.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    SOS.MainComment = GetList.Select(x => x.MainComment).FirstOrDefault();
                    SOS.VehicleCode = GetList.Select(x => x.VehicleCode).FirstOrDefault();
                    SOS.EmpCode = GetList.Select(x => x.EmpCode).FirstOrDefault();
                    SOS.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == SOS.RegionCode && x.EmpCode == SOS.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    SOS.ItemCode = item.ItemCode;
                    SOS.ItemDesc = item.ItemDesc;
                    SOS.ItemComment = GetList.Where(x => x.ItemCode == item.ItemCode).Select(x => x.ItemComment).FirstOrDefault();
                    
                    var Qty = GetList.Where(x =>x.ItemCode == item.ItemCode).Select(x => x.Quantity).FirstOrDefault();

                    if (Qty != null)
                    {
                        SOS.Quantity = (double)Qty;
                    }
                    else
                    {
                        SOS.Quantity = 0;
                    }

                    List.Add(SOS);
                }

                return List;
            }
        }

        public string AddEditSalesmanOpeningStock(SalesmanOpeningStock Obj,string SOSDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.SOSNo == null)
                {
                    var FoundSOStock = DefinitionContext.SalesmanOpeningStocks.Where(x => x.CompCode == CompanyCode).FirstOrDefault();
                    if (FoundSOStock == null)
                    {
                        Obj.SOSNo = "19/00001";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.SalesmanOpeningStocks.Where(x => x.CompCode == CompanyCode).Select(x => x.SOSNo).OrderByDescending(x => x).FirstOrDefault();
                        string date = DateTime.Now.ToString("yy");
                        string year = date + "/";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.SOSNo = year + Autogen;
                    }

                    var List = HttpContext.Current.Session["ItemDiscountSOS"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            SalesmanOpeningStock NewObj = new SalesmanOpeningStock();
                            NewObj.CompCode = CompanyCode;
                            NewObj.EmpCode = Obj.EmpCode;
                            NewObj.SOSNo = Obj.SOSNo;
                            NewObj.SOSDate = DateTime.ParseExact(SOSDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.VehicleCode = Obj.VehicleCode;
                            NewObj.MainComment = Obj.MainComment;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.ItemComment = item.ItemComment;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.SalesmanOpeningStocks.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.SalesmanOpeningStocks.RemoveRange(DefinitionContext.SalesmanOpeningStocks.Where(x => x.CompCode == CompanyCode && x.SOSNo == Obj.SOSNo).ToList());

                    var List = HttpContext.Current.Session["ItemDiscountSOS"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            SalesmanOpeningStock NewObj = new SalesmanOpeningStock();
                            NewObj.CompCode = CompanyCode;
                            NewObj.EmpCode = Obj.EmpCode;
                            NewObj.SOSNo = Obj.SOSNo;
                            NewObj.SOSDate = DateTime.ParseExact(SOSDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.VehicleCode = Obj.VehicleCode;
                            NewObj.MainComment = Obj.MainComment;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.ItemComment = item.ItemComment;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.SalesmanOpeningStocks.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully . . . !";
                }
            }

            return SuccessMsg;
        }


        public bool DeleteSOS(string SOSNo)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DefinitionContext.SalesmanOpeningStocks.RemoveRange(DefinitionContext.SalesmanOpeningStocks.Where(x => x.SOSNo == SOSNo).ToList());
                DefinitionContext.SaveChanges();
              
                SuccessReg = true;

                return SuccessReg;
            }
        }





        #endregion

        #region(---------------------------------------------Party Discount--------------------------------------------------------)

        public List<PartyDiscountVM> GetPartyDiscount()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                List<PartyDiscountVM> List = new List<PartyDiscountVM>();
                var GetList = DefinitionContext.PartyDiscounts.Where(x => x.CompCode == CompanyCode).OrderByDescending(x => x.PDCode).ToList();
                foreach (var item in GetList)
                {
                    PartyDiscountVM PD = new PartyDiscountVM();

                    PD.PDCode = item.PDCode;
                    PD.PDDate = item.PDDate;
                    PD.RegionCode = item.RegionCode;
                    PD.RegionDescription = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    PD.PartyCode = item.PartyCode;
                    PD.PartyName = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    PD.MilkFF = item.MilkFF;
                    PD.MilkLF = item.MilkLF;
                    PD.MilkNeutraHalf = item.MilkNeutraHalf;
                    PD.MilkNeutra = item.MilkNeutra;
                    PD.RawMilk = item.RawMilk;
                    PD.MilkFF450 = item.MilkFF450;
                    PD.MilkFF250 = item.MilkFF250;
                    PD.MilkLF250 = item.MilkLF250;
                    PD.ChokoMilk = item.ChokoMilk;
                    PD.MilkStrawBerry200 = item.MilkStrawBerry200;
                    PD.Ecolean = item.Ecolean;
                    PD.PremaRotation = item.PremaRotation;
                    PD.YgrtNatural400 = item.YgrtNatural400;
                    PD.YgrtNatural80 = item.YgrtNatural80;
                    PD.YgrtNatural1kg = item.YgrtNatural1kg;
                    PD.YgrtStrawBerry90 = item.YgrtStrawBerry90;
                    PD.YgrtStrawBerry80 = item.YgrtStrawBerry80;
                    PD.YgrtLF400 = item.YgrtLF400;
                    PD.YgrtBlueBerry90 = item.YgrtBlueBerry90;
                    PD.YgrtMango80 = item.YgrtMango80;
                    PD.YgrtVanilla100 = item.YgrtVanilla100;
                    PD.YgrtSweet400 = item.YgrtSweet400;
                    PD.YgrtPouch500 = item.YgrtPouch500;
                    PD.Podina = item.Podina;
                    PD.FreshCream = item.FreshCream;
                    PD.RaitaZeera250 = item.RaitaZeera250;
                    PD.RaitaPodina250 = item.RaitaPodina250;
                    PD.RaitaZeera80 = item.RaitaZeera80;
                    PD.PineApple90 = item.PineApple90;
                    PD.SmoothieMango220 = item.SmoothieMango220;
                    PD.SmoothieStrawberry220 = item.SmoothieStrawberry220;
                    PD.Cream = item.Cream;
                    PD.LabbanPlain = item.LabbanPlain;
                    PD.LabbanStrawberry = item.LabbanStrawberry;

                    List.Add(PD);
                }

                return List;
            }
        }

        public List<PartyDiscountNewVM> GetDistinctPartyDiscountInfo()
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefintionContext.GetDinstinctPartyDiscount(CompanyCode).ToList();
                List<PartyDiscountNewVM> NewList = new List<PartyDiscountNewVM>();

                foreach (var item in List)
                {
                    PartyDiscountNewVM Obj = new PartyDiscountNewVM();
                    Obj.PdCode = item.PdCode;
                    Obj.PdDate = item.PdDate;
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefintionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.PartyDesc = DefintionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    NewList.Add(Obj);
                }

                return NewList;
            }
        }

        public List<ItemDiscountRateVM> GetItemDiscountedRate(string RegionCode,string STaxReg,string Category)
        {
            using(AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefintionContext.RegionSetups.Where(x => x.RegionCode == RegionCode).ToList();
                List<ItemDiscountRateVM> NewList = new List<ItemDiscountRateVM>();

                foreach (var item in List)
                {
                    ItemDiscountRateVM Obj = new ItemDiscountRateVM();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = DefintionContext.SaleItems.Where(x => x.ItemCode == item.ItemCode).Select(x => x.ItemDesc).FirstOrDefault();
                    
                    if (STaxReg == "Reg" && Category == "CORPORATE")
                    {
                        Obj.DiscountRate = (double) item.RegCorporateRate;
                    }

                    if (STaxReg == "Un-Reg" && Category == "CORPORATE")
                    {
                        Obj.DiscountRate = (double)item.UnRegCorporateRate;
                    }

                    if (STaxReg == "Reg" && Category == "Retailer")
                    {
                        Obj.DiscountRate = (double)item.RegCorporateRate;
                    }

                    if (STaxReg == "Un-Reg" && Category == "Retailer")
                    {
                        Obj.DiscountRate = (double)item.UnRegRetailerRate;
                    }
                    NewList.Add(Obj);
                }

                return NewList;
            }
        }

        public List<SinglePartyDiscountNewVM> GetAllPartyDiscountNew()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.Discounts.Where(x => x.CompCode == CompanyCode).ToList();
                List<SinglePartyDiscountNewVM> NewList = new List<SinglePartyDiscountNewVM>();

                foreach (var item in OldList)
                {
                    SinglePartyDiscountNewVM Obj = new SinglePartyDiscountNewVM();
                    Obj.PdCode = item.PdCode;
                    Obj.PdDate = item.PdDate;
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Obj.Category = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.Category).FirstOrDefault();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == item.ItemCode).Select(x => x.ItemDesc).FirstOrDefault();
                    Obj.Amount = item.Amount;
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public List<SinglePartyDiscountNewVM> GetSinglePartyDiscountNew(string RegionCode,string PartyCode)
        {
            using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.Discounts.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode && x.PartyCode == PartyCode).ToList();
                var SaleItems = DefinitionContext.SaleItems.ToList();
                List<SinglePartyDiscountNewVM> NewList = new List<SinglePartyDiscountNewVM>();

                foreach (var item in SaleItems)
                {
                    var STaxReg = DefinitionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.STaxReg).FirstOrDefault();
                    var Category = DefinitionContext.SaleParties.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.Category).FirstOrDefault();


                    SinglePartyDiscountNewVM Obj = new SinglePartyDiscountNewVM();
                    Obj.PdCode = DefinitionContext.Discounts.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.PdCode).FirstOrDefault();
                    Obj.PdDate = DefinitionContext.Discounts.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.PdDate).FirstOrDefault();
                    Obj.RegionCode = RegionCode;
                    Obj.RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = PartyCode;
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = item.ItemDesc;

                    var ItemAmount = DefinitionContext.Discounts.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode && x.ItemCode == item.ItemCode).Select(x => x.Amount).FirstOrDefault();

                    if (ItemAmount != null)
                    {
                        Obj.Amount = ItemAmount;
                    }
                    else
                    {
                        Obj.Amount = 0;
                    }
                    

                    if (Category == "CORPORATE" && STaxReg == "Y")
                    {
                        Obj.ItemRate = DefinitionContext.RegionSetups.Where(x => x.RegionCode == RegionCode && x.ItemCode == item.ItemCode).Select(x => x.RegCorporateRate).FirstOrDefault();
                    }

                    if (Category == "CORPORATE" && STaxReg == "N")
                    {
                        Obj.ItemRate = DefinitionContext.RegionSetups.Where(x => x.RegionCode == RegionCode && x.ItemCode == item.ItemCode).Select(x => x.UnRegCorporateRate).FirstOrDefault();
                    }

                    if (Category == "Retailer" && STaxReg == "Y")
                    {
                        Obj.ItemRate = DefinitionContext.RegionSetups.Where(x => x.RegionCode == RegionCode && x.ItemCode == item.ItemCode).Select(x => x.RegRetailerRate).FirstOrDefault();
                    }

                    if (Category == "Retailer" && STaxReg == "N")
                    {
                        Obj.ItemRate = DefinitionContext.RegionSetups.Where(x => x.RegionCode == RegionCode && x.ItemCode == item.ItemCode).Select(x => x.UnRegRetailerRate).FirstOrDefault();
                    }

                   
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }
        public string AddEditPartyDiscount(PartyDiscount Obj)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.PDCode == null)
                {
                    var FoundPartyDiscount = DefinitionContext.PartyDiscounts.Where(x => x.CompCode == CompanyCode).ToList();
                    if (FoundPartyDiscount.Count == 0)
                    {
                        Obj.PDCode = "19/00001";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.PartyDiscounts.Where(x => x.CompCode == CompanyCode).Select(x => x.PDCode).OrderByDescending(x => x).FirstOrDefault();
                        string date = DateTime.Now.ToString("yy");
                        string year = date + "/";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.PDCode = year + Autogen;
                    }

                    Obj.CompCode = CompanyCode;
                    Obj.PDDate = DateTime.Now;
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdTerm = Environment.MachineName;
                    Obj.UpdUser = CommonDAL.UserName();
                    DefinitionContext.PartyDiscounts.Add(Obj);
                    DefinitionContext.SaveChanges();
                    SuccessMsg = "Saved Successfully . . . !";
                }
                else
                {
                    DefinitionContext.PartyDiscounts.Remove(DefinitionContext.PartyDiscounts.Where(x => x.CompCode == CompanyCode && x.PDCode == Obj.PDCode).FirstOrDefault());

                    Obj.CompCode = CompanyCode;
                    Obj.PDDate = DateTime.Now;
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdTerm = Environment.MachineName;
                    Obj.UpdUser = CommonDAL.UserName();
                    DefinitionContext.PartyDiscounts.Add(Obj);
                    DefinitionContext.SaveChanges();
                    SuccessMsg = "Updated Successfully . . . !";
                }
            }

            return SuccessMsg;
        }

        public string AddNewPartyDiscount(Discount Obj)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.PdCode == null)
                {
                    var GetDiscount = DefinitionContext.Discounts.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).ToList();
                    if (GetDiscount.Count == 0)
                    {
                        Obj.PdCode = "19/00001";
                    }
                    else
                    {
                        var Autogen = DefinitionContext.Discounts.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode).Select(x => x.PdCode).OrderByDescending(x => x).FirstOrDefault();
                        string date = DateTime.Now.ToString("yy");
                        string year = date + "/";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.PdCode = year + Autogen;
                    }
                    var List = HttpContext.Current.Session["ItemTable"] as ItemDiscountVM[];
                    HttpContext.Current.Session["RegionCode"] = Obj.RegionCode;

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            Discount NewObj = new Discount();
                            NewObj.CompCode = CompanyCode;
                            NewObj.PdCode = Obj.PdCode;
                            NewObj.PdDate = DateTime.Now.Date;
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.PartyCode = Obj.PartyCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Amount = item.DiscountAmt;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.Discounts.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }

                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.Discounts.RemoveRange(DefinitionContext.Discounts.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode && x.PdCode == Obj.PdCode).ToList());

                    var List = HttpContext.Current.Session["ItemTable"] as ItemDiscountVM[];
                    HttpContext.Current.Session["RegionCode"] = Obj.RegionCode;

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            Discount NewObj = new Discount();
                            NewObj.CompCode = CompanyCode;
                            NewObj.PdCode = Obj.PdCode;
                            NewObj.PdDate = DateTime.Now.Date;
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.PartyCode = Obj.PartyCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Amount = item.DiscountAmt;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.Discounts.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully. . . !";
                }

                return SuccessMsg;
            }
        }

        #endregion

        #region(-------------------------------------------Sale Item--------------------------------------------)

        public List<SaleItemVM> GetSingleItem(string Id)
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefintionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == Id).ToList();
                List<SaleItemVM> VMList = new List<SaleItemVM>();
                foreach (var item in List)
                {
                    SaleItemVM ItemData = new SaleItemVM();
                    ItemData.ItemCode = item.ItemCode;
                    ItemData.ItemDesc = item.ItemDesc;
                    ItemData.Brand = item.Brand.Trim();
                    ItemData.Category = item.Category.Trim();
                    ItemData.SubCategory = item.SubCategory.Trim();
                    ItemData.stdWeight = item.stdWeight;
                    ItemData.WeightType = item.WeightType;
                    ItemData.PackSize = item.PackSize;
                    ItemData.SaleRateReg = item.SaleRateReg;
                    ItemData.SaleRateUnReg = item.SaleRateUnReg;
                    ItemData.SaleGSTPer = item.SaleGSTPer;
                    ItemData.SaleFurtherTaxPer = item.SaleFurtherTaxPer;
                    ItemData.ProductionOpening = item.ProductionOpening;
                    ItemData.Consumer_Rate = item.Consumer_Rate;

                    ItemData.RetTaxPer_Act = item.RetTaxPer_Act;
                    ItemData.RetTaxPer_InAct = item.RetTaxPer_InAct;
                    ItemData.DistTaxPer_Act = item.DistTaxPer_Act;
                    ItemData.DistTaxPer_InAct = item.DistTaxPer_InAct;

                    ItemData.FEDTaxPer = item.FEDTaxPer;


                    if (item.ItemPic != null)
                    {
                        HttpContext.Current.Session["ItemPic"] = item.ItemPic;
                        var base64 = Convert.ToBase64String(item.ItemPic);
                        var img = string.Format("data:image/png;base64," + base64);
                        ItemData.ItemPic = img;
                    }
                    else
                    {
                        ItemData.ItemPic = "~/Content/plugins/images/noimage.png";
                    }

                    VMList.Add(ItemData);
                }

                return VMList;
            }
        }

        public List<SaleItem> GetSaleItemsWithCode()
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefintionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode).OrderBy(x => x.ItemCode).ToList();
                List<SaleItem> ItemList = new List<SaleItem>();
                foreach (var item in List)
                {
                    SaleItem I = new SaleItem();
                    I.ItemCode = item.ItemCode;
                    I.ItemDesc = (item.ItemCode + "  ==>  " + item.ItemDesc);
                    ItemList.Add(I);
                }

                return ItemList;
            }
        }

        public List<SaleItem> GetSaleItems()
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefintionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode).OrderBy(x => x.ItemCode).ToList();
                List<SaleItem> ItemList = new List<SaleItem>();
                foreach (var item in List)
                {
                    SaleItem I = new SaleItem();
                    I.ItemCode = item.ItemCode;
                    I.ItemDesc = item.ItemDesc;
                    I.Brand = item.Brand;
                    I.Category = item.Category;
                    I.SubCategory = item.SubCategory;
                    I.SaleGSTPer = item.SaleGSTPer;
                    I.SaleFurtherTaxPer = item.SaleFurtherTaxPer;
                    I.Consumer_Rate = item.Consumer_Rate;
                    ItemList.Add(I);
                }

                return ItemList;
            }
        }

        public string AddEditSaleItem(SaleItem Obj)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.ItemCode == null)
                {
                    var SaleItemCount = DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode).ToList();
                    if (SaleItemCount.Count == 0)
                    {
                        Obj.ItemCode = "001";
                    }
                    else
                    {
                        var Autogen = DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode).Select(x => x.ItemCode).OrderByDescending(x => x).FirstOrDefault();
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 3)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.ItemCode = Autogen;
                    }
                    Obj.CompanyCode = CompanyCode;
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdUser = CommonDAL.UserName();
                    Obj.UpdTerm = Environment.MachineName;
                    DefinitionContext.SaleItems.Add(Obj);
                    DefinitionContext.SaveChanges();
                    SuccessMsg = "Saved Successfully . . . !";
                }
                else
                {
                    DefinitionContext.SaleItems.Remove(DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == Obj.ItemCode).FirstOrDefault());

                    Obj.CompanyCode = CompanyCode;
                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdUser = CommonDAL.UserName();
                    Obj.UpdTerm = Environment.MachineName;
                    DefinitionContext.SaleItems.Add(Obj);
                    DefinitionContext.SaveChanges();
                    SuccessMsg = "Updated Successfully . . . !";
                }
            }
            return SuccessMsg;
        }

        #endregion

        #region(---------------------------------------------Sale Invoice ----------------------------------------------)

        public List<SaleInvoiceMaster> GetSaleInvoices()
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                return DefintionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.DelFlag == "N").ToList();
            }

        }

        public List<SaleInvoiceMasterVM> GetSaleInvoiceWithEmp(string DamageDate)
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(DamageDate, "dd/MM/yyyy", null);
              var List = DefintionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.ManInvoiceNo == "D-0000" && x.DelFlag == "N" && x.InvoiceDate == Date).ToList();

                List<SaleInvoiceMasterVM> SVM = new List<SaleInvoiceMasterVM>();

                foreach (var item in List)
                {
                    SaleInvoiceMasterVM New = new SaleInvoiceMasterVM();

                    New.InvoiceNo = item.InvoiceNo;
                    New.InvoiceDate = item.InvoiceDate.ToString("dd/MM/yyyy");
                    New.RepInvoiceNo = item.RepInvoiceNo;
                    New.ManInvoiceNo = item.ManInvoiceNo;
                    New.RegionCode = item.RegionCode;
                    New.RegionDesc = DefintionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    New.PartyCode = item.PartyCode;
                    New.PartyDesc = DefintionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    New.EmpName = DefintionContext.CompanyEmps.Where(x => x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();

                    SVM.Add(New);
                }

                return SVM;
            }

        }

        public List<SaleInvoiceMasterVM> GetSaleInvoiceCurrentDate()
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var Username = CommonDAL.UserName();
                DateTime Date = DateTime.Now.Date;
                var List = DefintionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.DelFlag == "N" && x.InvoiceDate == Date && x.UpdUser == Username).OrderByDescending(x=>x.InvoiceNo) .ToList();

                List<SaleInvoiceMasterVM> SVM = new List<SaleInvoiceMasterVM>();

                foreach (var item in List)
                {
                    SaleInvoiceMasterVM New = new SaleInvoiceMasterVM();

                    New.InvoiceNo = item.InvoiceNo;
                    New.InvoiceDate = item.InvoiceDate.ToString("dd/MM/yyyy");
                    New.RepInvoiceNo = item.RepInvoiceNo;
                    New.ManInvoiceNo = item.ManInvoiceNo;
                    New.RegionCode = item.RegionCode;
                    New.RegionDesc = DefintionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    New.PartyCode = item.PartyCode;
                    New.PartyDesc = DefintionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    New.EmpName = DefintionContext.CompanyEmps.Where(x => x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();

                    SVM.Add(New);
                }

                return SVM;
            }

        }

        public SaleInvoiceMasterVM  GetSingleMasterInvoice(int InvoiceNo)
        {
            using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var Found = DefinitionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.InvoiceNo == InvoiceNo).FirstOrDefault();
                SaleInvoiceMasterVM Obj = new SaleInvoiceMasterVM();

                Obj.InvoiceNo = Found.InvoiceNo;
                Obj.InvoiceDate = Found.InvoiceDate.ToString("dd/MM/yyyy");
                Obj.ManInvoiceNo = Found.ManInvoiceNo;
                Obj.RepInvoiceNo = Found.RepInvoiceNo;
                Obj.RegionCode = Found.RegionCode;
                Obj.RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == Found.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                Obj.PartyCode = Found.PartyCode;
                Obj.MainPartyCode = Found.MainPartyCode;
                Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == Found.RegionCode && x.PartyCode == Found.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                Obj.PartyStatus = Found.PartyStatus;
                Obj.TransType = Found.TransType;
                Obj.Category = DefinitionContext.SaleParties.Where(x => x.RegionCode == Found.RegionCode && x.PartyCode == Found.PartyCode).Select(x => x.Category).FirstOrDefault();
                Obj.ThirdSch_Category = DefinitionContext.SaleParties.Where(x => x.RegionCode == Found.RegionCode && x.PartyCode == Found.PartyCode).Select(x => x.ThirdSch_Category).FirstOrDefault();
                Obj.EmpCode = Found.EmpCode;
                Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == Found.RegionCode && x.EmpCode == Found.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                Obj.SaleTaxAmt = Found.SaleTaxAmt;
                Obj.Discount = Found.Discount;
                Obj.FurtherTaxAmt = Found.FurtherTaxAmt;
                Obj.IncTaxAmt = Found.IncTaxAmt;
                Obj.ExcTaxAmt = Found.ExcTaxAmt;
                Obj.TotalAmt = Found.IncTaxAmt;

                return Obj;
            }
        }

        public List<SaleInvoiceDetailVM> GetSingleDetailInvoice(int InvoiceNo)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var Found = DefinitionContext.SaleInvoiceDetails.Where(x => x.CompanyCode == CompanyCode && x.InvoiceNo == InvoiceNo).ToList();

                List<SaleInvoiceDetailVM> List = new List<SaleInvoiceDetailVM>();

                foreach (var item in Found)
                {

                    SaleInvoiceDetailVM Obj = new SaleInvoiceDetailVM();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == item.ItemCode).Select(x => x.ItemDesc).FirstOrDefault();
                    Obj.SaleQty = item.SaleQty;
                    Obj.Rate = item.Rate;
                    Obj.ConsumerRate = item.ConsumerRate;
                    Obj.Damage = item.Damage;
                    Obj.Replace = item.Replace;
                    Obj.FOC = item.FOC;
                    Obj.Sampling = item.Sampling;
                    Obj.Scheme = item.Scheme;
                    Obj.Retrn = item.Retrn;
                    Obj.WHT = item.WHT;
                    Obj.DiscPer = item.DiscPer;
                    Obj.DiscAmt = item.DiscAmt;
                    Obj.Amount = item.Amount;
                    Obj.FurtherTaxPer = item.FurtherTaxPer;
                    Obj.FurtherTaxAmt = item.FurtherTaxAmt;
                    Obj.IncTaxAmt = item.IncTaxAmt;
                    Obj.ExcTaxAmt = item.ExcTaxAmt;
                    Obj.SaleTaxPer = item.SaleTaxPer;
                    Obj.SaleTaxAmt = item.SaleTaxAmt;
                    Obj.IncomeTaxAmt = item.IncomeTaxAmt;
                    Obj.IncomeTaxPer = item.IncomeTaxPer;
                    Obj.FEDTaxPer = item.FEDTaxPer;
                    Obj.FEDTaxAmt = item.FEDTaxAmt;

                    List.Add(Obj);

                }

                return List;
            }
        }

        public bool CheckInvoiceDate(string ManInvoiceNo)
        {
            var Return = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime InvoiceDate = DefinitionContext.SaleInvoiceMasters.Where(x => x.ManInvoiceNo == ManInvoiceNo).Select(x => x.InvoiceDate).FirstOrDefault();

                string Date = DateTime.Now.ToString("yyyy-MM");
                string InvoiceDate2 = InvoiceDate.ToString("yyyy-MM");
                if (InvoiceDate2 == Date)
                {
                    Return = true;
                }
            }
            return Return;
        }

        public SaleInvoiceMasterVM GetSingleMasterManInvoice(string ManInvoiceNo)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var Found = DefinitionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.ManInvoiceNo == ManInvoiceNo).FirstOrDefault();
                SaleInvoiceMasterVM Obj = new SaleInvoiceMasterVM();

                if (Found != null)
                {

                    Obj.InvoiceNo = Found.InvoiceNo;
                    Obj.InvoiceDate = Found.InvoiceDate.ToString("dd/MM/yyyy");
                    Obj.ManInvoiceNo = Found.ManInvoiceNo;
                    Obj.RepInvoiceNo = Found.RepInvoiceNo;
                    Obj.RegionCode = Found.RegionCode;
                    Obj.RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == Found.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = Found.PartyCode;
                    Obj.MainPartyCode = Found.MainPartyCode;
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == Found.RegionCode && x.PartyCode == Found.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Obj.PartyStatus = Found.PartyStatus;
                    Obj.TransType = Found.TransType;
                    Obj.Category = DefinitionContext.SaleParties.Where(x => x.RegionCode == Found.RegionCode && x.PartyCode == Found.PartyCode).Select(x => x.Category).FirstOrDefault();
                    Obj.ThirdSch_Category = DefinitionContext.SaleParties.Where(x => x.RegionCode == Found.RegionCode && x.PartyCode == Found.PartyCode).Select(x => x.ThirdSch_Category).FirstOrDefault();
                    Obj.EmpCode = Found.EmpCode;
                    Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == Found.RegionCode && x.EmpCode == Found.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    Obj.SaleTaxAmt = Found.SaleTaxAmt;
                    Obj.Discount = Found.Discount;
                    Obj.FurtherTaxAmt = Found.FurtherTaxAmt;
                    Obj.IncTaxAmt = Found.IncTaxAmt;
                    Obj.ExcTaxAmt = Found.ExcTaxAmt;
                    Obj.TotalAmt = Found.IncTaxAmt;

                }
                return Obj;
            }
        }


        public double GetItemRateByManInvNo(string ManInvNo)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var Found = DefinitionContext.SaleInvoiceMasters.Where(x => x.ManInvoiceNo == ManInvNo).FirstOrDefault();




                return 0;
            }
        }

        public List<SaleInvoiceDetailVM> GetSingleDetailManInvoice(string ManInvoiceNo)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var Found = DefinitionContext.SaleInvoiceDetails.Where(x => x.CompanyCode == CompanyCode && x.ManInvoiceNo == ManInvoiceNo).ToList();
                List<SaleInvoiceDetailVM> List = new List<SaleInvoiceDetailVM>();
                if (Found != null)
                {
                    foreach (var item in Found)
                    {

                        SaleInvoiceDetailVM Obj = new SaleInvoiceDetailVM();
                        Obj.ItemCode = item.ItemCode;
                        Obj.ItemDesc = DefinitionContext.SaleItems.Where(x => x.CompanyCode == CompanyCode && x.ItemCode == item.ItemCode).Select(x => x.ItemDesc).FirstOrDefault();
                        Obj.SaleQty = item.SaleQty;
                        Obj.Rate = item.Rate;
                        Obj.ConsumerRate = item.ConsumerRate;
                        Obj.Damage = item.Damage;
                        Obj.Replace = item.Replace;
                        Obj.FOC = item.FOC;
                        Obj.Sampling = item.Sampling;
                        Obj.Scheme = item.Scheme;
                        Obj.Retrn = item.Retrn;
                        Obj.WHT = item.WHT;
                        Obj.DiscPer = item.DiscPer;
                        Obj.DiscAmt = item.DiscAmt;
                        Obj.Amount = item.Amount;
                        Obj.FurtherTaxPer = item.FurtherTaxPer;
                        Obj.FurtherTaxAmt = item.FurtherTaxAmt;
                        Obj.IncTaxAmt = item.IncTaxAmt;
                        Obj.ExcTaxAmt = item.ExcTaxAmt;
                        Obj.SaleTaxPer = item.SaleTaxPer;
                        Obj.SaleTaxAmt = item.SaleTaxAmt;
                        Obj.IncomeTaxAmt = item.IncomeTaxAmt;
                        Obj.IncomeTaxPer = item.IncomeTaxPer;

                        Obj.FEDTaxAmt = item.FEDTaxAmt;
                        Obj.FEDTaxPer = item.FEDTaxPer;

                        List.Add(Obj);

                    }
                }
                return List;
            }
        }
        public string AddEditSaleInvoice(SaleInvoiceMaster Obj,string InvoiceDate)
        {
            string UpdUser = CommonDAL.UserName();
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.InvoiceNo == 0)
                {
                    //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                    //{
                            try
                            {

                            //var FoundSaleInv = DefinitionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode).ToList();
                            //if (FoundSaleInv.Count == 0)
                            //{
                            //    Obj.InvoiceNo = "19/0000001";
                            //}
                            //else
                            //{

                            //    var Autogen = DefinitionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode).Select(x => x.InvoiceNo).OrderByDescending(x => x).FirstOrDefault();
                            //    string date = DateTime.Now.ToString("yy");
                            //    string year = date + "/";
                            //    Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 7)) + 1).ToString();
                            //    while (Autogen.Length != 7)
                            //    {
                            //        Autogen = '0' + Autogen;
                            //    }
                            //    Obj.InvoiceNo = year + Autogen;
                            //}

                            if (Obj.ManInvoiceNo == null)
                            {
                                Obj.ManInvoiceNo = "D-0000";
                            }

                            var List = HttpContext.Current.Session["SaleInvoiceDetail"] as SaleInvoiceDetail[];

                            Obj.VoucherNo = AddCRV(InvoiceDate, 
                                (double)Obj.IncTaxAmt, 
                                (double)List.AsEnumerable().Sum(x => x.SaleTaxAmt), 
                                (double)List.AsEnumerable().Sum(x => x.FurtherTaxAmt),
                                (double)List.AsEnumerable().Sum(x => x.IncomeTaxAmt),
                                (double)List.AsEnumerable().Sum(x => x.FEDTaxAmt),
                                Obj.ManInvoiceNo);

                            Obj.CompanyCode = "00001";
                            //Obj.InvoiceDate = Obj.InvoiceDate.Date;
                            Obj.InvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", null);

                            //Obj.InvoiceDate = DateTime.Now;
                            Obj.DelFlag = "N";
                            Obj.PostFlag = "N";
                            Obj.ExemptTax = "N";
                            Obj.TotalAmt = Obj.IncTaxAmt;
                            Obj.UpdDate = DateTime.Now;
                            Obj.UpdTerm = Environment.MachineName;
                            Obj.UpdUser = CommonDAL.UserName();
                            HttpContext.Current.Session["EmpCode"] = Obj.EmpCode;
                            HttpContext.Current.Session["RegionCode"] = Obj.RegionCode;
                            HttpContext.Current.Session["InvoiceDate"] = InvoiceDate;
                            HttpContext.Current.Session["MasterData"] = Obj as SaleInvoiceMaster;
                            DefinitionContext.SaleInvoiceMasters.Add(Obj);
                            //DefinitionContext.SaveChanges();

                            
                            List<SaleInvoiceDetail> DetailList = new List<SaleInvoiceDetail>();
                            foreach (var item in List)
                            {
                                SaleInvoiceDetail NewObj = new SaleInvoiceDetail();
                                NewObj.CompanyCode = "00001";
                                NewObj.InvoiceNo = Obj.InvoiceNo;
                                //NewObj.InvoiceDate = Obj.InvoiceDate.Date;
                                NewObj.InvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", null);
                                //NewObj.InvoiceDate = DateTime.Now;
                                NewObj.ManInvoiceNo = Obj.ManInvoiceNo;
                                NewObj.RepInvoiceNo = Obj.RepInvoiceNo;
                                NewObj.PartyCode = Obj.PartyCode;
                                NewObj.TransType = Obj.TransType;
                                NewObj.MainPartyCode = Obj.MainPartyCode;
                                NewObj.ItemCode = item.ItemCode;
                                NewObj.Rate = item.Rate;
                                NewObj.SaleQty = item.SaleQty;
                                NewObj.Amount = item.Amount;
                                NewObj.DiscAmt = item.DiscAmt;
                                NewObj.DiscPer = item.DiscPer;
                                NewObj.SaleTaxAmt = item.SaleTaxAmt;
                                NewObj.SaleTaxPer = item.SaleTaxPer;
                                NewObj.ExcTaxAmt = item.ExcTaxAmt;
                                NewObj.IncTaxAmt = item.IncTaxAmt;
                                NewObj.IncomeTaxAmt = item.IncomeTaxAmt;
                                NewObj.IncomeTaxPer = item.IncomeTaxPer;
                                NewObj.ConsumerRate = item.ConsumerRate;

                                NewObj.FEDTaxAmt = item.FEDTaxAmt;
                                NewObj.FEDTaxPer = item.FEDTaxPer; 

                                NewObj.Replace = item.Replace;
                                NewObj.Retrn = item.Retrn;
                                NewObj.Damage = item.Damage;
                                NewObj.FOC = item.FOC;
                                NewObj.Sampling = item.Sampling;
                                NewObj.Scheme = item.Scheme;
                                NewObj.WHT = item.WHT;

                                NewObj.FurtherTaxAmt = item.FurtherTaxAmt;
                                NewObj.FurtherTaxPer = item.FurtherTaxPer;
                                DetailList.Add(NewObj);
                            }

                            HttpContext.Current.Session["DetailData"] = DetailList as List<SaleInvoiceDetail>;
                            DefinitionContext.SaleInvoiceDetails.AddRange(DetailList);
                            DefinitionContext.SaveChanges();
                            //ts.Complete();
                            
                            SuccessMsg = "Saved Successfully. . . !";
                          
                            }
                            catch (Exception ex)
                            {

                                

                                //if (ex is SqlException || ex is Exception)
                                //{
                                        //ts.Dispose();
                            throw ex;
                                //return "DeadLock";
                                //}
                            }

                       

                    //}

                }
                else
                {

                    //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                    //{
                       
                        try
                            {

                            DefinitionContext.SaleInvoiceDetails.RemoveRange(DefinitionContext.SaleInvoiceDetails.Where(x => x.CompanyCode == CompanyCode && x.InvoiceNo == Obj.InvoiceNo).ToList());
                            DefinitionContext.SaleInvoiceMasters.Remove(DefinitionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.InvoiceNo == Obj.InvoiceNo).FirstOrDefault());

                            if (Obj.ManInvoiceNo == null)
                            {
                                Obj.ManInvoiceNo = "D-0000";
                            }

                            var List = HttpContext.Current.Session["SaleInvoiceDetail"] as SaleInvoiceDetail[];

                            Obj.VoucherNo = AddCRV(InvoiceDate, 
                                (double)Obj.IncTaxAmt, 
                                (double)List.AsEnumerable().Sum(x => x.SaleTaxAmt), 
                                (double)List.AsEnumerable().Sum(x => x.FurtherTaxAmt),
                                (double)List.AsEnumerable().Sum(x => x.IncomeTaxAmt),
                                (double)List.AsEnumerable().Sum(x => x.FEDTaxAmt),
                                Obj.ManInvoiceNo);
                            Obj.CompanyCode = CompanyCode;
                            Obj.DelFlag = "N";
                            Obj.PostFlag = "N";
                            Obj.ExemptTax = "N";
                            //Obj.InvoiceDate =(DateTime) Obj.InvoiceDate;
                            Obj.InvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", null);
                            Obj.TotalAmt = Obj.IncTaxAmt;
                            Obj.UpdDate = DateTime.Now;
                            Obj.UpdTerm = Environment.MachineName;
                            Obj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.SaleInvoiceMasters.Add(Obj);
                            DefinitionContext.SaveChanges();

                            

                            foreach (var item in List)
                            {
                                SaleInvoiceDetail NewObj = new SaleInvoiceDetail();
                                NewObj.CompanyCode = CompanyCode;
                                NewObj.InvoiceNo = Obj.InvoiceNo;
                                //NewObj.InvoiceDate = Obj.InvoiceDate;
                                //NewObj.InvoiceDate =(DateTime) Obj.InvoiceDate;
                                NewObj.InvoiceDate = DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", null);
                                NewObj.ManInvoiceNo = Obj.ManInvoiceNo;
                                NewObj.RepInvoiceNo = Obj.RepInvoiceNo;
                                NewObj.PartyCode = Obj.PartyCode;
                                NewObj.TransType = Obj.TransType;
                                NewObj.MainPartyCode = Obj.MainPartyCode;
                                NewObj.ItemCode = item.ItemCode;
                                NewObj.Rate = item.Rate;
                                NewObj.SaleQty = item.SaleQty;
                                NewObj.Amount = item.Amount;
                                NewObj.DiscAmt = item.DiscAmt;
                                NewObj.DiscPer = item.DiscPer;
                                NewObj.SaleTaxAmt = item.SaleTaxAmt;
                                NewObj.SaleTaxPer = item.SaleTaxPer;
                                NewObj.ExcTaxAmt = item.ExcTaxAmt;
                                NewObj.IncTaxAmt = item.IncTaxAmt;
                                NewObj.IncomeTaxAmt = item.IncomeTaxAmt;
                                NewObj.IncomeTaxPer = item.IncomeTaxPer;
                                NewObj.ConsumerRate = item.ConsumerRate;

                                NewObj.FEDTaxAmt = item.FEDTaxAmt;
                                NewObj.FEDTaxPer = item.FEDTaxPer;

                                NewObj.FurtherTaxAmt = item.FurtherTaxAmt;
                                NewObj.FurtherTaxPer = item.FurtherTaxPer;

                                NewObj.Replace = item.Replace;
                                NewObj.Retrn = item.Retrn;
                                NewObj.Damage = item.Damage;
                                NewObj.FOC = item.FOC;
                                NewObj.Sampling = item.Sampling;
                                NewObj.Scheme = item.Scheme;
                                NewObj.WHT = item.WHT;

                                DefinitionContext.SaleInvoiceDetails.Add(NewObj);
                                DefinitionContext.SaveChanges();
                                

                            }
                            
                            SuccessMsg = "Updated Successfully . . . !";
                            //ts.Complete();
                            }
                            catch (Exception ex)
                            {
                                //ts.Dispose();
                                throw ex;
                            }
                           
                      
                    //}
                }
            }

            return SuccessMsg;
        }


        public string AddCRV(string InvDate,double SaleAmount,double SaleTax,double FurtherTax,double IncomeTaxAmt,double FEDTaxAmt, string InvNos)
        {

            using (FAS_BlazorEntities FContext = new FAS_BlazorEntities())
            {
                DateTime IDate = DateTime.ParseExact(InvDate, "dd/MM/yyyy", null);

                CRV_Masters CObj = new CRV_Masters();

                string FindVoucher = FContext.CRV_Masters.Where(x => x.CreatedDate.Month == DateTime.Now.Month && x.Voucher_Ref_No.Length >= 16).OrderByDescending(x => x.CRV_No).Select(x => x.Voucher_Ref_No).FirstOrDefault();

                if (FindVoucher == null)
                {
                    CObj.Voucher_Ref_No = "CRV_INV_" + DateTime.Now.ToString("MMyyyy") + "_" + 1;
                }
                else
                {
                    var New = FindVoucher.Split('_');
                    int Code = Convert.ToInt32(New[3]);
                    Code += 1;
                    CObj.Voucher_Ref_No = "CRV_INV_" + DateTime.Now.ToString("MMyyyy") + "_" + Code;
                }

                CObj.CRV_Date = IDate;
                CObj.MainDescription = "Sale Invoice Related Vouchers..." + "ManInvoiceNo=" + InvNos;
                CObj.CreatedBy = CommonDAL.UserName();
                CObj.CreatedDate = DateTime.Now;
                CObj.UpdBy = CommonDAL.UserName();
                CObj.UpdDate = DateTime.Now;

                CRV_Details Obj1 = new CRV_Details();
                Obj1.Voucher_Ref_No = CObj.Voucher_Ref_No;
                Obj1.Detail_Acc_ChildCode = 1300000;
                Obj1.Debit = SaleAmount;
                Obj1.Credit = 0;
                Obj1.Description = "Debtor Debit Amount";
                CObj.CRV_Details.Add(Obj1);

                CRV_Details Obj2 = new CRV_Details();
                Obj2.Voucher_Ref_No = CObj.Voucher_Ref_No;
                Obj2.Detail_Acc_ChildCode = 1200000;
                Obj2.Debit = 0;
                Obj2.Credit = SaleAmount - (SaleTax + FurtherTax + IncomeTaxAmt + FEDTaxAmt);
                Obj2.Description = "Sale Credit Amount";
                CObj.CRV_Details.Add(Obj2);

                CRV_Details Obj3 = new CRV_Details();
                Obj3.Voucher_Ref_No = CObj.Voucher_Ref_No;
                Obj3.Detail_Acc_ChildCode = 1400000;
                Obj3.Debit = 0;
                Obj3.Credit = SaleTax;
                Obj3.Description = "Sale Tax Amount";
                CObj.CRV_Details.Add(Obj3);

                CRV_Details Obj4 = new CRV_Details();
                Obj4.Voucher_Ref_No = CObj.Voucher_Ref_No;
                Obj4.Detail_Acc_ChildCode = 1500000;
                Obj4.Debit = 0;
                Obj4.Credit = FurtherTax;
                Obj4.Description = "Further Tax Amount";
                CObj.CRV_Details.Add(Obj4);

                CRV_Details Obj5 = new CRV_Details();
                Obj5.Voucher_Ref_No = CObj.Voucher_Ref_No;
                Obj5.Detail_Acc_ChildCode = 1800000;
                Obj5.Debit = 0;
                Obj5.Credit = IncomeTaxAmt;
                Obj5.Description = "Advance Income Tax Amount";
                CObj.CRV_Details.Add(Obj5);

                CRV_Details Obj6 = new CRV_Details();
                Obj6.Voucher_Ref_No = CObj.Voucher_Ref_No;
                Obj6.Detail_Acc_ChildCode = 1700000;
                Obj6.Debit = 0;
                Obj6.Credit = FEDTaxAmt;
                Obj6.Description = "FED Tax Amount";
                CObj.CRV_Details.Add(Obj6);


                CObj.CRV_TotalDebit = Obj1.Debit + Obj2.Debit + Obj3.Debit + Obj4.Debit + Obj5.Debit + Obj6.Debit; 
                CObj.CRV_TotalCredit = Obj1.Credit + Obj2.Credit + Obj3.Credit + Obj4.Credit + Obj5.Credit + Obj6.Credit;

                FContext.Entry(CObj).State = System.Data.Entity.EntityState.Added;
                FContext.SaveChanges();

                return CObj.Voucher_Ref_No;
            }
        }

        public string AddEditSaleInvoiceAgain()
        {
            string SuccessMsg = "";
           
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    Thread.Sleep(2000);

                    SaleInvoiceMaster InvoiceMas = HttpContext.Current.Session["MasterData"] as SaleInvoiceMaster;
                    List<SaleInvoiceDetail> InvoiceDet = HttpContext.Current.Session["DetailData"] as List<SaleInvoiceDetail>;
                   
                    try
                    {
                   
                        //var Autogen = DefinitionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode).Select(x => x.InvoiceNo).OrderByDescending(x => x).FirstOrDefault();
                        //string date = DateTime.Now.ToString("yy");
                        //string year = date + "/";
                        //Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 7)) + 1).ToString();
                        //while (Autogen.Length != 7)
                        //{
                        //    Autogen = '0' + Autogen;
                        //}
                        //InvoiceMas.InvoiceNo = year + Autogen;
                      
                        //DefinitionContext.SaleInvoiceMasters.Add(InvoiceMas);
                      
                        //foreach (var item in InvoiceDet)
                        //{
                        //    item.InvoiceNo = InvoiceMas.InvoiceNo;
                        //}
                      
                        //DefinitionContext.SaleInvoiceDetails.AddRange(InvoiceDet);
                        //DefinitionContext.SaveChanges();
                        //ts.Complete();

                        //var Found = DefinitionContext.SaleInvoiceMasters.Where(x => x.InvoiceNo == InvoiceMas.InvoiceNo).FirstOrDefault();
                        //if (Found == null)
                        //{
                        //    SuccessMsg = "Not Saved Try Again...!";
                        //}
                        //else
                        //{
                            SuccessMsg = "Saved Successfully. . . !";
                        //}


                    }
                    catch (Exception ex)
                    {
                   
                        if (ex is SqlException || ex is Exception)
                        {
                            ts.Dispose();
                            return "DeadLock Occur, Try Again . . . !";
                        }
                    }

                }
              
            }

            return SuccessMsg;
        }

        public bool CheckManInvoice(string ManInvoiceNo)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                string FindInvoice = DefinitionContext.SaleInvoiceMasters.Where(x => x.ManInvoiceNo == ManInvoiceNo).Select(x => x.ManInvoiceNo).FirstOrDefault();

                if (FindInvoice != null)
                {
                    SuccessReg = true;
                }
                return SuccessReg;
            }
        }

        public bool CheckPartyDiscount(string RegionCode,string PartyCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                string FindDiscount = DefinitionContext.Discounts.Where(x => x.RegionCode == RegionCode && x.PartyCode == PartyCode).Select(x => x.PdCode).FirstOrDefault();

                if (FindDiscount != null)
                {
                    SuccessReg = true;
                }
                return SuccessReg;
            }
        }


        public bool DeleteInvoice(int InvoiceNo)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                SaleInvoiceMaster FindInvoice = DefinitionContext.SaleInvoiceMasters.Where(x => x.InvoiceNo == InvoiceNo).FirstOrDefault();


                DefinitionContext.SaleInvoiceMasters.Remove(DefinitionContext.SaleInvoiceMasters.Where(x => x.InvoiceNo == InvoiceNo).FirstOrDefault());
                DefinitionContext.SaleInvoiceDetails.RemoveRange(DefinitionContext.SaleInvoiceDetails.Where(x=>x.InvoiceNo == InvoiceNo).ToList());
                DefinitionContext.SaveChanges();

                //SaleInvoiceMaster SIM = new SaleInvoiceMaster();

                //  FindInvoice.DelFlag = "Y";
                //  FindInvoice.UpdDate = DateTime.Now;
                //  FindInvoice.UpdTerm = Environment.MachineName;
                //  FindInvoice.UpdUser = CommonDAL.UserName();

                //DefinitionContext.SaleInvoiceMasters.Add(FindInvoice);
                //DefinitionContext.SaveChanges();


                //var FoundDeleteReg = DefinitionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.InvoiceNo == InvoiceNo).Select(x => x.DelFlag).FirstOrDefault();
                //if (FoundDeleteReg == "Y")
                //{
                //    SuccessReg = true;
                //}
                SuccessReg = true;

                return SuccessReg;
            }
        }




        #endregion

        #region ( ------------------------------------------------Recovery Against Sale-----------------------------------------------)

        public List<RecoveryVM> GetAllRecoveryList(string GetRecDate)
        {
            using(AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(GetRecDate, "dd/MM/yyyy", null);
                var OldList = DefinitionContext.GetDistinctRecovery(CompanyCode).Where(x=>x.RecoveryDate == Date).ToList();
                List<RecoveryVM> NewList = new List<RecoveryVM>();
                foreach (var item in OldList)
                {
                    RecoveryVM Obj = new RecoveryVM();
                    Obj.RecoveryNo = item.RecoveryNo;
                    Obj.RecoveryDate = item.RecoveryDate.ToString("dd/MM/yyyy");
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public List<RecoveryVM> GetSingleRecovery(int RecoveryNo)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.Recoveries.Where(x => x.CompanyCode == CompanyCode && x.RecoveryNo == RecoveryNo).ToList();
                
                List<RecoveryVM> NewList = new List<RecoveryVM>();

                foreach (var item in OldList)
                {
                    RecoveryVM Obj = new RecoveryVM();
                    Obj.RecoveryNo = RecoveryNo;
                    Obj.RecoveryDate = item.RecoveryDate.ToString("dd/MM/yyyy");
                    Obj.InvoiceNo = item.InvoiceNo;
                    Obj.Type = item.Type;
                    Obj.Remarks = item.Remarks;
                    Obj.ManInvoiceNo = item.ManInvoiceNo;
                    Obj.RepInvoiceNo = item.RepInvoiceNo;
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.MainPartyCode = item.MainPartyCode;
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    //Obj.PartyType = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode && x.PartyCode == Obj.PartyCode).Select(x => x.PartyType).FirstOrDefault();
                    //Obj.STaxReg = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == Obj.RegionCode && x.PartyCode == Obj.PartyCode).Select(x => x.STaxReg).FirstOrDefault();
                    Obj.EmpCode = item.EmpCode;
                    Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    //Obj.ItemCode = item.ItemCode;
                    //Obj.ItemDesc = item.ItemDesc;
                    Obj.CheqNo = item.CheqNo;
                    Obj.CheqDate = item.CheqDate.ToString();
                    Obj.AccountCode = item.AccountCode;

                    var RecAmt = item.RecAmount;
                    var RecDis = item.RecDiscount;
                    var RecWHT = item.RecWHT;

                    if (RecAmt != null)
                    {
                        Obj.RecAmount = RecAmt;
                    }
                    else
                    {
                        Obj.RecAmount = 0;
                    }

                    if (RecDis != null)
                    {
                        Obj.RecDiscount = RecDis;
                    }
                    else
                    {
                        Obj.RecDiscount = 0;
                    }

                    if (RecWHT != null)
                    {
                        Obj.RecWHT = RecWHT;
                    }
                    else
                    {
                        Obj.RecWHT = 0;
                    }

                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public bool CheckRecoveryCheqNo(string CheqNo)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                int FindInvoice = DefinitionContext.Recoveries.Where(x => x.CheqNo == CheqNo).Select(x => x.RecoveryNo).FirstOrDefault();

                if (FindInvoice != 0)
                {
                    SuccessReg = true;
                }
                return SuccessReg;
            }
        }

        public List<SaleInvoiceMaster> GetCreditSaleInvoices()
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                return DefintionContext.SaleInvoiceMasters.Where(x => x.CompanyCode == CompanyCode && x.DelFlag == "N" && x.TransType == "Credit").ToList();
            }

        }

        public string AddEditRecovery(Recovery Obj,string RecoveryDate,string CheqDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.RecoveryNo == 0)
                {
                    //var FoundRecovery = DefinitionContext.Recoveries.Where(x => x.CompanyCode == CompanyCode).ToList();
                    //if (FoundRecovery.Count == 0)
                    //{
                    //    Obj.RecoveryNo = "00000001";
                    //}
                    //else
                    //{

                    //    var Autogen = DefinitionContext.Recoveries.Where(x => x.CompanyCode == CompanyCode).Select(x => x.RecoveryNo).OrderByDescending(x => x).FirstOrDefault();
                       
                    //    Autogen = (Convert.ToInt64(Autogen) + 1).ToString();
                    //    while (Autogen.Length != 8)
                    //    {
                    //        Autogen = '0' + Autogen;
                    //    }
                    //    Obj.RecoveryNo =  Autogen;
                    //}

                    HttpContext.Current.Session["SessRecDate"] = RecoveryDate;
                   

                    var List = HttpContext.Current.Session["RecoveryDetail"] as Recovery[];



                    foreach (var item in List)
                    {
                        if (item.RecAmount != 0)
                        {
                            Recovery NewObj = new Recovery(); 
                            NewObj.CompanyCode = CompanyCode;
                            //NewObj.RecoveryNo = Obj.RecoveryNo;
                            NewObj.RecoveryDate = DateTime.ParseExact(RecoveryDate, "dd/MM/yyyy", null);
                            NewObj.Type = Obj.Type;
                            NewObj.Remarks = Obj.Remarks;
                            NewObj.InvoiceNo = Obj.InvoiceNo;
                            NewObj.ManInvoiceNo = Obj.ManInvoiceNo;
                            NewObj.RepInvoiceNo = Obj.RepInvoiceNo;
                            NewObj.CheqNo = Obj.CheqNo;
                            NewObj.CheqDate = (CheqDate != null) ? DateTime.ParseExact(CheqDate, "dd/MM/yyyy", null) : Obj.CheqDate;
                            NewObj.AccountCode = Obj.AccountCode;
                            NewObj.RegionCode = item.RegionCode;
                            NewObj.ItemCode = "001";
                            NewObj.PartyCode = item.PartyCode;
                            NewObj.MainPartyCode = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.MainPartyCode).FirstOrDefault();
                            NewObj.EmpCode = item.EmpCode;
                            NewObj.RecAmount = item.RecAmount;
                            NewObj.RecWHT = item.RecWHT;
                            NewObj.RecDiscount = item.RecDiscount;
                            NewObj.DelFlag = "N";
                            NewObj.PostFlag = "Y";
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.Recoveries.Add(NewObj);
                            DefinitionContext.SaveChanges();

                            HttpContext.Current.Session["SessRecRegionCode"] = item.RegionCode;
                            HttpContext.Current.Session["SessRecEmpCode"] = item.EmpCode;
                        }
                    }
                   
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.Recoveries.RemoveRange(DefinitionContext.Recoveries.Where(x => x.CompanyCode == CompanyCode && x.RecoveryNo == Obj.RecoveryNo).ToList());

                    HttpContext.Current.Session["SessRecDate"] = RecoveryDate;
                    

                    var List = HttpContext.Current.Session["RecoveryDetail"] as Recovery[];

                    foreach (var item in List)
                    {
                        if (item.RecAmount != 0)
                        {
                            Recovery NewObj = new Recovery();
                            NewObj.CompanyCode = CompanyCode;
                            //NewObj.RecoveryNo = Obj.RecoveryNo;
                            NewObj.RecoveryDate = DateTime.ParseExact(RecoveryDate, "dd/MM/yyyy", null);
                            NewObj.Type = Obj.Type;
                            NewObj.Remarks = Obj.Remarks;
                            NewObj.InvoiceNo = Obj.InvoiceNo;
                            NewObj.ManInvoiceNo = Obj.ManInvoiceNo;
                            NewObj.RepInvoiceNo = Obj.RepInvoiceNo;
                            NewObj.CheqNo = Obj.CheqNo;
                            NewObj.CheqDate = (CheqDate != null) ? DateTime.ParseExact(CheqDate, "dd/MM/yyyy", null) : Obj.CheqDate;
                            NewObj.AccountCode = Obj.AccountCode;
                            NewObj.RegionCode = item.RegionCode;
                            NewObj.ItemCode = "001";
                            NewObj.PartyCode = item.PartyCode;
                            NewObj.MainPartyCode = DefinitionContext.SaleParties.Where(x => x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.MainPartyCode).FirstOrDefault();
                            NewObj.EmpCode = item.EmpCode;
                            NewObj.RecAmount = item.RecAmount;
                            NewObj.RecWHT = item.RecWHT;
                            NewObj.RecDiscount = item.RecDiscount;
                            NewObj.DelFlag = "N";
                            NewObj.PostFlag = "Y";
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.Recoveries.Add(NewObj);
                            DefinitionContext.SaveChanges();

                            HttpContext.Current.Session["SessRecRegionCode"] = item.RegionCode;
                            HttpContext.Current.Session["SessRecEmpCode"] = item.EmpCode;
                        }
                    }
                    
                    SuccessMsg = "Updated Successfully. . . !";
                }
            }

            return SuccessMsg;
        }


        public bool DeleteRecovery(int RecoveryNo)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var FindRecovery = DefinitionContext.Recoveries.Where(x => x.CompanyCode == CompanyCode && x.RecoveryNo == RecoveryNo).ToList();

                DefinitionContext.Recoveries.RemoveRange(DefinitionContext.Recoveries.Where(x => x.CompanyCode == CompanyCode && x.RecoveryNo == RecoveryNo).ToList());
                DefinitionContext.SaveChanges();

                //foreach (var item in FindRecovery)
                //{
                //    Recovery NewObj = new Recovery();
                //    NewObj.CompanyCode = CompanyCode;
                //    NewObj.RecoveryNo = item.RecoveryNo;
                //    NewObj.RecoveryDate = item.RecoveryDate;
                //    NewObj.InvoiceNo = item.InvoiceNo;
                //    NewObj.ManInvoiceNo = item.ManInvoiceNo;
                //    NewObj.RepInvoiceNo = item.RepInvoiceNo;
                //    NewObj.CheqNo = item.CheqNo;
                //    NewObj.RegionCode = item.RegionCode;
                //    NewObj.ItemCode = item.ItemCode.Trim();
                //    NewObj.PartyCode = item.PartyCode;
                //    NewObj.EmpCode = item.EmpCode;
                //    NewObj.RecAmount = item.RecAmount;
                //    NewObj.RecWHT = item.RecWHT;
                //    NewObj.RecDiscount = item.RecDiscount;
                //    NewObj.DelFlag = "Y";
                //    NewObj.PostFlag = "Y";
                //    NewObj.UpdDate = DateTime.Now;
                //    NewObj.UpdTerm = Environment.MachineName;
                //    NewObj.UpdUser = CommonDAL.UserName();
                //    DefinitionContext.Recoveries.Add(NewObj);
                //    DefinitionContext.SaveChanges();
                //}
              
                //var FoundDeleteReg = DefinitionContext.Recoveries.Where(x => x.CompanyCode == CompanyCode && x.RecoveryNo == RecoveryNo).Select(x => x.DelFlag).FirstOrDefault();
                //if (FoundDeleteReg == "Y")
                //{
                    SuccessReg = true;
                //}

                return SuccessReg;
            }
        }

        #endregion


      


        #region(-------------------------------------------------- DAILY PRODUCTION ----------------------------------------------------)


        public List<DailyProductionVM> GetDinstinctDP()
        {
            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefintionContext.DistinctDPCode().ToList();
                List<DailyProductionVM> NewList = new List<DailyProductionVM>();

                foreach (var item in List)
                {
                    DailyProductionVM Obj = new DailyProductionVM();
                    Obj.DPCode = item;
                    Obj.DPDate = DefintionContext.DailyProductions.Where(x => x.DPCode == item).Select(x => x.DPDate).FirstOrDefault();
                    NewList.Add(Obj);
                }

                return NewList;
            }
        }

        public List<DailyProductionVM> GetSingleDPCode(string DPCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.DailyProductions.Where(x =>  x.DPCode == DPCode).ToList();
                var ItemList = DefinitionContext.SaleItems.ToList();
                List<DailyProductionVM> NewList = new List<DailyProductionVM>();

                foreach (var item in ItemList)
                {
                    DailyProductionVM Obj = new DailyProductionVM();
                    Obj.DPCode = DPCode;
                    Obj.DPDate = DefinitionContext.DailyProductions.Where(x => x.DPCode == DPCode).Select(x => x.DPDate).FirstOrDefault();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = item.ItemDesc;
                    var Quantity = DefinitionContext.DailyProductions.Where(x => x.DPCode == DPCode && x.ItemCode == Obj.ItemCode).Select(x => x.Quantity).FirstOrDefault();
                    if (Quantity != null)
                    {
                        Obj.Quantity = Quantity;
                    }
                    else
                    {
                        Obj.Quantity = 0;
                    }
                    
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }


        public string AddEditDailyProduction(DailyProduction Obj,string DPDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.DPCode == null)
                {
                    var FoundDailyProduction = DefinitionContext.DailyProductions.ToList();
                    if (FoundDailyProduction.Count == 0)
                    {
                        Obj.DPCode = "DP-00001";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.DailyProductions.Select(x => x.DPCode).OrderByDescending(x => x).FirstOrDefault();
                        string date = "DP-";
                       
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.DPCode = date + Autogen;
                    }

                    var List = HttpContext.Current.Session["DailyProductionData"] as DailyProduction[];

                    foreach (var item in List)
                    {
                        if (item.Quantity != 0)
                        {
                            DailyProduction NewObj = new DailyProduction();
                            NewObj.DPCode = Obj.DPCode;
                            NewObj.DPDate = DateTime.ParseExact(DPDate, "dd/MM/yyyy", null);
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.Quantity;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyProductions.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.DailyProductions.RemoveRange(DefinitionContext.DailyProductions.Where(x => x.DPCode == Obj.DPCode).ToList());

                    var List = HttpContext.Current.Session["DailyProductionData"] as DailyProduction[];

                    foreach (var item in List)
                    {
                        if (item.Quantity != 0)
                        {
                            DailyProduction NewObj = new DailyProduction();
                            NewObj.DPCode = Obj.DPCode;
                            NewObj.DPDate = DateTime.ParseExact(DPDate, "dd/MM/yyyy", null);
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.Quantity;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyProductions.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully. . . !";
                }
            }

            return SuccessMsg;
        }



        public bool DeleteDailyProduction(string DPCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                DefinitionContext.DailyProductions.RemoveRange(DefinitionContext.DailyProductions.Where(x => x.DPCode == DPCode).ToList());
                DefinitionContext.SaveChanges();


                SuccessReg = true;

                return SuccessReg;
            }
        }

        #endregion


        #region (-----------------------------------------------------DailyDispatch---------------------------------------------)


        public string AddEditDailyDispatch(DailyDispatch Obj,string DispatchDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.DispatchCode == null)
                {
                    var FoundDailyDispatch = DefinitionContext.DailyDispatches.ToList();
                    if (FoundDailyDispatch.Count == 0)
                    {
                        Obj.DispatchCode = "DD-00001";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.DailyDispatches.Select(x => x.DispatchCode).OrderByDescending(x => x).FirstOrDefault();
                        string date = "DD-";

                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.DispatchCode = date + Autogen;
                    }

                    var List = HttpContext.Current.Session["DailyDispatchData"] as DailyDispatch[];

                    foreach (var item in List)
                    {
                        if (item.DispatchQty != 0)
                        {
                            DailyDispatch NewObj = new DailyDispatch();

                            NewObj.DispatchCode = Obj.DispatchCode;
                            NewObj.DispatchDate = DateTime.ParseExact(DispatchDate, "dd/MM/yyyy", null); 
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.VehicleCode = Obj.VehicleCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.DispatchQty = item.DispatchQty;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyDispatches.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.DailyDispatches.RemoveRange(DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == Obj.DispatchCode).ToList());

                    var List = HttpContext.Current.Session["DailyDispatchData"] as DailyDispatch[];

                    foreach (var item in List)
                    {
                        if (item.DispatchQty != 0)
                        {
                            DailyDispatch NewObj = new DailyDispatch();

                            NewObj.DispatchCode = Obj.DispatchCode;
                            NewObj.DispatchDate = DateTime.ParseExact(DispatchDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.VehicleCode = Obj.VehicleCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.DispatchQty = item.DispatchQty;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyDispatches.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully. . . !";
                }
            }

            return SuccessMsg;
        }

        public List<DailyDispatchVM> GetSingleDDCode(string DDCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                //var OldList = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == DDCode).ToList();
                var ItemList = DefinitionContext.SaleItems.ToList();
                List<DailyDispatchVM> NewList = new List<DailyDispatchVM>();

                foreach (var item in ItemList)
                {
                    DailyDispatchVM Obj = new DailyDispatchVM();
                    Obj.DispatchCode = DDCode;
                    Obj.DispatchDate = DefinitionContext.DailyDispatches.Where(x=>x.DispatchCode == DDCode).Select(x=>x.DispatchDate).FirstOrDefault();
                    Obj.RegionCode = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == DDCode).Select(x => x.RegionCode).FirstOrDefault();
                    Obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == Obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.VehicleCode = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == DDCode).Select(x => x.VehicleCode).FirstOrDefault();
                    Obj.VehicleRegNo = DefinitionContext.Vehicles.Where(x => x.VehicleCode == Obj.VehicleCode).Select(x => x.VehicleRegNo).FirstOrDefault();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = item.ItemDesc;
                    var DispatchQty = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == DDCode && x.ItemCode == Obj.ItemCode).Select(x => x.DispatchQty).FirstOrDefault();
                    if (DispatchQty != null)
                    {
                        Obj.DispatchQty = DispatchQty;
                    }
                    else
                    {
                        Obj.DispatchQty = 0;
                    }
                   
                    NewList.Add(Obj);
                }
                return NewList;
            }
        }


        public bool DeleteDispatch(string DispatchCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
             
                DefinitionContext.DailyDispatches.RemoveRange(DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == DispatchCode).ToList());
                DefinitionContext.SaveChanges();

               
                SuccessReg = true;

                return SuccessReg;
            }
        }




        #endregion


        #region --------------------------------------------------------(Short Cash)----------------------------------------------------------

        public List<ShortCashVM> GetDistinctShortCash()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var DisctinctList = DefinitionContext.GetDistinctShortCash().ToList();
                List<ShortCashVM> NewList = new List<ShortCashVM>();

                foreach (var item in DisctinctList)
                {
                    ShortCashVM Obj = new ShortCashVM();
                    Obj.CSCode = item;
                    Obj.CSDate = DefinitionContext.ShortCashes.Where(x => x.CSCode == item).Select(x => x.CSDate).FirstOrDefault();
                    Obj.RegionCode = DefinitionContext.ShortCashes.Where(x => x.CSCode == item).Select(x => x.RegionCode).FirstOrDefault();
                    Obj.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == Obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.EmpCode = DefinitionContext.ShortCashes.Where(x => x.CSCode == item).Select(x => x.EmpCode).FirstOrDefault();
                    Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.EmpCode == Obj.EmpCode).Select(x => x.EmpName).FirstOrDefault();

                    NewList.Add(Obj);
                }
                return NewList;
            }
        }


        public List<ShortCashVM> GetSingleShortCash(string CSCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
               
                var RegionCode = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode).Select(x => x.RegionCode).FirstOrDefault();
                var EmpList = DefinitionContext.CompanyEmps.Where(x=>x.RegionCode == RegionCode).OrderBy(x=>x.EmpName).ToList();
                List<ShortCashVM> NewList = new List<ShortCashVM>();

                foreach (var item in EmpList)
                {
                    ShortCashVM Obj = new ShortCashVM();
                    Obj.CSCode = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode).Select(x => x.CSCode).FirstOrDefault();
                    Obj.CSDate = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode).Select(x => x.CSDate).FirstOrDefault();
                    Obj.RegionCode = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode).Select(x => x.RegionCode).FirstOrDefault();
                    Obj.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == Obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.EmpCode = item.EmpCode;
                    Obj.EmpName = item.EmpName;
                    
                    var CashSubmited = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode && x.EmpCode == Obj.EmpCode).Select(x => x.CashSubmited).FirstOrDefault();
                    var CashPaidBack = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode && x.EmpCode == Obj.EmpCode).Select(x => x.CashPaidBack).FirstOrDefault();
                    var CheqPaidBack = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode && x.EmpCode == Obj.EmpCode).Select(x => x.cheqPaidBack).FirstOrDefault();
                    var BouncedCheque = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode && x.EmpCode == Obj.EmpCode).Select(x => x.BouncedCheque).FirstOrDefault();
                    var Adjustment = DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode && x.EmpCode == Obj.EmpCode).Select(x => x.Adjustment).FirstOrDefault();

                    if (CashSubmited != null)
                    {
                        Obj.CashSubmited = CashSubmited;
                    }
                    else
                    {
                        Obj.CashSubmited = 0;
                    }

                    if (CashPaidBack != null)
                    {
                        Obj.CashPaidBack = CashPaidBack;
                    }
                    else
                    {
                        Obj.CashPaidBack = 0;
                    }

                    if (CheqPaidBack != null)
                    {
                        Obj.CheqPaidBack = CheqPaidBack;
                    }
                    else
                    {
                        Obj.CheqPaidBack = 0;
                    }

                    if (BouncedCheque != null)
                    {
                        Obj.BouncedCheque = BouncedCheque;
                    }
                    else
                    {
                        Obj.BouncedCheque = 0;
                    }

                    if (Adjustment != null)
                    {
                        Obj.Adjustment = Adjustment;
                    }
                    else
                    {
                        Obj.Adjustment = 0;
                    }

                    NewList.Add(Obj);
                }
                return NewList;
            }
        }


        public string AddEditShortCash(ShortCash Obj)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.CSCode == null)
                {
                    var GetShortCash = DefinitionContext.ShortCashes.ToList();
                    if (GetShortCash.Count == 0)
                    {
                        Obj.CSCode = "CS-00001";
                    }
                    else
                    {
                        var Autogen = DefinitionContext.ShortCashes.Select(x => x.CSCode).OrderByDescending(x => x).FirstOrDefault();
                        string year = "CS" + "-";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.CSCode = year + Autogen;
                    }
                    var List = HttpContext.Current.Session["ShortCashDetail"] as ShortCash[];

                    foreach (var item in List)
                    {
                        if (item.CashSubmited != 0 || item.CashPaidBack != 0 || item.BouncedCheque != 0 || item.cheqPaidBack != 0 || item.Adjustment != 0 )
                        {
                            ShortCash NewObj = new ShortCash();
                           
                            NewObj.CSCode = Obj.CSCode;
                            NewObj.CSDate = Obj.CSDate;
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.EmpCode = item.EmpCode;
                            NewObj.CashSubmited = item.CashSubmited;
                            NewObj.CashPaidBack = item.CashPaidBack;
                            NewObj.cheqPaidBack = item.cheqPaidBack;
                            NewObj.BouncedCheque = item.BouncedCheque;
                            NewObj.Adjustment = item.Adjustment;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.ShortCashes.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }

                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.ShortCashes.RemoveRange(DefinitionContext.ShortCashes.Where(x => x.CSCode == Obj.CSCode).ToList());

                    var List = HttpContext.Current.Session["ShortCashDetail"] as ShortCash[];

                    foreach (var item in List)
                    {
                        if (item.CashSubmited != 0 || item.CashPaidBack != 0 || item.BouncedCheque != 0 || item.cheqPaidBack != 0 || item.Adjustment != 0 )
                        {
                            ShortCash NewObj = new ShortCash();

                            NewObj.CSCode = Obj.CSCode;
                            NewObj.CSDate = Obj.CSDate;
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.EmpCode = item.EmpCode;
                            NewObj.CashSubmited = item.CashSubmited;
                            NewObj.CashPaidBack = item.CashPaidBack;
                            NewObj.cheqPaidBack = item.cheqPaidBack;
                            NewObj.BouncedCheque = item.BouncedCheque;
                            NewObj.Adjustment = item.Adjustment;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.ShortCashes.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully. . . !";
                }

                return SuccessMsg;
            }
        }


        public bool DeleteShortCash(string CSCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DefinitionContext.ShortCashes.RemoveRange(DefinitionContext.ShortCashes.Where(x => x.CSCode == CSCode).ToList());
                DefinitionContext.SaveChanges();

                SuccessReg = true;

                return SuccessReg;
            }
        }


        #endregion


        #region(-------------------------------------------Daily Stock Receive Terminal---------------------------------------------)

        public List<DailyStockReceiveTerminalVM> GetDinstinctDSRNoInfoTerminal(string GetDSRDate)
        {

            using (AT_Tahur_SUITEEntities DefintionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(GetDSRDate, "dd/MM/yyyy", null);
                var List = DefintionContext.GetDinstinctDSRNoTerminals(CompanyCode).Where(x => x.DSRDate == Date).GroupBy(x => x.RegionCode).Select(x => x.FirstOrDefault()).ToList();
                List<DailyStockReceiveTerminalVM> NewList = new List<DailyStockReceiveTerminalVM>();

                foreach (var item in List)
                {
                    DailyStockReceiveTerminalVM Obj = new DailyStockReceiveTerminalVM();
                    Obj.DSRNo = item.DSRNo;
                    Obj.DSRDate = item.DSRDate.ToString("dd/MM/yyyy");
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDescription = DefintionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    NewList.Add(Obj);
                }

                return NewList;
            }
        }


        public List<DailyStockReceiveTerminalVM> GetSingleDSRNoTerminal(DateTime DSRDate,string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.DailyStockReceiveTerminals.Where(x => x.CompCode == CompanyCode && x.DSRDate == DSRDate && x.RegionCode == RegionCode).ToList();
                var ItemList = DefinitionContext.SaleItems.ToList();
                List<DailyStockReceiveTerminalVM> NewList = new List<DailyStockReceiveTerminalVM>();

                foreach (var item in ItemList)
                {
                    DailyStockReceiveTerminalVM Obj = new DailyStockReceiveTerminalVM();
                    Obj.DSRDate = DSRDate.ToString("dd/MM/yyyy");
                    Obj.RegionCode = RegionCode;
                    Obj.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == Obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.ItemCode = item.ItemCode;
                    Obj.ItemDesc = item.ItemDesc;

                    var Qty = OldList.Where(x => x.ItemCode == item.ItemCode).Select(x => x.Quantity).FirstOrDefault();

                    if (Qty != null)
                    {
                        Obj.Quantity = (double)Qty;
                    }
                    else
                    {
                        Obj.Quantity = 0;
                    }

                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public List<DailyStockReceiveTerminalVM> GetDailyStockReceivesTerminal()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                List<DailyStockReceiveTerminalVM> List = new List<DailyStockReceiveTerminalVM>();
                var GetList = DefinitionContext.DailyStockReceiveTerminals.Where(x => x.CompCode == CompanyCode).OrderByDescending(x => x.DSRNo).ToList();
                foreach (var item in GetList)
                {
                    DailyStockReceiveTerminalVM DSR = new DailyStockReceiveTerminalVM();


                    List.Add(DSR);
                }

                return List;
            }
        }

        public string AddEditDailyStockReceiveTerminal(DailyStockReceiveTerminal Obj, string DSRDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(DSRDate, "dd/MM/yyyy", null);
                var FOld = DefinitionContext.DailyStockReceiveTerminals.Where(x => x.DSRDate == Date && x.RegionCode == Obj.RegionCode).FirstOrDefault();

                if (FOld == null)
                {
                    //var FoundDailyStock = DefinitionContext.DailyStockReceiveTerminals.Where(x => x.CompCode == CompanyCode).ToList();
                    //if (FoundDailyStock.Count == 0)
                    //{
                    //    Obj.DSRNo = "19/00001";
                    //}
                    //else
                    //{

                    //    var Autogen = DefinitionContext.DailyStockReceives.Where(x => x.CompCode == CompanyCode).Select(x => x.DSRNo).OrderByDescending(x => x).FirstOrDefault();
                    //    string date = DateTime.Now.ToString("yy");
                    //    string year = date + "/";
                    //    Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                    //    while (Autogen.Length != 5)
                    //    {
                    //        Autogen = '0' + Autogen;
                    //    }
                    //    Obj.DSRNo = year + Autogen;
                    //}

                    var List = HttpContext.Current.Session["ItemDiscountDSR"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            DailyStockReceiveTerminal NewObj = new DailyStockReceiveTerminal();
                            NewObj.CompCode = CompanyCode;
                            //NewObj.DSRNo = Obj.DSRNo;
                            NewObj.DSRDate = DateTime.ParseExact(DSRDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyStockReceiveTerminals.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.DailyStockReceiveTerminals.RemoveRange(DefinitionContext.DailyStockReceiveTerminals.Where(x => x.CompCode == CompanyCode && x.DSRDate == Date && x.RegionCode == Obj.RegionCode).ToList());

                    var List = HttpContext.Current.Session["ItemDiscountDSR"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            DailyStockReceiveTerminal NewObj = new DailyStockReceiveTerminal();
                            NewObj.CompCode = CompanyCode;
                            NewObj.DSRNo = Obj.DSRNo;
                            NewObj.DSRDate = DateTime.ParseExact(DSRDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyStockReceiveTerminals.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully. . . !";
                }
            }

            return SuccessMsg;
        }


        #endregion


        #region(---------------------------------------------Salesman Opening Stock Terminal------------------------------------------------)

        

        


        public List<SalesmanOpeningStockTerminalVM> GetDinstinctSOSNoTerminal(string GetSOSDate)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(GetSOSDate, "dd/MM/yyyy", null);
                List<SalesmanOpeningStockTerminalVM> List = new List<SalesmanOpeningStockTerminalVM>();
                var GetList = DefinitionContext.GetDinstinctSOSNoTerminal(CompanyCode).Where(x => x.SOSDate == Date).GroupBy(x => new { x.RegionCode, x.EmpCode }).Select(x => x.FirstOrDefault()).ToList();
                foreach (var item in GetList)
                {
                    SalesmanOpeningStockTerminalVM SOS = new SalesmanOpeningStockTerminalVM();

                    SOS.SOSNo = item.SOSNo;
                    SOS.SOSDate = item.SOSDate.ToString("dd/MM/yyyy");
                    SOS.RegionCode = item.RegionCode;
                    SOS.RegionDescription = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    SOS.EmpCode = item.EmpCode;
                    SOS.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    List.Add(SOS);
                }

                return List;
            }
        }

        public List<SalesmanOpeningStockTerminalVM> GetSingleSOSNoTerminal(DateTime SOSDate,string RegionCode, string EmpCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                List<SalesmanOpeningStockTerminalVM> List = new List<SalesmanOpeningStockTerminalVM>();
                var GetList = DefinitionContext.SalesmanOpeningStockTerminals.Where(x => x.CompCode == CompanyCode && x.SOSDate == SOSDate && x.RegionCode == RegionCode && x.EmpCode == EmpCode).ToList();
                var ItemList = DefinitionContext.SaleItems.Select(x => new { x.ItemCode, x.ItemDesc }).ToList();
                foreach (var item in ItemList)
                {
                    SalesmanOpeningStockTerminalVM SOS = new SalesmanOpeningStockTerminalVM();

                    //SOS.SOSDate = DefinitionContext.SalesmanOpeningStockTerminals.Where(x => x.SOSDate == SOSDate && x.RegionCode == RegionCode && x.EmpCode == EmpCode).Select(x => x.SOSDate).FirstOrDefault().ToString("dd/MM/yyyy");
                    //SOS.RegionCode = DefinitionContext.SalesmanOpeningStockTerminals.Where(x => x.SOSDate == SOSDate && x.RegionCode == RegionCode && x.EmpCode == EmpCode).Select(x => x.RegionCode).FirstOrDefault();
                    //SOS.RegionDescription = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == SOS.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    //SOS.EmpCode = DefinitionContext.SalesmanOpeningStockTerminals.Where(x => x.SOSDate == SOSDate && x.RegionCode == RegionCode && x.EmpCode == EmpCode).Select(x => x.EmpCode).FirstOrDefault();
                    //SOS.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == SOS.RegionCode && x.EmpCode == SOS.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    SOS.ItemCode = item.ItemCode;
                    SOS.ItemDesc = item.ItemDesc;
                    SOS.VehicleNo = GetList.Where(x => x.EmpCode == EmpCode).Select(x => x.VehicleNo).FirstOrDefault();
                    var Qty = GetList.Where(x => x.ItemCode == item.ItemCode).Select(x => x.Quantity).FirstOrDefault();

                    if (Qty != null)
                    {
                        SOS.Quantity = (double)Qty;
                    }
                    else
                    {
                        SOS.Quantity = 0;
                    }

                    List.Add(SOS);
                }

                return List;
            }
        }

        public string AddEditSalesmanOpeningStockTerminal(SalesmanOpeningStockTerminal Obj, string SOSDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(SOSDate, "dd/MM/yyyy", null);
                var FData = DefinitionContext.SalesmanOpeningStockTerminals.Where(x => x.SOSDate == Date && x.RegionCode == Obj.RegionCode && x.EmpCode == Obj.EmpCode).FirstOrDefault();
                if (FData == null)
                {
                   
                    var List = HttpContext.Current.Session["ItemDiscountSOSTerminal"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            SalesmanOpeningStockTerminal NewObj = new SalesmanOpeningStockTerminal();
                            NewObj.CompCode = CompanyCode;
                            NewObj.EmpCode = Obj.EmpCode;
                            NewObj.VehicleNo = Obj.VehicleNo;
                            //NewObj.SOSNo = Obj.SOSNo;
                            NewObj.SOSDate = DateTime.ParseExact(SOSDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.SalesmanOpeningStockTerminals.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.SalesmanOpeningStockTerminals.RemoveRange(DefinitionContext.SalesmanOpeningStockTerminals.Where(x => x.CompCode == CompanyCode && x.SOSDate == Date && x.RegionCode == Obj.RegionCode && x.EmpCode == Obj.EmpCode).ToList());

                    var List = HttpContext.Current.Session["ItemDiscountSOSTerminal"] as ItemDiscountVM[];

                    foreach (var item in List)
                    {
                        if (item.DiscountAmt != 0)
                        {
                            SalesmanOpeningStockTerminal NewObj = new SalesmanOpeningStockTerminal();
                            NewObj.CompCode = CompanyCode;
                            NewObj.EmpCode = Obj.EmpCode;
                            NewObj.VehicleNo = Obj.VehicleNo;
                            //NewObj.SOSNo = Obj.SOSNo;
                            NewObj.SOSDate = DateTime.ParseExact(SOSDate, "dd/MM/yyyy", null);
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.Quantity = item.DiscountAmt;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.SalesmanOpeningStockTerminals.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully . . . !";
                }
            }

            return SuccessMsg;
        }


        public bool DeleteSOSTerminal(DateTime SOSDate, string RegionCode, string EmpCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DefinitionContext.SalesmanOpeningStockTerminals.RemoveRange(DefinitionContext.SalesmanOpeningStockTerminals.Where(x => x.SOSDate == SOSDate && x.RegionCode == RegionCode && x.EmpCode == EmpCode).ToList());
                DefinitionContext.SaveChanges();

                SuccessReg = true;

                return SuccessReg;
            }
        }

        #endregion


        #region (---------------------------------------- Daily Demand 27-07-2021 -----------------------------------------)


        public string AddEditDailyDemand(DailyDemand Obj, string DDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(DDate, "dd/MM/yyyy", null);
                List<DailyDemand> Found = DefinitionContext.DailyDemands.Where(x => x.DDate == Date && x.RegionCode == Obj.RegionCode).ToList();

                if (Found.Count == 0)
                {

                    var List = HttpContext.Current.Session["DailyDemandData"] as DailyDemand[];

                    foreach (var item in List)
                    {
                        if (item.DemandQty != 0)
                        {
                            DailyDemand NewObj = new DailyDemand();


                            NewObj.DDate = Date;
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.DemandQty = item.DemandQty;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyDemands.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {
                    DefinitionContext.DailyDemands.RemoveRange(Found);

                    var List = HttpContext.Current.Session["DailyDemandData"] as DailyDemand[];

                    foreach (var item in List)
                    {
                        if (item.DemandQty != 0)
                        {
                            DailyDemand NewObj = new DailyDemand();


                            NewObj.DDate = Date;
                            NewObj.RegionCode = Obj.RegionCode;
                            NewObj.ItemCode = item.ItemCode.Trim();
                            NewObj.DemandQty = item.DemandQty;
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdTerm = Environment.MachineName;
                            NewObj.UpdUser = CommonDAL.UserName();
                            DefinitionContext.DailyDemands.Add(NewObj);
                            DefinitionContext.SaveChanges();
                        }
                    }
                    SuccessMsg = "Updated Successfully. . . !";
                }
            }

            return SuccessMsg;
        }

        public List<DailyDemandVM> GetSingleDemand(string DDate,string RegionCode)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(DDate, "dd/MM/yyyy", null);
                var Found = DefinitionContext.DailyDemands.Where(x => x.DDate == Date && x.RegionCode == RegionCode).ToList();
                var ItemList = DefinitionContext.SaleItems.ToList();
                List<DailyDemandVM> NewList = new List<DailyDemandVM>();

                foreach (var item in ItemList)
                {
                    DailyDemandVM Obj = new DailyDemandVM();

                    Obj.DDate = DDate;
                    Obj.RegionCode = Found.Select(x => x.RegionCode).FirstOrDefault();
                    Obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == Obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.ItemCode = item.ItemCode.Trim();
                    Obj.ItemDesc = item.ItemDesc;
                    var DemandQty = Found.Where(x => x.RegionCode == Obj.RegionCode && x.ItemCode == Obj.ItemCode).Select(x => x.DemandQty).FirstOrDefault();
                    if (DemandQty != null)
                    {
                        Obj.DemandQty = DemandQty;
                    }
                    else
                    {
                        Obj.DemandQty = 0;
                    }

                    NewList.Add(Obj);
                }
                return NewList;
            }
        }


        public bool DeleteDemand(string DDate,string[] RegionCode)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(DDate, "dd/MM/yyyy", null);
                string AllRegion = string.Join(",", RegionCode);
                DefinitionContext.DailyDemands.RemoveRange(DefinitionContext.DailyDemands.Where(x => x.DDate == Date && x.RegionCode == AllRegion).ToList());
                DefinitionContext.SaveChanges();


                SuccessReg = true;

                return SuccessReg;
            }
        }

        public bool CheckDuplicateDemand(string DDate,string[] RegionCode)
        {
            bool SuccessReg = false;
            string AllRegion = string.Join(",",RegionCode);
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (DDate != "" && RegionCode != null)
                {

                    DateTime Date = DateTime.ParseExact(DDate, "dd/MM/yyyy", null);
                    List<DailyDemand> Found = new List<DailyDemand>();
                    if (AllRegion.Length == 3)
                    {
                        Found = DefinitionContext.DailyDemands.Where(x => x.DDate == Date && x.RegionCode == AllRegion).ToList();
                    }
                    else
                    {
                        Found = DefinitionContext.DailyDemands.Where(x => x.DDate == Date && x.RegionCode == AllRegion).ToList();
                    }


                    if (Found.Count != 0)
                    {
                        SuccessReg = true;
                    }

                }
                return SuccessReg;
            }
        }


        #endregion



        #region (---------------------- ShortCash Opening  -------------------------------------)

        public List<ShortCashOpeningVM> LazyShortCashOpening()
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var Opening = DefinitionContext.CompanyEmps.Where(x => x.DelFlag == "N").ToList();
                List<ShortCashOpeningVM> ObjList = new List<ShortCashOpeningVM>();

                foreach (var item in Opening)
                {
                    ShortCashOpeningVM Obj = new ShortCashOpeningVM();

                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.EmpCode = item.EmpCode;
                    Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    var Op = DefinitionContext.ShortCashOpenings.Where(x => x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.Opening).FirstOrDefault();
                    Obj.Opening = (Op == null) ? 0 : Op;

                    ObjList.Add(Obj);
                }

                return ObjList;
            }

        }


        public string AddEditShortCashOpening(ShortCashOpening[] OpeningList)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {

                foreach (var item in OpeningList)
                {
                    if (item.Opening != 0)
                    {
                        var Found = DefinitionContext.ShortCashOpenings.Where(x => x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).FirstOrDefault();
                        if (Found == null)
                        {
                            DefinitionContext.Entry(item).State = System.Data.Entity.EntityState.Added;
                            DefinitionContext.SaveChanges();
                        }
                        else
                        {
                            Found.Opening = item.Opening;
                            DefinitionContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                            DefinitionContext.SaveChanges();
                        }
                    }
                }
                SuccessMsg = "Saved Successfully. . . !";

            }

            return SuccessMsg;
        }

        #endregion


        #region ( ---------------------------------------- Short Cash Recovery ----------------------------------------)

        public List<ShortCashRecoveryVM> GetAllSCRecoveryList(string GetRecDate)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DateTime Date = DateTime.ParseExact(GetRecDate, "dd/MM/yyyy", null);
                var OldList = DefinitionContext.ShortCashRecoveries.Where(x => x.RecDate == Date).ToList();
                List<ShortCashRecoveryVM> NewList = new List<ShortCashRecoveryVM>();
                foreach (var item in OldList)
                {
                    ShortCashRecoveryVM Obj = new ShortCashRecoveryVM();
                    Obj.Id = item.Id;
                    Obj.Type = item.Type;
                    Obj.RecDate = item.RecDate.ToString("dd/MM/yyyy");
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefinitionContext.RegionSetups.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Obj.EmpCode = item.EmpCode;
                    Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();
                    Obj.RecAmount = item.RecoveryAmount;

                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public List<ShortCashRecoveryVM> GetSingleSCRecovery(int Id)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var OldList = DefinitionContext.ShortCashRecoveries.Where(x => x.Id == Id).ToList();

                List<ShortCashRecoveryVM> NewList = new List<ShortCashRecoveryVM>();

                foreach (var item in OldList)
                {
                    ShortCashRecoveryVM Obj = new ShortCashRecoveryVM();
                    Obj.Id = item.Id;
                    Obj.RecDate = item.RecDate.ToString("dd/MM/yyyy");
                    Obj.Type = item.Type;
                    Obj.CheqNo = item.CheqNo;
                    Obj.CheqDate = (item.CheqDate == null) ? "" : item.CheqDate.ToString();
                    Obj.RegionCode = item.RegionCode;
                    Obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == item.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    Obj.PartyCode = item.PartyCode;
                    Obj.PartyDesc = DefinitionContext.SaleParties.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.PartyCode == item.PartyCode).Select(x => x.PartyName).FirstOrDefault();
                    Obj.EmpCode = item.EmpCode;
                    Obj.EmpName = DefinitionContext.CompanyEmps.Where(x => x.CompCode == CompanyCode && x.RegionCode == item.RegionCode && x.EmpCode == item.EmpCode).Select(x => x.EmpName).FirstOrDefault();

                    Obj.RecAmount = (item.RecoveryAmount == null) ? 0 : item.RecoveryAmount;

                    NewList.Add(Obj);
                }
                return NewList;
            }
        }

        public bool CheckSCRecoveryCheqNo(string CheqNo)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                int FindInvoice = DefinitionContext.ShortCashRecoveries.Where(x => x.CheqNo == CheqNo).Select(x => x.Id).FirstOrDefault();

                if (FindInvoice != 0)
                {
                    SuccessReg = true;
                }
                return SuccessReg;
            }
        }

        public string AddEditSCRecovery(ShortCashRecovery Obj, string RecDate)
        {
            string SuccessMsg = "";
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.Id == 0)
                {

                    HttpContext.Current.Session["SessSCRecDate"] = RecDate;

                    var List = HttpContext.Current.Session["SCRecoveryDetail"] as ShortCashRecovery[];

                    foreach (var item in List)
                    {
                        if (item.RecoveryAmount != 0)
                        {
                            ShortCashRecovery NewObj = new ShortCashRecovery();
                           
                            NewObj.RecDate = DateTime.ParseExact(RecDate, "dd/MM/yyyy", null);
                            NewObj.Type = Obj.Type;
                            NewObj.CheqNo = Obj.CheqNo;
                            NewObj.CheqDate = Obj.CheqDate;
                            NewObj.RegionCode = item.RegionCode;
                          
                            NewObj.PartyCode = item.PartyCode;
                          
                            NewObj.EmpCode = item.EmpCode;
                            NewObj.RecoveryAmount = item.RecoveryAmount;
                            NewObj.CreateDate = DateTime.Now;
                            NewObj.CreateBy = CommonDAL.UserName();
                            NewObj.UpdDate = DateTime.Now;
                            NewObj.UpdBy = CommonDAL.UserName();
                            DefinitionContext.ShortCashRecoveries.Add(NewObj);
                            DefinitionContext.SaveChanges();

                            HttpContext.Current.Session["SessRecRegionCode"] = item.RegionCode;
                            HttpContext.Current.Session["SessRecEmpCode"] = item.EmpCode;
                        }
                    }

                    SuccessMsg = "Saved Successfully. . . !";
                }
                else
                {

                    HttpContext.Current.Session["SessSCRecDate"] = RecDate;

                    var List = HttpContext.Current.Session["SCRecoveryDetail"] as ShortCashRecovery[];

                    foreach (var item in List)
                    {
                        if (item.RecoveryAmount != 0)
                        {
                            var Found = DefinitionContext.ShortCashRecoveries.Where(x => x.Id == Obj.Id).FirstOrDefault();

                            Found.RecDate = DateTime.ParseExact(RecDate, "dd-MM-yyyy", null);
                            Found.Type = Obj.Type;
                            Found.CheqNo = Obj.CheqNo;
                            Found.CheqDate = Obj.CheqDate;
                            Found.RegionCode = item.RegionCode;
                            Found.PartyCode = item.PartyCode;
                            Found.EmpCode = item.EmpCode;
                            Found.RecoveryAmount = item.RecoveryAmount;
                            Found.UpdDate = DateTime.Now;
                            Found.UpdBy = CommonDAL.UserName();
                            DefinitionContext.Entry(Found).State = System.Data.Entity.EntityState.Modified;
                            DefinitionContext.SaveChanges();

                            HttpContext.Current.Session["SessRecRegionCode"] = item.RegionCode;
                            HttpContext.Current.Session["SessRecEmpCode"] = item.EmpCode;
                        }
                    }

                    SuccessMsg = "Updated Successfully. . . !";
                }
            }

            return SuccessMsg;
        }


        public bool DeleteSCRecovery(int Id)
        {
            bool SuccessReg = false;
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                ShortCashRecovery FindRecovery = DefinitionContext.ShortCashRecoveries.Where(x => x.Id == Id).FirstOrDefault();

                DefinitionContext.ShortCashRecoveries.Remove(FindRecovery);
                DefinitionContext.SaveChanges();
                
                SuccessReg = true;

                return SuccessReg;
            }
        }

        #endregion
    }
}