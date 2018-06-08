namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAuction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auction",
                c => new
                    {
                        AuctionId = c.Guid(nullable: false),
                        Name = c.String(),
                        Image = c.Binary(),
                        Duration = c.Long(nullable: false),
                        StartPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        CreatingTime = c.DateTime(nullable: false),
                        OpeningTime = c.DateTime(),
                        ClosingTime = c.DateTime(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.AuctionId)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auction", "User_Id", "dbo.User");
            DropIndex("dbo.Auction", new[] { "User_Id" });
            DropTable("dbo.Auction");
        }
    }
}
