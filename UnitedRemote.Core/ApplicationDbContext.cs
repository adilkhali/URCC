using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UnitedRemote.Core.Models.V1;

namespace UnitedRemote.Core
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shop>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<FavoriteShops>()
                .HasKey(fs => new { fs.UserId, fs.ShopId });

            modelBuilder.Entity<FavoriteShops>().Property(e => e.ShopId).ValueGeneratedNever();
            modelBuilder.Entity<FavoriteShops>().Property(e => e.UserId).ValueGeneratedNever();

            modelBuilder.Entity<FavoriteShops>()
                .HasOne(fs => fs.User)
                .WithMany(fs => fs.LikedShops)
                .HasForeignKey(fs => fs.UserId);

            modelBuilder.Entity<FavoriteShops>()
                .HasOne(fs => fs.LikedShop)
                .WithMany(fs => fs.LikedShops)
                .HasForeignKey(fs => fs.ShopId);

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<FavoriteShops> FavoriteShops { get; set; }

    }
}
