using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quick_Bite.Models;

namespace Quick_Bite.Data
{
    public class QuickContext : IdentityDbContext<IdentityUser>
    {
        public QuickContext(DbContextOptions<QuickContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<SideDish> SideDishes { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Rating> Ratings { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure Recipe entity
            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasIndex(r => r.RecipeName).IsUnique();
                entity.Property(r => r.RecipeName).HasMaxLength(100).IsRequired();
                entity.Property(r => r.PrepTime).HasColumnType("smallint");
                entity.Property(r => r.CookTime).HasColumnType("smallint");
                entity.Property(r => r.DownloadCount).HasDefaultValue(0);
            });

            // Configure Drink entity
            modelBuilder.Entity<Drink>(entity =>
            {
                entity.HasIndex(d => d.RecipeName).IsUnique();
                entity.Property(d => d.RecipeName).HasMaxLength(100).IsRequired();
                entity.Property(d => d.PrepTime).HasColumnType("smallint");
                entity.Property(d => d.DownloadCount).HasDefaultValue(0);
            });

            // Configure SideDish entity
            modelBuilder.Entity<SideDish>(entity =>
            {
                entity.HasIndex(s => s.RecipeName).IsUnique();
                entity.Property(s => s.RecipeName).HasMaxLength(100).IsRequired();
                entity.Property(s => s.PrepTime).HasColumnType("smallint");
                entity.Property(s => s.CookTime).HasColumnType("smallint");
                entity.Property(s => s.DownloadCount).HasDefaultValue(0);
            });

            // Configure Download entity
            modelBuilder.Entity<Download>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.DownloadDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.Recipe)
                      .WithMany()
                      .HasForeignKey(d => d.RecipeId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.Drink)
                      .WithMany()
                      .HasForeignKey(d => d.DrinkId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.SideDish)
                      .WithMany()
                      .HasForeignKey(d => d.SideDishId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Download_HasReference",
                    $"[RecipeId] IS NOT NULL OR [DrinkId] IS NOT NULL OR [SideDishId] IS NOT NULL"));
            });

            // Configure Rating entity (correct placement - outside Download configuration)
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(r => r.Id);

                // Configure relationships
                entity.HasOne(r => r.Recipe)
                    .WithMany(r => r.Ratings)
                    .HasForeignKey(r => r.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Drink)
                    .WithMany(d => d.Ratings)
                    .HasForeignKey(r => r.DrinkId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.SideDish)
                    .WithMany(s => s.Ratings)
                    .HasForeignKey(r => r.SideDishId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Add constraint to ensure exactly one foreign key is set
                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Rating_HasReference",
                    $"[RecipeId] IS NOT NULL OR [DrinkId] IS NOT NULL OR [SideDishId] IS NOT NULL"));

                // Index for IP address (optional for duplicate checking)
                entity.HasIndex(r => r.IpAddress);

                // Configure value constraints
                entity.Property(r => r.Value)
                    .IsRequired()
                    .HasDefaultValue(3);

                entity.Property(r => r.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        }
    }
}