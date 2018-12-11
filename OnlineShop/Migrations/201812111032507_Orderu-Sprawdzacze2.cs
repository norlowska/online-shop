namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderuSprawdzacze2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "State", c => c.String(nullable: false, maxLength: 160));
            AlterColumn("dbo.Orders", "PostalCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PostalCode", c => c.String(nullable: false, maxLength: 160));
            AlterColumn("dbo.Orders", "State", c => c.String(nullable: false));
        }
    }
}
