﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZEntityFrameworkExtensionsDemo;

namespace ZEntityFrameworkExtensionsDemo.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20201027015736_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ZEntityFrameworkExtensionsDemo.Models.Chicken", b =>
                {
                    b.Property<int>("ChickenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChickenBreedId")
                        .HasColumnType("int");

                    b.Property<int>("ChickenCoopId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdoptable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("ChickenId");

                    b.HasIndex("ChickenBreedId");

                    b.HasIndex("ChickenCoopId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Chickens");
                });

            modelBuilder.Entity("ZEntityFrameworkExtensionsDemo.Models.ChickenBreed", b =>
                {
                    b.Property<int>("ChickenBreedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PrimaryColor")
                        .HasColumnType("int");

                    b.HasKey("ChickenBreedId");

                    b.ToTable("ChickenBreeds");
                });

            modelBuilder.Entity("ZEntityFrameworkExtensionsDemo.Models.ChickenCoop", b =>
                {
                    b.Property<int>("ChickenCoopId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("ChickenCoopId");

                    b.HasIndex("OwnerId");

                    b.ToTable("ChickenCoops");
                });

            modelBuilder.Entity("ZEntityFrameworkExtensionsDemo.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("ZEntityFrameworkExtensionsDemo.Models.Chicken", b =>
                {
                    b.HasOne("ZEntityFrameworkExtensionsDemo.Models.ChickenBreed", "ChickenBreed")
                        .WithMany()
                        .HasForeignKey("ChickenBreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZEntityFrameworkExtensionsDemo.Models.ChickenCoop", "ChickenCoop")
                        .WithMany("HousedChickens")
                        .HasForeignKey("ChickenCoopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ZEntityFrameworkExtensionsDemo.Models.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("ZEntityFrameworkExtensionsDemo.Models.ChickenCoop", b =>
                {
                    b.HasOne("ZEntityFrameworkExtensionsDemo.Models.Owner", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}