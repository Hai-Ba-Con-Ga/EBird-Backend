﻿// <auto-generated />
using System;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:IdentityIncrement", 1)
                .HasAnnotation("SqlServer:IdentitySeed", 1L)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EBird.Domain.Entities.AccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleString")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Role");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.AccountResourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ResourceId");

                    b.ToTable("AccountResources", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int")
                        .HasColumnName("BirdAge");

                    b.Property<Guid>("BirdTypeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("BirdTypeId");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdColor");

                    b.Property<DateTime>("CreatedDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("BirdCreatedDatetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("BirdDescription");

                    b.Property<int>("Elo")
                        .HasColumnType("int")
                        .HasColumnName("BirdElo");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdName");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("OwnerId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdStatus");

                    b.Property<double>("Weight")
                        .HasColumnType("float")
                        .HasColumnName("BirdWeight");

                    b.HasKey("Id");

                    b.HasIndex("BirdTypeId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Bird", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdResourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BirdId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BirdId");

                    b.HasIndex("ResourceId");

                    b.ToTable("BirdResources", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("BirdTypeCreatedDatetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("TypeCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdTypeCode");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("BirdTypeName");

                    b.HasKey("Id");

                    b.HasIndex("TypeCode")
                        .IsUnique();

                    b.ToTable("BirdType", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.GroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDatetime")
                        .HasColumnType("datetime2")
                        .HasColumnName("GroupCreateDatetime");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MaxELO")
                        .HasColumnType("int")
                        .HasColumnName("GroupMaxELO");

                    b.Property<int>("MinELO")
                        .HasColumnType("int")
                        .HasColumnName("GroupMinELO");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("GroupName");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("GroupStatus");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Group", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.PlaceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Address");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedDate");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Longitude");

                    b.Property<string>("Longitude")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("RefreshToken", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.RequestEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BirdId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("CreateDatetime");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("RequestDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("RequestDatetime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.HasIndex("BirdId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("GroupId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("EBird.Domain.Entities.ResourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreateDate");

                    b.Property<string>("Datalink")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("varchar");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CreateById");

                    b.ToTable("Resource", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.RoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("RoomCity");

                    b.Property<Guid>("CreateById")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RoomCreateById");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("RoomCreateDateTime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("RoomName");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("RoomStatus");

                    b.HasKey("Id");

                    b.HasIndex("CreateById");

                    b.ToTable("Room", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.RuleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<Guid>("CreateById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreateById");

                    b.ToTable("Rule", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.VerifcationStoreEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("VerifcationStore", (string)null);
                });

            modelBuilder.Entity("EBird.Domain.Entities.AccountResourceEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "Account")
                        .WithMany("AccountResources")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EBird.Domain.Entities.ResourceEntity", "Resource")
                        .WithMany("AccountResources")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.BirdTypeEntity", "BirdType")
                        .WithMany("Birds")
                        .HasForeignKey("BirdTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EBird.Domain.Entities.AccountEntity", "Owner")
                        .WithMany("Birds")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BirdType");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdResourceEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.BirdEntity", "Bird")
                        .WithMany("BirdResources")
                        .HasForeignKey("BirdId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EBird.Domain.Entities.ResourceEntity", "Resource")
                        .WithMany("BirdResources")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bird");

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("EBird.Domain.Entities.GroupEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "CreatedBy")
                        .WithMany("Groups")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RefreshTokenEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "Account")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RequestEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.BirdEntity", "Bird")
                        .WithMany("Requests")
                        .HasForeignKey("BirdId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EBird.Domain.Entities.AccountEntity", "CreatedBy")
                        .WithMany("Requests")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EBird.Domain.Entities.GroupEntity", "Group")
                        .WithMany("Requests")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("EBird.Domain.Entities.PlaceEntity", "Place")
                        .WithMany("Requests")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bird");

                    b.Navigation("CreatedBy");

                    b.Navigation("Group");

                    b.Navigation("Place");
                });

            modelBuilder.Entity("EBird.Domain.Entities.ResourceEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "Account")
                        .WithMany("Resources")
                        .HasForeignKey("CreateById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RoomEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "CreateBy")
                        .WithMany("Rooms")
                        .HasForeignKey("CreateById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreateBy");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RuleEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "Account")
                        .WithMany("Rules")
                        .HasForeignKey("CreateById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("EBird.Domain.Entities.AccountEntity", b =>
                {
                    b.Navigation("AccountResources");

                    b.Navigation("Birds");

                    b.Navigation("Groups");

                    b.Navigation("RefreshTokens");

                    b.Navigation("Requests");

                    b.Navigation("Resources");

                    b.Navigation("Rooms");

                    b.Navigation("Rules");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdEntity", b =>
                {
                    b.Navigation("BirdResources");

                    b.Navigation("Requests");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdTypeEntity", b =>
                {
                    b.Navigation("Birds");
                });

            modelBuilder.Entity("EBird.Domain.Entities.GroupEntity", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("EBird.Domain.Entities.PlaceEntity", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("EBird.Domain.Entities.ResourceEntity", b =>
                {
                    b.Navigation("AccountResources");

                    b.Navigation("BirdResources");
                });
#pragma warning restore 612, 618
        }
    }
}
