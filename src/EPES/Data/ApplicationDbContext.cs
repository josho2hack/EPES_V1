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
        }

        public DbSet<Office> Offices { get; set; }
        public DbSet<PointOfEvaluation> PointOfEvaluations { get; set; }
        public DbSet<DataForEvaluation> DataForEvaluations { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
