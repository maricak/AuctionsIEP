namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDefaultValuesAndBid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bid",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PlacingTime = c.DateTime(nullable: false),
                        NumberOfTokens = c.Long(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AuctionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auction", t => t.AuctionId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AuctionId);
            
            CreateTable(
                "dbo.DefaultValues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NumberOfAuctionsPerPage = c.Long(nullable: false),
                        AuctionDuration = c.Long(nullable: false),
                        SilverTokenNumber = c.Long(nullable: false),
                        GoldTokenNumber = c.Long(nullable: false),
                        PlatinuTokenNumber = c.Long(nullable: false),
                        TokenValue = c.Long(nullable: false),
                        Currency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bid", "UserId", "dbo.User");
            DropForeignKey("dbo.Bid", "AuctionId", "dbo.Auction");
            DropIndex("dbo.Bid", new[] { "AuctionId" });
            DropIndex("dbo.Bid", new[] { "UserId" });
            DropTable("dbo.DefaultValues");
            DropTable("dbo.Bid");
        }
    }
}
