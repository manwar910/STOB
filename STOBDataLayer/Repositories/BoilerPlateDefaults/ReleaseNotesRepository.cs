using System.Collections.Generic;
using STOBDataLayer.Models.BoilerPlateDefaults;
using STOBEntities.BoilerPlateDefaults;

namespace STOBDataLayer.Repositories.BoilerPlateDefaults
{
    public class ReleaseNotesRepository : GenericRepository<Contexts.BoilerPlateDefaults, RELS_NOTES, ReleaseNotes>
    {
        public IEnumerable<ReleaseNotes> GetAllModels(bool showDeletes)
        {
            if (!showDeletes)
                return FindModelsBy(x => x.DELETE_DT == null);

            return base.GetAllModels();
        }

        public override RELS_NOTES ConvertToDbObject(ReleaseNotes entity)
        {
            RELS_NOTES notes = new RELS_NOTES()
            {
                RELS_NOTES_ID = entity.ReleaseNotesId,
                RELS_VER_NO = entity.ReleaseVersionNumber,
                RELS_NOTES_TXT = entity.ReleaseNotesText,
                RELS_DT = entity.ReleaseDate,
                DELETE_DT = entity.DeleteDate
            };
            return notes;
        }

        public override ReleaseNotes ConvertToModel(RELS_NOTES entity)
        {
            ReleaseNotes notes = new ReleaseNotes()
            {
                ReleaseNotesId = entity.RELS_NOTES_ID,
                ReleaseVersionNumber = entity.RELS_VER_NO,
                ReleaseNotesText = entity.RELS_NOTES_TXT,
                ReleaseDate = entity.RELS_DT,
                DeleteDate = entity.DELETE_DT
            };

            return notes;
        }
    }
}
