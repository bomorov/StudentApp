namespace StudentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentDB1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Facultats", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Subjects", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subjects", "Name", c => c.String());
            AlterColumn("dbo.Students", "Name", c => c.String());
            AlterColumn("dbo.Groups", "Name", c => c.String());
            AlterColumn("dbo.Facultats", "Name", c => c.String());
        }
    }
}
