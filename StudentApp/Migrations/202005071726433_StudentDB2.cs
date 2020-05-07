namespace StudentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentDB2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Name", c => c.String(nullable: false));
        }
    }
}
