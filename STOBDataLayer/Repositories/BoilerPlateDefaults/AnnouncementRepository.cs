using System;
using System.Collections.Generic;
using STOBDataLayer.Models.BoilerPlateDefaults;
using STOBEntities.BoilerPlateDefaults;

namespace STOBDataLayer.Repositories.BoilerPlateDefaults
{
    public class AnnouncementsRepository : GenericRepository<Contexts.BoilerPlateDefaults, ANNCMNT, Announcement>
    {
        public IEnumerable<Announcement> GetAllModels(bool currentOnly)
        {
            if (currentOnly)
                return FindModelsBy(x => x.DELETE_DT == null && x.STRT_DT <= DateTime.Today && (x.END_DT == null || x.END_DT >= DateTime.Today));

            return base.GetAllModels();
        }

        public override ANNCMNT ConvertToDbObject(Announcement entity)
        {
            ANNCMNT ann = new ANNCMNT()
            {
                ANNCMNT_ID = entity.AnnouncementId,
                ANNCMNT_TXT = entity.AnnouncementText,
                STRT_DT = entity.StartDate,
                END_DT = entity.EndDate,
                DELETE_DT = entity.DeleteDate
            };

            return ann;
        }

        public override Announcement ConvertToModel(ANNCMNT entity)
        {
            Announcement announcement = new Announcement()
            {
                AnnouncementId = entity.ANNCMNT_ID,
                AnnouncementText = entity.ANNCMNT_TXT,
                StartDate = entity.STRT_DT,
                EndDate = entity.END_DT,
                DeleteDate = entity.DELETE_DT
            };

            return announcement;
        }
    }
}
