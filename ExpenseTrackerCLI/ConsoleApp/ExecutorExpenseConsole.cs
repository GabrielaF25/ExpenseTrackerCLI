using ExpenseTrackerCLI.ConsoleServices;
using Microsoft.Extensions.Logging;

namespace ExpenseTrackerCLI.ConsoleApp;

public class ExecutorExpenseConsole(ExpenseConsole expenseConsole, IConsoleService consoleService, ILogger<ExecutorExpenseConsole> logger)
{
    private readonly  ExpenseConsole _expenseConsole = expenseConsole;
    private readonly IConsoleService _consoleService = consoleService;
    private readonly ILogger<ExecutorExpenseConsole> _logger = logger;

    public void Run()
    {
        while (true)
        {
            _consoleService.Menu();
            var choice = _consoleService.Read();
            _logger.LogInformation($" User selected option {choice}");
            if (choice == "6") 
            {
                Environment.Exit(0);
            }
            _expenseConsole.ExecuteExpenseConsole(choice);
        }
    }
}
