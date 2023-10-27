﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSW_Clean.Infrastructure.Persistence;
using System;

#nullable disable

namespace Infrastructure.Persistence.Migrations;
[DbContext(typeof(ApplicationDbContext))]
[Migration("20230315031155_Initial")]
partial class Initial
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "7.0.3")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Domain.Entities.TodoItem", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTimeOffset>("CreatedAt")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("CreatedBy")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("Done")
                    .HasColumnType("bit");

                b.Property<string>("Note")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("Priority")
                    .HasColumnType("int");

                b.Property<DateTime>("Reminder")
                    .HasColumnType("datetime2");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<DateTimeOffset?>("UpdatedAt")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("UpdatedBy")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("TodoItems");
            });
#pragma warning restore 612, 618
    }
}