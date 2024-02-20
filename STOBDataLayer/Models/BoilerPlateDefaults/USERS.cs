namespace STOBDataLayer.Models.BoilerPlateDefaults
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USERS()
        {
            HIST = new HashSet<HIST>();
        }

        [Key]
        public int USER_ID { get; set; }

        [StringLength(128)]
        public string FIRST_NM { get; set; }

        [StringLength(128)]
        public string LAST_NM { get; set; }

        [Required]
        [StringLength(20)]
        public string USER_NM { get; set; }

        [StringLength(12)]
        public string PH_NO { get; set; }

        [StringLength(255)]
        public string EMAIL_ADDR_TXT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DELETE_DT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HIST> HIST { get; set; }
    }
}
