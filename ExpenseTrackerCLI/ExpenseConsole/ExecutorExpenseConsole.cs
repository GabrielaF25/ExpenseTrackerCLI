using ExpenseTrackerCLI.ConsoleServices;

namespace ExpenseTrackerCLI.ExpenseConsole;

public class ExecutorExpenseConsole
{
    private readonly 
        ExpenseConsole _expenseConsole;
    private readonly IConsoleService _consoleService;

    public ExecutorExpenseConsole(ExpenseConsole expenseConsole, IConsoleService consoleService )
    {
        _expenseConsole = expenseConsole;
        _consoleService = consoleService;
    }
    public void Run()
    {
        while (true)
        {
            _consoleService.Menu();
            var choice = _consoleService.Read();

            _expenseConsole.ExecuteExpenseConsole(choice);
        }
    }
}
