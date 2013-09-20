namespace FindIt.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendingmigration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Keywords", "IdResult", "dbo.Results");
            DropIndex("dbo.Keywords", new[] { "IdResult" });
            AddColumn("dbo.Keywords", "Result_IdResult", c => c.Guid());
            AddForeignKey("dbo.Keywords", "Result_IdResult", "dbo.Results", "IdResult");
            CreateIndex("dbo.Keywords", "Result_IdResult");
            DropColumn("dbo.Keywords", "IdResult");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Keywords", "IdResult", c => c.Guid(nullable: false));
            DropIndex("dbo.Keywords", new[] { "Result_IdResult" });
            DropForeignKey("dbo.Keywords", "Result_IdResult", "dbo.Results");
            DropColumn("dbo.Keywords", "Result_IdResult");
            CreateIndex("dbo.Keywords", "IdResult");
            AddForeignKey("dbo.Keywords", "IdResult", "dbo.Results", "IdResult", cascadeDelete: true);
        }
    }
}
