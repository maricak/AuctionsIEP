namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequiredAttributes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bid", "Auction_Id", "dbo.Auction");
            DropForeignKey("dbo.Bid", "User_Id", "dbo.User");
            DropIndex("dbo.Bid", new[] { "Auction_Id" });
            DropIndex("dbo.Bid", new[] { "User_Id" });
            AlterColumn("dbo.Bid", "Auction_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Bid", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Bid", "Auction_Id");
            CreateIndex("dbo.Bid", "User_Id");
            AddForeignKey("dbo.Bid", "Auction_Id", "dbo.Auction", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bid", "User_Id", "dbo.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bid", "User_Id", "dbo.User");
            DropForeignKey("dbo.Bid", "Auction_Id", "dbo.Auction");
            DropIndex("dbo.Bid", new[] { "User_Id" });
            DropIndex("dbo.Bid", new[] { "Auction_Id" });
            AlterColumn("dbo.Bid", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Bid", "Auction_Id", c => c.Guid());
            CreateIndex("dbo.Bid", "User_Id");
            CreateIndex("dbo.Bid", "Auction_Id");
            AddForeignKey("dbo.Bid", "User_Id", "dbo.User", "Id");
            AddForeignKey("dbo.Bid", "Auction_Id", "dbo.Auction", "Id");
        }
    }
}
