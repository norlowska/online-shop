namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "miasto", c => c.String());
            AddColumn("dbo.AspNetUsers", "ulica", c => c.String());
            AddColumn("dbo.AspNetUsers", "nr", c => c.String());
            AddColumn("dbo.AspNetUsers", "kod_pocztowy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "kod_pocztowy");
            DropColumn("dbo.AspNetUsers", "nr");
            DropColumn("dbo.AspNetUsers", "ulica");
            DropColumn("dbo.AspNetUsers", "miasto");
        }
    }
}
