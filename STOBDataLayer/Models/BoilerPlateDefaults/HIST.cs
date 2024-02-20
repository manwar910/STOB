namespace STOBDataLayer.Models.BoilerPlateDefaults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HIST")]
    public partial class HIST
    {
        [Key]
        public int HIST_ID { get; set; }

        public int USER_ID { get; set; }

        [StringLength(4000)]
        public string ACTN_TXT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CREATE_DT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DELETE_DT { get; set; }

        public virtual USERS USERS { get; set; }
    }
}
