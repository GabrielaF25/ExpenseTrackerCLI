// See httpsusing GabrielasLibrary.ConsoleApp;
using ExpenseTrackerCLI.ConsoleServices;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.ExpenseConsole;
using ExpenseTrackerCLI.ExpensesDatabase;
using ExpenseTrackerCLI.Repositories;
using ExpenseTrackerCLI.Services;
using ExpenseTrackerCLI.ValidateEntities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        var confing = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        services.AddDbContext<ExpensesDB>(option =>
                    option.UseSqlServer(confing.GetConnectionString("ExpensesDB")));

        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IExpensesServices,ExpensesServices>();
        services.AddScoped<IConsoleService, ConsoleService>();
        services.AddScoped<ExpenseConsole>();
        services.AddScoped<IValidator<Expense>,ValidateExpenses>();
        services.AddScoped<ExecutorExpenseConsole>();

        var serviceProvider = services.BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<ExecutorExpenseConsole>();
        app.Run();
    }
}