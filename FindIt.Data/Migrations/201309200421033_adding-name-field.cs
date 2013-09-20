namespace FindIt.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingnamefield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "Name", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "Name");
        }
    }
}
