using Microsoft.EntityFrameworkCore;
using OnlineCredits.Core.Entities;

namespace OnlineCredits.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CreditRequest> CreditRequests { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<CreditEvaluation> CreditEvaluations { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.Status).IsRequired();
            });

            // CreditRequest
            modelBuilder.Entity<CreditRequest>(entity =>
            {
                entity.ToTable("CreditRequests");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.TermInMonths).IsRequired();
                entity.Property(e => e.MonthlyIncome).IsRequired();
                entity.Property(e => e.WorkSeniority).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.CreditRequests)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Analyst)
                      .WithMany()
                      .HasForeignKey(e => e.EvaluatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Document
            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Documents");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DocumentType).IsRequired();
                entity.Property(e => e.FilePath).IsRequired();
                entity.HasOne(e => e.CreditRequest)
                      .WithMany(c => c.Documents)
                      .HasForeignKey(e => e.CreditRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CreditEvaluation
            modelBuilder.Entity<CreditEvaluation>(entity =>
            {
                entity.ToTable("CreditEvaluations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(e => e.CreditRequest)
                      .WithMany(c => c.CreditEvaluations)
                      .HasForeignKey(e => e.CreditRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // AuditLog
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLogs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.AuditLogs)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
} 