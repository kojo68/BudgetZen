namespace BudgetZen.Application.Dtos;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public class CreateAccountRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
}

public class UpdateAccountRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
}
