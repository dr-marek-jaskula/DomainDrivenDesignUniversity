﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shopway.Persistence.Framework;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    [DbContext(typeof(ShopwayDbContext))]
    [Migration("20230514145251_Add_Unique_Key_To_Review")]
    partial class Add_Unique_Key_To_Review
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Shopway.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<DateTimeOffset?>("DateOfBirth")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("VarChar(7)");

                    b.Property<string>("Rank")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VarChar(8)")
                        .HasDefaultValue("Standard");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("UniqueIdentifier");

                    b.HasKey("Id");

                    b.ToTable("Customer", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.OrderHeader", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("VarChar(10)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("UniqueIdentifier");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("OrderHeader", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.OrderLine", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<Guid>("OrderHeaderId")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderLine", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<Guid>("OrderHeaderId")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("VarChar(10)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.HasKey("Id");

                    b.ToTable("Payment", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.HasKey("Id");

                    b.ToTable("Product", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Review", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.RoleUser", b =>
                {
                    b.Property<byte>("RoleId")
                        .HasColumnType("TinyInt");

                    b.Property<Guid>("UserId")
                        .HasColumnType("UniqueIdentifier");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("RoleUser", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("UniqueIdentifier");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("VarChar(30)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("DateTimeOffset(2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique()
                        .HasFilter("[CustomerId] IS NOT NULL");

                    b.ToTable("User", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Enumerations.Permission", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TinyInt");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VarChar(128)");

                    b.HasKey("Id");

                    b.ToTable("Permission", "Master");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Name = "Read"
                        },
                        new
                        {
                            Id = (byte)2,
                            Name = "Create"
                        },
                        new
                        {
                            Id = (byte)3,
                            Name = "Update"
                        },
                        new
                        {
                            Id = (byte)4,
                            Name = "Delete"
                        },
                        new
                        {
                            Id = (byte)5,
                            Name = "CRUD_Review"
                        });
                });

            modelBuilder.Entity("Shopway.Domain.Enumerations.Role", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TinyInt");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VarChar(128)");

                    b.HasKey("Id");

                    b.ToTable("Role", "Master");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            Name = "Customer"
                        },
                        new
                        {
                            Id = (byte)2,
                            Name = "Employee"
                        },
                        new
                        {
                            Id = (byte)3,
                            Name = "Manager"
                        },
                        new
                        {
                            Id = (byte)4,
                            Name = "Administrator"
                        });
                });

            modelBuilder.Entity("Shopway.Domain.Enumerations.RolePermission", b =>
                {
                    b.Property<byte>("RoleId")
                        .HasColumnType("TinyInt");

                    b.Property<byte>("PermissionId")
                        .HasColumnType("TinyInt");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermission", "Master");

                    b.HasData(
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)2
                        },
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)3
                        },
                        new
                        {
                            RoleId = (byte)4,
                            PermissionId = (byte)4
                        },
                        new
                        {
                            RoleId = (byte)3,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)3,
                            PermissionId = (byte)2
                        },
                        new
                        {
                            RoleId = (byte)3,
                            PermissionId = (byte)3
                        },
                        new
                        {
                            RoleId = (byte)2,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)2,
                            PermissionId = (byte)2
                        },
                        new
                        {
                            RoleId = (byte)1,
                            PermissionId = (byte)1
                        },
                        new
                        {
                            RoleId = (byte)1,
                            PermissionId = (byte)5
                        });
                });

            modelBuilder.Entity("Shopway.Persistence.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("VarChar(5000)");

                    b.Property<string>("Error")
                        .HasColumnType("VarChar(8000)");

                    b.Property<DateTimeOffset>("OccurredOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("ProcessedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("VarChar(100)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessage", "Outbox");
                });

            modelBuilder.Entity("Shopway.Persistence.Outbox.OutboxMessageConsumer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id", "Name");

                    b.ToTable("OutboxMessageConsumer", "Outbox");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Customer", b =>
                {
                    b.OwnsOne("Shopway.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Building")
                                .HasMaxLength(1000)
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<int?>("Flat")
                                .HasMaxLength(1000)
                                .HasColumnType("int");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("nvarchar(5)");

                            b1.HasKey("Id");

                            b1.HasIndex("CustomerId")
                                .IsUnique();

                            b1.ToTable("Address", "Master");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.FirstName", "FirstName", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("FirstName");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer", "Master");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.LastName", "LastName", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("LastName");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer", "Master");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.PhoneNumber", "PhoneNumber", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(9)
                                .HasColumnType("nvarchar(9)")
                                .HasColumnName("PhoneNumber");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer", "Master");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Address");

                    b.Navigation("FirstName")
                        .IsRequired();

                    b.Navigation("LastName")
                        .IsRequired();

                    b.Navigation("PhoneNumber")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.OrderHeader", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Payment", "Payment")
                        .WithOne("OrderHeader")
                        .HasForeignKey("Shopway.Domain.Entities.OrderHeader", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Entities.User", "User")
                        .WithMany("OrderHeaders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.OrderLine", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.OrderHeader", null)
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Shopway.Domain.ValueObjects.Discount", "LineDiscount", b1 =>
                        {
                            b1.Property<Guid>("OrderLineId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<decimal>("Value")
                                .HasPrecision(3, 2)
                                .HasColumnType("decimal(3,2)")
                                .HasColumnName("Discount");

                            b1.HasKey("OrderLineId");

                            b1.ToTable("OrderLine", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("OrderLineId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.Amount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("OrderLineId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<int>("Value")
                                .HasColumnType("int")
                                .HasColumnName("Amount");

                            b1.HasKey("OrderLineId");

                            b1.ToTable("OrderLine", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("OrderLineId");
                        });

                    b.Navigation("Amount")
                        .IsRequired();

                    b.Navigation("LineDiscount")
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Payment", b =>
                {
                    b.OwnsOne("Shopway.Domain.ValueObjects.Discount", "TotalDiscount", b1 =>
                        {
                            b1.Property<Guid>("PaymentId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<decimal>("Value")
                                .HasPrecision(3, 2)
                                .HasColumnType("decimal(3,2)")
                                .HasColumnName("Discount");

                            b1.HasKey("PaymentId");

                            b1.ToTable("Payment", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("PaymentId");
                        });

                    b.Navigation("TotalDiscount")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Product", b =>
                {
                    b.OwnsOne("Shopway.Domain.ValueObjects.Price", "Price", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<decimal>("Value")
                                .HasPrecision(10, 2)
                                .HasColumnType("decimal(10,2)")
                                .HasColumnName("Price");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.ProductName", "ProductName", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("ProductName");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.Revision", "Revision", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Revision");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.UomCode", "UomCode", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("VarChar(8)")
                                .HasColumnName("UomCode");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Price")
                        .IsRequired();

                    b.Navigation("ProductName")
                        .IsRequired();

                    b.Navigation("Revision")
                        .IsRequired();

                    b.Navigation("UomCode")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Review", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Product", null)
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Shopway.Domain.ValueObjects.Username", "Username", b1 =>
                        {
                            b1.Property<Guid>("ReviewId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)")
                                .HasColumnName("Username");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Review", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.Description", "Description", b1 =>
                        {
                            b1.Property<Guid>("ReviewId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(600)
                                .HasColumnType("nvarchar(600)")
                                .HasColumnName("Description");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Review", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.Stars", "Stars", b1 =>
                        {
                            b1.Property<Guid>("ReviewId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<byte>("Value")
                                .HasColumnType("TinyInt")
                                .HasColumnName("Stars");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Review", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.Title", "Title", b1 =>
                        {
                            b1.Property<Guid>("ReviewId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(45)
                                .HasColumnType("nvarchar(45)")
                                .HasColumnName("Title");

                            b1.HasKey("ReviewId");

                            b1.ToTable("Review", "Shopway");

                            b1.WithOwner()
                                .HasForeignKey("ReviewId");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Stars")
                        .IsRequired();

                    b.Navigation("Title")
                        .IsRequired();

                    b.Navigation("Username")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.RoleUser", b =>
                {
                    b.HasOne("Shopway.Domain.Enumerations.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.User", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Customer", "Customer")
                        .WithOne("User")
                        .HasForeignKey("Shopway.Domain.Entities.User", "CustomerId");

                    b.OwnsOne("Shopway.Domain.ValueObjects.Username", "Username", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("nvarchar(30)")
                                .HasColumnName("Username");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasDatabaseName("UX_Username_Email");

                            b1.ToTable("User", "Master");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(40)
                                .HasColumnType("nvarchar(40)")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.HasIndex("Value")
                                .IsUnique()
                                .HasDatabaseName("UX_User_Email");

                            b1.ToTable("User", "Master");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("Shopway.Domain.ValueObjects.PasswordHash", "PasswordHash", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("UniqueIdentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("NChar(514)")
                                .HasColumnName("PasswordHash");

                            b1.HasKey("UserId");

                            b1.ToTable("User", "Master");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Customer");

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("PasswordHash")
                        .IsRequired();

                    b.Navigation("Username")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Enumerations.RolePermission", b =>
                {
                    b.HasOne("Shopway.Domain.Enumerations.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Enumerations.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Customer", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.OrderHeader", b =>
                {
                    b.Navigation("OrderLines");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Payment", b =>
                {
                    b.Navigation("OrderHeader")
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Product", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.User", b =>
                {
                    b.Navigation("OrderHeaders");
                });
#pragma warning restore 612, 618
        }
    }
}