using STOBDataLayer.Repositories.STOB;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using STOBWeb.Authorization;
using System.DirectoryServices.AccountManagement;
using System;
using STOBDataLayer.Contexts;
using STOBDataLayer.Models;
using System.Globalization;
using System.Data.Entity;
using STOBDataLayer.ViewModels;

namespace STOBWeb.Areas.Home.Controllers
{
    public class NominationController : BaseController
    {
        private NominationRepositoryDAL _nRepository = null;
        private AwardNameRepositoryDAL _awardRepository = null;
        private BussinessUnitNameRepositoryDAL _businessUnitNamesRepository = null;
        private STOBContext context = null;

        DataTable dt;
        
        Three60RoleProvider three60RoleProvider;
        const string Role_BusinessUnit = "WSF-Stob-Role-BusinessUnit";
        const string Role_Coordinator = "WSF-Stob-Role-NominationCoordinator";
        const string Role_Commitee = "WSF-Stob-Role-Committee";
        const string Role_Admin = "AdminRole";
        const string Supper_Admin_Role = "AdminRole,WSF-Stob-Role-BusinessUnit";
        private string DestinationURL
        {
            get
            {
                string url = TempData["Destionation_URL"] as string;
                TempData.Keep();
                return url;
            }
            set
            {
                TempData["Destionation_URL"] = value;
            }
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
        public NominationController()
        {
            _nRepository = new NominationRepositoryDAL();
            _awardRepository = new AwardNameRepositoryDAL();
            _businessUnitNamesRepository = new BussinessUnitNameRepositoryDAL(); 
            three60RoleProvider = new Three60RoleProvider();
            context = new STOBContext();
            dt = DataAccess_Oracle.GetDataSet();    //New
            Employeedetails = dt;   //New
        }
        //[AuthFilter(groups = new string[] { Role_Admin })]
        public ActionResult Index()
        {
            string statusId = string.Empty;
            List<string> userRoles = three60RoleProvider.GetRolesForUser(UserName).ToList();

            userRoles.Add(Role_Admin);  //Adding this line resolved the issue

            if (userRoles.Count == 0 || !CheckUserHasStobRoles(userRoles))
            {
                return RedirectToAction("Add");
            }

            if (userRoles.Contains(Role_Admin))
            {
                return RedirectToAction("Index", "Home");

            }
            else if (userRoles.Contains(Role_BusinessUnit))
            {
                return RedirectToAction("BuheadApprovalList");
            }
            else
            {
                return RedirectToAction("CordinatorApprovalList");
            }

        }

        [HttpPost]
        public ActionResult SendEmailToApprovedNominees(int NominationId, string message)
        {
            var nominees = context.Nominations.Find(NominationId).Nominees;
            foreach (var nominee in nominees)
                SendMail(message, nominee.EMAIL, "URL");
            TempData["EmailSuccess"] = "Email sent successfully.";
            return Json(new { success = true, url = Url.Action("BuheadApprovalList") });
        }

        //[Authorize(Roles = Supper_Admin_Role)]
        [OutputCache(NoStore = true, Duration = 1, VaryByParam = "None")]
        public ActionResult BuheadApprovalList()
        {
            ViewBag.toggleNominations = context.ToggleNominations.FirstOrDefault().IsEnabled;
            DestinationURL = Request.Url.AbsoluteUri;
            var controller = new BaseController();
            ViewBag.user = controller.GetUserName();
            var allNominations =  _nRepository.GetAllBuHeadNominations("badshah@gmail.com");

            if (allNominations.Count() < 1)
            {
                TempData["BuHeadNull"] = "You are not a member of our BuHead Group";
                return RedirectToAction("Add");
            }

            return View(allNominations);
        }

        public ActionResult GetBuDepartmentsList()
        {
            var buDepts = context.BUHeads.Where(h => h.BU_EMAIL == NominatorEmail).FirstOrDefault().BUDepartments.ToList();
            var buDeptsList = buDepts.Select(d => d.BU_DEPT).ToList();
            return PartialView("partialBuDepartments", buDeptsList);
        }
        public ActionResult AwardDescription(int id)
        {
            string desc = string.Empty; string type = string.Empty;
            var award = context.Awards.Find(id);
            
            if (award!=null)
            {
                type = award.AWARD_TYPE;
                if (award.AWARD_DESC != null)
                    desc = award.AWARD_DESC;
            }
            return Json(new { type, desc }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 1, VaryByParam = "None")]
        [HttpGet]
        public ActionResult Add()
        {
            var awards = GetAwardNames();
            List<SelectListItem> awardsList = new List<SelectListItem>();
            foreach (var award in awards)
            {
                if (award.AWARD_TYPE=="Team")
                    awardsList.Add(new SelectListItem() { Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " -" + award.AWARD_TYPE }); 
                else
                    awardsList.Add(new SelectListItem() { Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE +" ("+award.AWARD_EXEMPTION +")" });
            }

            awardsList = awardsList.OrderBy(a => a.Text).ToList();
            SelectListItem item = new SelectListItem() { Text = "--Select an Award--", Value = "" };
            awardsList.Insert(0, item);

            ViewBag.AwardList = awardsList;

            var employees = GetEmployeeDetails();
            List<SelectListItem> listEmployeesMulti = new List<SelectListItem>();
            foreach (var emp in employees)
                listEmployeesMulti.Add(new SelectListItem() { Value = emp.ID.ToString(), Text = emp.LAST_NAME.ToString() + ", " + emp.FIRST_NAME.ToString() + " (" + emp.TITLE.ToString() + " - " + emp.DEPARTMENT.ToString() + " )" });
            listEmployeesMulti = listEmployeesMulti.OrderBy(a => a.Text).ToList();
            ViewBag.EmployeesMulti = listEmployeesMulti;
            
            List<SelectListItem> listEmployees = new List<SelectListItem>();
            foreach (var emp in employees)
                listEmployees.Add(new SelectListItem() { Value = emp.ID.ToString(), Text = emp.LAST_NAME.ToString() + ", " + emp.FIRST_NAME.ToString() + " (" + emp.TITLE.ToString() + " - " + emp.DEPARTMENT.ToString() + " )" });

            listEmployees = listEmployees.OrderBy(a => a.Text).ToList();
            SelectListItem firstOption = new SelectListItem() { Text = "--Choose Nominee--", Value = "" };
            listEmployees.Insert(0, firstOption);
            ViewBag.Employees = listEmployees;

            return View();
        }
        public List<Nominees> NomineesList(List<int> nomIds)
        {
            var filteredRows = dt.AsEnumerable().Where(row => nomIds.Contains((int)row["ID"])).ToArray();
            List<Nominees> nomList = new List<Nominees>();
            if (filteredRows != null && filteredRows!=null)
            {
                nomList = (from DataRow dr in filteredRows
                           select new Nominees()
                           {
                               EMP_ID = Convert.ToInt32(dr["ID"]),
                               FIRST_NM = dr["FIRST_NAME"].ToString(),
                               LAST_NM = dr["LAST_NAME"].ToString(),
                               TITLE_TXT = dr["TITLE"].ToString(),
                               EXEMPT_CD = dr["EXMSTATUS"].ToString() == "E" ? "Exempt" : "NonExempt",
                               BU_Dept = dr["DEPARTMENT"].ToString(),
                               EMAIL = dr["EMAIL"].ToString()
                           }).ToList();
            }
            return nomList;
        }
        [HttpPost]
        public async Task<ActionResult> Add(NominationVM model, int btnStatus)
        {
            model.NOMNR_NM = NominatorFullName;
            model.NOMNR_BU_DEPT = "IT";
            model.Nominees = NomineesList(model.NomineeIds.ToList());
            model.NOMNR_EMAIL = NominatorEmail;

            int nomId = await _nRepository.AddNomination(model, btnStatus);

            if (nomId > 0)
            {
                var buDepts = context.BUDepartments.Where(d => d.BU_DEPT == model.NOMNR_BU_DEPT).ToList();
                string msgHead = String.Format(@" <a href='{0}/StobWeb/Home/Nomination/EditByHead?nomId={1}'>Please Click Here </a>", Request.Url.Authority, nomId);
                buDepts.ForEach(x =>
                {
                    var buHeadEmail = x.BUHead.BU_EMAIL;
                    SendMail(msgHead, buHeadEmail, "URL");
                });
                TempData["SaveMessage"] = "Nomination saved succesfully";
            }
            return Json(new { success = true });
        }

        public ActionResult EmployeesListPartial(string exType)
        {
            List<Employees> emps = GetEmployeeDetails();
            if (exType == "Exempt")
                emps = emps.Where(e => e.EXMSTATUS == "E" && e.manager_level != 1 && e.manager_level != 2 && e.manager_level != 3).ToList();
            else if (exType == "Non-Exempt")
                emps = emps.Where(e => e.EXMSTATUS == "N").ToList();
            if (emps.Any())
                emps = emps.OrderBy(e=>e.LAST_NAME).ToList();
            return PartialView("_EmployeesList", emps);
        }

        [AcceptVerbs("GET", "POST")]
        public JsonResult IsTeamExists(string TEAM_NM, string ExistingTEAM_NM)
        {
            bool tnExists;
            if (ExistingTEAM_NM != null || ExistingTEAM_NM != "")
                tnExists = context.Nominations.FirstOrDefault(x => x.TEAM_NM == TEAM_NM && x.TEAM_NM != ExistingTEAM_NM) != null;
            else
                tnExists = context.Nominations.FirstOrDefault(x => x.TEAM_NM == TEAM_NM) != null;

            return Json(!tnExists, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteNomination(int nomId)
        {
            try
            {
                var nom = context.Nominations.Where(n => n.NOMN_ID == nomId).FirstOrDefault();
                context.Nominations.Remove(nom);
                context.SaveChanges();
                TempData["DeleteNom"] = "Nomination deleted successfully.";
                return Json(new { redirectToUrl = Url.Action("Add") });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;  // Suggest internal server error
                return Json(new
                {
                    code = ex.InnerException.HResult,
                    Message = ex.InnerException.GetBaseException().Message
                });
            }
        }

        private IEnumerable<Awards> GetAwardNames()
        {
            var awards =  _awardRepository.GetAllAwardNameAsync();
            return awards;
        }

        public ActionResult getEmployeeDetail(int empId)
        {
            TextInfo ProperCase = new CultureInfo("en-US", false).TextInfo;

            var emp = context.Employees.FirstOrDefault(e=>e.ID == empId);
            var empDetail = new
            {
                Department = ProperCase.ToTitleCase(emp.DEPARTMENT.ToLower()),
                Exemption = ProperCase.ToTitleCase(emp.EXMSTATUS.ToLower()),
                Email = emp.EMAIL,
                Title = ProperCase.ToTitleCase(emp.TITLE.ToLower())
            };
            return Json(empDetail, JsonRequestBehavior.AllowGet);
        }

        private List<Employees> GetEmployeeDetails()
        {
            DataRow[] drows = dt.AsEnumerable().ToArray();
            List<Employees> empList = new List<Employees>();
            if (drows != null && drows.Length > 0)
            {
                empList = (from DataRow dr in dt.Rows
                           select new Employees()
                           {
                               ID = Convert.ToInt32(dr["ID"]),
                               FIRST_NAME = dr["FIRST_NAME"].ToString(),
                               LAST_NAME = dr["LAST_NAME"].ToString(),
                               TITLE = dr["TITLE"].ToString(),
                               EXMSTATUS = dr["EXMSTATUS"].ToString(),
                               DEPARTMENT = dr["DEPARTMENT"].ToString(),
                               manager_level = Convert.ToInt16(dr["manager_level"])
                           }).ToList();
            }
            return empList;
        }

        [HttpGet]
        public ActionResult EditByNomnr_Team(int nomId)
        {            
            var nomination = context.Nominations.AsQueryable().FirstOrDefault(n=>n.NOMN_ID == nomId);
            var nomStatus = nomination == null ? null : nomination.NominationStatuses.LastOrDefault();
            //To hadle trying to click email-link by Nominator when nomination has been deleted or its status has been changed to upper status than ReviewSent
            if (nomination == null || nomStatus.STAT_ID > 3) return RedirectToAction("Add", "Nomination");

            NominationVM editNominationModel = new NominationVM
            {
                NOMN_ID = nomination.NOMN_ID,
                NomineeIds = nomination.Nominees.Select(n => n.EMP_ID).ToArray(),
                AWARD_ID = nomination.AWARD_ID,
                NOMNR_NM = nomination.NOMNR_NM,
                NOMNR_EMPL_ID = nomination.NOMNR_EMPL_ID,
                NOMNR_BU_DEPT = nomination.NOMNR_BU_DEPT,
                NOMNR_EMAIL = nomination.NOMNR_EMAIL,
                TEAM_NM = nomination.TEAM_NM,
                LAST_COMMENT = nomStatus.COMMENT,
                DATE = nomStatus.DATE,
                STAT_TXT = nomStatus.Status.STAT_TXT,
                STAT_BY = nomStatus.BY,
                REASON = nomination.NOMN_RSN,
                STAT_ID = nomStatus.STAT_ID,
                ExistingTEAM_NM = nomination.TEAM_NM
            };


            var employees = GetEmployeeDetails();
            List<SelectListItem> listEmployeesMulti = new List<SelectListItem>();
            foreach (var emp in employees)
                listEmployeesMulti.Add(new SelectListItem() { Value = emp.ID.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME + " (" + emp.TITLE + " - " + emp.DEPARTMENT + " )" });
            listEmployeesMulti = listEmployeesMulti.OrderBy(a => a.Text).ToList();
            ViewBag.EmployeesMulti = listEmployeesMulti;

            //Get Awards
            var awards = GetAwardNames().Where(a => a.AWARD_TYPE == "Team").ToList();
            List<SelectListItem> awardsList = new List<SelectListItem>();
            foreach (var award in awards)
            {
                if (award.AWARD_ID == nomination.AWARD_ID)
                    awardsList.Add(new SelectListItem() { Selected = true, Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE });
                else
                    awardsList.Add(new SelectListItem() { Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE  });
            }
            awardsList = awardsList.OrderBy(a => a.Text).ToList();//
            SelectListItem item = new SelectListItem() { Text = "--Select an Award--", Value = "" };//
            awardsList.Insert(0, item);//
            ViewBag.AwardList = awardsList;

            return View(editNominationModel);
        }

        [HttpGet]
        public ActionResult EditByNomnr_Ind(int nomId)
        {
            var nomination = context.Nominations.AsQueryable().FirstOrDefault(n => n.NOMN_ID == nomId);
            var nomStatus = nomination == null ? null : nomination.NominationStatuses.LastOrDefault();
            //To hadle trying to click email-link by Nominator when nomination has been deleted or its status has been changed to upper status than ReviewSent
            if (nomination == null || nomStatus.STAT_ID > 3) return RedirectToAction("Add", "Nomination");

            var nomineeEmpId = nomination.Nominees.Select(n=>n.EMP_ID).FirstOrDefault();
            NominationVM editNominationModel = new NominationVM
            {
                NOMN_ID = nomination.NOMN_ID,
                AWARD_ID = nomination.AWARD_ID,
                Award_Description = nomination.Award.AWARD_DESC,
                NOMNR_NM = nomination.NOMNR_NM,
                NOMNR_EMPL_ID = nomination.NOMNR_EMPL_ID,
                NOMNR_BU_DEPT = nomination.NOMNR_BU_DEPT,
                NOMNR_EMAIL = nomination.NOMNR_EMAIL,
                TEAM_NM = nomination.TEAM_NM,
                DATE = nomStatus.DATE,
                LAST_COMMENT = nomStatus.COMMENT,
                STAT_TXT = nomStatus.Status.STAT_TXT,
                REASON = nomination.NOMN_RSN,
                STAT_ID = nomStatus.STAT_ID,
                STAT_BY = nomStatus.BY
            };
            //Get Awards
            var awards = GetAwardNames().Where(a => a.AWARD_TYPE == "Individual").ToList();
            List<SelectListItem> awardsList = new List<SelectListItem>();
            foreach (var award in awards)
            {
                if (award.AWARD_ID == nomination.AWARD_ID)
                    awardsList.Add(new SelectListItem() { Selected = true, Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE + " (" + award.AWARD_EXEMPTION + ")" });
                else
                    awardsList.Add(new SelectListItem() { Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE + " (" + award.AWARD_EXEMPTION + ")" });
            }
            awardsList = awardsList.OrderBy(a => a.Text).ToList();//
            SelectListItem item = new SelectListItem() { Text = "--Select an Award--", Value = "" };//
            awardsList.Insert(0, item);//
            ViewBag.AwardList = awardsList;

            //Get Employees
            var employees = GetEmployeeDetails();
            var exmStatus =  "";
            if (nomination.Award.AWARD_EXEMPTION == "Exempt") exmStatus = "E"; 
            else if (nomination.Award.AWARD_EXEMPTION == "Non-Exempt") exmStatus = "N";

            if (exmStatus.Length > 0) employees = employees.Where(e => e.EXMSTATUS == exmStatus).ToList();

            List<SelectListItem> listEmployees = new List<SelectListItem>();
            foreach (var emp in employees)
            {
                if (emp.ID == nomineeEmpId)
                    listEmployees.Add(new SelectListItem() { Selected = true, Value = emp.ID.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME + " (" + emp.TITLE + " - " + emp.DEPARTMENT + " )" });
                else 
                    listEmployees.Add(new SelectListItem() { Value = emp.ID.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME + " (" + emp.TITLE + " - " + emp.DEPARTMENT + " )" });
            }
            listEmployees = listEmployees.OrderBy(a => a.Text).ToList();
            SelectListItem firstOption = new SelectListItem() { Text = "--Choose Nominee--", Value = "" };
            listEmployees.Insert(0, firstOption);
            ViewBag.Employees = listEmployees;


            return View(editNominationModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditByNomnr(NominationVM model, string type)
        {
            var modifier = "Nominator";
            model.Nominees = NomineesList(model.NomineeIds.ToList());    //New
            model.NOMNR_NM = NominatorFullName + " ["+ modifier +"]";
            var result = await _nRepository.UpdateNomination(model, type, modifier);
            if (result > 0)
                TempData["EditMessage"] = "Nomination updated successfully";

            return Json(new { success = true, url = Url.Action("Add", "Nomination") });
        }

        [HttpPost]
        public ActionResult Submit(int NomId)  
        {
            var result = _nRepository.SubmitById(NomId, NominatorFullName);
            if (result > 0)
            {
                TempData["SubmitMessage"] = "Nomination Submitted Successfully";
            }
            return Json(new { success = true });
        }
      
        [HttpGet]
        public  ActionResult DraftNominationPartial(string type)
        {
            ViewBag.type = type;
            IEnumerable<NominationVM> noms = _nRepository.GetNominationsByUserName(NominatorFullName);

            if (!String.IsNullOrEmpty(type))
                noms = noms.Where(n => n.Award_Type.Equals(type)).ToList();

            return PartialView("DraftNominationPartial", noms);
        }        
        public  ActionResult ApproveByBuHead(int NominationId)
        {
            var result = _nRepository.AddApprovalFlowByBuHead(NominationId, NominatorFullName);
            if (result > 0)
            {
                TempData["ApproveMessage"] = "Approved successfully.";
            }
            return Json(new { redirectToUrl = Url.Action("BuheadApprovalList") });
        }

        public ActionResult Rep_BUNomination(int NominationId)
        {
            var nomination = context.Nominations.Find(NominationId);
            return View(nomination);
        }
        public ActionResult RejectByBuHead(int NominationId, string reason)
        {
            var result = _nRepository.AddRejectedFlowByBuHead(NominationId, NominatorFullName, reason);
            if (result > 0)
            {
                TempData["RejectMessage"] = "Reject successfully. Conversation has been held with nominator.";
            }
            return Json(new { redirectToUrl = Url.Action("BuheadApprovalList") });
        }
        public IEnumerable<SelectListItem> GetExemptDropDown()
        {
            object selectedValue = "NO";
            return new List<SelectListItem>
        {
            new SelectListItem{ Text="", Value = "", Selected = true},
            new SelectListItem{ Text="Exempt", Value = "E", Selected = "E" == selectedValue.ToString()},
            new SelectListItem{ Text="Non-Exempt", Value = "N", Selected = "N" == selectedValue.ToString()},
            new SelectListItem{ Text="Exempt", Value = "X", Selected = "X" == selectedValue.ToString()},

        };

        }
        #region View/Edit
        [HttpGet]
        public async Task<ActionResult> EditByHead(string type, int NominationId, string by)
        {
            //ViewBag.Exemptlist = new SelectList(GetExemptDropDown(), "Value", "Text");
            //dt = DataAccess_Oracle.GetDataSet();
            //TempData["EmployeeDetails"] = dt;
            //TempData["EditedBy"] = by;
            ViewBag.by = by;
            var nomination = await context.Nominations.AsQueryable().FirstOrDefaultAsync(n => n.NOMN_ID == NominationId); 
            var nomStatus = nomination.NominationStatuses.LastOrDefault(); 

            NominationVM editNominationModel = new NominationVM
            {
                NOMN_ID = nomination.NOMN_ID,
                AWARD_ID = nomination.AWARD_ID,
                Award_Description = nomination.Award.AWARD_DESC,
                NOMNR_NM = nomination.NOMNR_NM,
                NOMNR_EMPL_ID = nomination.NOMNR_EMPL_ID,
                NOMNR_BU_DEPT = nomination.NOMNR_BU_DEPT,
                NOMNR_EMAIL = nomination.NOMNR_EMAIL,
                TEAM_NM = nomination.TEAM_NM,
                REASON = nomination.NOMN_RSN,
                DATE = nomination.NOMN_DT,
                STAT_ID = nomStatus.STAT_ID,   
                STAT_BY = nomStatus.BY, 
                ExistingTEAM_NM = nomination.TEAM_NM    //New
            };

            if (type=="Individual")
            {
                var awards = GetAwardNames().Where(a => a.AWARD_TYPE == "Individual").ToList();
                List<SelectListItem> awardsList = new List<SelectListItem>();
                foreach (var award in awards)
                {
                    if (award.AWARD_ID == nomination.AWARD_ID)
                        awardsList.Add(new SelectListItem() { Selected = true, Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE });
                    else
                        awardsList.Add(new SelectListItem() { Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE });
                }
                awardsList = awardsList.OrderBy(a => a.Text).ToList();//
                SelectListItem item = new SelectListItem() { Text = "--Select an Award--", Value = "" };//
                awardsList.Insert(0, item);//
                ViewBag.AwardList = awardsList;

                var nomineeEmpId = nomination.Nominees.Select(n => n.EMP_ID).FirstOrDefault();

                var employees = await context.Employees.AsQueryable().ToListAsync(); //Async
                List<SelectListItem> listEmployees = new List<SelectListItem>();
                foreach (var emp in employees)
                {
                    if (emp.ID == nomineeEmpId)
                        listEmployees.Add(new SelectListItem() { Selected = true, Value = emp.ID.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME + " (" + emp.TITLE + " - " + emp.DEPARTMENT + " )" });
                    else
                        listEmployees.Add(new SelectListItem() { Value = emp.ID.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME + " (" + emp.TITLE + " - " + emp.DEPARTMENT + " )" });
                }
                listEmployees = listEmployees.OrderBy(a => a.Text).ToList();
                SelectListItem labelItem = new SelectListItem() { Text = "--Choose Nominee--", Value = "" };//
                listEmployees.Insert(0, labelItem);//
                ViewBag.Employees = listEmployees;
                editNominationModel.Employees = listEmployees;
                return View("EditByHead_Ind", editNominationModel);
            }
            else
            {
                var awards = GetAwardNames().Where(a => a.AWARD_TYPE == "Team").ToList();
                List<SelectListItem> awardsList = new List<SelectListItem>();
                foreach (var award in awards)
                {
                    if (award.AWARD_ID == nomination.AWARD_ID)
                        awardsList.Add(new SelectListItem() { Selected = true, Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE });
                    else
                        awardsList.Add(new SelectListItem() { Value = award.AWARD_ID.ToString(), Text = award.AWARD_NM + " - " + award.AWARD_TYPE });
                }
                awardsList = awardsList.OrderBy(a => a.Text).ToList();  //
                SelectListItem item = new SelectListItem() { Text = "--Select an Award--", Value = "" };    //
                awardsList.Insert(0, item); //
                ViewBag.AwardList = awardsList;
                var employees = await context.Employees.AsQueryable().ToListAsync();
                List<SelectListItem> listEmployeesMulti = new List<SelectListItem>();
                foreach (var emp in employees)
                {
                    listEmployeesMulti.Add(new SelectListItem() { Value = emp.ID.ToString(), Text = emp.LAST_NAME + ", " + emp.FIRST_NAME + " (" + emp.TITLE + " - " + emp.DEPARTMENT + " )" });
                }
                listEmployeesMulti = listEmployeesMulti.OrderBy(a => a.Text).ToList();
                ViewBag.EmployeesMulti = listEmployeesMulti;

                editNominationModel.NomineeIds = nomination.Nominees.Select(n => n.EMP_ID).ToArray();
                return View("EditByHead_Team", editNominationModel);
            }
        }
        [HttpPost]
        public async Task<ActionResult> EditByHead(NominationVM model, string type, string modifier) //Used in EditByHead_Ind and _NominationListPartials
        {
            model.Nominees = NomineesList(model.NomineeIds.ToList());    //New

            //var modifier = Convert.ToString(TempData["EditedBy"]);  //Comes from GET action
            model.NOMNR_NM = NominatorFullName + " ["+ model +"]";
            var result = await _nRepository.UpdateNomination(model, type, modifier);    //Same function used in Edit action above
            if (result > 0)
                TempData["UpdatedMessage"] = "Nomination Updated Successfully";

            string destUrl;
            if (modifier == "BuHead")
                destUrl = Url.Action("BuheadApprovalList", "Nomination");
            else
                destUrl = Url.Action("Index", "Home");

            return Json(new { success = result > 0, url = destUrl });
        }

        #endregion
    }

}