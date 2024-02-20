using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOBEntities.Models
{
    public class BussinesUnitName_Admin
    {
        public int BU_ID { get; set; }
        [Required]
        public string BU_NM { get; set; }
        [Required]
        public string BU_EMAIL { get; set; }
        [Required]
        public string BU_DEPT { get; set; }
        public virtual ICollection<Associate_Awards_Nominations> Associate_Awards_Nominations { get; set; }
    }
}
