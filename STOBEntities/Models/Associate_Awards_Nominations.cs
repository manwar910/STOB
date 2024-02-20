using System;

namespace STOBEntities.Models
{
    public class Associate_Awards_Nominations
    {
        public long[] NOMINEES { get; set; }    //For Team Nomination
        public int NOMN_ID { get; set; }
        public int AWARD_ID { get; set; }
        public string TEAM_NM { get; set; }
        public int NOMNR_EMPL_ID { get; set; }
        public string NOMNR_NM { get; set; }
        public string NOMNR_EMAIL { get; set; }
        public string NOMNR_BU_DEPT { get; set; }

        public string NOMINEE_EMAIL { get; set; }
        public int? STAT_ID { get; set; }
        public string BU_DEPT { get; set; }

        public string CMNT_TXT { get; set; }
        public string APRVD_BY_NM { get; set; }
        public DateTime? NOMN_CREATE_DT { get; set; }
        public DateTime? APRVD_DT { get; set; }
        public string FIRST_NM { get; set; }
        public string LAST_NM { get; set; }
        public string TITLE_TXT { get; set; }
        public string Status { get; set; }
        public string TeamName { get; set; }
        public string EXEMPT_CD { get; set; }
        public bool? ACTV_FLG { get; set; }
        public string CREATED_BY_NM { get; set; }
        public string MODFD_BY_NM { get; set; }
        public DateTime? MODFD_DT { get; set; }
        public bool IsCoordinator { get; set; }
        public bool IsBuHead { get; set; }
        public string Exemptstring
        {
            get
            {
                string result = string.Empty;
                if (!string.IsNullOrEmpty(EXEMPT_CD))
                {
                    result = EXEMPT_CD == "E" || EXEMPT_CD == "X" ? "Exempt" : "Non Exempt";
                }
                return result;
            }         
        }
        public string NominatorFullName { get; set; }
    }
}
