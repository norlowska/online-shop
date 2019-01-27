namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kkk : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.discount_for_user", name: "User_Id", newName: "pom_Id");
            RenameIndex(table: "dbo.discount_for_user", name: "IX_User_Id", newName: "IX_pom_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.discount_for_user", name: "IX_pom_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.discount_for_user", name: "pom_Id", newName: "User_Id");
        }
    }
}
