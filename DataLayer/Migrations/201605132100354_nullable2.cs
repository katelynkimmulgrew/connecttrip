namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "CurrentGameId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "CurrentGameId", c => c.Int(nullable: false));
        }
    }
}
