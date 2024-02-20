using STOBDataLayer.Contexts;
using STOBDataLayer.Interfaces;
using STOBDataLayer.Repositories.STOB;
using STOBEntities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;
using STOBDataLayer.ViewModels;
using System.IO;
using System.Reflection;
using STOBDataLayer.Models;
using STOBWeb.Authorization;

namespace STOBWeb.Areas.Home.Controllers
{
    [AuthFilter(groups = new string[] { "AdminRole" })]
    public class HomeController : BaseController
    {
        private DashboardRepositoryDAL _dboardRepo = null;
        private NominationRepositoryDAL _nRepository = null;
        private AwardNameRepositoryDAL _awardRepository = null;
        private BussinessUnitNameRepositoryDAL _businessUnitNamesRepository = null;
        private STOBContext context = null;
        public HomeController()
        {
            _dboardRepo = new DashboardRepositoryDAL();
            _nRepository = new NominationRepositoryDAL();           
            _awardRepository = new AwardNameRepositoryDAL();
            _businessUnitNamesRepository = new BussinessUnitNameRepositoryDAL();
            context = new STOBContext();
        }
        [Authorize(Roles = "AdminRole")]
        [OutputCache(NoStore = true, Duration = 1, VaryByParam = "None")]
        public ActionResult Index()
        {
            var toggleNoms = context.ToggleNominations.ToList();

            ViewBag.IsEnabled = toggleNoms.Count == 0 ? 0 : Convert.ToInt16(toggleNoms.FirstOrDefault().IsEnabled) ;
            var _dResult = _dboardRepo.GetNominationCountDetailsAsync();
            return View(_dResult);
        }

        //ToggleNominations
        [HttpPost]
        public ActionResult ToggleNominations(string toggle)
        {
            if (toggle == "false")
            {
                context.ToggleNominations.FirstOrDefault().IsEnabled = false;
                context.SaveChanges();
            } else
            {
                context.ToggleNominations.FirstOrDefault().IsEnabled = true;
                context.SaveChanges();
            }

            return Json(new { success = true });
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

        public ActionResult Rep_Nomination(int NominationId, string page)
        {
            var nomination = _dboardRepo.NominationsById(NominationId);
            ViewBag.page = page;
            return View(nomination);
        }
        public ActionResult Rep_NominationsByType(string type, string page)
        {
            List<NomineesExcelVM> nomineesList = new List<NomineesExcelVM>();
            if (page == "BuHead")
                nomineesList = _nRepository.GetNomineesForBuHeadByType(type, NominatorEmail).ToList();
            else
                nomineesList = _nRepository.GetNomineesForAdminByType(type).ToList();

            ViewBag.type = type;
            ViewBag.page = page;
            return View(nomineesList);
        }

        [HttpPost]
        public ActionResult ExportNominationsReport(string type, string page)
        {
            List<NomineesExcelVM> nomineesList = new List<NomineesExcelVM>();
            if (page == "BuHead")
                nomineesList = _nRepository.GetNomineesForBuHeadByType(type, NominatorEmail).ToList();
            else
                nomineesList = _nRepository.GetNomineesForAdminByType(type).ToList();

            var grdreport = new System.Web.UI.WebControls.GridView
            {
                DataSource = ToDataTable(nomineesList)
            };
            grdreport.DataBind();
            System.IO.StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grdreport.RenderControl(htw);
            byte[] binddata = System.Text.Encoding.ASCII.GetBytes(sw.ToString());
            return File(binddata, "application/ms-excel", "NominationsReport");
        }

        public ActionResult Rep_NominationHistory (int NominationId)
        {
            var nomination = _dboardRepo.NominationsById(NominationId);
            NominationHistoryVM nomHistory = new NominationHistoryVM
            {
                Nomination = nomination,
                Award = nomination.Award,
                NominationStatuses = nomination.NominationStatuses
            };

            return View(nomHistory);
        }
        public ActionResult getDepartmentsList()
        {
            //var depts = context.Nominations.Select(d=>d.NOMNR_BU_DEPT).Distinct().ToList();
            var depts = context.Nominees.Select(d => d.BU_Dept).Distinct().ToList();
            return PartialView("partialDepartments", depts);
        }
        public ActionResult NominationDetailPartial(int nomId)
        {
            var nomination = context.Nominations.Find(nomId);
            return PartialView(nomination);
        }
        public  ActionResult NominationListPartials(int StatusId)
        {
			var toggleNoms = context.ToggleNominations.ToList();

			ViewBag.toggleNominations = toggleNoms.Count == 0 ? false : toggleNoms.FirstOrDefault().IsEnabled;

            var allNoms = _nRepository.GetAllNominations().ToList();
            if (StatusId > 0)
                if (StatusId == 5)
                    allNoms = allNoms.FindAll(n=> n.STAT_ID == 5 || n.STAT_ID == 6);
                else if (StatusId == 2)
                    allNoms = allNoms.FindAll(n=> n.STAT_ID == 2 || n.STAT_ID == 3 || n.STAT_ID == 4);
                else
                    allNoms = allNoms.FindAll(n => n.STAT_ID == StatusId);

            return PartialView("_NominationListPartials", allNoms);
        }
        public ActionResult GetEMployeeNamesAndTitle(string search)
        {
            var listOfNamesWithTitle = GetEmployeeDetails(search);

            ViewBag.NomineeNames = new SelectList(listOfNamesWithTitle, "Id", "Name");

            return Json(listOfNamesWithTitle, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<object> GetEmployeeDetails(string search)
        {
            DataTable dt = Employeedetails;
            TempData.Keep();
            IEnumerable<object> listOfNamesWithTitle = new List<object>();

            DataRow[] drows = dt.AsEnumerable().Where(x => x.Field<string>("Last_Name").StartsWith(search, System.StringComparison.OrdinalIgnoreCase) || x.Field<string>("First_Name").StartsWith(search, System.StringComparison.OrdinalIgnoreCase)).ToArray();


            if (drows != null && drows.Length > 0)
            {
                listOfNamesWithTitle = drows.CopyToDataTable()
                              .AsEnumerable().Select(x => new
                              {
                                  val = x.Field<string>("EMPLID"),
                                  label = x.Field<string>("First_Name") + ", " + x.Field<string>("Last_Name"),
                                  title = x.Field<string>("Title"),
                                  firstName = x.Field<string>("First_Name"),
                                  lastName = x.Field<string>("Last_Name"),
                                  exemptStatus = x.Field<string>("ExmStatus")

                              });
            }
            return listOfNamesWithTitle;
        }

        [HttpPost]
        public ActionResult ApprovebyAdmin(int NominationId)
        {
            var result = _nRepository.AddApprovalFlowByAdmin(NominationId, NominatorFullName);
            if (result > 0)
                TempData["ApprovedMessage"] = "Approved successfully.";
            return Json(new { success = true, url = Url.Action("Index") });
        }

        [HttpPost]
        public ActionResult RejectbyAdmin(int NominationId, string reason)
        {
            var result = _nRepository.RejectedFlowByAdmin(NominationId, NominatorFullName, reason);
            var nomnr_email = context.Nominations.Find(NominationId).NOMNR_EMAIL;

            if (result > 0)
                TempData["RejectedMessage"] = "Reject successfully. Conversation has been held with nominator.";
            return Json(new { success = true, url = Url.Action("Index") });
        }

        [HttpPost]
        public ActionResult ExportToExcelAllApproved()
        {
            var noms = context.Nominations.ToList();
            List<Nominations> approvedNoms = new List<Nominations>();
            noms.ForEach(n =>
            {
                var lastNomStatus = n.NominationStatuses.LastOrDefault().Status.STAT_TXT;
                if (lastNomStatus == "BUApproved" || lastNomStatus == "AdminApproved")
                    approvedNoms.Add(n);
            });

            List<NomineesExcelVM> nominees = new List<NomineesExcelVM>();
            approvedNoms.ForEach(x =>
            {
                var gridNominees = (from a in x.Nominees
                                    select new NomineesExcelVM
                                    {
                                        NomineeFirstName = a.FIRST_NM,
                                        NomineeLastName = a.LAST_NM,
                                        Title = a.TITLE_TXT,
                                        Department = a.BU_Dept,
                                        Email = a.EMAIL,
                                        Exemption = a.EXEMPT_CD,
                                        Award = x.Award.AWARD_NM,
                                        TeamName = x.TEAM_NM,
                                        Nominator = x.NOMNR_NM,
                                        Status = x.NominationStatuses.LastOrDefault().Status.STAT_TXT,
                                        AwardType = x.Award.AWARD_TYPE
                                    }).ToList();
                nominees.AddRange(gridNominees);
            });
            var grdreport = new System.Web.UI.WebControls.GridView
            {
                DataSource = ToDataTable(nominees)
            };
            grdreport.DataBind();
            System.IO.StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grdreport.RenderControl(htw);
            byte[] binddata = System.Text.Encoding.ASCII.GetBytes(sw.ToString());
            return File(binddata, "application/ms-excel", "NomineesReport-Approved.xls");
        }
        [HttpPost]
        public ActionResult ExportToExcelApprovedNominees(int NominationId)
        {
            var nomination = context.Nominations.Find(NominationId);
            var nominees = nomination.Nominees.ToList();
            var gridNominees = (from a in nominees
                          select new NomineesExcelVM { 
                              NomineeFirstName = a.FIRST_NM, 
                              NomineeLastName = a.LAST_NM,
                              Title = a.TITLE_TXT,
                              Department = a.BU_Dept,
                              Email = a.EMAIL,
                              Exemption = a.EXEMPT_CD,
                              Award = nomination.Award.AWARD_NM,
                              Status = nomination.NominationStatuses.LastOrDefault().Status.STAT_TXT,
                              TeamName = nomination.TEAM_NM,
                              AwardType = nomination.Award.AWARD_TYPE,
                              Nominator = nomination.NOMNR_NM
                          }).ToList();

            var grdreport = new System.Web.UI.WebControls.GridView
            {
                DataSource = ToDataTable(gridNominees)
            };
            grdreport.DataBind();
            System.IO.StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grdreport.RenderControl(htw);
            byte[] binddata = System.Text.Encoding.ASCII.GetBytes(sw.ToString());
            return File(binddata, "application/ms-excel", "NomineesReport.xls");
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public ActionResult Nominationgraph()
        {
            var _gResult =  _dboardRepo.GetNominationCountDetailsAsync();
            var lstModel = new List<NominationGraphAdmin>();
            lstModel.Add(new NominationGraphAdmin
            {
                Dimention = "Drafted",
                statusvalue = _gResult.TotalDraft
            });
            lstModel.Add(new NominationGraphAdmin
            {
                Dimention = "Submitted",
                statusvalue = _gResult.ToTalSubmitted + _gResult.ToTalReviewSent + _gResult.ToTalModified
            });
            lstModel.Add(new NominationGraphAdmin
            {
                Dimention = "Approved",
                statusvalue = _gResult.ToTalApproved
            });
            lstModel.Add(new NominationGraphAdmin
            {
                Dimention = "Rejected",
                statusvalue = _gResult.ToTalRejected
            });

            return PartialView("_Nominationgraph", lstModel);
        }

        public JsonResult NominationAwardStatusgraphJson()
        {
            var _gResult = _dboardRepo.GetAdminNominationAwardStatusGraphAsync();
            List<string> listAwards = new List<string>();
            List<string> listValues = new List<string>();
            _gResult.ForEach(x =>
            {
                listAwards.Add(x.AwardName);
            });
            _gResult.ForEach(x =>
            {
                var value = x.TotalNomination.ToString();
                listValues.Add(value);
            });
            return Json(new { awards = listAwards, values = listValues }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult Review(ReviewVM model)
        {
            model.ReviewerName = model.ReviewBy == "BuHead" ? NominatorFullName + " [BuHead]" : NominatorFullName + " [Admin]";
            var result = _nRepository.UpdateReviewStatus(model);
            if (result > 0)
            {
                string emailtxt = "";
                if (model.AwardType == "Team")
                { 
                    emailtxt = String.Format(@" <a href='{0}/StobWeb/Home/Nomination/EditByNomnr_Team?nomId={1}'>Please Click Here </a>", Request.Url.Authority, model.NominationId); 
                } else
                { 
                    emailtxt = String.Format(@" <a href='{0}/StobWeb/Home/Nomination/EditByNomnr_Ind?nomId={1}'>Please Click Here </a>", Request.Url.Authority, model.NominationId); 
                }
                SendMail(model.ReviewText + emailtxt, "xyz@google.com", "URL"); //obj.NOMNR_EMAIL
                TempData["ReviewSuccess"] = "Successfully sent for Review.";
            }

            string destUrl;
            if (model.ReviewBy == "BuHead")
                destUrl = Url.Action("BuheadApprovalList", "Nomination");
            else
                destUrl = Url.Action("Index", "Home");

            return Json(new { success = true, url = destUrl });
        }

    }
}
