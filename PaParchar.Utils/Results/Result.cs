
namespace PaParchar.Utils.Results
{
    public class Result<T> : IResult<T>
    {
        public T Data { get; set; } = default!;
        public bool? Found { get; set; } = null;
        public string? Message { get; set; } = null;
        public string? ExceptionMessage { get; set; } = null;
        public bool? Successful { get; set; } = null;

        public static IResult<T> Success()
        {
            return new Result<T>
            {
                Successful = true,
            };
        }

        public static IResult<T> Success(T data)
        {
            return new Result<T>
            {
                Successful = true,
                Data = data
            };
        }

        public static IResult<T> Success(T data, string message)
        {
            return new Result<T>
            {
                Successful = true,
                Data = data,
                Message = message
            };
        }

        public static IResult<T> Failure()
        {
            return new Result<T>
            {
                Successful = false,
                Found = true
            };
        }

        public static IResult<T> Failure(T data)
        {
            return new Result<T>
            {
                Successful = false,
                Found = true,
                Data = data
            };
        }

        public static IResult<T> Failure(IResult<T> result)
        {
            return new Result<T>
            {
                Successful = false,
                Found = result.Found,
                Message = result.Message,
                ExceptionMessage = result.ExceptionMessage,
                Data = result.Data
            };
        }

        public static IResult<T> Failure(string message)
        {
            return new Result<T>
            {
                Successful = false,
                Found = true,
                Message = message
            };
        }

        public static IResult<T> Failure(string message, string exceptionMessage)
        {
            return new Result<T>
            {
                Successful = false,
                Found = true,
                Message = message,
                ExceptionMessage = exceptionMessage
            };
        }

        public static IResult<T> NotFound()
        {
            return new Result<T>
            {
                Successful = false,
                Found = false
            };
        }

        public static IResult<T> NotFound(T data)
        {
            return new Result<T>
            {
                Successful = false,
                Found = false,
                Data = data
            };
        }

        public static IResult<T> NotFound(string message)
        {
            return new Result<T>
            {
                Successful = false,
                Found = false,
                Message = message
            };
        }

        public static IResult<T> NotFound(string message, string exceptionMessage)
        {
            return new Result<T>
            {
                Successful = false,
                Found = false,
                Message = message,
                ExceptionMessage = exceptionMessage
            };
        }


        public static async Task<IResult<T>> SuccessAsync()
        {
            return await Task.FromResult(Success());
        }

        public static async Task<IResult<T>> SuccessAsync(T data)
        {
            return await Task.FromResult(Success(data));
        }

        public static async Task<IResult<T>> SuccessAsync(T data, string message)
        {
            return await Task.FromResult(Success(data, message));
        }

        public static async Task<IResult<T>> FailureAsync()
        {
            return await Task.FromResult(Failure());
        }

        public static async Task<IResult<T>> FailureAsync(T data)
        {
            return await Task.FromResult(Failure(data));
        }

        public static async Task<IResult<T>> FailureAsync(IResult<T> result)
        {
            return await Task.FromResult(Failure(result));
        }

        public static async Task<IResult<T>> FailureAsync(string message)
        {
            return await Task.FromResult(Failure(message));
        } 

        public static async Task<IResult<T>> FailureAsync(string message, string exceptionMessage)
        {
            return await Task.FromResult(Failure(message, exceptionMessage));
        }

        public static async Task<IResult<T>> NotFoundAsync()
        {
            return await Task.FromResult(NotFound());
        }

        public static async Task<IResult<T>> NotFoundAsync(T data)
        {
            return await Task.FromResult(NotFound(data));
        }

        public static async Task<IResult<T>> NotFoundAsync(string message)
        {
            return await Task.FromResult(NotFound(message));
        }

        public static async Task<IResult<T>> NotFoundAsync(string message, string exceptionMessage)
        {
            return await Task.FromResult(NotFound(message, exceptionMessage));
        }
    }
}
