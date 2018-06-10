namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedIdsFromBid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bid", "AuctionId", "dbo.Auction");
            DropForeignKey("dbo.Bid", "UserId", "dbo.User");
            DropIndex("dbo.Bid", new[] { "UserId" });
            DropIndex("dbo.Bid", new[] { "AuctionId" });
            RenameColumn(table: "dbo.Bid", name: "AuctionId", newName: "Auction_Id");
            RenameColumn(table: "dbo.Bid", name: "UserId", newName: "User_Id");
            AlterColumn("dbo.Bid", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Bid", "Auction_Id", c => c.Guid());
            CreateIndex("dbo.Bid", "Auction_Id");
            CreateIndex("dbo.Bid", "User_Id");
            AddForeignKey("dbo.Bid", "Auction_Id", "dbo.Auction", "Id");
            AddForeignKey("dbo.Bid", "User_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bid", "User_Id", "dbo.User");
            DropForeignKey("dbo.Bid", "Auction_Id", "dbo.Auction");
            DropIndex("dbo.Bid", new[] { "User_Id" });
            DropIndex("dbo.Bid", new[] { "Auction_Id" });
            AlterColumn("dbo.Bid", "Auction_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Bid", "User_Id", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.Bid", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Bid", name: "Auction_Id", newName: "AuctionId");
            CreateIndex("dbo.Bid", "AuctionId");
            CreateIndex("dbo.Bid", "UserId");
            AddForeignKey("dbo.Bid", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bid", "AuctionId", "dbo.Auction", "Id", cascadeDelete: true);
        }
    }
}
