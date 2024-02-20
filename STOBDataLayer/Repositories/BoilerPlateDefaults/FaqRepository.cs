using System.Collections.Generic;
using STOBDataLayer.Models.BoilerPlateDefaults;
using STOBEntities.BoilerPlateDefaults;

namespace STOBDataLayer.Repositories.BoilerPlateDefaults
{
    public class FaqRepository : GenericRepository<Contexts.BoilerPlateDefaults, FAQ, Faq>
    {
        public IEnumerable<Faq> GetAllModels(bool showDeletes)
        {
            if (!showDeletes)
                return FindModelsBy(x => x.DELETE_DT == null);

            return base.GetAllModels();
        }

        public override FAQ ConvertToDbObject(Faq entity)
        {
            FAQ faq = new FAQ()
            {
                FAQ_ID = entity.FaqId,
                SORT_ORDER_NO = entity.SortOrderNumber,
                QSTN_TXT = entity.QuestionText,
                ANSR_TXT = entity.AnswerText,
                DELETE_DT = entity.DeleteDate
            };

            return faq; ;
        }

        public override Faq ConvertToModel(FAQ entity)
        {
            Faq faq = new Faq()
            {
                FaqId = entity.FAQ_ID,
                SortOrderNumber = entity.SORT_ORDER_NO,
                QuestionText = entity.QSTN_TXT,
                AnswerText = entity.ANSR_TXT,
                DeleteDate = entity.DELETE_DT
            };

            return faq;
        }
    }
}
