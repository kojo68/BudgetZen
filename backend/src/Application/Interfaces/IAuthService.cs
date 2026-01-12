using BudgetZen.Application.Dtos;
using BudgetZen.Application.Models;

namespace BudgetZen.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
