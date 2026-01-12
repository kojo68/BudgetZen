using BudgetZen.Application.Dtos;
using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;

namespace BudgetZen.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<IReadOnlyList<AccountDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllAsync(userId, cancellationToken);
        return accounts.Select(Map).ToList();
    }

    public async Task<AccountDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(userId, id, cancellationToken);
        return account is null ? null : Map(account);
    }

    public async Task<AccountDto> CreateAsync(Guid userId, CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name.Trim(),
            InitialBalance = request.InitialBalance,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _accountRepository.AddAsync(account, cancellationToken);
        await _accountRepository.SaveChangesAsync(cancellationToken);
        return Map(account);
    }

    public async Task<AccountDto?> UpdateAsync(Guid userId, Guid id, UpdateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(userId, id, cancellationToken);
        if (account is null)
        {
            return null;
        }

        account.Name = request.Name.Trim();
        account.InitialBalance = request.InitialBalance;
        await _accountRepository.SaveChangesAsync(cancellationToken);
        return Map(account);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(userId, id, cancellationToken);
        if (account is null)
        {
            return false;
        }

        _accountRepository.Remove(account);
        await _accountRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static AccountDto Map(Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            Name = account.Name,
            InitialBalance = account.InitialBalance,
            CreatedAtUtc = account.CreatedAtUtc
        };
    }
}
