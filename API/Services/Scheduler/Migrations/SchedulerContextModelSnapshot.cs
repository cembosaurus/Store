﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Scheduler.Data;

#nullable disable

namespace Scheduler.Migrations
{
    [DbContext(typeof(SchedulerDBContext))]
    partial class SchedulerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Scheduler.Models.CartItemLock", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Locked")
                        .HasColumnType("datetime2");

                    b.Property<int>("LockedForDays")
                        .HasColumnType("int");

                    b.HasKey("ItemId", "CartId");

                    b.ToTable("CartItemLocks");
                });
#pragma warning restore 612, 618
        }
    }
}
