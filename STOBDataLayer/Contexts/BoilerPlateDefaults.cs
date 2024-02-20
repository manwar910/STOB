using STOBDataLayer.Models.BoilerPlateDefaults;
using System.Configuration;
using System.Data.Entity;

namespace STOBDataLayer.Contexts
{

    public partial class BoilerPlateDefaults : DbContext
    {
        public BoilerPlateDefaults()
            : base("name=stob_Entities")
        {
        }

        public virtual DbSet<ANNCMNT> ANNCMNT { get; set; }
        public virtual DbSet<ERR> ERR { get; set; }
        public virtual DbSet<FAQ> FAQ { get; set; }
        public virtual DbSet<HIST> HIST { get; set; }
        public virtual DbSet<LOG> LOG { get; set; }
        public virtual DbSet<RELS_NOTES> RELS_NOTES { get; set; }
        public virtual DbSet<USERS> USERS { get; set; }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ANNCMNT>()
                .Property(e => e.ANNCMNT_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<ERR>()
                .Property(e => e.USER_NM)
                .IsUnicode(false);

            modelBuilder.Entity<ERR>()
                .Property(e => e.ERR_TYP)
                .IsUnicode(false);

            modelBuilder.Entity<ERR>()
                .Property(e => e.ERR_SRC_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<ERR>()
                .Property(e => e.ERR_MSG_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<ERR>()
                .Property(e => e.STACK_TRACE_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<ERR>()
                .Property(e => e.SRVR_NM)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.SORT_ORDER_NO)
                .HasPrecision(3, 0);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.QSTN_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.ANSR_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<HIST>()
                .Property(e => e.ACTN_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.USER_NM)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.SESN_ID)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.TITLE_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.CLASS_NM)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.MTHD_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.INP_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.DUR_TM_QTY)
                .HasPrecision(9, 3);

            modelBuilder.Entity<LOG>()
                .Property(e => e.SRVR_NM)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.USER_DEF_TXT_1)
                .IsUnicode(false);

            modelBuilder.Entity<LOG>()
                .Property(e => e.USER_DEF_TXT_2)
                .IsUnicode(false);

            modelBuilder.Entity<RELS_NOTES>()
                .Property(e => e.RELS_VER_NO)
                .HasPrecision(5, 3);

            modelBuilder.Entity<RELS_NOTES>()
                .Property(e => e.RELS_NOTES_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<USERS>()
                .Property(e => e.FIRST_NM)
                .IsUnicode(false);

            modelBuilder.Entity<USERS>()
                .Property(e => e.LAST_NM)
                .IsUnicode(false);

            modelBuilder.Entity<USERS>()
                .Property(e => e.USER_NM)
                .IsUnicode(false);

            modelBuilder.Entity<USERS>()
                .Property(e => e.PH_NO)
                .IsUnicode(false);

            modelBuilder.Entity<USERS>()
                .Property(e => e.EMAIL_ADDR_TXT)
                .IsUnicode(false);

            modelBuilder.Entity<USERS>()
                .HasMany(e => e.HIST)
                .WithRequired(e => e.USERS)
                .WillCascadeOnDelete(false);
        }
    }
}
