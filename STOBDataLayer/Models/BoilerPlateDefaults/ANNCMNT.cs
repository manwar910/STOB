namespace STOBDataLayer.Models.BoilerPlateDefaults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ANNCMNT")]
    public partial class ANNCMNT
    {
        [Key]
        public int ANNCMNT_ID { get; set; }

        [Required]
        public string ANNCMNT_TXT { get; set; }

        [Column(TypeName = "date")]
        public DateTime STRT_DT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? END_DT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DELETE_DT { get; set; }
    }
}
