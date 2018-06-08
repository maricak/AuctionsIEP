namespace Auction.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedColumnNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetRoles", newName: "Role");
            RenameTable(name: "dbo.AspNetUserRoles", newName: "UserRole");
            RenameTable(name: "dbo.AspNetUsers", newName: "User");
            RenameTable(name: "dbo.AspNetUserClaims", newName: "Claim");
            RenameTable(name: "dbo.AspNetUserLogins", newName: "Login");
            RenameColumn(table: "dbo.Role", name: "Id", newName: "RoleId");
            RenameColumn(table: "dbo.User", name: "Id", newName: "UserId");
            RenameColumn(table: "dbo.Claim", name: "Id", newName: "ClaimId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Claim", name: "ClaimId", newName: "Id");
            RenameColumn(table: "dbo.User", name: "UserId", newName: "Id");
            RenameColumn(table: "dbo.Role", name: "RoleId", newName: "Id");
            RenameTable(name: "dbo.Login", newName: "AspNetUserLogins");
            RenameTable(name: "dbo.Claim", newName: "AspNetUserClaims");
            RenameTable(name: "dbo.User", newName: "AspNetUsers");
            RenameTable(name: "dbo.UserRole", newName: "AspNetUserRoles");
            RenameTable(name: "dbo.Role", newName: "AspNetRoles");
        }
    }
}
