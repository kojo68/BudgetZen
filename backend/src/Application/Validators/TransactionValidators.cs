using BudgetZen.Application.Dtos;
using FluentValidation;

namespace BudgetZen.Application.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).LessThanOrEqualTo(1000000m);
        RuleFor(x => x.Description).MaximumLength(240);
        RuleFor(x => x.OccurredAtLocal).NotEmpty();
    }
}

public class UpdateTransactionRequestValidator : AbstractValidator<UpdateTransactionRequest>
{
    public UpdateTransactionRequestValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).LessThanOrEqualTo(1000000m);
        RuleFor(x => x.Description).MaximumLength(240);
        RuleFor(x => x.OccurredAtLocal).NotEmpty();
    }
}
