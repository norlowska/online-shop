namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductfilePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "filePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "filePath");
        }
    }
}
