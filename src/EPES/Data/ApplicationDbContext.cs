using System;
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
            .Property(p => p.HasSub)
            .HasDefaultValue(false);

            builder.Entity<Office>()
            .HasMany(o => o.OwnerPointOfEvaluations)
            .WithOne(p => p.OwnerOffice)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Office>()
            .HasMany(o => o.AuditPointOfEvaluations)
            .WithOne(p => p.AuditOffice)
            .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<PointOfEvaluation> PointOfEvaluations { get; set; }
        public DbSet<DataForEvaluation> DataForEvaluations { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<ScoreDraft> ScoreDrafts { get; set; }
        public DbSet<Round> Rounds { get; set; }
    }
}
