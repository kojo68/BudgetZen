using BudgetZen.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetZen.Infrastructure.Persistence;

public class BudgetZenDbContext : DbContext
{
    public BudgetZenDbContext(DbContextOptions<BudgetZenDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(120);
            builder.Property(a => a.InitialBalance).HasPrecision(18, 2);
            builder.HasIndex(a => new { a.UserId, a.Name });
            builder.HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(120);
            builder.HasIndex(c => new { c.UserId, c.Name });
            builder.HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Amount).HasPrecision(18, 2);
            builder.Property(t => t.Description).HasMaxLength(240);
            builder.HasIndex(t => new { t.UserId, t.OccurredAtUtc });
            builder.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Budget>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.MonthlyLimit).HasPrecision(18, 2);
            builder.HasIndex(b => new { b.UserId, b.Month, b.Year });
            builder.HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
