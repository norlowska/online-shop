namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "newsletter", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "newsletterów");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "newsletterów", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "newsletter");
        }
    }
}
