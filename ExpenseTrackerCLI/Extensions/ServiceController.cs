using ExpenseTrackerCLI.ConsoleApp;
using ExpenseTrackerCLI.ConsoleServices;
using ExpenseTrackerCLI.DateTimeExchangeRate;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExchangeRate;
using ExpenseTrackerCLI.Repositories;
using ExpenseTrackerCLI.Services.ExpenseChange;
using ExpenseTrackerCLI.Services.ExpenseService;
using ExpenseTrackerCLI.ValidateEntities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerCLI.Extensions;

public static class ServiceController
{
    public static IServiceCollection ServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<ExpenseConsole>();
        services.AddScoped<ViewExpensesHelper>();
        services.AddScoped<IConsoleService, ConsoleService>();
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddScoped<IDateTimeRate, DateTimeRate>();
        services.AddScoped<IExpenseExchangeService, ExpenseExchangeService>();
        services.AddScoped<IExpensesServices, ExpensesServices>();
        services.AddScoped<IValidator<Expense>, ValidateExpenses>();
        services.AddScoped<ExecutorExpenseConsole>();
        return services;
    }
}
