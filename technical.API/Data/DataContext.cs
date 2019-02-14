using Microsoft.EntityFrameworkCore;
using technical.API.Models;

namespace technical.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        { }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Log> Logging { get; set; }

        public DbSet<Sell> Sells { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Sell>()
                .HasKey(k => new {k.SellerId, k.AssetId});

            builder.Entity<Sell>()
                .HasOne(u => u.Seller)
                .WithMany(u => u.Assets)
                .HasForeignKey(u => u.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sell>()
                .HasOne(u => u.Asset)
                .WithMany(u => u.Sellers)
                .HasForeignKey(u => u.AssetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
