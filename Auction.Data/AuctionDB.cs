using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Data.Models
{
    public enum CurrencyType { RSD = 0, USD = 1, EUR = 2 }

    public enum AuctionStatus { READY = 0, OPENED = 1, COMPLETED = 2 }

    public enum OrderStatus { SUBMITTED = 0, CANCELED = 1, COMPLETED = 2 }

    public enum OrderPackage { SILVER = 0, GOLD = 1, PLATINUM = 2 , ERROR}

    public class AuctionDB : IdentityDbContext<User>
    {
        public AuctionDB()
            : base("AuctionConnection", throwIfV1Schema: false)
        {
        }

        public static AuctionDB Create()
        {
            return new AuctionDB();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("Claim");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("Login");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");

            modelBuilder.Entity<Auction>().Property(a => a.Id).HasColumnName("Id");
        }

        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<DefaultValues> DefaultValues { get; set; }
    }
}