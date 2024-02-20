using STOBDataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace STOBDataLayer.ViewModels
{
    public class NominationVM
    {
        public int NOMN_ID { get; set; }
        public int AWARD_ID { get; set; }
        public string Award_Name { get; set; }
        public string Award_Type { get; set; }
        public string Award_Exemption { get; set; }
        public string Award_Description { get; set; }
        public int STAT_ID { get; set; }
        public string STAT_TXT { get; set; }
        public string STAT_BY { get; set; }

        [Remote(action: "IsTeamExists", controller: "Nomination", AdditionalFields = "ExistingTEAM_NM", HttpMethod = "POST", ErrorMessage = "Team Name already exists. Please choose another name.")]
        public string TEAM_NM { get; set; }
        public string ExistingTEAM_NM { get; set; }

        public int NOMNR_EMPL_ID { get; set; }
        public string NOMNR_EMAIL { get; set; }
        public string NOMNR_NM { get; set; }
        public string NOMNR_BU_DEPT { get; set; }
        public bool? ACTV_FLG { get; set; }
        public int[] NomineeIds { get; set; }
        public string REASON { get; set; }
        public DateTime DATE { get; set; } = DateTime.Now;
        public string COMMENT { get; set; }
        public string LAST_COMMENT { get; set; }
        public IEnumerable<Nominees> Nominees { get; set; }
        public IList<SelectListItem> Employees { get; set; }
    }
    public class ReviewVM
    {
        public int NominationId { get; set; }
        public string ReviewText { get; set; }
        public string NOMNR_EMAIL { get; set; }
        public string ReviewBy { get; set; }
        public string ReviewerName { get; set; }
        public string AwardType { get; set; }
    }
    public class NominationHistoryVM
    {
        public Nominations Nomination { get; set; }
        public Awards Award { get; set; }
        public IEnumerable<NominationStatuses> NominationStatuses { get; set; }
    }

    public class NomineesExcelVM {
        public string NomineeFirstName { get; set; }
        public string NomineeLastName { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Exemption { get; set; }
        public string Nominator { get; set; }
        public DateTime NomDate { get; set; }

        public string Award { get; set; }
        public string AwardType { get; set; }
        public string TeamName { get; set; }
        public string Status { get; set; }
    }
    public class BuHeadNominationsVM
    {
        public Nominations Nomination { get; set; }
        public IEnumerable<Nominees> Nominees { get; set; }
    }
}
