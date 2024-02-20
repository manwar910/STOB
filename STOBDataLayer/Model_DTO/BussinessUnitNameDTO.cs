using STOBEntities.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOBDataLayer.Model_DTO
{
    public class BUViewModelDTO
    {
        public int BU_ID { get; set; }

        [Required]
        [Display(Name = "BuHead Name")]
        public string BU_NM { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string BU_EMAIL { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string BU_DEPT { get; set; }

        [Display(Name = "Additional Departments")]
        public string[] DEPTS { get; set; }

    }

}
