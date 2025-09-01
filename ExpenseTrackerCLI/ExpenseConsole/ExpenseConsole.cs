using ExpenseTrackerCLI.ConsoleServices;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.Services;
using ExpenseTrackerCLI.Helpers;
namespace ExpenseTrackerCLI.ExpenseConsole;

public class ExpenseConsole
{
    private readonly IExpensesServices _expensesServices;
    private readonly IConsoleService _consoleService;
    private IDictionary<string, Action> _menuActions;

    public ExpenseConsole(IExpensesServices expensesServices, IConsoleService consoleService)
    {
        _expensesServices = expensesServices;
        _consoleService = consoleService;
        _menuActions = new Dictionary<string, Action>
        {
            { "1", AddExpense },
            { "2", ViewExpenses },
            { "3", UpdateExpense },
            { "4", DeleteExpense },
            { "5", () => Environment.Exit(0) }
        };
    }
    public void ExecuteExpenseConsole(string choice)
    {
        if (_menuActions.TryGetValue(choice, out var action))
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                _consoleService.Write($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            _consoleService.Write("Invalid option. Please try again.");
        }
    }

    private void ViewExpenses()
    {
        var expenses = _expensesServices.GetAllExpenses();
        if (!expenses.Any())
        {
            _consoleService.Write("No expenses found.");
            return;
        }
        foreach (var expense in expenses)
        {
            _consoleService.DisplayExpense(expense);
        }
    }

    private void AddExpense()
    {
        var title = _consoleService.GetValueString("Enter Title:");

        var description = _consoleService.GetValueString("Enter Description:");

        var amountFromInput = _consoleService.GetValueString("Enter Amount:");
        var amount = 0m;
        try
        {
            amount = GetValueClass.GetExpenseDecimal(amountFromInput);
        }
        catch (ArgumentException ex)
        {
            _consoleService.Write(ex.Message);
            return;
        }

        ExpenseType expenseType = ExpenseType.UnknownExpenses;
        var typeFromInput = _consoleService.GetValueString("Enter Type Of Expense:");
        try
        {
            expenseType = GetValueClass.GetExpenseType(typeFromInput);
        }
        catch (ArgumentException ex)
        {
            _consoleService.Write(ex.Message);
            _consoleService.Write("The type of expense will be set to 'UnknownExpenses'.");
        }

        var expense = new Expense
        {
            Title = title,
            Description = description,
            Amount = amount,
            ExpenseType = expenseType,
            CreatedExpense = DateTimeOffset.UtcNow
        };

        var resultAdd = _expensesServices.AddExpenses(expense);
        if (!resultAdd.IsSuccess)
        {
            _consoleService.Write(resultAdd.Message);
        }
        else
        {
            _consoleService.Write("Expense added successfully.");
        }
    }

    private void DeleteExpense()
    {
        var id = _consoleService.GetValueString("Enter Expense ID to delete:");
        var parsedId = 0;
        try
        {
            parsedId = GetValueClass.GetExpenseInt(id);
        }
        catch (ArgumentException ex)
        {
            _consoleService.Write(ex.Message);
            return;
        }
        var resultDelete = _expensesServices.RemoveExpenses(parsedId);
        if (!resultDelete.IsSuccess)
        {
            _consoleService.Write(resultDelete.Message);
        }
        else
        {
            _consoleService.Write("Expense deleted successfully.");
        }
    }
    private void UpdateExpense()
    {
        var id = _consoleService.GetValueString("Enter Expense ID to update:");
        var parsedId = 0;
        try
        {
            parsedId = GetValueClass.GetExpenseInt(id);
        }
        catch (ArgumentException ex)
        {
            _consoleService.Write(ex.Message);
            return;
        }

        var expensesFromRepo = _expensesServices.GetExpenseById(parsedId);
        if (expensesFromRepo == null)
        {
            _consoleService.Write("Expense not found.");
            return;
        }
        var titleForUpdate = string.Empty;
        if (ChangeFieldAnswer("title"))
        {
            titleForUpdate = _consoleService.GetValueString("Enter new Title :");
        }

        var descriptionForUpdate = string.Empty;
        if (ChangeFieldAnswer("description"))
        {
            descriptionForUpdate = _consoleService.GetValueString("Enter new Description :");
        }

        var amountFromInput = string.Empty;
        var amountForUpdate = expensesFromRepo.Amount;
        if (ChangeFieldAnswer("amount"))
        {
            amountFromInput = _consoleService.GetValueString("Enter new Amount :");
            try
            {
                amountForUpdate = GetValueClass.GetExpenseDecimal(amountFromInput);
            }
            catch (ArgumentException ex)
            {
                _consoleService.Write(ex.Message);
                _consoleService.Write("The amount will remain unchanged.");
            }
        }

        var parsedExpenseType = expensesFromRepo.ExpenseType;
        if (ChangeFieldAnswer("expense type"))
        {
            _consoleService.Write("Available types: " + string.Join(", ", Enum.GetNames<ExpenseType>()));
            var expenseTypeFromInput = _consoleService.GetValueString("Enter type expense :");
            try
            {
                parsedExpenseType = GetValueClass.GetExpenseType(expenseTypeFromInput);
            }
            catch (ArgumentException ex)
            {
                _consoleService.Write(ex.Message);
                _consoleService.Write("The type will remain unchanged.");
            }
        }

        var expenseForUpdate = new Expense
        {
            Id = parsedId,
            Title = titleForUpdate,
            Description = descriptionForUpdate,
            Amount = amountForUpdate,
            CreatedExpense = expensesFromRepo.CreatedExpense,
            ExpenseType = parsedExpenseType
        };

        var resultUpdate = _expensesServices.Update(expenseForUpdate);
        if (!resultUpdate.IsSuccess)
        {
            _consoleService.Write(resultUpdate.Message);
        }
        else
        {
            _consoleService.Write("Expense updated successfully.");
        }
    }
    public bool ChangeFieldAnswer(string field)
    {
        var answer = _consoleService.GetValueString($"Do you want to change this {field}? (y/n):");
        return answer.Equals("y", StringComparison.OrdinalIgnoreCase);
    }
}
