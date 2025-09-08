using ExpenseTrackerCLI.ConsoleServices;
using Microsoft.Extensions.Logging;

namespace ExpenseTrackerCLI.ExpenseConsole;

public class ExecutorExpenseConsole
{
    private readonly  ExpenseConsole _expenseConsole;
    private readonly IConsoleService _consoleService;
    ILogger<ExecutorExpenseConsole> _logger;    

    public ExecutorExpenseConsole(ExpenseConsole expenseConsole, IConsoleService consoleService, ILogger<ExecutorExpenseConsole> logger )
    {
        _expenseConsole = expenseConsole;
        _consoleService = consoleService;
        _logger = logger;
    }
    public void Run()
    {
        while (true)
        {
            _consoleService.Menu();
            var choice = _consoleService.Read();
            _logger.LogInformation($" User selected option {choice}");
            if (choice == "5") 
            {
                Environment.Exit(0);
            }
            _expenseConsole.ExecuteExpenseConsole(choice);
        }
    }
}
