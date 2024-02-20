using System.Collections.Generic;
using System.Threading.Tasks;
using STOBDataLayer.Models;
using STOBDataLayer.ViewModels;
using STOBEntities.Models;

namespace STOBDataLayer.Interfaces
{
    public interface INominationRepositoryDAL:IGenericRepository<Associate_Awards_Nominations>
    {
        IEnumerable<NominationVM> GetAllNominations();
        IEnumerable<NominationVM> GetNominationsByUserName(string UserFullName);
        IEnumerable<BuHeadNominationsVM> GetAllBuHeadNominations(string email);
        int AddApprovalFlowByBuHead(int NominationId, string buHeadName);
        int AddRejectedFlowByBuHead(int NominationId, string buHeadName, string reason);
        int AddApprovalFlowByAdmin(int NominationId, string adminName);
        int RejectedFlowByAdmin(int NominationId, string adminName, string reason);
        int SubmitById(int NomId, string Nominator);
        int UpdateReviewStatus(ReviewVM model);
        Task<int> AddNomination(NominationVM model, int btnStatus);
        Task<int> UpdateNomination(NominationVM model, string type, string modifier);
    }
}
