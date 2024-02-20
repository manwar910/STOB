using STOBDataLayer.Models.BoilerPlateDefaults;
using STOBEntities.BoilerPlateDefaults;

namespace STOBDataLayer.Repositories.BoilerPlateDefaults
{
    public class HistoryRepository : GenericRepository<Contexts.BoilerPlateDefaults, HIST, History>
    {
        public override HIST ConvertToDbObject(History entity)
        {
            var hi = new HIST()
            {
                HIST_ID = entity.HistoryId,
                USER_ID = entity.UserId,
                ACTN_TXT = entity.ActionText,
                CREATE_DT = entity.CreateDate,
                DELETE_DT = entity.DeleteDate
            };
            return hi;
        }

        public override History ConvertToModel(HIST entity)
        {
            var hi = new History()
            {
                HistoryId = entity.HIST_ID,
                UserId = entity.USER_ID,
                ActionText = entity.ACTN_TXT,
                CreateDate = entity.CREATE_DT,
                DeleteDate = entity.DELETE_DT
            };
            return hi;
        }
    }
}
