namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Answers", newName: "theAnswers");
            AddColumn("dbo.Games", "gameCancelled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Games", "cancelled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "cancelled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Games", "gameCancelled");
            RenameTable(name: "dbo.theAnswers", newName: "Answers");
        }
    }
}
