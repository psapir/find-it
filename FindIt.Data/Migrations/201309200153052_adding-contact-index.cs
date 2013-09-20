namespace FindIt.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingcontactindex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "ContactIndex_IdContactIndex", c => c.Guid());
            AddForeignKey("dbo.Results", "ContactIndex_IdContactIndex", "dbo.ContactIndexes", "IdContactIndex");
            CreateIndex("dbo.Results", "ContactIndex_IdContactIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Results", new[] { "ContactIndex_IdContactIndex" });
            DropForeignKey("dbo.Results", "ContactIndex_IdContactIndex", "dbo.ContactIndexes");
            DropColumn("dbo.Results", "ContactIndex_IdContactIndex");
        }
    }
}
