﻿// <auto-generated />
using System;
using EPES.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EPES.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EPES.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Class");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DOB");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FName");

                    b.Property<string>("GroupName");

                    b.Property<string>("LName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("OfficeId");

                    b.Property<string>("OfficeName");

                    b.Property<string>("PIN");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PosName");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Title");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EPES.Models.DataForEvaluation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Approve");

                    b.Property<string>("AttachFile");

                    b.Property<string>("AuditComment");

                    b.Property<string>("CommentApprove");

                    b.Property<DateTime?>("CompletedDate");

                    b.Property<decimal>("Expect")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(38, 10)")
                        .HasDefaultValue(0m);

                    b.Property<int>("Month");

                    b.Property<int>("OfficeId");

                    b.Property<decimal?>("OldResult")
                        .HasColumnType("decimal(38, 10)");

                    b.Property<int>("PointOfEvaluationId");

                    b.Property<decimal>("Result")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(38, 10)")
                        .HasDefaultValue(0m);

                    b.Property<string>("UpdateUserId");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.HasIndex("PointOfEvaluationId");

                    b.HasIndex("UpdateUserId");

                    b.ToTable("DataForEvaluations");
                });

            modelBuilder.Entity("EPES.Models.Office", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Code")
                        .HasMaxLength(8);

                    b.Property<string>("Name");

                    b.Property<string>("Remark");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("EPES.Models.PointOfEvaluation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuditOfficeId");

                    b.Property<string>("Detail2Rate1");

                    b.Property<string>("Detail2Rate2");

                    b.Property<string>("Detail2Rate3");

                    b.Property<string>("Detail2Rate4");

                    b.Property<string>("Detail2Rate5");

                    b.Property<string>("DetailPlan");

                    b.Property<string>("DetailRate1");

                    b.Property<string>("DetailRate2");

                    b.Property<string>("DetailRate3");

                    b.Property<string>("DetailRate4");

                    b.Property<string>("DetailRate5");

                    b.Property<string>("Name");

                    b.Property<int?>("OwnerOfficeId");

                    b.Property<int>("Plan");

                    b.Property<int>("Point");

                    b.Property<decimal>("Rate1")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<DateTime?>("Rate1MonthStart");

                    b.Property<DateTime?>("Rate1MonthStart2");

                    b.Property<DateTime?>("Rate1MonthStop");

                    b.Property<DateTime?>("Rate1MonthStop2");

                    b.Property<decimal>("Rate2")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<DateTime?>("Rate2MonthStart");

                    b.Property<DateTime?>("Rate2MonthStart2");

                    b.Property<DateTime?>("Rate2MonthStop");

                    b.Property<DateTime?>("Rate2MonthStop2");

                    b.Property<decimal>("Rate3")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<DateTime?>("Rate3MonthStart");

                    b.Property<DateTime?>("Rate3MonthStart2");

                    b.Property<DateTime?>("Rate3MonthStop");

                    b.Property<DateTime?>("Rate3MonthStop2");

                    b.Property<decimal>("Rate4")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<DateTime?>("Rate4MonthStart");

                    b.Property<DateTime?>("Rate4MonthStart2");

                    b.Property<DateTime?>("Rate4MonthStop");

                    b.Property<DateTime?>("Rate4MonthStop2");

                    b.Property<decimal>("Rate5")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<DateTime?>("Rate5MonthStart");

                    b.Property<DateTime?>("Rate5MonthStart2");

                    b.Property<DateTime?>("Rate5MonthStop");

                    b.Property<DateTime?>("Rate5MonthStop2");

                    b.Property<int>("SubPoint");

                    b.Property<int?>("Unit");

                    b.Property<string>("UpdateUserId");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(7, 4)");

                    b.Property<DateTime>("Year");

                    b.HasKey("Id");

                    b.HasIndex("AuditOfficeId");

                    b.HasIndex("OwnerOfficeId");

                    b.HasIndex("UpdateUserId");

                    b.ToTable("PointOfEvaluations");
                });

            modelBuilder.Entity("EPES.Models.Score", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LastMonth");

                    b.Property<int>("OfficeId");

                    b.Property<int>("PointOfEvaluationId");

                    b.Property<decimal>("ScoreValue")
                        .HasColumnType("decimal(5, 4)");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.HasIndex("PointOfEvaluationId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("EPES.Models.ScoreDraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LastMonth");

                    b.Property<int>("OfficeId");

                    b.Property<int>("PointOfEvaluationId");

                    b.Property<decimal>("ScoreValue")
                        .HasColumnType("decimal(5, 4)");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.HasIndex("PointOfEvaluationId");

                    b.ToTable("ScoreDrafts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EPES.Models.DataForEvaluation", b =>
                {
                    b.HasOne("EPES.Models.Office", "Office")
                        .WithMany("DataForEvaluations")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EPES.Models.PointOfEvaluation", "PointOfEvaluation")
                        .WithMany("DataForEvaluations")
                        .HasForeignKey("PointOfEvaluationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EPES.Models.ApplicationUser", "UpdateUser")
                        .WithMany("DataForEvaluations")
                        .HasForeignKey("UpdateUserId");
                });

            modelBuilder.Entity("EPES.Models.Office", b =>
                {
                    b.HasOne("EPES.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("EPES.Models.PointOfEvaluation", b =>
                {
                    b.HasOne("EPES.Models.Office", "AuditOffice")
                        .WithMany("AuditPointOfEvaluations")
                        .HasForeignKey("AuditOfficeId");

                    b.HasOne("EPES.Models.Office", "OwnerOffice")
                        .WithMany("OwnerPointOfEvaluations")
                        .HasForeignKey("OwnerOfficeId");

                    b.HasOne("EPES.Models.ApplicationUser", "UpdateUser")
                        .WithMany("PointOfEvaluations")
                        .HasForeignKey("UpdateUserId");
                });

            modelBuilder.Entity("EPES.Models.Score", b =>
                {
                    b.HasOne("EPES.Models.Office", "Office")
                        .WithMany("Scores")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPES.Models.PointOfEvaluation", "PointOfEvaluation")
                        .WithMany()
                        .HasForeignKey("PointOfEvaluationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPES.Models.ScoreDraft", b =>
                {
                    b.HasOne("EPES.Models.Office", "Office")
                        .WithMany()
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPES.Models.PointOfEvaluation", "PointOfEvaluation")
                        .WithMany()
                        .HasForeignKey("PointOfEvaluationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EPES.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EPES.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPES.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EPES.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
