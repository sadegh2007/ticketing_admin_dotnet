﻿// <auto-generated />
using System;
using ERP.Ticketing.HttpApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ERP.Ticketing.HttpApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230118153837_ticket_comment_user_seen")]
    partial class ticketcommentuserseen
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Departments.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Departments.DepartmentUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("UserId");

                    b.ToTable("DepartmentUsers");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClosedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastCommentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Number"));

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClosedById");

                    b.HasIndex("CreatorId");

                    b.HasIndex("DeletedById");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("StatusId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketComments");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketCommentSeen", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("CreatorId");

                    b.ToTable("TicketCommentSeens");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TicketStatuses");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketStatusHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketStatusHistories");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TicketId");

                    b.HasIndex("UserId");

                    b.ToTable("TicketUsers");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Family")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Picture")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Departments.Department", b =>
                {
                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Departments.DepartmentUser", b =>
                {
                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Departments.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Department");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.Ticket", b =>
                {
                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "ClosedBy")
                        .WithMany()
                        .HasForeignKey("ClosedById");

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "DeletedBy")
                        .WithMany()
                        .HasForeignKey("DeletedById");

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Departments.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Ticketing.TicketStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClosedBy");

                    b.Navigation("Creator");

                    b.Navigation("DeletedBy");

                    b.Navigation("Department");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketComment", b =>
                {
                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Ticketing.Ticket", "Ticket")
                        .WithMany("Comments")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketCommentSeen", b =>
                {
                    b.HasOne("ERP.Ticketing.HttpApi.Features.Ticketing.TicketComment", "Comment")
                        .WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketStatusHistory", b =>
                {
                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Ticketing.TicketStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Ticketing.Ticket", "Ticket")
                        .WithMany()
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Status");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.TicketUser", b =>
                {
                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Ticketing.Ticket", "Ticket")
                        .WithMany("Users")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP.Ticketing.HttpApi.Features.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ERP.Ticketing.HttpApi.Features.Ticketing.Ticket", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
