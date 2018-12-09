namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "jsonDictionary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "jsonDictionary");
        }
    }
}
