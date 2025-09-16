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
    public void AddExpense_WhenCalled_ShoudAddInDb()
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
        var resultResponse = _services.AddExpenses(expense);

        //Assert

        Assert.That(resultResponse.IsSuccess, Is.True);
        Assert.That(resultResponse.Expense, Is.EqualTo(expense));

        _mockRepo.Verify(x => x.AddExpense(expense), Times.Once);
    }

    [Test]
    public void AddExpense_FailedInvalidate_ShoudReturnAFailedMessage()
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
        var resultResponse = _services.AddExpenses(expense);

        //Assert

        Assert.That(resultResponse.IsSuccess, Is.False);
        Assert.That(resultResponse.Message, Is.EqualTo("Title is required!"));

        _mockRepo.Verify(x => x.AddExpense(expense), Times.Never);
    }

    [Test]
    public void AddExpense_NullObject_ShoudReturnAFailedMessage()
    {
        //Arrange & Act
        var resultResponse = _services.AddExpenses(null!);

        //Assert

        Assert.That(resultResponse.IsSuccess, Is.False);
        Assert.That(resultResponse.Message, Is.EqualTo("Expense is null!"));

        _mockRepo.Verify(x => x.AddExpense(It.IsAny<Expense>()), Times.Never);
    }
    #endregion

    #region RemoveExpense

    [Test]
    public void RemoveExpense_WhenCalled_RemoveExpenseFromDb()
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
        _mockRepo.Setup(x => x.GetExpenseById(1)).Returns(expense);

        //Act
       var result = _services.RemoveExpenses(1);

        //Assert

        Assert.IsTrue(result.IsSuccess);
        _mockRepo.Verify(x => x.RemoveExpense(expense), Times.Once);
    }

    [Test]
    public void RemoveExpense_WhenCalled_ReturnFailedMessage()
    {
        //Arrange
        _mockRepo.Setup(x => x.GetExpenseById(1)).Returns((Expense?)null);

        //Act
        var result = _services.RemoveExpenses(1);

        //Assert

        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Sorry! Expense not found."));
        _mockRepo.Verify(x => x.RemoveExpense(null!), Times.Never);
    }
    #endregion

    # region GetAllExpenses
    [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataEmptyListExpenses))]
    [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCaseDataExpenses))]
    public void GetAllExpenses_WhenCalled_ShouldReturnExpenses(List<Expense> listFromParameter)
    {
        //Arrange
        _mockRepo.Setup(x => x.GetAllExpenses()).Returns(listFromParameter);

        //Act
        var list = _services.GetAllExpenses();

        //Assert
        Assert.That(list.Count, Is.EqualTo(listFromParameter.Count));
        _mockRepo.Verify(x => x.GetAllExpenses(), Times.Once);  
    }
    #endregion

    #region GetExpenseById
    [Test]
    public void GetExpenseById_WhenCalled_ShouldReturnExpense()
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
        _mockRepo.Setup(x => x.GetExpenseById(1)).Returns(expense);

        //Act
        var expenseFromDb = _services.GetExpenseById(1);

        //Assert
        Assert.That(expenseFromDb, Is.EqualTo(expense));
        _mockRepo.Verify(x => x.GetExpenseById(1), Times.Once);
    }
    #endregion

    #region UpdateExpense
    [TestCaseSource(typeof(TestDataCases), nameof(TestDataCases.TestCasesUpdateExpenseService))]
    public void UpdateExpense_WhenCalled_UpdateAllFields(List<Expense> expenses, string expectedTitle, 
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

        _mockRepo.SetupSequence(x => x.GetExpenseById(1))
            .Returns(expenseForUpdate);

        //Act
        var result = _services.Update(expenseFromList);

        //Assert

        Assert.IsTrue(result.IsSuccess);
        Assert.Multiple(() =>
        {
            Assert.That(expenseForUpdate.Title, Is.EqualTo(expectedTitle));
            Assert.That(expenseForUpdate.Description, Is.EqualTo(expectedDescription));
            Assert.That(expenseForUpdate.Amount, Is.EqualTo(expectedAmount));
        });
        _mockRepo.Verify(x => x.UpdateExpense(expenseFromList), Times.Once);
    }
    [Test]
    public void UpdateExpense_WhenCalled_ShouldReturnFailedMessage()
    {
        //Arrange

        //Act
        var result = _services.Update(null!);

        //Assert

        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Expense is null!"));
        _mockRepo.Verify(x => x.UpdateExpense(It.IsAny<Expense>()), Times.Never);
    }

    [Test]
    public void UpdateExpense_WhenCalled_ShouldReturnFailedMessage2()
    {
        //Arrange

        _mockRepo.SetupSequence(x => x.GetExpenseById(1))
            .Returns((Expense?)null);

        //Act
        var result = _services.Update(new Expense());

        //Assert

        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Message, Is.EqualTo("Expense not found!"));
        _mockRepo.Verify(x => x.UpdateExpense(It.IsAny<Expense>()), Times.Never);
    }
    #endregion
}
