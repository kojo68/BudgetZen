using BudgetZen.Application.Dtos;
using FluentValidation;

namespace BudgetZen.Application.Validators;

public class CreateBudgetRequestValidator : AbstractValidator<CreateBudgetRequest>
{
    public CreateBudgetRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.MonthlyLimit).GreaterThan(0).LessThanOrEqualTo(1000000m);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100);
    }
}

public class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetRequest>
{
    public UpdateBudgetRequestValidator()
    {
        RuleFor(x => x.MonthlyLimit).GreaterThan(0).LessThanOrEqualTo(1000000m);
    }
}
