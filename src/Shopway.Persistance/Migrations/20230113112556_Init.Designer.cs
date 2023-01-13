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
    [Migration("20230113112556_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<int>("PermissionsId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("PermissionsId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("PermissionRole", "Master");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<int>("Amount")
                        .HasColumnType("INT");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(10)")
                        .HasDefaultValue("Create")
                        .HasComment("Create, InProgress, Done or Rejected");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("datetimeoffset(2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.HasIndex(new[] { "ProductId", "Status" }, "IX_Order_ProductId_Status")
                        .HasFilter("Status IN ('Create', 'InProgress')");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex(new[] { "ProductId", "Status" }, "IX_Order_ProductId_Status"), new[] { "Amount", "CustomerId" });

                    b.ToTable("Order", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Parents.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<DateTimeOffset?>("DateOfBirth")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("VARCHAR(7)")
                        .HasComment("Male, Female or Unknown");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex(new[] { "Email" }, "IX_Person_Email")
                        .IsUnique();

                    b.HasIndex(new[] { "Email" }, "UX_Person_Email")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex(new[] { "Email" }, "UX_Person_Email"), new[] { "FirstName", "LastName" });

                    b.ToTable("Person", "Master");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Parents.WorkItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("nvarchar(600)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(10)")
                        .HasDefaultValue("Create")
                        .HasComment("Create, InProgress, Done or Rejected");

                    b.Property<int>("StoryPoints")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("WorkItem", "Workflow");

                    b.HasDiscriminator<string>("Discriminator").HasValue("WorkItem");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<decimal>("Discount")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.Property<DateTimeOffset?>("OccurredOn")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("Create, InProgress, Done or Rejected");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "OrderId", "Status" }, "IX_Payment_OrderId_Status")
                        .HasFilter("Status <> 'Rejected'");

                    b.ToTable("Payment", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Revision")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("UomCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("Id");

                    b.ToTable("Product", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<decimal>("Stars")
                        .HasColumnType("TINYINT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Review", "Shopway");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(40)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("NCHAR(514)");

                    b.Property<Guid?>("PersonId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("VARCHAR(60)");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique()
                        .HasFilter("[PersonId] IS NOT NULL");

                    b.HasIndex(new[] { "Email" }, "IX_User_Email")
                        .IsUnique();

                    b.HasIndex(new[] { "Username" }, "IX_User_Username")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex(new[] { "Username" }, "IX_User_Username"), new[] { "Email" });

                    b.ToTable("User", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Enumerations.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Permission", "Master");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Read"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Create"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Update"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Delete"
                        });
                });

            modelBuilder.Entity("Shopway.Domain.Enumerations.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(128)");

                    b.HasKey("Id");

                    b.ToTable("Role", "Master");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Customer"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Employee"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Manager"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Administrator"
                        });
                });

            modelBuilder.Entity("Shopway.Persistence.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("OccurredOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("ProcessedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                    b.HasBaseType("Shopway.Domain.Entities.Parents.Person");

                    b.Property<string>("Rank")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("VARCHAR(8)")
                        .HasDefaultValue("Standard");

                    b.ToTable("Customer", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Employee", b =>
                {
                    b.HasBaseType("Shopway.Domain.Entities.Parents.Person");

                    b.Property<DateTimeOffset>("HireDate")
                        .HasColumnType("datetimeoffset(2)");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("UNIQUEIDENTIFIER");

                    b.HasIndex("ManagerId");

                    b.ToTable("Employee", "Master");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Bug", b =>
                {
                    b.HasBaseType("Shopway.Domain.Entities.Parents.WorkItem");

                    b.HasDiscriminator().HasValue("Bug");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Feature", b =>
                {
                    b.HasBaseType("Shopway.Domain.Entities.Parents.WorkItem");

                    b.HasDiscriminator().HasValue("Feature");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("Shopway.Domain.Enumerations.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Enumerations.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Shopway.Domain.Enumerations.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Order", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Entities.Payment", "Payment")
                        .WithOne()
                        .HasForeignKey("Shopway.Domain.Entities.Order", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Payment");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Parents.Person", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Employee", null)
                        .WithMany("Customers")
                        .HasForeignKey("EmployeeId");

                    b.OwnsOne("Shopway.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<byte>("Building")
                                .HasColumnType("TINYINT");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<byte?>("Flat")
                                .HasColumnType("TINYINT");

                            b1.Property<Guid>("PersonId")
                                .HasColumnType("UNIQUEIDENTIFIER");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("nvarchar(5)");

                            b1.HasKey("Id");

                            b1.HasIndex("PersonId")
                                .IsUnique();

                            b1.ToTable("Address", "Master");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Parents.WorkItem", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Employee", "Employee")
                        .WithMany("WorkItems")
                        .HasForeignKey("EmployeeId");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Review", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Product", null)
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.User", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Parents.Person", "Person")
                        .WithOne("User")
                        .HasForeignKey("Shopway.Domain.Entities.User", "PersonId");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Customer", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Parents.Person", null)
                        .WithOne()
                        .HasForeignKey("Shopway.Domain.Entities.Customer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Employee", b =>
                {
                    b.HasOne("Shopway.Domain.Entities.Parents.Person", null)
                        .WithOne()
                        .HasForeignKey("Shopway.Domain.Entities.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shopway.Domain.Entities.Employee", "Manager")
                        .WithMany("Subordinates")
                        .HasForeignKey("ManagerId");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Parents.Person", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Product", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Shopway.Domain.Entities.Employee", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("Subordinates");

                    b.Navigation("WorkItems");
                });
#pragma warning restore 612, 618
        }
    }
}
