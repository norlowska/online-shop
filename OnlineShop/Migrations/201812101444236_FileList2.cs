namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileList2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "jsonList", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "jsonList");
        }
    }
}
