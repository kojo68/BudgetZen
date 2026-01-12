namespace BudgetZen.Application.Models;

public class AuthResult
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
}
