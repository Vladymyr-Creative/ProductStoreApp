using Microsoft.EntityFrameworkCore;
using System;
using ProductStoreApp.Models;

namespace ProductStoreApp
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Store> Store { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductStore> ProductStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>().HasIndex(p => new { p.Name, p.Address }).IsUnique();

            modelBuilder.Entity<Category>().HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<Currency>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<Currency>().Property(p => p.Rate).HasColumnType("decimal(18,9)");

            modelBuilder.Entity<Product>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(t => t.Products).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Product>().HasOne(p => p.Currency).WithMany(t => t.Products).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProductStore>().HasKey(p => new { p.ProductId, p.StoreId });

            modelBuilder.SeedData();
        }
    }
}
