﻿// <auto-generated />
using System;
using ASM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASM.Data.Migrations
{
    [DbContext(typeof(AsmContext))]
    [Migration("20230604004240_isfreeperiod")]
    partial class isfreeperiod
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ASM.Data.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("ASM.Data.Entities.MeliAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("MeliSellerId")
                        .HasColumnType("bigint");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SellerId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("MeliAccounts");
                });

            modelBuilder.Entity("ASM.Data.Entities.PaymentHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetaData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("PaymentHistory");
                });

            modelBuilder.Entity("ASM.Data.Entities.PaymentInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ExpireIn")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsFreePeriod")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastPayment")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SellerId")
                        .IsUnique();

                    b.ToTable("PaymentInformations");
                });

            modelBuilder.Entity("ASM.Data.Entities.Seller", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfirmationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("ASM.Data.Entities.SellerFcmToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FcmToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("SellerFcmToken");
                });

            modelBuilder.Entity("ASM.Data.Entities.SellerMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Activated")
                        .HasColumnType("bit");

                    b.Property<Guid?>("MeliAccountId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MeliAccountId");

                    b.ToTable("SellerMessages");
                });

            modelBuilder.Entity("ASM.Data.Entities.SellerOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("AfterSellerMessageStatus")
                        .HasColumnType("bit");

                    b.Property<bool?>("DeliveredMessageStatus")
                        .HasColumnType("bit");

                    b.Property<long?>("MeliSellerId")
                        .HasColumnType("bigint");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("ShippingMessageStatus")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("SellerOrder");
                });

            modelBuilder.Entity("ASM.Data.Entities.Attachment", b =>
                {
                    b.HasOne("ASM.Data.Entities.SellerMessage", "Message")
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("ASM.Data.Entities.MeliAccount", b =>
                {
                    b.HasOne("ASM.Data.Entities.Seller", "Seller")
                        .WithMany("MeliAccounts")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("ASM.Data.Entities.PaymentHistory", b =>
                {
                    b.HasOne("ASM.Data.Entities.Seller", "Seller")
                        .WithMany("PaymentHistory")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("ASM.Data.Entities.PaymentInformation", b =>
                {
                    b.HasOne("ASM.Data.Entities.Seller", "Seller")
                        .WithOne("BillingInformation")
                        .HasForeignKey("ASM.Data.Entities.PaymentInformation", "SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("ASM.Data.Entities.SellerFcmToken", b =>
                {
                    b.HasOne("ASM.Data.Entities.Seller", "Seller")
                        .WithMany("SellerFcmTokens")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("ASM.Data.Entities.SellerMessage", b =>
                {
                    b.HasOne("ASM.Data.Entities.MeliAccount", "MeliAccount")
                        .WithMany("Messages")
                        .HasForeignKey("MeliAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeliAccount");
                });

            modelBuilder.Entity("ASM.Data.Entities.SellerOrder", b =>
                {
                    b.HasOne("ASM.Data.Entities.Seller", "Seller")
                        .WithMany("Orders")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("ASM.Data.Entities.MeliAccount", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ASM.Data.Entities.Seller", b =>
                {
                    b.Navigation("BillingInformation")
                        .IsRequired();

                    b.Navigation("MeliAccounts");

                    b.Navigation("Orders");

                    b.Navigation("PaymentHistory");

                    b.Navigation("SellerFcmTokens");
                });

            modelBuilder.Entity("ASM.Data.Entities.SellerMessage", b =>
                {
                    b.Navigation("Attachments");
                });
#pragma warning restore 612, 618
        }
    }
}
