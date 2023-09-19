namespace BlazorClient.Shared;

public class ApiResponse<T> where T : class
{
    public bool IsSuccess { get; private set; }
    public T Result { get; private set; }
    public string ErrorMessage { get; private set; }
    public bool IsError => !IsSuccess;

    public static ApiResponse<T> Success(T result) => new ApiResponse<T>
    {
        IsSuccess = true,
        Result = result,
        ErrorMessage = null
    };
    
    public static ApiResponse<T> Error(string errorMessage) => new ApiResponse<T>
    {
        IsSuccess = false,
        Result = null,
        ErrorMessage = errorMessage
    };
}