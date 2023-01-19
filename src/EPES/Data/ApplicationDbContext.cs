﻿using System;
using System.Collections.Generic;
using System.Text;
using EPES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPES.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DataForEvaluation>()
                .HasOne(d => d.PointOfEvaluation)
                .WithMany(p => p.DataForEvaluations)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DataForEvaluation>()
                .HasOne(d => d.Office)
                .WithMany(o => o.DataForEvaluations)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DataForEvaluation>()
            .Property(d => d.Result)
            .HasDefaultValue(0);

            builder.Entity<DataForEvaluation>()
            .Property(d => d.ResultLevelRate)
            .HasDefaultValue(0);

            builder.Entity<DataForEvaluation>()
            .Property(d => d.Expect)
            .HasDefaultValue(0);

            builder.Entity<PointOfEvaluation>()
            .Property(p => p.SubPoint)
            .HasDefaultValue(0);

            builder.Entity<PointOfEvaluation>()
            .Property(p => p.WeightAll)
            .HasDefaultValue(true);

            builder.Entity<PointOfEvaluation>()
            .Property(p => p.StartZero)
            .HasDefaultValue(false);

            builder.Entity<PointOfEvaluation>()
            .Property(p => p.HasSub)
            .HasDefaultValue(false);

            builder.Entity<PointOfEvaluation>()
            .Property(p => p.FixExpect)
            .HasDefaultValue(false);

            builder.Entity<PointOfEvaluation>()
            .Property(p => p.CalPerMonth)
            .HasDefaultValue(false);

            builder.Entity<Office>()
            .HasMany(o => o.OwnerPointOfEvaluations)
            .WithOne(p => p.OwnerOffice)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Office>()
            .HasMany(o => o.AuditPointOfEvaluations)
            .WithOne(p => p.AuditOffice)
            .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Office>()
            //            .HasOne(x => x.OfficeGroup)
            //            .WithOne()
            //            .HasForeignKey<Office>(s => s.OfficeGroupId);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<PointOfEvaluation> PointOfEvaluations { get; set; }
        public DbSet<DataForEvaluation> DataForEvaluations { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<ScoreDraft> ScoreDrafts { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<IssueForEvaluations> IssueForEvaluations { get; set; }
        public DbSet<UserOffices> UserOffices { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public DbSet<End> End { get; set; }
        public DbSet<Way> Way { get; set; }
    }
}
