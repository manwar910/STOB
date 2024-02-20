using STOBDataLayer.Models;
using STOBEntities.Models;
using System.Collections.Generic;

namespace STOBDataLayer.Interfaces
{
    public interface IDashboardRepositoryDAL
    {
        DashBoardDTO GetNominationCountDetailsAsync();
        List<DashBoardNominationAwardStatusGraphDTO> GetAdminNominationAwardStatusGraphAsync();
        IEnumerable<Nominees> NomineesByDept(string dept);
    }
}
