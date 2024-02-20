using STOBDataLayer.Models;
using System;
using System.Data.Entity;

namespace STOBDataLayer.Contexts
{
    public class STOBContext : DbContext
    {
        public DbSet<Awards> Awards { get; set; }
        public DbSet<Nominations> Nominations { get; set; }
        public DbSet<Nominees> Nominees { get; set; }
        public DbSet<NominationStatuses> NominationStatuses { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<BUHeads> BUHeads { get; set; }
        public DbSet<BUDepartments> BUDepartments { get; set; }
        public DbSet<ToggleNominations> ToggleNominations { get; set; }
        public STOBContext()
          : base("stob_Entities")
        {
        }
    }
}
