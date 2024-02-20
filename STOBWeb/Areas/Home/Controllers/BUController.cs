using STOBDataLayer.Contexts;
using STOBDataLayer.Interfaces;
using STOBDataLayer.Model_DTO;
using STOBDataLayer.Models;
using STOBDataLayer.Repositories.STOB;
using STOBWeb.Authorization;
using STOBWeb.ViewModals;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace STOBWeb.Areas.Home.Controllers
{
    public class BUController : BaseController
    {
        private IBussinessUnitNameNameRepositoryDAL _bRepository = null;
        private AwardNameRepositoryDAL _awardRepository = null;
        private BussinessUnitNameRepositoryDAL _businessUnitNamesRepository = null;
        DataTable dt;
        private STOBContext context = null;

        Three60RoleProvider three60RoleProvider;
        const string Role_BusinessUnit = "WSF-Stob-Role-BusinessUnit";
        const string Role_Coordinator = "WSF-Stob-Role-NominationCoordinator";
        const string Role_Commitee = "WSF-Stob-Role-Committee";
        const string Role_Admin = "AdminRole";
        const string Supper_Admin_Role = "AdminRole,WSF-Stob-Role-BusinessUnit";
        public BUController()
        {
            _bRepository = new BussinessUnitNameRepositoryDAL();
            _awardRepository = new AwardNameRepositoryDAL();
            _businessUnitNamesRepository = new BussinessUnitNameRepositoryDAL();
            three60RoleProvider = new Three60RoleProvider();
            context = new STOBContext();
        }
        private DataTable Employeedetails
        {
            get
            {
                DataTable dt = TempData["EmployeeDetails"] as DataTable;
                TempData.Keep();
                return dt;
            }
            set
            {
                TempData["EmployeeDetails"] = value;
            }
        }
        private DataTable Employeedept
        {
            get
            {
                DataTable dt = TempData["EmployeeDept"] as DataTable;
                TempData.Keep();
                return dt;
            }
            set
            {
                TempData["EmployeeDept"] = value;
            }
        }

        [OutputCache(NoStore = true, Duration = 1, VaryByParam = "None")]
        public ActionResult Add()
        {
            DataTable dt = DataAccess_Oracle.GetDataSet();
            Employeedetails = dt;

            var buHeads = context.BUHeads.ToList();
            var employees = context.Employees.ToList(); //Use your own code
            //Get list of all employees who are not selected as buHeads 
            employees = employees.Where(e => !buHeads.Any(bh => bh.BU_EMAIL == e.EMAIL)).ToList();
            List<SelectListItem> listEmployees = new List<SelectListItem>();
            foreach (var emp in employees)
                listEmployees.Add(new SelectListItem() { Value = emp.EMAIL.ToString() + "," + emp.DEPARTMENT.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME });
            SelectListItem empLabel = new SelectListItem() { Text = "--Select Employee--", Value = "" };//
            listEmployees.Insert(0, empLabel);
            ViewBag.employees = listEmployees;

            return View();
        }
        [HttpPost]
        public ActionResult Add(BUViewModelDTO model)
        {
            var result = _bRepository.SaveTrans(model);
            if (result == 1)
                TempData["Message"] = "BuHead added succesfully.";
            else
                TempData["Message"] = "[Error:" + result + "] BuHead can't be added.";

            return RedirectToAction("Add");
        }

        [HttpGet]
        public ActionResult PartialList()
        {
            BUViewModelList model = new BUViewModelList
            {
                BU = _bRepository.GetAllBussinessUnitNameAsync(),
                BU_DEPT = _bRepository.GetAllBussinessUnitDepartAsync()
            };
            return PartialView("PartialBUGrid", model);
        }
        [HttpGet]
        public ActionResult PartialEditList(int Id)
        {
            var model = _bRepository.GetAllBussinessUnitDepartAsync().Where(x => x.BU_ID == Id).ToList();
            return PartialView("PartialBUEdit", model);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            bool success = false;
            var result = _bRepository.DeleteAsync(Id);
            if (result == 1)
            {
                TempData["Message"] = "BuHead deleted succesfully";
                success = true;
            }
            else
                TempData["Message"] = "[Error:" + result + "] BuHead can't be deleted.";

            return Json(new { success });
        }
        [HttpPost]
        public ActionResult DeleteDept(int Id)
        {
            var success = false;
            var result = _bRepository.DeleteDepart(Id);
            if (result == 1)
            {
                TempData["Message"] = "Department deleted succesfully.";
                success = true;
            }
            else
                TempData["Message"] = "[Error:" + result + "] Department can't be deleted.";

            return Json(new { success });
        }

        [HttpGet]
        public ActionResult Edit(int buId)
        {
            DataTable dt = DataAccess_Oracle.GetDataSetDept();
            Employeedept = dt;
            //var buDept = context.BUDepartments.Where(d => d.BU_ID == buId).First();
            var buDepts = context.BUDepartments.Where(d => d.BU_ID == buId).ToList();
            if (buDepts.Any())
            {
                var buDept = buDepts.Where(d=>d.IS_BU_DEPT == true).First();    
                var buHeads = context.BUHeads.Where(b => b.BU_ID != buId).ToList(); //All BuHeads without current department's buHead
                var employees = context.Employees.ToList();
                //Get list of all employees who are not selected as buHeads 
                employees = employees.Where(e => !buHeads.Any(bh => bh.BU_EMAIL == e.EMAIL)).ToList();

                List<SelectListItem> listEmployees = new List<SelectListItem>();

                foreach (var emp in employees)
                {
                    if (emp.EMAIL == buDept.BUHead.BU_EMAIL)
                        listEmployees.Add(new SelectListItem() { Selected = true, Value = emp.EMAIL.ToString() + "," + emp.DEPARTMENT.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME });
                    else
                        listEmployees.Add(new SelectListItem() { Value = emp.EMAIL.ToString() + "," + emp.DEPARTMENT.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME });
                }
                ViewBag.employees = listEmployees;

                var depts = context.Departments.ToList();
                var buHeadDepts = buDept.BUHead.BUDepartments;
                //Get list of all employees who are not selected as buHeads 
                depts = depts.Where(d => !buHeadDepts.Any(bh => bh.BU_DEPT == d.Name)).ToList();

                List<SelectListItem> listDepts = new List<SelectListItem>();

                foreach (var d in depts)
                    listDepts.Add(new SelectListItem() { Value = d.Name, Text = d.Name });
                ViewBag.depts = listDepts;

                BUViewModelDTO model = new BUViewModelDTO
                {
                    BU_ID = buDept.BU_ID,
                    BU_DEPT = buDept.BU_DEPT,
                    BU_EMAIL = buDept.BUHead.BU_EMAIL,
                    BU_NM = buDept.BUHead.BU_NM
                };
                return View(model);
            }
            else
                return RedirectToAction("Add");
        }

        [HttpPost]
        public ActionResult Edit(BUViewModelDTO model)
        {
            var result = _bRepository.UpdateTrans(model);
            if (result == 1)
                TempData["Message"] = "Department(s) added succesfully.";
            else
                TempData["Message"] = "[Error:" + result + "] Department can't be added.";

            return RedirectToAction("Edit", new {buId = model.BU_ID});
        }
    }
}