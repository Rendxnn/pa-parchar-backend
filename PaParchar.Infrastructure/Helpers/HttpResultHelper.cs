
using Microsoft.AspNetCore.Mvc;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Helpers
{
    public static class HttpResultHelper
    {
        public static ActionResult MapResult<T>(IResult<T> result)
        {
            if (result.Found.HasValue && !result.Found.Value)
            {
                return new NotFoundObjectResult(result);
            }

            if (result.Successful.HasValue && !result.Successful.Value)
            {
                return new BadRequestObjectResult(result);
            }

            return new OkObjectResult(result);
        }
    }
}
