namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onlyRows : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rows", "columnNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Rows", "gameID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rows", "gameID");
            DropColumn("dbo.Rows", "columnNumber");
        }
    }
}
