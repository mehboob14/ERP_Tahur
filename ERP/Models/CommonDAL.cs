using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    public class CommonDAL
    {
        SecurityDB_AtTahurEntities SContext = new SecurityDB_AtTahurEntities();
        AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities();

        //public bool Authenticate(string userName, string password)
        //{
        //    bool value = false;
        //    var Authenticate = SContext.SecurityUsers.Where(s => s.LoginName == userName && s.Password == password && s.ActiveFlag == "Y").ToList();
        //    if (Authenticate.Count != 0)
        //    {
        //        string AppCode = "003";
        //        foreach (var item in Authenticate)
        //        {
        //            var SComp = item.CompCode;
        //            var SBSCompanyCode = (from a in DefinitionContext.CompanySetups where a.CompanyCode == SComp select a.CompanyCode).SingleOrDefault();
        //            if (SComp == SBSCompanyCode)
        //            {
        //                System.Web.HttpContext.Current.Session["UserName"] = item.LoginName;
        //                System.Web.HttpContext.Current.Session["UserCode"] = item.UserCode;
        //                System.Web.HttpContext.Current.Session["CompanyCode"] = item.CompCode;
        //                var UserRights = SContext.SecurityUserAccesses.Where(s => s.UserCode == item.UserCode && s.ApplicationCode == AppCode).ToList();
        //                System.Web.HttpContext.Current.Session["UserRights"] = UserRights;
        //                value = true;
        //            }
        //            else
        //            {
        //               System.Web.HttpContext.Current.Session["LoginError"]= "User Name or Password Is Incorrect...!";
        //            }

        //        }
        //    }

        //    return value;
        //}        
        public bool Authenticate(string userName, string password)
        {
            bool value = false;
            var Authenticate = SContext.SecurityUsers.Where(s => s.LoginName == userName.Trim() && s.Password == password.Trim() && s.ActiveFlag == "Y").ToList();
            if (Authenticate.Count != 0)
            {
                string SBSAppCode = "001";
                foreach (var item in Authenticate)
                {
                    string uUserCode = item.UserCode.Trim();
                    string uUserCompanyCode = item.CompCode.Trim();
                    var SBSCompanyCode = (from a in DefinitionContext.CompanySetups where a.CompCode == uUserCompanyCode select a.CompCode).SingleOrDefault();
                    if (uUserCompanyCode == SBSCompanyCode)
                    {
                        var sCompanyApplications = (from a in SContext.CompanyApplications where a.CompanyCode == uUserCompanyCode && a.ActiveFlag == "Y" select a.ApplicationCode.Trim()).ToList();
                        if (sCompanyApplications.Contains(SBSAppCode))
                        {
                            string sSecUserApps = (from a in SContext.SecurityUserApplications where a.UserCode == uUserCode && a.ApplicationCode == SBSAppCode && a.AllowFlag == "Y" select a.ApplicationCode).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(sSecUserApps))
                            {
                                System.Web.HttpContext.Current.Session["UserName"] = item.LoginName;
                                System.Web.HttpContext.Current.Session["UserCode"] = item.UserCode;
                                System.Web.HttpContext.Current.Session["CompanyCode"] = item.CompCode;
                                System.Web.HttpContext.Current.Session["CompanyDesc"] = DefinitionContext.CompanySetups.Where(x => x.CompCode== item.CompCode).Select(x => x.CompName).FirstOrDefault();
                                var UserRights = SContext.SecurityUserAccesses.Where(s => s.UserCode == item.UserCode).ToList();
                                System.Web.HttpContext.Current.Session["UserRights"] = UserRights;
                                var UserRole = SContext.SecurityUserRoles.Where(x => x.UserCode == item.UserCode && x.EnableFlag == "Y").Select(x => x.RoleCode).FirstOrDefault();
                                System.Web.HttpContext.Current.Session["UserRoleName"] = SContext.SecurityRoles.Where(x => x.RoleCode == UserRole).Select(x => x.RoleName).FirstOrDefault();
                                System.Web.HttpContext.Current.Session["UserApplication"] = (from a in SContext.SecurityUserApplications where a.UserCode == uUserCode select a).ToList();
                                System.Web.HttpContext.Current.Session["RegionCode"] = item.RegionCode;
                                value = true;
                            }
                        }
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["LoginError"] = "User Name or Password Is Incorrect...!";
                    }

                }
            }
            return value;
        }
        public bool UserRight(string FormCode, string ActionCode)
        {
            bool Access = false;
            if (System.Web.HttpContext.Current.Session["UserRights"] != null)
            {
                var Rights = System.Web.HttpContext.Current.Session["UserRights"] as List<SecurityUserAccess>;
                if (Rights.Count > 0)
                {
                    foreach (var i in Rights)
                    {
                        if (i.FormCode.Trim() == FormCode.Trim() && i.FormActionCode.Trim() == ActionCode.Trim() && i.ActionValue == "Y")
                        {
                            Access = true;
                            return Access;
                        }
                    }
                }
            }
            return Access;
        }

        public static bool UserApplication(string ApplicationCode)
        {
            bool Access = false;
            if (System.Web.HttpContext.Current.Session["UserApplication"] != null)
            {
                var Rights = System.Web.HttpContext.Current.Session["UserApplication"] as List<SecurityUserApplication>;
                if (Rights.Count > 0)
                {
                    foreach (var i in Rights)
                    {
                        if (i.ApplicationCode.Trim() == ApplicationCode.Trim() && i.AllowFlag == "Y")
                        {
                            Access = true;
                            return Access;
                        }
                    }
                }
            }
            return Access;
        }

        public static bool UserRights(string FormCode, string ActionCode)
        {
            bool Access = false;
            if (System.Web.HttpContext.Current.Session["UserRights"] != null)
            {
                var Rights = System.Web.HttpContext.Current.Session["UserRights"] as List<SecurityUserAccess>;
                if (Rights.Count > 0)
                {
                    foreach (var i in Rights)
                    {
                        if (i.FormCode.Trim() == FormCode.Trim() && i.FormActionCode.Trim() == ActionCode.Trim() && i.ActionValue == "Y")
                        {
                            Access = true;
                            return Access;
                        }
                    }
                }
            }
            return Access;
        }
        public static string CompCode()
        {
            string Code = null;
            if (System.Web.HttpContext.Current.Session["CompanyCode"] != null)
            {
                Code = System.Web.HttpContext.Current.Session["CompanyCode"].ToString();
            }
            return Code;
        }
        public static string UserName()
        {
            string Code = null;
            if (System.Web.HttpContext.Current.Session["UserName"] != null)
            {
                Code = System.Web.HttpContext.Current.Session["UserName"].ToString();
            }
            return Code;
        }

        public static string UserRoleName()
        {
            string Code = null;
            if (System.Web.HttpContext.Current.Session["UserRoleName"] != null)
            {
                Code = System.Web.HttpContext.Current.Session["UserRoleName"].ToString();
            }
            return Code;
        }

       
    }
}