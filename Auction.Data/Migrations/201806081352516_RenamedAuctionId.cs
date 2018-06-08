namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDefaultValuesAndDroppedBid : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Auction");
            AddColumn("dbo.Auction", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Auction", "Id");
            DropColumn("dbo.Auction", "AuctionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Auction", "AuctionId", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.Auction");
            DropColumn("dbo.Auction", "Id");
            AddPrimaryKey("dbo.Auction", "AuctionId");
        }
    }
}
