using ExpenseProjectNUnitTests.TestData;
using ExpenseTrackerCLI.Entities;
using ExpenseTrackerCLI.Repositories;
using ExpenseTrackerCLI.Services.ExpenseService;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ExpenseProjectNUnitTests.ServicesTests;

public class ServiceExpenseUnitTests
{
    private Mock<IValidator<Expense?>> _mockValidator;
    private Mock<IExpensesRepository> _mockRepo;
    private IExpensesServices _services;

    [SetUp]
    public void Setup()
    {
        _mockValidator = new Mock<IValidator<Expense?>>();
        _mockRepo = new Mock<IExpensesRepository>();

        _services = new ExpensesServices(_mockRepo.Object, _mockValidator.Object);
    }

    #region AddExpense
    [Test]
    public async Task AddExpense_WhenCalled_ShoudAddInDb()
    {
        //Arrange
        var expense = new Expense
        {
            Title = "Title",
            Description = "Description",
            Amount = 10,
            CreatedExpense = DateTimeOffset.Now
        };

        _mockValidator.Setup(v => v.Validate(expense))
                      .Returns(new ValidationResult());
        //Act
        var resultResponse = await _services.AddExpenses(expense);

        //Assert

        Assert.That(resultResponse.IsSuccess, Is.True);
        Assert.That(resultResponse.Data, Is.EqualTo(expense));

        _mockRepo.Verify(x => x.AddExpense(expense, default), Times.Once);
    }

    [Test]
    public async Task AddExpense_FailedInvalidate_ShoudReturnAFailedMessage()
    {
        //Arrange
        var expense = new Expense
        {
            Title = "",
            Description = "Description",
            Amount = 10,
            CreatedExpense = DateTimeOffset.Now
        };

        _mockValidator.Setup(v => v.Validate(expense))
                      .Returns(new ValidationResult( new List<ValidationFailure>
                      {
                          new ValidationFailure ( "Title", "Title is required!")
                      }));
        //Act
        var resultResponse = await _services.AddExpenses(expense);

        //Assert

        Assert.That(resultResponse.IsSuccess, Is.False);
        Assert.That(resultResponse.Message, Is.EqualTo("Title is required!"));

        _mockRepo.Verify(x => x.AddExpense(expense, default), Times.Never);
    }

    [Test]
    public async Task AddExpense_NullObject_ShoudReturnAFailedMessage()
    {
        //Arrange & Act
        var resultResponse = await _services.AddExpenses(null!);

        //Assert

        Assert.That(resultResponse.IsSuccess, Is.False);
        Assert.That(resultResponse.Message, Is.EqualTo("Expense is null!"));

        _mockRepo.Verify(x => x.AddExpense(It.IsAny<Expense>(), default), Times.Never);
    }
    #endregion

    #region RemoveExpense

    [Test]
    public async Task RemoveExpense_WhenCalled_RemoveExpenseFromDb()
    {
        //Arrange
        var expense = new Expense
        {
            Id = 1,
            Title = "Title",
            Description = "Description",
            Amount = 10,
            CreatedExpense = DateTimeOffset.Now
        };
        _mockRepo.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync(expense);

        //Act
       var result = await _services.RemoveExpenses(1);

        //Assert

        Assert.IsTrue(result.IsSuccess);
        _mockRepo.Verify(x => x.RemoveExpense(expense, default), Times.Once);
    }

    [Test]
    public async Task RemoveExpense_WhenCalled_ReturnFailedMessage()
    {
        //Arrange
        _mockRepo.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync((Expense?)null);

        //Act
        var result = await _services.RemoveExpenses(1);

        //Assert

        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Sorry! Expense not found."));
        _mockRepo.Verify(x => x.RemoveExpense(It.IsAny<Expense>(), default), Times.Never);
    }
    #endregion

    # region GetAllExpenses
    [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataEmptyListExpenses))]
    [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
    public async Task GetAllExpenses_WhenCalled_ShouldReturnExpenses(List<Expense> listFromParameter)
    {
        //Arrange
        _mockRepo.Setup(x => x.GetAllExpenses(default)).ReturnsAsync(listFromParameter);

        //Act
        var list = await _services.GetAllExpenses();

        //Assert
        Assert.That(list.Count, Is.EqualTo(listFromParameter.Count));
        _mockRepo.Verify(x => x.GetAllExpenses(default), Times.Once);  
    }
    #endregion

    #region GetExpenseById
    [Test]
    public async Task GetExpenseById_WhenCalled_ShouldReturnExpense()
    {
        //Arrange
        var expense = new Expense
        {
            Id = 1,
            Title = "Title",
            Description = "Description",
            Amount = 10,
            CreatedExpense = DateTimeOffset.Now
        };
        _mockRepo.Setup(x => x.GetExpenseById(1, default)).ReturnsAsync(expense);

        //Act
        var expenseFromDb = await _services.GetExpenseById(1, default);

        //Assert
        Assert.That(expenseFromDb, Is.EqualTo(expense));
        _mockRepo.Verify(x => x.GetExpenseById(1, default), Times.Once);
    }
    #endregion

    #region UpdateExpense
    [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCasesUpdateExpenseService))]
    public async Task UpdateExpense_WhenCalled_UpdateAllFields(List<Expense> expenses, string expectedTitle, 
        string expectedDescription, decimal expectedAmount)
    {
        var expenseFromList = expenses[0];
        var expenseForUpdate = new Expense()
        {
            Id = 1,
            Title = "Test Title",
            Description = "Test Description",
            Amount = 12,
        };

        _mockRepo.SetupSequence(x => x.GetExpenseById(1, default))
            .ReturnsAsync(expenseForUpdate);

        //Act
        var result = await _services.Update(expenseFromList);

        //Assert

        Assert.IsTrue(result.IsSuccess);
        Assert.Multiple(() =>
        {
            Assert.That(expenseForUpdate.Title, Is.EqualTo(expectedTitle));
            Assert.That(expenseForUpdate.Description, Is.EqualTo(expectedDescription));
            Assert.That(expenseForUpdate.Amount, Is.EqualTo(expectedAmount));
        });
        _mockRepo.Verify(x => x.UpdateExpense(expenseFromList, default), Times.Once);
    }
    [Test]
    public async Task UpdateExpense_WhenCalled_ShouldReturnFailedMessage()
    {
        //Arrange

        //Act
        var result = await _services.Update(null!);

        //Assert

        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Expense is null!"));
        _mockRepo.Verify(x => x.UpdateExpense(It.IsAny<Expense>(), default), Times.Never);
    }

    [Test]
    public async Task UpdateExpense_WhenCalled_ShouldReturnFailedMessage2()
    {
        //Arrange

        _mockRepo.SetupSequence(x => x.GetExpenseById(1, default))
            .ReturnsAsync((Expense?)null);

        //Act
        var result = await _services.Update(new Expense());

        //Assert

        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Expense not found!"));
        _mockRepo.Verify(x => x.UpdateExpense(It.IsAny<Expense>(), default), Times.Never);
    }
    #endregion
}
