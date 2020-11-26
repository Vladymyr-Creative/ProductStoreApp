using Microsoft.EntityFrameworkCore;
using ProductStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreApp
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Food" },
                new Category { Id = 2, Name = "Sport" },
                new Category { Id = 3, Name = "Health" }
            );

            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Name = "USD - Долар США", Code = 840, Rate = 1m, UpdatedAt = DateTime.Now.AddDays(-1) },
                new Currency { Id = 2, Name = "EUR - Євро", Code = 978, Rate = 0.8m, UpdatedAt = DateTime.Now.AddDays(-1) },
                new Currency { Id = 3, Name = "UAH - Українська Гривня", Code = 980, Rate = 25m, UpdatedAt = DateTime.Now.AddDays(-1) }
            );

            modelBuilder.Entity<Store>().HasData(
                new Store { Id = 1, Name = "Store1", Address = "Address1" },
                new Store { Id = 2, Name = "Store2", Address = "Address2" },
                new Store { Id = 3, Name = "Store3", Address = "Address3" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Code = "10000001", Name = "Orange", PriceBase = 1, Price = 1, CategoryId = 1, CurrencyId = 1 },
                new Product { Id = 2, Code = "10000002", Name = "Ball", PriceBase = 2, Price = 2, CategoryId = 2, CurrencyId = 1 },
                new Product { Id = 3, Code = "10000003", Name = "Vitamin C", PriceBase = 3, Price = 3, CategoryId = 3, CurrencyId = 1 }
            );

            modelBuilder.Entity<ProductStore>().HasData(
                new ProductStore { StoreId = 1, ProductId = 1, Quantity = 11 },
                new ProductStore { StoreId = 1, ProductId = 2, Quantity = 21 },
                new ProductStore { StoreId = 1, ProductId = 3, Quantity = 31 },

                new ProductStore { StoreId = 2, ProductId = 1, Quantity = 12 },
                new ProductStore { StoreId = 2, ProductId = 2, Quantity = 22 },
                new ProductStore { StoreId = 2, ProductId = 3, Quantity = 32 },

                new ProductStore { StoreId = 3, ProductId = 1, Quantity = 13 },
                new ProductStore { StoreId = 3, ProductId = 2, Quantity = 23 },
                new ProductStore { StoreId = 3, ProductId = 3, Quantity = 33 }
            );
        }
    }
}
