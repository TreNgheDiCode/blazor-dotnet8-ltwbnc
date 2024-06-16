using BaseLibrary.Models;
using BaseLibrary.Models.Products;
using KimVinhHung.Api.Models.Address;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }
        public DbSet<AdministrativeRegion> AdministrativeRegions { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }
        public DbSet<DiscountWarehouse> DiscountWarehouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.Restrict); // This line prevents cascade delete
        }
    }
}
