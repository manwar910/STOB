namespace STOBDataLayer.Models.BoilerPlateDefaults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RELS_NOTES
    {
        [Key]
        public int RELS_NOTES_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal RELS_VER_NO { get; set; }

        [Required]
        [StringLength(5000)]
        public string RELS_NOTES_TXT { get; set; }

        [Column(TypeName = "date")]
        public DateTime RELS_DT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DELETE_DT { get; set; }
    }
}
