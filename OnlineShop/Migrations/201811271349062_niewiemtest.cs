namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class niewiemtest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "cat_pro_Id", "dbo.Categories");
            AddColumn("dbo.Categories", "parent_Id", c => c.Int());
            AddColumn("dbo.Products", "count", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Category_Id", c => c.Int());
            AddColumn("dbo.Products", "Category_Id1", c => c.Int());
            CreateIndex("dbo.Categories", "parent_Id");
            CreateIndex("dbo.Products", "Category_Id");
            CreateIndex("dbo.Products", "Category_Id1");
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Categories", "parent_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Products", "Category_Id1", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Category_Id1", "dbo.Categories");
            DropForeignKey("dbo.Categories", "parent_Id", "dbo.Categories");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "Category_Id1" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.Categories", new[] { "parent_Id" });
            DropColumn("dbo.Products", "Category_Id1");
            DropColumn("dbo.Products", "Category_Id");
            DropColumn("dbo.Products", "count");
            DropColumn("dbo.Categories", "parent_Id");
            AddForeignKey("dbo.Products", "cat_pro_Id", "dbo.Categories", "Id");
        }
    }
}
