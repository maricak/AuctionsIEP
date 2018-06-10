namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedVirtaulFields : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Auction", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Order", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Auction", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Order", name: "IX_UserId", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Order", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Auction", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Order", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Auction", name: "User_Id", newName: "UserId");
        }
    }
}
