using STOBDataLayer.Interfaces;
using STOBDataLayer.Repositories.STOB;
using STOBEntities.Models;
using STOBWeb.Authorization;
using System.Data;
using System.Web.Mvc;

namespace STOBWeb.Areas.Home.Controllers
{
    public class AwardController : BaseController
    {
        private IAwardNameRepositoryDAL _bRepository = null;
        private AwardNameRepositoryDAL _awardRepository = null;
        private BussinessUnitNameRepositoryDAL _businessUnitNamesRepository = null;
        DataTable dt;

        Three60RoleProvider three60RoleProvider;
        const string Role_BusinessUnit = "WSF-Stob-Role-BusinessUnit";
        const string Role_Coordinator = "WSF-Stob-Role-NominationCoordinator";
        const string Role_Commitee = "WSF-Stob-Role-Committee";
        const string Role_Admin = "AdminRole";
        const string Supper_Admin_Role = "AdminRole,WSF-Stob-Role-BusinessUnit";
        public AwardController()
        {
            _bRepository = new AwardNameRepositoryDAL();
            _awardRepository = new AwardNameRepositoryDAL();
            _businessUnitNamesRepository = new BussinessUnitNameRepositoryDAL();

            three60RoleProvider = new Three60RoleProvider();
        }

        [OutputCache(NoStore = true, Duration = 1, VaryByParam = "None")]
        public ActionResult Add()
        {
            var listTypes = new SelectList(new[]
            {
                new {ID="Individual", Name="Individual"},
                new {ID="Team", Name="Team"},
            }, "ID", "Name", 1);
            ViewBag.AwardTypes = listTypes;

            var listExemptions = new SelectList(new[]
            {
                new {ID="Exempt", Name="Exempt"},
                new {ID="Non-Exempt", Name="Non-Exempt"},
                new {ID="Exempt/Non-Exempt", Name="Exempt/Non-Exempt"},
            }, "ID", "Name", 1);
            ViewBag.exemptions = listExemptions;

            return View();
        }
        [HttpPost]
        public ActionResult Add(AwardName_Admin model)
        {
            var result = _bRepository.AddAsync(model);
            if (result == 1)
                TempData["Message"] = "Award saved succesfully.";
            else
                TempData["Message"] = "Error occured.";
            return RedirectToAction("Add");
        }
        [HttpGet]
        public ActionResult PartialList()
        {
            var awards = _bRepository.GetAllAwardNameAsync();
            return PartialView("PartialAwardGrid", awards);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            bool success = false;
            var result = _bRepository.DeleteAsync(Id);
            if (result == 1)
            {
                TempData["Message"] = "Award deleted succesfully";
                success = true;
            }
            else
                TempData["Message"] = "[Error: " + result + "] Award can't be deleted.";

            return Json(new { success });
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var model = _bRepository.GetAwardById(Id);

            var listTypes = new SelectList(new[]
            {
                new {ID="Individual", Name="Individual"},
                new {ID="Team", Name="Team"},
            }, "ID", "Name", model.AWARD_TYPE);
            ViewBag.AwardTypes = listTypes;
            var listExemptions = new SelectList(new[]
            {
                new {ID="Exempt", Name="Exempt"},
                new {ID="Non-Exempt", Name="Non-Exempt"},
                new {ID="Exempt/Non-Exempt", Name="Exempt/Non-Exempt"},
            }, "ID", "Name", model.AWARD_EXEMPTION);
            ViewBag.exemptions = listExemptions;

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(AwardName_Admin model)
        {
            var result = _bRepository.UpdateAsync(model);
            if (result == 1)
                TempData["Message"] = "Award updated succesfully";
            else
                TempData["Message"] = "[Error: " + result + "] Award can't be updated.";

            return RedirectToAction("Add");
        }
    }
}