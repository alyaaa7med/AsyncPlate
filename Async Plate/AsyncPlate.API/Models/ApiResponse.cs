namespace AsyncPlate.API.Models
{
    public class ApiResponse<T>
    {
        //for only success response but other responses will be handled by the global error handler
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool isSuccess, string message, T? data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }




    }
}
