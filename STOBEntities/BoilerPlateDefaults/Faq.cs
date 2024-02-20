using System;

namespace STOBEntities.BoilerPlateDefaults
{
    [Serializable]
    public class Faq
    {
        public int FaqId { get; set; }
        public decimal SortOrderNumber { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
