namespace FindIt.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedresulttype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "ResultType", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "ResultType");
        }
    }
}
