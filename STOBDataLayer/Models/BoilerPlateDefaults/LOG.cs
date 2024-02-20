namespace STOBDataLayer.Models.BoilerPlateDefaults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOG")]
    public partial class LOG
    {
        [Key]
        public int LOG_ID { get; set; }

        [Column("LOG_GLBL_UNIQUE-IDENTIFIER")]
        public Guid? LOG_GLBL_UNIQUE_IDENTIFIER { get; set; }

        [Required]
        [StringLength(20)]
        public string USER_NM { get; set; }

        [Required]
        [StringLength(50)]
        public string SESN_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string TITLE_TXT { get; set; }

        [Required]
        [StringLength(100)]
        public string CLASS_NM { get; set; }

        [Required]
        [StringLength(100)]
        public string MTHD_TXT { get; set; }

        [StringLength(8000)]
        public string INP_TXT { get; set; }

        public DateTime? LOG_DTM { get; set; }

        public decimal DUR_TM_QTY { get; set; }

        [Required]
        [StringLength(50)]
        public string SRVR_NM { get; set; }

        public int PROC_ID { get; set; }

        public int THREAD_ID { get; set; }

        [StringLength(8000)]
        public string USER_DEF_TXT_1 { get; set; }

        [StringLength(8000)]
        public string USER_DEF_TXT_2 { get; set; }
    }
}
