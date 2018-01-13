﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PartialFoods.Services.InventoryServer.Entities;
using System;

namespace PartialFoods.Services.InventoryServer.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20171009163554_MoreKeys")]
    partial class MoreKeys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("PartialFoods.Services.InventoryServer.Entities.Product", b =>
                {
                    b.Property<string>("SKU")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("OriginalQuantity");

                    b.HasKey("SKU");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PartialFoods.Services.InventoryServer.Entities.ProductActivity", b =>
                {
                    b.Property<string>("ActivityID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityType");

                    b.Property<long>("CreatedOn");

                    b.Property<string>("OrderID");

                    b.Property<int>("Quantity");

                    b.Property<string>("SKU");

                    b.HasKey("ActivityID");

                    b.ToTable("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
