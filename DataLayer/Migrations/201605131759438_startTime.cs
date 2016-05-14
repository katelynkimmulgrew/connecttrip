namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class startTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.mathProblemResults", "start", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.mathProblemResults", "start");
        }
    }
}
