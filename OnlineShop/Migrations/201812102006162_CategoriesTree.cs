namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriesTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "parent_Id", c => c.Int());
            AddColumn("dbo.Products", "jsonDictionary", c => c.String());
            AddColumn("dbo.Products", "jsonList", c => c.String());
            AddColumn("dbo.Products", "count", c => c.Int(nullable: false));
            CreateIndex("dbo.Categories", "parent_Id");
            AddForeignKey("dbo.Categories", "parent_Id", "dbo.Categories", "Id");
            DropColumn("dbo.Products", "filePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "filePath", c => c.String());
            DropForeignKey("dbo.Categories", "parent_Id", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "parent_Id" });
            DropColumn("dbo.Products", "count");
            DropColumn("dbo.Products", "jsonList");
            DropColumn("dbo.Products", "jsonDictionary");
            DropColumn("dbo.Categories", "parent_Id");
        }
    }
}
