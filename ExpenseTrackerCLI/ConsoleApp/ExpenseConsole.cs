using ExpenseTrackerCLI.Common;
using ExpenseTrackerCLI.ConsoleServices;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExchangeRate;
using ExpenseTrackerCLI.Helpers;
using ExpenseTrackerCLI.Services.ExpenseChange;
using ExpenseTrackerCLI.Services.ExpenseService;
using System;
namespace ExpenseTrackerCLI.ConsoleApp;

public class ExpenseConsole
{
    private readonly IExpensesServices _expensesServices;
    private readonly IConsoleService _consoleService;
    private readonly Dictionary<string, Func<Task>> _menuActions;
    private readonly IExchangeRateProvider _expenseRate;
    private readonly IExpenseExchangeService _expenseExchangeService;

    private ViewExpensesHelper ViewExpensesHelper { get; set; }

    public ExpenseConsole(IExpensesServices expensesServices, IConsoleService consoleService, IExchangeRateProvider expenseRate,IExpenseExchangeService expenseExchangeService )
    {
        _expensesServices = expensesServices;
        _consoleService = consoleService;
        _menuActions = new Dictionary<string, Func<Task>>
        {
            { "1", AddExpense },
            { "2", ViewExpenses },
            { "3", UpdateExpense },
            { "4", DeleteExpense },
            { "5", ConvertExpenseCurrency },
        };
        ViewExpensesHelper = new ViewExpensesHelper(_consoleService);
        _expenseRate = expenseRate;
        _expenseExchangeService = expenseExchangeService;
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

    private async Task ViewExpenses()
    {
        var expenses = await _expensesServices.GetAllExpenses();

        if (!expenses.Any())
        {
            _consoleService.Write("No expenses found.");
            return;
        }

        _consoleService.Write("Filter by a specific year?(Y/N)");
        var answerYear = _consoleService.Read();
        if (answerYear.ToLower().Equals("y"))
        {
            expenses = ViewExpensesHelper.ChooseYearFilter(expenses);
        }

        var answerMonth = _consoleService.GetValueString("Filter by a specific month?(Y/N)");
        if (answerMonth.ToLower().Equals("y"))
        {
            int selectYear = 0;

            if (expenses.Any())
            {
                selectYear = expenses.Max(d => d.CreatedExpense.Year);
            }

            expenses = ViewExpensesHelper.ChooseTheMonthFilter(expenses, selectYear);
        }
        if(_consoleService.GetValueString("Do you wanna View the expenses by range of amount? y/n").Equals("y"))
        {

            expenses = ViewExpensesHelper.ChooseExpensesByAmountRange(expenses);
        }

        var lastExpensesToDisplay = _consoleService.GetValueString("Do you want to see a specific number of expenses?" +
            " Yes -> Enter a number, No -> Please, press enter.");

        int number = 0;
        if (!string.IsNullOrEmpty(lastExpensesToDisplay))
        {
            try
            {
                number = GetValueClass.GetExpenseInt(lastExpensesToDisplay);
                expenses = [..expenses.OrderByDescending(p => p.Id).Take(number)];
            }
            catch (Exception ex)
            {
                _consoleService.Write(ex.Message);
            }
        }

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
    private async Task AddExpense()
    {
        var title = _consoleService.GetValueString("Enter Title:");

        var description = _consoleService.GetValueString("Enter Description:");

        var amountFromInput = _consoleService.GetValueString("Enter Amount:");
        decimal amount;
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

        var resultAdd = await _expensesServices.AddExpenses(expense);
        if (!resultAdd.IsSuccess)
        {
            _consoleService.Write(resultAdd.Message);
        }
        else
        {
            _consoleService.Write("Expense added successfully.");
        }
    }

    private async Task DeleteExpense()
    {
        var id = _consoleService.GetValueString("Enter Expense ID to delete:");
        int parsedId;
        try
        {
            parsedId = GetValueClass.GetExpenseInt(id);
        }
        catch (ArgumentException ex)
        {
            _consoleService.Write(ex.Message);
            return;
        }
        var resultDelete = await _expensesServices.RemoveExpenses(parsedId);
        if (!resultDelete.IsSuccess)
        {
            _consoleService.Write(resultDelete.Message);
        }
        else
        {
            _consoleService.Write("Expense deleted successfully.");
        }
    }
    private async Task UpdateExpense()
    {
        var id = _consoleService.GetValueString("Enter Expense ID to update:");
        int parsedId;
        try
        {
            parsedId = GetValueClass.GetExpenseInt(id);
        }
        catch (ArgumentException ex)
        {
            _consoleService.Write(ex.Message);
            return;
        }

        var expensesFromRepo = await _expensesServices.GetExpenseById(parsedId);
        if (expensesFromRepo == null)
        {
            _consoleService.Write("Expense not found.");
            return;
        }
        var titleForUpdate = string.Empty;
        if (ViewExpensesHelper.ChangeFieldAnswer("title"))
        {
            titleForUpdate = _consoleService.GetValueString("Enter new Title :");
        }

        var descriptionForUpdate = string.Empty;
        if (ViewExpensesHelper.ChangeFieldAnswer("description"))
        {
            descriptionForUpdate = _consoleService.GetValueString("Enter new Description :");
        }

        string amountFromInput;
        var amountForUpdate = expensesFromRepo.Amount;
        if (ViewExpensesHelper.ChangeFieldAnswer("amount"))
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
        if (ViewExpensesHelper.ChangeFieldAnswer("expense type"))
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

        var resultUpdate = await _expensesServices.Update(expenseForUpdate);
        if (!resultUpdate.IsSuccess)
        {
            _consoleService.Write(resultUpdate.Message);
        }
        else
        {
            _consoleService.Write("Expense updated successfully.");
        }
    }
    public async Task ConvertExpenseCurrency()
    {
        var idForExpense = _consoleService.GetValueString("Please enter the id for  the expense to convert currency from Ron:");
        int id;
        try
        {
            id = GetValueClass.GetExpenseInt(idForExpense);
        }
        catch(Exception ex)
        {
            _consoleService.Write(ex.Message);
            return;
        }

        var currencyFromInput = _consoleService.GetValueString("Please enter the currency for convert:");
        CurrencyType currency;

        try
        {   
            currency = GetValueClass.GetExpenseCurrency(currencyFromInput);
        }
        catch(Exception ex) 
        {
            _consoleService.Write(ex.Message);
            return;
        }
        ResultResponse<Expense> resultResponse = await _expenseExchangeService.ConvertExpenseCurrencyFromRon(id, currency);

        if (!resultResponse.IsSuccess)
        {
             _consoleService.Write(resultResponse.Message);
            return;
        }
        await _expensesServices.Update(resultResponse.Data!);

        _consoleService.Write("The procces was a success!");
    }
}
