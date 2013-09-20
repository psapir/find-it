namespace FindIt.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        IdResult = c.Guid(nullable: false),
                        CustomerKey = c.String(maxLength: 500),
                        Name = c.String(maxLength: 500),
                        ResultType = c.String(maxLength: 200),
                        Path = c.String(),
                        URL = c.String(maxLength: 1000),
                        ThumbnailURL = c.String(maxLength: 1000),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IdContactIndex = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.IdResult)
                .ForeignKey("dbo.ContactIndexes", t => t.IdContactIndex, cascadeDelete: true)
                .Index(t => t.IdContactIndex);
            
            CreateTable(
                "dbo.ContactIndexes",
                c => new
                    {
                        IdContactIndex = c.Guid(nullable: false),
                        mid = c.String(maxLength: 150),
                        LastIndexRun = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdContactIndex);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        IdKeyword = c.Guid(nullable: false),
                        IdResult = c.Guid(nullable: false),
                        KeywordText = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.IdKeyword)
                .ForeignKey("dbo.Results", t => t.IdResult, cascadeDelete: true)
                .Index(t => t.IdResult);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Keywords", new[] { "IdResult" });
            DropIndex("dbo.Results", new[] { "IdContactIndex" });
            DropForeignKey("dbo.Keywords", "IdResult", "dbo.Results");
            DropForeignKey("dbo.Results", "IdContactIndex", "dbo.ContactIndexes");
            DropTable("dbo.Keywords");
            DropTable("dbo.ContactIndexes");
            DropTable("dbo.Results");
        }
    }
}
