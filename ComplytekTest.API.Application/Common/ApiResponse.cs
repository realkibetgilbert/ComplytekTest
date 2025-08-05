namespace ComplytekTest.API.Application.Common
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Title = "Success",
                Message = "Request completed successfully.",
                Data = data
            };
        }

        public static ApiResponse<T> Failure()
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Title = "Fail",
                Message = "Request Failed",
                Data = default
            };
        }
    }
}
