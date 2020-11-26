﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductStoreApp;

namespace ProductStoreApp.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductStoreApp.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Food"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Sport"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Health"
                        });
                });

            modelBuilder.Entity("ProductStoreApp.Models.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,9)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Currency");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = 840,
                            Name = "USD - Долар США",
                            Rate = 1m,
                            UpdatedAt = new DateTime(2020, 11, 19, 17, 20, 54, 537, DateTimeKind.Local).AddTicks(6059)
                        },
                        new
                        {
                            Id = 2,
                            Code = 978,
                            Name = "EUR - Євро",
                            Rate = 0.8m,
                            UpdatedAt = new DateTime(2020, 11, 19, 17, 20, 54, 538, DateTimeKind.Local).AddTicks(7695)
                        },
                        new
                        {
                            Id = 3,
                            Code = 980,
                            Name = "UAH - Українська Гривня",
                            Rate = 25m,
                            UpdatedAt = new DateTime(2020, 11, 19, 17, 20, 54, 538, DateTimeKind.Local).AddTicks(7736)
                        });
                });

            modelBuilder.Entity("ProductStoreApp.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceBase")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("CurrencyId");

                    b.ToTable("Product");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Code = "10000001",
                            CurrencyId = 1,
                            Name = "Orange",
                            Price = 1m,
                            PriceBase = 1m
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Code = "10000002",
                            CurrencyId = 1,
                            Name = "Ball",
                            Price = 2m,
                            PriceBase = 2m
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Code = "10000003",
                            CurrencyId = 1,
                            Name = "Vitamin C",
                            Price = 3m,
                            PriceBase = 3m
                        });
                });

            modelBuilder.Entity("ProductStoreApp.Models.ProductStore", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "StoreId");

                    b.HasIndex("StoreId");

                    b.ToTable("ProductStore");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            StoreId = 1,
                            Quantity = 11
                        },
                        new
                        {
                            ProductId = 2,
                            StoreId = 1,
                            Quantity = 21
                        },
                        new
                        {
                            ProductId = 3,
                            StoreId = 1,
                            Quantity = 31
                        },
                        new
                        {
                            ProductId = 1,
                            StoreId = 2,
                            Quantity = 12
                        },
                        new
                        {
                            ProductId = 2,
                            StoreId = 2,
                            Quantity = 22
                        },
                        new
                        {
                            ProductId = 3,
                            StoreId = 2,
                            Quantity = 32
                        },
                        new
                        {
                            ProductId = 1,
                            StoreId = 3,
                            Quantity = 13
                        },
                        new
                        {
                            ProductId = 2,
                            StoreId = 3,
                            Quantity = 23
                        },
                        new
                        {
                            ProductId = 3,
                            StoreId = 3,
                            Quantity = 33
                        });
                });

            modelBuilder.Entity("ProductStoreApp.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name", "Address")
                        .IsUnique();

                    b.ToTable("Store");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Address1",
                            Name = "Store1"
                        },
                        new
                        {
                            Id = 2,
                            Address = "Address2",
                            Name = "Store2"
                        },
                        new
                        {
                            Id = 3,
                            Address = "Address3",
                            Name = "Store3"
                        });
                });

            modelBuilder.Entity("ProductStoreApp.Models.Product", b =>
                {
                    b.HasOne("ProductStoreApp.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ProductStoreApp.Models.Currency", "Currency")
                        .WithMany("Products")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ProductStoreApp.Models.ProductStore", b =>
                {
                    b.HasOne("ProductStoreApp.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductStoreApp.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}