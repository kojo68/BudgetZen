using BudgetZen.Domain.Entities;
using BudgetZen.Domain.Enums;
using BudgetZen.Infrastructure.Persistence;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace BudgetZen.Infrastructure.Seed;

public static class DevDataSeeder
{
    public static async Task SeedAsync(BudgetZenDbContext dbContext)
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "demo@budgetzen.local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("ChangeMe123!"),
            CreatedAtUtc = DateTime.UtcNow
        };

        var account = new Account
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Compte courant",
            InitialBalance = 1500m,
            CreatedAtUtc = DateTime.UtcNow
        };

        var groceries = new Category
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Courses",
            IsIncome = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        var salary = new Category
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Salaire",
            IsIncome = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var transactions = new List<Transaction>
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AccountId = account.Id,
                CategoryId = groceries.Id,
                Type = TransactionType.Expense,
                Amount = 80m,
                Description = "Supermarché",
                OccurredAtUtc = DateTime.UtcNow.AddDays(-3),
                CreatedAtUtc = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AccountId = account.Id,
                CategoryId = salary.Id,
                Type = TransactionType.Income,
                Amount = 2500m,
                Description = "Salaire mensuel",
                OccurredAtUtc = DateTime.UtcNow.AddDays(-10),
                CreatedAtUtc = DateTime.UtcNow.AddDays(-10)
            }
        };

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CategoryId = groceries.Id,
            MonthlyLimit = 400m,
            Month = DateTime.UtcNow.Month,
            Year = DateTime.UtcNow.Year,
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        dbContext.Accounts.Add(account);
        dbContext.Categories.AddRange(groceries, salary);
        dbContext.Transactions.AddRange(transactions);
        dbContext.Budgets.Add(budget);
        await dbContext.SaveChangesAsync();
    }
}
