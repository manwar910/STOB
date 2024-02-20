namespace STOBDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AWARD",
                c => new
                    {
                        AWARD_ID = c.Int(nullable: false, identity: true),
                        AWARD_NM = c.String(),
                        AWARD_TYPE = c.String(),
                        AWARD_EXEMPTION = c.String(),
                        AWARD_DESC = c.String(),
                    })
                .PrimaryKey(t => t.AWARD_ID);
            
            CreateTable(
                "dbo.ASSOC_AWARD_NOMN",
                c => new
                    {
                        NOMN_ID = c.Int(nullable: false, identity: true),
                        AWARD_ID = c.Int(nullable: false),
                        TEAM_NM = c.String(),
                        NOMNR_EMPL_ID = c.Int(nullable: false),
                        NOMNR_EMAIL = c.String(),
                        NOMNR_NM = c.String(),
                        NOMNR_BU_DEPT = c.String(),
                        ACTV_FLG = c.Boolean(),
                        NOMN_DT = c.DateTime(nullable: false),
                        NOMN_RSN = c.String(),
                    })
                .PrimaryKey(t => t.NOMN_ID)
                .ForeignKey("dbo.AWARD", t => t.AWARD_ID, cascadeDelete: true)
                .Index(t => t.AWARD_ID);
            
            CreateTable(
                "dbo.NOMN_STATUSES",
                c => new
                    {
                        NOMN_STAT_ID = c.Int(nullable: false, identity: true),
                        DATE = c.DateTime(nullable: false),
                        BY = c.String(),
                        NOMN_ID = c.Int(nullable: false),
                        STAT_ID = c.Int(nullable: false),
                        COMMENT = c.String(),
                    })
                .PrimaryKey(t => t.NOMN_STAT_ID)
                .ForeignKey("dbo.ASSOC_AWARD_NOMN", t => t.NOMN_ID, cascadeDelete: true)
                .ForeignKey("dbo.STATUSES", t => t.STAT_ID, cascadeDelete: true)
                .Index(t => t.NOMN_ID)
                .Index(t => t.STAT_ID);
            
            CreateTable(
                "dbo.STATUSES",
                c => new
                    {
                        STAT_ID = c.Int(nullable: false, identity: true),
                        STAT_TXT = c.String(),
                    })
                .PrimaryKey(t => t.STAT_ID);
            
            CreateTable(
                "dbo.NOMINEES",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        NOMN_ID = c.Int(nullable: false),
                        EMP_ID = c.Int(nullable: false),
                        FIRST_NM = c.String(),
                        LAST_NM = c.String(),
                        TITLE_TXT = c.String(),
                        BU_Dept = c.String(),
                        EMAIL = c.String(),
                        EXEMPT_CD = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ASSOC_AWARD_NOMN", t => t.NOMN_ID, cascadeDelete: true)
                .ForeignKey("dbo.EMPLOYEES", t => t.EMP_ID, cascadeDelete: true)
                .Index(t => t.NOMN_ID)
                .Index(t => t.EMP_ID);
            
            CreateTable(
                "dbo.EMPLOYEES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FIRST_NAME = c.String(),
                        LAST_NAME = c.String(),
                        TITLE = c.String(),
                        EXMSTATUS = c.String(),
                        DEPARTMENT = c.String(),
                        EMAIL = c.String(),
                        manager_level = c.Short(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BU_DEPT",
                c => new
                    {
                        BU_DEPT_ID = c.Int(nullable: false, identity: true),
                        BU_DEPT = c.String(),
                        IS_BU_DEPT = c.Boolean(nullable: false),
                        BU_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BU_DEPT_ID)
                .ForeignKey("dbo.BU", t => t.BU_ID, cascadeDelete: true)
                .Index(t => t.BU_ID);
            
            CreateTable(
                "dbo.BU",
                c => new
                    {
                        BU_ID = c.Int(nullable: false, identity: true),
                        BU_NM = c.String(),
                        BU_EMAIL = c.String(),
                    })
                .PrimaryKey(t => t.BU_ID);
            
            CreateTable(
                "dbo.Dept",
                c => new
                    {
                        DepId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DepId);
            
            CreateTable(
                "dbo.TOGGLE_NOMN",
                c => new
                    {
                        Id = c.Short(nullable: false, identity: true),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BU_DEPT", "BU_ID", "dbo.BU");
            DropForeignKey("dbo.NOMINEES", "EMP_ID", "dbo.EMPLOYEES");
            DropForeignKey("dbo.NOMINEES", "NOMN_ID", "dbo.ASSOC_AWARD_NOMN");
            DropForeignKey("dbo.NOMN_STATUSES", "STAT_ID", "dbo.STATUSES");
            DropForeignKey("dbo.NOMN_STATUSES", "NOMN_ID", "dbo.ASSOC_AWARD_NOMN");
            DropForeignKey("dbo.ASSOC_AWARD_NOMN", "AWARD_ID", "dbo.AWARD");
            DropIndex("dbo.BU_DEPT", new[] { "BU_ID" });
            DropIndex("dbo.NOMINEES", new[] { "EMP_ID" });
            DropIndex("dbo.NOMINEES", new[] { "NOMN_ID" });
            DropIndex("dbo.NOMN_STATUSES", new[] { "STAT_ID" });
            DropIndex("dbo.NOMN_STATUSES", new[] { "NOMN_ID" });
            DropIndex("dbo.ASSOC_AWARD_NOMN", new[] { "AWARD_ID" });
            DropTable("dbo.TOGGLE_NOMN");
            DropTable("dbo.Dept");
            DropTable("dbo.BU");
            DropTable("dbo.BU_DEPT");
            DropTable("dbo.EMPLOYEES");
            DropTable("dbo.NOMINEES");
            DropTable("dbo.STATUSES");
            DropTable("dbo.NOMN_STATUSES");
            DropTable("dbo.ASSOC_AWARD_NOMN");
            DropTable("dbo.AWARD");
        }
    }
}
