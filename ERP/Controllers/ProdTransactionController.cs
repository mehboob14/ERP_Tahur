using ERP.Models;
using ERP.Models.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Controllers
{
    public class ProdTransactionController : Controller
    {
        TransactionHandler Handler = new TransactionHandler();
        CommonDAL Common = new CommonDAL();

        // GET: ProdTransaction
        public ActionResult Farm()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                ViewBag.FarmList = DefinitionContext.Farms.ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Farm(Farm Obj)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.FarmCode == null)
                {
                    var FoundFarm = DefinitionContext.Farms.ToList();
                    if (FoundFarm.Count == 0)
                    {
                        Obj.FarmCode = "F-01";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.Farms.Select(x => x.FarmCode).OrderByDescending(x => x).FirstOrDefault();
                        string date = "F-";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 2)) + 1).ToString();
                        while (Autogen.Length != 2)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.FarmCode = date + Autogen;
                    }

                    DefinitionContext.Farms.Add(Obj);
                    DefinitionContext.SaveChanges();
                    TempData["SuccessMsg"] = "Added Successfully . . . !";
                }
                else
                {
                    DefinitionContext.Farms.Remove(DefinitionContext.Farms.Where(x => x.FarmCode == Obj.FarmCode).FirstOrDefault());

                    DefinitionContext.Farms.Add(Obj);
                    DefinitionContext.SaveChanges();

                    TempData["SuccessMsg"] = "Updated Successfully . . . !";
                }
               

               
            }
            return RedirectToAction("Farm");
        }

       
        public ActionResult DeleteFarm(string FarmCode)
        {

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DefinitionContext.Farms.Remove(DefinitionContext.Farms.Where(x => x.FarmCode == FarmCode).FirstOrDefault());
                DefinitionContext.SaveChanges();
                TempData["DeleteMsg"] = "Deleted Successfully . . . !";
            }

            return RedirectToAction("Farm");
        }


        public ActionResult DailyIntake()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                var List = DefinitionContext.Farms.ToList();
                ViewBag.FarmDDL = new SelectList(List, "FarmCode", "FarmName");

                var DIList = DefinitionContext.DailyIntakes.ToList();
                List<DailyIntakeVM> NewList = new List<DailyIntakeVM>();
                foreach (var item in DIList)
                {
                    DailyIntakeVM Obj = new DailyIntakeVM();
                    Obj.DICode = item.DICode;
                    Obj.DIDate = item.DIDate;
                    Obj.FarmCode = item.FarmCode;
                    Obj.FarmName = DefinitionContext.Farms.Where(x => x.FarmCode == item.FarmCode).Select(x => x.FarmName).FirstOrDefault();
                    Obj.RawMilkQty = item.RawMilkQty;

                    NewList.Add(Obj);
                }
                ViewBag.DailyIntakeList = NewList;
            }
            return View();
        }

        [HttpPost]
        public ActionResult DailyIntake(DailyIntake Obj)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.DICode == null)
                {
                    var FoundFarm = DefinitionContext.DailyIntakes.ToList();
                    if (FoundFarm.Count == 0)
                    {
                        Obj.DICode = "DI-00001";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.DailyIntakes.Select(x => x.DICode).OrderByDescending(x => x).FirstOrDefault();
                        string date = "DI-";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 5)) + 1).ToString();
                        while (Autogen.Length != 5)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.DICode = date + Autogen;
                    }

                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdUser = CommonDAL.UserName();
                    Obj.UpdTerm = Environment.MachineName;
                    DefinitionContext.DailyIntakes.Add(Obj);
                    DefinitionContext.SaveChanges();
                    TempData["SuccessMsg"] = "Added Successfully . . . !";
                }
                else
                {
                    DefinitionContext.DailyIntakes.Remove(DefinitionContext.DailyIntakes.Where(x => x.DICode == Obj.DICode).FirstOrDefault());

                    Obj.UpdDate = DateTime.Now;
                    Obj.UpdUser = CommonDAL.UserName();
                    Obj.UpdTerm = Environment.MachineName;

                    DefinitionContext.DailyIntakes.Add(Obj);
                    DefinitionContext.SaveChanges();

                    TempData["SuccessMsg"] = "Updated Successfully . . . !";
                }



            }
            return RedirectToAction("DailyIntake");
        }

        public ActionResult DeleteDailyIntake(string DICode)
        {

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DefinitionContext.DailyIntakes.Remove(DefinitionContext.DailyIntakes.Where(x => x.DICode == DICode).FirstOrDefault());
                DefinitionContext.SaveChanges();
                TempData["DeleteMsg"] = "Deleted Successfully . . . !";
            }

            return RedirectToAction("DailyIntake");
        }

        public ActionResult DailyProduction()
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.GetDistinctDp = Handler.GetDinstinctDP().ToList();
            ViewBag.ItemList = Handler.GetSaleItems();
            return View();
        }

        public ActionResult GetSingleDP(string DPCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var GetSingeDPList = Handler.GetSingleDPCode(DPCode);
            return Json(GetSingeDPList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DailyProductionDetail(DailyProduction[] ItemQuantity)
        {
          
            Session["DailyProductionData"] = ItemQuantity;
            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DailyProduction(DailyProduction Obj,string DPDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            TempData["SuccessMsg"] = Handler.AddEditDailyProduction(Obj,DPDate);
            return RedirectToAction("DailyProduction");
        }

        [HttpPost]
        public ActionResult DeleteDailyProduction(string DPCode)
        {
            bool data = Handler.DeleteDailyProduction(DPCode);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Vehicle()
        {

            if (Session["CompanyCode"] == null)
            { 
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                ViewBag.VehicleList = DefinitionContext.Vehicles.ToList();
            }

            return View();
        }



        [HttpPost]
        public ActionResult Vehicle(Vehicle Obj)
        {
            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Obj.VehicleCode == null)
                {
                    var FoundVehicle = DefinitionContext.Vehicles.ToList();
                    if (FoundVehicle.Count == 0)
                    {
                        Obj.VehicleCode = "V-001";
                    }
                    else
                    {

                        var Autogen = DefinitionContext.Vehicles.Select(x => x.VehicleCode).OrderByDescending(x => x).FirstOrDefault();
                        string date = "V-";
                        Autogen = (Convert.ToInt64(Autogen.Substring(Autogen.Length - 2)) + 1).ToString();
                        while (Autogen.Length != 3)
                        {
                            Autogen = '0' + Autogen;
                        }
                        Obj.VehicleCode = date + Autogen;
                    }

                    DefinitionContext.Vehicles.Add(Obj);
                    DefinitionContext.SaveChanges();
                    TempData["SuccessMsg"] = "Added Successfully . . . !";
                }
                else
                {
                    DefinitionContext.Vehicles.Remove(DefinitionContext.Vehicles.Where(x => x.VehicleCode == Obj.VehicleCode).FirstOrDefault());

                    DefinitionContext.Vehicles.Add(Obj);
                    DefinitionContext.SaveChanges();

                    TempData["SuccessMsg"] = "Updated Successfully . . . !";
                }



            }
            return RedirectToAction("Vehicle");
        }

        public ActionResult DeleteVehicle(string VehicleCode)
        {

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                DefinitionContext.Vehicles.Remove(DefinitionContext.Vehicles.Where(x => x.VehicleCode == VehicleCode).FirstOrDefault());
                DefinitionContext.SaveChanges();
                TempData["DeleteMsg"] = "Deleted Successfully . . . !";
            }

            return RedirectToAction("Vehicle");
        }


        public ActionResult DailyDispatch()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                //ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
                var List = DefinitionContext.GetDistinctRegion().ToList();
                ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");

                var VehicleList = DefinitionContext.Vehicles.ToList();
                ViewBag.VehicleDDL = new SelectList(VehicleList, "VehicleCode", "VehicleRegNo");

                var DispatchList = DefinitionContext.GetDistinctDispatchList().ToList();

                List<DailyDispatchVM> NewList = new List<DailyDispatchVM>();
                foreach (var item in DispatchList)
                {
                    DailyDispatchVM obj = new DailyDispatchVM();

                    obj.DispatchCode = item;
                    obj.DispatchDate = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == item).Select(x => x.DispatchDate).FirstOrDefault(); 
                    obj.RegionCode = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == item).Select(x => x.RegionCode).FirstOrDefault();
                    obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                    obj.VehicleCode = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == item).Select(x => x.VehicleCode).FirstOrDefault();
                    obj.VehicleRegNo = DefinitionContext.Vehicles.Where(x => x.VehicleCode == obj.VehicleCode).Select(x => x.VehicleRegNo).FirstOrDefault();
                    obj.ItemCode = DefinitionContext.DailyDispatches.Where(x => x.DispatchCode == item).Select(x => x.ItemCode).FirstOrDefault();
                    obj.ItemDesc = DefinitionContext.SaleItems.Where(x => x.ItemCode == obj.ItemCode).Select(x => x.ItemDesc).FirstOrDefault();
                    NewList.Add(obj);
                }

                ViewBag.DispatchList = NewList;
            }

            ViewBag.ItemList = Handler.GetSaleItems();


            return View();
        }

        public ActionResult DailyDispatchDetail(DailyDispatch[] ItemQuantity)
        {

            Session["DailyDispatchData"] = ItemQuantity;
            return Json(JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DailyDispatch(DailyDispatch Obj,string DispatchDate)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            TempData["SuccessMsg"] = Handler.AddEditDailyDispatch(Obj, DispatchDate);
            return RedirectToAction("DailyDispatch");
        }

        public ActionResult GetSingleDispatch(string DispatchCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var GetSingeDDList = Handler.GetSingleDDCode(DispatchCode);
            return Json(GetSingeDDList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDispatch(string DispatchCode)
        {
            bool data = Handler.DeleteDispatch(DispatchCode);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #region (---------------------------------------------- Daily Demand --------------------------------------)

        public ActionResult DailyDemand()
        {

            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<GetDistinctRegion_Result> List;
            var Role = Session["UserRoleName"].ToString();



            using (AT_Tahur_SUITEEntities DefinitionContext = new AT_Tahur_SUITEEntities())
            {
                if (Role == "Admin")
                {
                    List = DefinitionContext.GetDistinctRegion().ToList();
                }
                else
                {
                    var Username = Session["UserName"].ToString();
                    var RegionCode = DefinitionContext.SaleDemandRights.Where(x => x.UserName == Username).Select(x => x.RegionCode).FirstOrDefault().Trim();
                    if (RegionCode.Length == 3)
                    {
                        List = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == RegionCode).ToList();
                    }
                    else
                    {
                        string[] RegionCodes = RegionCode.Split(',');
                        List = DefinitionContext.GetDistinctRegion().Where(x => RegionCodes.Contains(x.RegionCode)).ToList();
                    }
                }
                //ViewBag.RegionList = DefinitionContext.GetDistinctRegion().ToList();
                //List = DefinitionContext.GetDistinctRegion().ToList();
                ViewBag.RegionDDL = new SelectList(List, "RegionCode", "RegionDescription");

                if (CommonDAL.UserRights("510","003"))
                {
                    var DemandList = DefinitionContext.Sp_GetDistinctDemandList().ToList();

                    List<DailyDemandVM> NewList = new List<DailyDemandVM>();
                    foreach (var item in DemandList)
                    {
                        DailyDemandVM obj = new DailyDemandVM();

                        obj.DDate = item.DDate.ToString("dd/MM/yyyy");
                        obj.RegionCode = item.RegionCode.Trim();
                        if (obj.RegionCode.Length > 3)
                        {
                            obj.RegionDesc = "North Region";
                        }
                        else
                        {

                            obj.RegionDesc = DefinitionContext.GetDistinctRegion().Where(x => x.RegionCode == obj.RegionCode).Select(x => x.RegionDescription).FirstOrDefault();
                        }

                        NewList.Add(obj);
                    }

                    ViewBag.DemandList = NewList;
                }
               
            }

            ViewBag.ItemList = Handler.GetSaleItems();


            return View();
        }

        public ActionResult DailyDemandDetail(DailyDemand[] ItemQuantity)
        {

            Session["DailyDemandData"] = ItemQuantity;
            return Json(JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DailyDemand(DailyDemand Obj, string DDate, string[] RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            
            Obj.RegionCode = string.Join(",", RegionCode);

            TempData["SuccessMsg"] = Handler.AddEditDailyDemand(Obj, DDate);

            return RedirectToAction("DailyDemand");
        }

        public ActionResult GetSingleDemand(string DDate,string RegionCode)
        {
            if (Session["CompanyCode"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var GetSingeDDList = Handler.GetSingleDemand(DDate, RegionCode);

            return Json(GetSingeDDList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDemand(string DDate,string[] RegionCode)
        {
            bool data = Handler.DeleteDemand(DDate,RegionCode);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckDuplicateDemand(string DDate,string[] RegionCode)
        {
            bool data = false;
            bool right = false;
            //if (CommonDAL.UserRights("510", "003"))
            //{
                data = Handler.CheckDuplicateDemand(DDate, RegionCode);
                right = true;
            //}
          
            return Json(new { data,right }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}