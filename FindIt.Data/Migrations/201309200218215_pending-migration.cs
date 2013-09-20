namespace FindIt.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendingmigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Results", "ContactIndex_IdContactIndex", "dbo.ContactIndexes");
            DropForeignKey("dbo.Keywords", "IdKeyword", "dbo.Results");
            DropIndex("dbo.Results", new[] { "ContactIndex_IdContactIndex" });
            DropIndex("dbo.Keywords", new[] { "IdKeyword" });
            RenameColumn(table: "dbo.Results", name: "ContactIndex_IdContactIndex", newName: "IdContactIndex");
            AddColumn("dbo.Keywords", "IdResult", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Results", "IdContactIndex", "dbo.ContactIndexes", "IdContactIndex", cascadeDelete: true);
            AddForeignKey("dbo.Keywords", "IdResult", "dbo.Results", "IdResult", cascadeDelete: true);
            CreateIndex("dbo.Results", "IdContactIndex");
            CreateIndex("dbo.Keywords", "IdResult");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Keywords", new[] { "IdResult" });
            DropIndex("dbo.Results", new[] { "IdContactIndex" });
            DropForeignKey("dbo.Keywords", "IdResult", "dbo.Results");
            DropForeignKey("dbo.Results", "IdContactIndex", "dbo.ContactIndexes");
            DropColumn("dbo.Keywords", "IdResult");
            RenameColumn(table: "dbo.Results", name: "IdContactIndex", newName: "ContactIndex_IdContactIndex");
            CreateIndex("dbo.Keywords", "IdKeyword");
            CreateIndex("dbo.Results", "ContactIndex_IdContactIndex");
            AddForeignKey("dbo.Keywords", "IdKeyword", "dbo.Results", "IdResult");
            AddForeignKey("dbo.Results", "ContactIndex_IdContactIndex", "dbo.ContactIndexes", "IdContactIndex");
        }
    }
}
