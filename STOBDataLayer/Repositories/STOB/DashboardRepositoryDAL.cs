using STOBEntities.Models;
using STOBDataLayer.Contexts;
using System.Collections.Generic;
using System.Linq;
using STOBDataLayer.Models;

namespace STOBDataLayer.Interfaces
{
    public class DashboardRepositoryDAL : IDashboardRepositoryDAL
    {
        STOBContext _dbcontext = null;
        public DashboardRepositoryDAL()
        {
            _dbcontext = new STOBContext();
        }

        public Nominations NominationsById(int id)
        {
            var nomination = _dbcontext.Nominations.Find(id);
            return nomination;
        }

        public IEnumerable<Nominees> NomineesByDept(string dept) //IEnumerable<Nominations> NominationsByDept(string dept)
        {
            //var sList = _dbcontext.Nominations.Where(n=>n.NOMNR_BU_DEPT == dept).ToList();
            var sList = _dbcontext.Nominees.Where(n => n.BU_Dept == dept).ToList();
            return sList;
        }

        public DashBoardDTO GetNominationCountDetailsAsync()
        {
            var nominations = _dbcontext.Nominations.ToList();
            var T_Drafted = (from n in nominations
                              where n.NominationStatuses.LastOrDefault().STAT_ID == 1
                              select n).Count();
            var T_Submitted = (from n in nominations
                               where n.NominationStatuses.LastOrDefault().STAT_ID == 2
                               select n).Count();
            var T_Approved = (from n in nominations
                               where n.NominationStatuses.LastOrDefault().STAT_ID == 5 || n.NominationStatuses.LastOrDefault().STAT_ID == 6
                               select n).Count();
            var T_Rejected = (from n in nominations
                               where n.NominationStatuses.LastOrDefault().STAT_ID == 7
                               select n).Count();
            var T_ReviewSent = (from n in nominations
                                where n.NominationStatuses.LastOrDefault().STAT_ID == 3
                                select n).Count();
            var T_Modified = (from n in nominations
                                where n.NominationStatuses.LastOrDefault().STAT_ID == 4
                                select n).Count();

            return new DashBoardDTO
            {
                TotalCounts = nominations.Count,
                ToTalSubmitted = T_Submitted + T_ReviewSent + T_Modified,
                //ToTalReviewSent = T_ReviewSent,
                //ToTalModified = T_Modified,
                TotalDraft = T_Drafted,
                ToTalApproved = T_Approved,
                ToTalRejected = T_Rejected
            };
        }
        public List<DashBoardNominationAwardStatusGraphDTO> GetAdminNominationAwardStatusGraphAsync()
        {
            List<DashBoardNominationAwardStatusGraphDTO> awardNominationsList = new List<DashBoardNominationAwardStatusGraphDTO>();
            //Get all awards which are used inside Nomination table
            var awards = _dbcontext.Awards
                           .Where(l => _dbcontext.Nominations.Any(a => a.AWARD_ID == l.AWARD_ID))
                           .ToList();
            foreach (var award in awards)
            {
                var awardNominations = new DashBoardNominationAwardStatusGraphDTO
                {
                    AwardName = award.AWARD_NM,
                    TotalNomination = award.Nominations.Count
                };

                awardNominationsList.Add(awardNominations);
            }
            return awardNominationsList;
        }
    }
}
