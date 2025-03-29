
using Microsoft.AspNetCore.Mvc;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Extensions
{
    public static class ResultExtension
    {
        public static ActionResult<T> ToHttpResponse<T>(this IResult<T> result)
        {
            if (result.Found.HasValue && !result.Found.Value) return new NotFoundObjectResult(result);

            if (result.Successful.HasValue && !result.Successful.Value) return new BadRequestObjectResult(result);

            return new OkObjectResult(result.Data);
        }
    }
}
