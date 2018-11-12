namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "cat_pro_Id", c => c.Int());
            CreateIndex("dbo.Products", "cat_pro_Id");
            AddForeignKey("dbo.Products", "cat_pro_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "cat_pro_Id", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "cat_pro_Id" });
            DropColumn("dbo.Products", "cat_pro_Id");
        }
    }
}
