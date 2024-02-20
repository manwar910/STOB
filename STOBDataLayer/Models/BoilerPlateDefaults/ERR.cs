namespace STOBDataLayer.Models.BoilerPlateDefaults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ERR")]
    public partial class ERR
    {
        [Key]
        public int ERR_ID { get; set; }

        [Column("LOG_GLBL_UNIQUE-IDENTIFIER")]
        public Guid? LOG_GLBL_UNIQUE_IDENTIFIER { get; set; }

        public DateTime ERR_DTM { get; set; }

        [Required]
        [StringLength(20)]
        public string USER_NM { get; set; }

        [Required]
        [StringLength(250)]
        public string ERR_TYP { get; set; }

        [Required]
        [StringLength(250)]
        public string ERR_SRC_TXT { get; set; }

        [Required]
        [StringLength(5000)]
        public string ERR_MSG_TXT { get; set; }

        [Required]
        [StringLength(5000)]
        public string STACK_TRACE_TXT { get; set; }

        [Required]
        [StringLength(50)]
        public string SRVR_NM { get; set; }
    }
}
