namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cancelled : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        content = c.String(nullable: false),
                        Ques_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ques", t => t.Ques_Id)
                .Index(t => t.Ques_Id);
            
            CreateTable(
                "dbo.Columns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColumnNumber = c.Int(nullable: false),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Rows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        columnNumber = c.Int(nullable: false),
                        gameID = c.Int(nullable: false),
                        RowNumber = c.Int(nullable: false),
                        Value = c.Boolean(),
                        Column_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Columns", t => t.Column_Id)
                .Index(t => t.Column_Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        maxRows = c.Int(nullable: false),
                        maxCols = c.Int(nullable: false),
                        Player1Id = c.Int(nullable: false),
                        Player2Id = c.Int(nullable: false),
                        currentUser = c.Boolean(nullable: false),
                        finished = c.Boolean(nullable: false),
                        cancelled = c.Boolean(nullable: false),
                        winnerID = c.Int(nullable: false),
                        level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.mathProblemResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        question = c.String(),
                        answer = c.String(),
                        isRight = c.Boolean(nullable: false),
                        level = c.Int(nullable: false),
                        current = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        CurrentGameId = c.Int(nullable: false),
                        CatchPhrase = c.String(),
                        LevelOneWins = c.Double(nullable: false),
                        LevelTwoWins = c.Double(nullable: false),
                        LevelThreeWins = c.Double(nullable: false),
                        LevelOneLose = c.Double(nullable: false),
                        LevelTwoLose = c.Double(nullable: false),
                        LevelThreeLose = c.Double(nullable: false),
                        Answered = c.Double(nullable: false),
                        DidNotAnswer = c.Double(nullable: false),
                        answeredMathQuestion = c.Boolean(nullable: false),
                        levelOneAnsweredCorrectly = c.Int(nullable: false),
                        levelOneAnsweredIncorrectly = c.Int(nullable: false),
                        levelTwoAnsweredCorrectly = c.Int(nullable: false),
                        levelTwoAnsweredIncorrectly = c.Int(nullable: false),
                        levelThreeAnsweredCorrectly = c.Int(nullable: false),
                        levelThreeAnsweredIncorrectly = c.Int(nullable: false),
                        overallAnsweredCorrectly = c.Int(nullable: false),
                        overllAndsweredIncorrectly = c.Int(nullable: false),
                        isPlaying = c.Boolean(nullable: false),
                        assignedBool = c.Boolean(),
                        currentMathProblemID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ques",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.startGamePlayers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        player1Id = c.Int(nullable: false),
                        player2Id = c.Int(nullable: false),
                        level = c.Int(nullable: false),
                        isStarted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Ques_Id", "dbo.Ques");
            DropForeignKey("dbo.Columns", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Rows", "Column_Id", "dbo.Columns");
            DropIndex("dbo.Rows", new[] { "Column_Id" });
            DropIndex("dbo.Columns", new[] { "Game_Id" });
            DropIndex("dbo.Answers", new[] { "Ques_Id" });
            DropTable("dbo.startGamePlayers");
            DropTable("dbo.Ques");
            DropTable("dbo.People");
            DropTable("dbo.mathProblemResults");
            DropTable("dbo.Games");
            DropTable("dbo.Rows");
            DropTable("dbo.Columns");
            DropTable("dbo.Answers");
        }
    }
}
