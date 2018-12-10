namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriesTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "parent_Id", c => c.Int());
            AddColumn("dbo.Products", "count", c => c.Int(nullable: false));
            CreateIndex("dbo.Categories", "parent_Id");
            AddForeignKey("dbo.Categories", "parent_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "parent_Id", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "parent_Id" });
            DropColumn("dbo.Products", "count");
            DropColumn("dbo.Categories", "parent_Id");
        }
    }
}
