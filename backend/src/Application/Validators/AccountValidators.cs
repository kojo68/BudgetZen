using BudgetZen.Application.Dtos;
using FluentValidation;

namespace BudgetZen.Application.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.InitialBalance).InclusiveBetween(-1000000m, 1000000m);
    }
}

public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
        RuleFor(x => x.InitialBalance).InclusiveBetween(-1000000m, 1000000m);
    }
}
