using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOBEntities.Models
{
    public class AwardName_Admin
    {
        public int AWARD_ID { get; set; }
        [Required]
        public string AWARD_NM { get; set; }
        [Required]
        public string AWARD_TYPE { get; set; }
        public string AWARD_EXEMPTION { get; set; }
        public string AWARD_DESC { get; set; }

        public List<Associate_Awards_Nominations> listassociateawardNomination { get; set; }
    }
}
