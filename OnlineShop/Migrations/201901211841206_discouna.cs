namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discouna : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.discount_for_user", "count", c => c.Int(nullable: false));
            AddColumn("dbo.discount_for_user", "percent", c => c.Int(nullable: false));
            AddColumn("dbo.discount_for_user", "expiration_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.discount_for_user", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.discount_for_user", "User_Id");
            AddForeignKey("dbo.discount_for_user", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.discount_for_user", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.discount_for_user", new[] { "User_Id" });
            DropColumn("dbo.discount_for_user", "User_Id");
            DropColumn("dbo.discount_for_user", "expiration_date");
            DropColumn("dbo.discount_for_user", "percent");
            DropColumn("dbo.discount_for_user", "count");
        }
    }
}
