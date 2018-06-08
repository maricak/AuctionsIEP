namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Auction", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Bid", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Bid", name: "Auction_Id", newName: "AuctionId");
            RenameColumn(table: "dbo.Order", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Auction", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Bid", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Bid", name: "IX_Auction_Id", newName: "IX_AuctionId");
            RenameIndex(table: "dbo.Order", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Order", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Bid", name: "IX_AuctionId", newName: "IX_Auction_Id");
            RenameIndex(table: "dbo.Bid", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Auction", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Order", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Bid", name: "AuctionId", newName: "Auction_Id");
            RenameColumn(table: "dbo.Bid", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Auction", name: "UserId", newName: "User_Id");
        }
    }
}
