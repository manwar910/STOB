namespace STOBDataLayer.Models.BoilerPlateDefaults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FAQ")]
    public partial class FAQ
    {
        [Key]
        public int FAQ_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SORT_ORDER_NO { get; set; }

        [Required]
        public string QSTN_TXT { get; set; }

        [Required]
        public string ANSR_TXT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DELETE_DT { get; set; }
    }
}
