using ExpenseTrackerCLI.Entities;

namespace ExpenseTrackerCLI.Common;

public class ResultResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; private set; }  
    public ErrorType Error { get; set; }

    private ResultResponse(bool isSuccess, T? data, ErrorType error )
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    public static ResultResponse<T> Success(T? data = default) => new(true, data, ErrorType.None){ };
    public static ResultResponse<T> Failure(string message, ErrorType error) => new(false, default, error)   { Message = message };
}
