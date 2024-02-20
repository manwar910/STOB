using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STOBDataLayer.Models
{
    [Table("TOGGLE_NOMN")]
    public class ToggleNominations
    {
        [Key]
        public short Id { get; set; }
        public bool IsEnabled { get; set; }
    }
    [Table("AWARD")]
    public class Awards
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AWARD_ID { get; set; }
        public string AWARD_NM { get; set; }
        public string AWARD_TYPE { get; set; }
        public string AWARD_EXEMPTION { get; set; }
        public string AWARD_DESC { get; set; }

        public virtual ICollection<Nominations> Nominations { get; set; }

    }

    [Table("EMPLOYEES")]
    public class Employees
    {
        public int ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string TITLE { get; set; }
        public string EXMSTATUS { get; set; }

        public string DEPARTMENT { get; set; }

        public string EMAIL { get; set; }
        public short? manager_level { get; set; }
        public virtual ICollection<Nominees> Nominees { get; set; }

    }

    [Table("ASSOC_AWARD_NOMN")]
    public class Nominations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NOMN_ID { get; set; }
        public int AWARD_ID { get; set; }
        public virtual Awards Award { get; set; }   //
        public string TEAM_NM { get; set; }
        public virtual ICollection<Nominees> Nominees { get; set; }
        public int NOMNR_EMPL_ID { get; set; }
        public string NOMNR_EMAIL { get; set; }
        public string NOMNR_NM { get; set; }
        public string NOMNR_BU_DEPT { get; set; }
        public bool? ACTV_FLG { get; set; }
        public DateTime NOMN_DT { get; set; }
        public string NOMN_RSN { get; set; }
        public virtual ICollection<NominationStatuses> NominationStatuses { get; set; }   //

        //[NotMapped]
        //public int[] NomineeIds { get; set; }
    }
    [Table("NOMN_STATUSES")]
    public class NominationStatuses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NOMN_STAT_ID { get; set; }
        public DateTime DATE { get; set; }
        public string BY { get; set; }

        [ForeignKey("Nomination")]
        public int NOMN_ID { get; set; }
        public virtual Nominations Nomination { get; set; }

        [ForeignKey("Status")]
        public int STAT_ID { get; set; }
        public virtual Statuses Status { get; set; }
        public string COMMENT { get; set; }

    }

    [Table("STATUSES")]
    public class Statuses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int STAT_ID { get; set; }
        public string STAT_TXT { get; set; }
        public virtual ICollection<NominationStatuses> NominationStatuses { get; set; }
    }

    [Table("NOMINEES")]
    public class Nominees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Display(Name = "Nomination")]
        [ForeignKey("Nomination")]
        public int NOMN_ID { get; set; }
        public virtual Nominations Nomination { get; set; }

        [Display(Name = "Nominee")]
        [ForeignKey("Nominee")]
        public int EMP_ID { get; set; }
        public virtual Employees Nominee { get; set; }

        public string FIRST_NM { get; set; }
        public string LAST_NM { get; set; }
        public string TITLE_TXT { get; set; }
        public string BU_Dept { get; set; }
        public string EMAIL { get; set; }
        public string EXEMPT_CD { get; set; }
    }

    [Table("Dept")]
    public class Departments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepId { get; set; }

        public string Name { get; set; }
        //public virtual ICollection<Employees> Employees { get; set; }

    }

    [Table("BU")]
    public class BUHeads
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BU_ID { get; set; }
        public string BU_NM { get; set; }
        public string BU_EMAIL { get; set; }
        public virtual ICollection<BUDepartments> BUDepartments { get; set; }
    }

    [Table("BU_DEPT")]
    public class BUDepartments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BU_DEPT_ID { get; set; }
        public string BU_DEPT { get; set; }
        public bool IS_BU_DEPT { get; set; }

        [ForeignKey("BUHead")]
        public int BU_ID { get; set; }
        public virtual BUHeads BUHead { get; set; }
    }

}
