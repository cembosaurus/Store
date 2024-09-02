﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ordering.Data;

#nullable disable

namespace Ordering.Migrations
{
    [DbContext(typeof(OrderingContext))]
    [Migration("20221018090328_RemoveLockedDateTimeFromCartItemEntityModel")]
    partial class RemoveLockedDateTimeFromCartItemEntityModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Services.Ordering.Models.ActiveCart", b =>
                {
                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CartId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ActiveCarts");
                });

            modelBuilder.Entity("Services.Ordering.Models.Cart", b =>
                {
                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CartId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Services.Ordering.Models.CartItem", b =>
                {
                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("CartId", "ItemId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Services.Ordering.Models.Order", b =>
                {
                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Dispatched")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CartId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Services.Ordering.Models.OrderDetails", b =>
                {
                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CartId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Services.Ordering.Models.ActiveCart", b =>
                {
                    b.HasOne("Services.Ordering.Models.Cart", "Cart")
                        .WithOne("ActiveCart")
                        .HasForeignKey("Services.Ordering.Models.ActiveCart", "CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("Services.Ordering.Models.CartItem", b =>
                {
                    b.HasOne("Services.Ordering.Models.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("Services.Ordering.Models.Order", b =>
                {
                    b.HasOne("Services.Ordering.Models.Cart", "Cart")
                        .WithOne("Order")
                        .HasForeignKey("Services.Ordering.Models.Order", "CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("Services.Ordering.Models.OrderDetails", b =>
                {
                    b.HasOne("Services.Ordering.Models.Order", "Order")
                        .WithOne("OrderDetails")
                        .HasForeignKey("Services.Ordering.Models.OrderDetails", "CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Services.Ordering.Models.Cart", b =>
                {
                    b.Navigation("ActiveCart")
                        .IsRequired();

                    b.Navigation("CartItems");

                    b.Navigation("Order")
                        .IsRequired();
                });

            modelBuilder.Entity("Services.Ordering.Models.Order", b =>
                {
                    b.Navigation("OrderDetails")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}