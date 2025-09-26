// See httpsusing GabrielasLibrary.ConsoleApp;
using ExpenseTrackerCLI.ConsoleApp;
using ExpenseTrackerCLI.ExpensesDatabase;
using ExpenseTrackerCLI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Program
{
    static void Main(string[] args)
    {
        var confing = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "hh:mm:ss ";
            });
        });

        services.AddDbContext<ExpensesDB>(option =>
                    option.UseSqlServer(confing.GetConnectionString("ExpensesDB")));
        services.ServiceCollection();

        var serviceProvider = services.BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<ExecutorExpenseConsole>();
        app.Run();
    }
}