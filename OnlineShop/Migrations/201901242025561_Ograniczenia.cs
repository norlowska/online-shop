namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ograniczenia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Ograniczeni", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ograniczeni");
        }
    }
}
