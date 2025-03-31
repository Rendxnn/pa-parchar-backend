namespace PaParchar.Utils.Results
{
    public class Result<T> : IResult<T>
    {
        public T Data { get; set; } = default!;
        public bool? Found { get; set; } = true;
        public bool? Successful { get; set; } = true;
        public string? Message { get; set; }
        public string? ExceptionMessage { get; set; }

        public static Result<T> Success()
        {
            return new Result<T>
            {
                Successful = true,
            };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Successful = true,
                Data = data
            };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T>
            {
                Successful = true,
                Data = data,
                Message = message
            };
        }

        public static Result<T> Failure()
        {
            return new Result<T>
            {
                Successful = false,
                Found = true
            };
        }

        public static Result<T> Failure(T data)
        {
            return new Result<T>
            {
                Successful = false,
                Found = true,
                Data = data
            };
        }

        public static Result<T> Failure(IResult<T> result)
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

        public static Result<T> Failure(string message)
        {
            return new Result<T>
            {
                Successful = false,
                Found = true,
                Message = message
            };
        }

        public static Result<T> Failure(string message, string exceptionMessage)
        {
            return new Result<T>
            {
                Successful = false,
                Found = true,
                Message = message,
                ExceptionMessage = exceptionMessage
            };
        }

        public static Result<T> NotFound()
        {
            return new Result<T>
            {
                Successful = false,
                Found = false
            };
        }

        public static Result<T> NotFound(T data)
        {
            return new Result<T>
            {
                Successful = false,
                Found = false,
                Data = data
            };
        }

        public static Result<T> NotFound(string message)
        {
            return new Result<T>
            {
                Successful = false,
                Found = false,
                Message = message
            };
        }

        public static Result<T> NotFound(string message, string exceptionMessage)
        {
            return new Result<T>
            {
                Successful = false,
                Found = false,
                Message = message,
                ExceptionMessage = exceptionMessage
            };
        }

        public static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }

        public static Task<Result<T>> FailureAsync()
        {
            return Task.FromResult(Failure());
        }

        public static Task<Result<T>> FailureAsync(T data)
        {
            return Task.FromResult(Failure(data));
        }

        public static Task<Result<T>> FailureAsync(IResult<T> result)
        {
            return Task.FromResult(Failure(result));
        }

        public static Task<Result<T>> FailureAsync(string message)
        {
            return Task.FromResult(Failure(message));
        }

        public static Task<Result<T>> FailureAsync(string message, string exceptionMessage)
        {
            return Task.FromResult(Failure(message, exceptionMessage));
        }

        public static Task<Result<T>> NotFoundAsync()
        {
            return Task.FromResult(NotFound());
        }

        public static Task<Result<T>> NotFoundAsync(T data)
        {
            return Task.FromResult(NotFound(data));
        }

        public static Task<Result<T>> NotFoundAsync(string message)
        {
            return Task.FromResult(NotFound(message));
        }

        public static Task<Result<T>> NotFoundAsync(string message, string exceptionMessage)
        {
            return Task.FromResult(NotFound(message, exceptionMessage));
        }
    }
}
