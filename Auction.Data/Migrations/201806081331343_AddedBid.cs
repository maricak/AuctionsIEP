namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Auction", "User_Id", "dbo.User");
            DropIndex("dbo.Auction", new[] { "User_Id" });
            RenameColumn(table: "dbo.Role", name: "RoleId", newName: "Id");
            RenameColumn(table: "dbo.User", name: "UserId", newName: "Id");
            RenameColumn(table: "dbo.Claim", name: "ClaimId", newName: "Id");
            CreateTable(
                "dbo.Bid",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PlacingTime = c.DateTime(nullable: false),
                        NumberOfTokens = c.Long(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Auction_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auction", t => t.Auction_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Auction_Id);
            
            AddColumn("dbo.Order", "NumberOfTokens", c => c.Long(nullable: false));
            AlterColumn("dbo.Auction", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Auction", "User_Id");
            AddForeignKey("dbo.Auction", "User_Id", "dbo.User", "Id");
            DropColumn("dbo.Order", "TokenNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "TokenNumber", c => c.Int(nullable: false));
            DropForeignKey("dbo.Auction", "User_Id", "dbo.User");
            DropForeignKey("dbo.Bid", "User_Id", "dbo.User");
            DropForeignKey("dbo.Bid", "Auction_Id", "dbo.Auction");
            DropIndex("dbo.Bid", new[] { "Auction_Id" });
            DropIndex("dbo.Bid", new[] { "User_Id" });
            DropIndex("dbo.Auction", new[] { "User_Id" });
            AlterColumn("dbo.Auction", "User_Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Order", "NumberOfTokens");
            DropTable("dbo.Bid");
            RenameColumn(table: "dbo.Claim", name: "Id", newName: "ClaimId");
            RenameColumn(table: "dbo.User", name: "Id", newName: "UserId");
            RenameColumn(table: "dbo.Role", name: "Id", newName: "RoleId");
            CreateIndex("dbo.Auction", "User_Id");
            AddForeignKey("dbo.Auction", "User_Id", "dbo.User", "UserId", cascadeDelete: true);
        }
    }
}
