using ExpenseTrackerCLI.Entities;
using FluentValidation;

namespace ExpenseTrackerCLI.ValidateEntities;

public class ValidateExpenses : AbstractValidator<Expense?>
{
    public ValidateExpenses()
    {
        RuleFor(expense => expense)
            .NotNull()
            .WithMessage("Expense object cannot be null.");

        When(expense => expense != null, () =>
        {
            RuleFor(expense => expense!.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");

            RuleFor(expense => expense!.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

            RuleFor(expense => expense!.Amount)
                .GreaterThan(-1).WithMessage("Amount must be greater than zero.");

            RuleFor(expense => expense!.CreatedExpense)
                .LessThanOrEqualTo(_ => DateTimeOffset.UtcNow).WithMessage("CreatedExpense cannot be in the future.");
        });
    }
}
