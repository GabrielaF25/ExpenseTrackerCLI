using ExpenseTrackerCLI.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace ExpenseTrackerCLI.ValidateEntities;

public class ValidateExpenses : AbstractValidator<Expense?>
{
    protected override bool PreValidate(ValidationContext<Expense?> context, ValidationResult result)
    {
        if (context.InstanceToValidate is null)
        {
            result.Errors.Add(new ValidationFailure(nameof(Expense), "Expense onject cannot be null"));
            return false;
        }

        return true;
    }
    public ValidateExpenses()
    {
        RuleFor(expense => expense!.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");

        RuleFor(expense => expense!.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

        RuleFor(expense => expense!.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(expense => expense!.CreatedExpense)
            .LessThanOrEqualTo(_ => DateTimeOffset.UtcNow).WithMessage("CreatedExpense cannot be in the future.");
    }
}
