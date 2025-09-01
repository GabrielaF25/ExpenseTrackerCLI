using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Helpers;

public static class GetValueClass
{
    public static ExpenseType GetExpenseType(string type)
    {
        if (!Enum.TryParse<ExpenseType>(type, ignoreCase: true, out var expenseType)
            || !Enum.IsDefined(typeof(ExpenseType), expenseType))
        {
            throw new ArgumentException("Invalid type of expense.");
        }

        return expenseType;
    }
    public static int GetExpenseInt(string id)
    {
        if (!int.TryParse(id, out var expenseId))
        {
            throw new ArgumentException("Invalid Id of expense.");
        }
        return expenseId;
    }
    public static decimal GetExpenseDecimal(string amount)
    {
        if (!decimal.TryParse(amount, out var expenseAmount))
        {
            throw new ArgumentException("Invalid amount of expense.");
        }
        return expenseAmount;
    }
}
