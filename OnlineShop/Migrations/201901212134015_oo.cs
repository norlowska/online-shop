namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "imie", c => c.String());
            AddColumn("dbo.AspNetUsers", "Nazwisko", c => c.String());
            AddColumn("dbo.AspNetUsers", "Adres", c => c.String());
            AddColumn("dbo.AspNetUsers", "Województwo", c => c.String());
            AddColumn("dbo.AspNetUsers", "Kraj", c => c.String());
            DropColumn("dbo.AspNetUsers", "ulica");
            DropColumn("dbo.AspNetUsers", "nr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "nr", c => c.String());
            AddColumn("dbo.AspNetUsers", "ulica", c => c.String());
            DropColumn("dbo.AspNetUsers", "Kraj");
            DropColumn("dbo.AspNetUsers", "Województwo");
            DropColumn("dbo.AspNetUsers", "Adres");
            DropColumn("dbo.AspNetUsers", "Nazwisko");
            DropColumn("dbo.AspNetUsers", "imie");
        }
    }
}
